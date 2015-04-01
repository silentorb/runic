using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using runic.retreat.rhymes;

namespace runic.retreat
{
    public class Loaded_Grammar : Grammar
    {
        public Loaded_Grammar(string source)
        {
            load(source);
        }

        void process_grammar(List<Legend> children)
        {
            foreach (var pattern in children)
            {
                var name = pattern.children[0].text;
                rhymes[name] = create_root(pattern);
            }

            foreach (var pattern in children)
            {
                var name = pattern.children[0].text;
                var rhyme = rhymes[name];
                rhyme.initialize(pattern.children[2], this);
            }
        }

        void load(string source)
        {
            var parser = new Parser(source, Parser.parser_grammar);
            var legend = parser.read();
            process_grammar(legend.children);
        }

        Rhyme create_root(Legend pattern)
        {
            var rhyme = create_rhyme(pattern.children[2], pattern.children[0].text);
            var attributes = pattern.children[1];
            if (attributes != null)
            {
                foreach (var attribute in attributes.children)
                {
                    if (attribute.text == "global")
                        global_rhymes.Add(rhyme);
                }
            }

            return rhyme;
        }

        Rhyme create_rhyme(Legend group, string name = null)
        {
            Rhyme result;

            switch (group.type)
            {
                case "and":
                    result = new And_Rhyme(name);
                    break;

                case "repetition":
                    result = new Repetition_Rhyme(name);
                    break;

                case "or":
                    result = new Or_Rhyme(name);
                    break;

                case "regex":
                    result = new Regex_Rhyme(name, group.text);
                    break;

                case "string":
                    result = new String_Rhyme(name ?? group.text, group.text);
                    rhymes[group.text] = result;
                    break;

                default:
                    throw new Exception("Not implemented.");

            }

            return result;
        }

        public Rhyme create_child(Legend pattern)
        {
            if (pattern.type == "id")
            {
                return rhymes[pattern.text];
            }

            var rhyme = create_rhyme(pattern);
            rhyme.initialize(pattern, this);
            return rhyme;
        }

        public Rhyme get_whisper_rhyme(string name)
        {
            return rhymes[name];
        }
    }
}
