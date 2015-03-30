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
                rhymes[name] = create_rhyme(pattern);
            }

            foreach (var pattern in children)
            {
                var name = pattern.children[0].text;
                var rhyme = rhymes[name];
                rhyme.initialize(pattern.children[1], this);
            }
        }

        void load(string source)
        {
            var parser = new Parser(source, Parser.parser_grammar);
            var legend = parser.read();
            process_grammar(legend.children);
        }

        Rhyme create_rhyme(Legend pattern)
        {
            var name = pattern.children[0].text;
            var group = pattern.children[1];
            var children = group.children;
            switch (group.type)
            {
                case "and":
                    //                    if (children.Count > 1)
                    return new And_Rhyme(name);

                //                    if (group.type == "repetition")
                case "repetition":
                    return new Repetition_Rhyme(name);


                case "or":
                    return new Or_Rhyme(name);

            }

            throw new Exception("Not implemented.");
            //            return new Single_Rhyme(name);

            //            return null;
        }

        public Rhyme get_whisper_rhyme(string name)
        {
            return rhymes[name];
            //            return rhymes.ContainsKey(name)
            //                ? rhymes[name]
            //                : new Single_Rhyme(name, lexer.whispers[name]);
        }

        public Rhyme create_child(Legend pattern)
        {
            var text = pattern.text;
            if (pattern.type == "id")
                return get_whisper_rhyme(text);

            if (pattern.type == "repetition")
            {
                var repetition = new Repetition_Rhyme(text);
                repetition.initialize(pattern, this);
                return repetition;
            }

            throw new Exception("Not supported.");
        }

    }
}
