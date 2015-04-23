using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using runic.lexer.whispers;
using runic.parser;

namespace runic.lexer
{
    public class Lexer
    {
        public Dictionary<string, Whisper> whispers = new Dictionary<string, Whisper>();
        public static Lexer_Lexicon lexer_lexicon = new Lexer_Lexicon();
        public static Parser_Lexicon parser_lexicon = new Parser_Lexicon();

        public Lexer()
        {
        }

        public Lexer(string lexicon)
        {
            load_lexicon(lexicon);
        }

        void load_lexicon(string lexicon)
        {
            var runes = lexer_lexicon.read(lexicon);
            var legend = Parser.read(lexicon, runes, Parser.lexer_grammar.start);
            process_lexicon(legend.children);
        }

        Whisper add_whisper(string name, Whisper whisper)
        {
            if (whispers.ContainsKey(name))
                throw new Exception("Lexer already contains a whisper named " + name + ".");

            whispers[name] = whisper;
            return whisper;
        }

        protected Whisper add_whisper(Whisper whisper)
        {
            if (whispers.ContainsKey(whisper.name))
                throw new Exception("Lexer already contains a whisper named " + whisper.name + ".");

            whispers[whisper.name] = whisper;
            return whisper;
        }

        void process_lexicon(List<Legend> children)
        {
            foreach (var pattern in children)
            {
                var name = pattern.children[0].text;
                add_whisper(name, create_whisper(pattern));
            }

            foreach (var pattern in children)
            {
                initialize_whisper(pattern);
            }

            foreach (var pattern in children.Where(p => p.children[2].children.Count > 1))
            {
                var name = pattern.children[0].text;
                var whisper = (Whisper_Group)whispers[name];
                whisper.whispers = pattern.children[2].children.Select(p =>
                    {
                        var child = create_sub_whisper(p.text, p);
                        if (p.type == "string" || p.type == "regex")
                            add_whisper(child.name, child);

                        return child;
                    }).ToArray();
            }
        }

        void initialize_whisper(Legend source)
        {
            var attributes = source.children[1];
            if (attributes == null)
                return;

            var name = source.children[0].text;
            var whisper = whispers[name];

            var whisper_attributes = new List<Whisper.Attribute>();
            foreach (var p in attributes.children)
            {
                if (p.text.Last() == '>')
                {
                    var other_name = p.text.Substring(0, p.text.Length - 1);
                    whispers[other_name].add_target(whisper);
                }
                else
                {
                    Whisper.Attribute result;
                    Enum.TryParse(p.text, out result);
                    whisper_attributes.Add(result);
                }
            }

            if (whisper_attributes.Count > 0)
            {
                whisper.attributes = whisper_attributes.ToArray();
//                if (whisper.has_attribute(Whisper.Attribute.tween))
//                {
//                    whispers[name] = new Tween(whisper.name, source.children[2].children[0].text);
//                }
            }
        }

        Whisper create_whisper(Legend source)
        {
            var name = source.children[0].text;
            var children = source.children[2].children;

            var whisper = children.Count > 1
                ? new Whisper_Group(name)
                : create_sub_whisper(name, children[0]);

            return whisper;
        }

        Whisper create_sub_whisper(string name, Legend source)
        {
            switch (source.type)
            {
                case "regex":
                    return new Regex_Whisper(name, source.text);

                case "string":
                    return new String_Whisper(name, source.text);
            }

            var text = source.text;
            if (text != null)
            {
                if (whispers.ContainsKey(text))
                    return whispers[text];
            }

            throw new Exception("Unknown whisper type: " + source.type + ".");
        }

        public List<Rune> read(string input)
        {
            var result = new List<Rune>();
            var position = new Position(input);

            while (position.index < input.Length)
            {
                var rune = next(input, position);
                if (rune == null)
                    throw new Exception("Could not find match at " + position.get_position_string() + " " + get_safe_substring(input, position.index, 10));

                if (rune.length == 0)
                    throw new Exception("Invalid Whisper:" + rune.whisper.name);

                if (!rune.whisper.has_attribute(Whisper.Attribute.ignore))
                {
                    rune.index = result.Count;
                    result.Add(rune);
                }

                position = rune.range.end;
            }

            return result;
        }

        Rune next(string input, Position position)
        {
            foreach (var whisper in whispers.Values)
            {
                var rune = whisper.match(input, position);
                if (rune != null)
                    return rune;
            }

            return null;
        }

        public static string load_resource(string filename)
        {
            var path = "runic.resources." + filename;
            var assembly = Assembly.GetExecutingAssembly();

            var stream = assembly.GetManifestResourceStream(path);
            if (stream == null)
                throw new Exception("Could not find file " + path + ".");

            var reader = new StreamReader(stream);
            return reader.ReadToEnd().Replace("\r\n", "\n");
        }

        public static string get_safe_substring(string text, int start)
        {
            return start >= text.Length ? "" : text.Substring(start);
        }

        public static string get_safe_substring(string text, int start, int end)
        {
            if (start >= text.Length)
                return "";

            if (start + end >= text.Length)
                return text.Substring(start);

            return text.Substring(start, end);
        }
    }
}
