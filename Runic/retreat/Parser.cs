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

        public int furthest_index;
        public int furthest_failure_substeps;
        public Entry furthest_success;
        public Position furthest_failure_position;
        public Legend_Result furthest_failure;
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

        //        public void pop()
        //        {
        //            while (failure_stack.Count > depth)
        //            {
        //                failure_stack.Pop();
        //            }
        //        }

        public void add_entry(string value, Rhyme rhyme, Position start, Position end)
        {
            var entry = new Entry(rhyme, start, end, value, this);
            history.Add(entry);
            if (start.index >= furthest_index)
            {
                furthest_index = start.index;
                furthest_success = entry;
            }
        }

        public static string get_safe_substring(string text, int start, int end)
        {
            if (start >= text.Length)
                return "";

            if (start + end >= text.Length)
                return text.Substring(start);

            return text.Substring(start, end);
        }

        public Legend_Result check_globals(Position position, Rhyme except = null)
        {
            foreach (var rhyme in grammar.global_rhymes)
            {
                if (rhyme == except)
                    continue;

                var result = rhyme.match(position, null);
                if (result.success)
                    return result;
            }
            return null;
        }

        public Legend_Result check_globals_greedy(Position stone, Rhyme except = null)
        {
            Legend_Result extra;
            Legend_Result last = null;
            while ((extra = check_globals(stone, except)) != null && extra.success && extra.end.index > stone.index)
            {
                stone = extra.end;
                last = extra;
            }

            return last;
        }

        public void update_failure(Legend_Result result, int substeps)
        {
            if (result.start.index != furthest_success.end.index)
                return;

//            if (substeps <)
            furthest_failure = result;
            furthest_failure_substeps = substeps;
        }

        public Legend_Result match(Position stone, Rhyme rhyme, Rhyme parent, bool use_global = true)
        {
            var result = rhyme.match(stone, parent);
            if (!result.success)
            {
                if (use_global)
                {
                    var global_result = check_globals(stone);
                    if (global_result != null && global_result.success)
                        result = match(global_result.end, rhyme, parent);
                }
            }
            //            if (result == null)
            //            {
            //            }
            //            else
            //            {
            //                    failure_stack.Pop();
            //            }
            return result;
        }

        public Legend read(string start_name = "start")
        {
            var start = grammar.rhymes[start_name];
            var stone = new Position(code, this);
            var result = match(stone, start, null);

            if (result.success && result.end.index == code.Length)
            {
                return result.legend;
            }

//            var furthest_failure = result.get_endpoint();
            if (furthest_success == null)
            {
                throw new Exception("Could not find match at 1:1.");
            }
            else
            {
                var message = "Could not find match at " + furthest_success.end.get_position_string();
                if (furthest_failure.rhyme != null)
                {
                    if (furthest_failure.rhyme.type == Rhyme_Type.text)
                    {
                        message += "  Expected '" + ((String_Rhyme)furthest_failure.rhyme).pattern + "' but got "
                                   + furthest_failure.start.get_sample();
                    }
                    else if (furthest_failure.rhyme.type == Rhyme_Type.regex)
                    {
                        message += "Expected '" + ((Regex_Rhyme)furthest_failure.rhyme).regex + "' but got "
                                   + furthest_failure.start.get_sample();
                    }
                }

                throw new Exception(message);
            }

        }

        public bool is_latest_failure(Legend_Result result)
        {
            return true;
            return furthest_success != null
                   && result.start.index == furthest_success.end.index;
        }


    }
}
