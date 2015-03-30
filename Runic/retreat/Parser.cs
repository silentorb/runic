using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using runic.Properties;
using runic.retreat.rhymes;

namespace runic.retreat
{
    public class Parser
    {
        internal static Parser_Grammar parser_grammar = new Parser_Grammar();

        public int furthest;
        public List<Entry> history = new List<Entry>();
        public string code;
        public Grammar grammar;

        public Parser(string code)
        {
            this.code = code;
            grammar = parser_grammar;
        }

        public Parser(string code, Grammar grammar)
        {
            this.code = code;
            this.grammar = grammar;
        }

        public void add_entry(string value, Rhyme rhyme, Position position)
        {
            history.Add(new Entry(rhyme, position.index, value, this));
        }

        public static string get_safe_substring(string text, int start, int end)
        {
            if (start >= text.Length)
                return "";

            if (start + end >= text.Length)
                return text.Substring(start);

            return text.Substring(start, end);
        }

//        public Legend read(string code, string start_name = "start")
//        {
//            var start = rhymes[start_name];
//            return read(code, start);
//        }

        public Legend read(string start_name = "start")
        {
            var start = grammar.rhymes[start_name];
            var stone = new Position(code, this);
            var result = start.match(stone, null);

            if (result != null)
            {
                //                if (result.stone.is_at_end)
                return result.legend;

                //                if (runes.Skip(result.stone.get_position())
                //                    .All(r => r.whisper.has_attribute(Whisper.Attribute.optional)))
                //                    return result.legend;
            }

            //            var furthest = runes[stone.tracker.furthest];
            //            var last = stone.tracker.history.LastOrDefault(h => h.success);
            //            if (last == null)
            //            {
            throw new Exception("Could not find match at 1:1.");
            //            }
            //            else
            //            {
            //                throw new Exception("Could not find match at "
            //                    + furthest.range.end.y + ":" + furthest.range.end.x
            //                    + ", " + furthest.whisper.name + "."
            //                    + "  Last match was " + last.rhyme.name + "."
            //                );
            //            }
        }

        public Legend_Result check_globals(Position position)
        {
            foreach (var rhyme in grammar.global_rhymes)
            {
                var result = rhyme.match(position, null);
                if (result != null)
                    return result;
            }
            return null;
        }
    }
}
