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
        public Legend_Result furthest_legend_result;
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
            history.Add(new Entry(rhyme, position.index, value != null ? position.index + value.Length : position.index, value, this));
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

        public Legend_Result check_globals(Position position, Rhyme except = null)
        {
            foreach (var rhyme in grammar.global_rhymes)
            {
                if (rhyme == except)
                    continue;

                var result = rhyme.match(position, null);
                if (result != null)
                {
                    rhyme.match(position, null);
                    return result;
                }
            }
            return null;
        }

        public Position check_globals_greedy(Position stone, Rhyme except = null)
        {
            Legend_Result extra = null;
            while ((extra = check_globals(stone, except)) != null && extra.stone.index > stone.index)
            {
                stone = extra.stone;
            }

            return stone;
        }

        public Legend_Result match(Position stone, Rhyme rhyme, Rhyme parent, bool use_global = true)
        {
//            if (!grammar.global_rhymes.Contains(rhyme))
//            {
                stone = check_globals_greedy(stone, rhyme);
//            }

            var result = rhyme.match(stone, parent);
            if (result == null)
            {
                if (!use_global)
                {
                    stone.parser.add_entry(null, rhyme, stone);
                    return null;
                }

                var global_result = check_globals(stone);
                if (global_result == null)
                    return null;

                stone.parser.add_entry(global_result.legend.text, global_result.legend.rhyme, stone);
                result = match(global_result.stone, rhyme, parent);
            }

            if (result != null && result.legend != null)
                stone.parser.add_entry(result.legend.text, result.legend.rhyme, stone);


            return result;
        }

        public Legend read(string start_name = "start")
        {
            var start = grammar.rhymes[start_name];
            var stone = new Position(code, this);
            var result = match(stone, start, null);

            if (result != null)
            {
                stone = result.stone;
                if (stone.index < code.Length)
                {
                    stone = check_globals_greedy(stone);

                    if (stone.index < code.Length)
                    {
                        throw new Exception("Could not find match at " + furthest_legend_result.stone.get_position_string());
                    }
                }

                return result.legend;
            }

            if (furthest_legend_result == null)
            {
                throw new Exception("Could not find match at 1:1.");
            }
            else
            {
                throw new Exception("Could not find match at " + furthest_legend_result.stone.get_position_string());
            }

        }
    }
}
