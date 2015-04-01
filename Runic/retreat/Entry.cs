using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using runic.retreat.rhymes;

namespace runic.retreat
{
    [DebuggerDisplay("{debug_info}")]
    public class Entry
    {
        public Rhyme rhyme;
        public Position start;
        public Position end;
        public Parser parser;
        public string value;

        public Entry(Rhyme rhyme, Position start, Position end, string value, Parser parser)
        {
            this.rhyme = rhyme;
            this.start = start;
            this.end = end;
            this.value = value;
            this.parser = parser;
        }

        public string debug_info
        {
            get
            {
                var suffix = " " + value;
//                var suffix = value != null
//                    ? " " + value
//                    : " NO MATCH (" + Parser.get_safe_substring(parser.code, start.index, 10) + ")";

                return rhyme.type + " " + start.get_position_string() + "-" + end.get_position_string() + " " + rhyme.name + " " + suffix;
            }
        }
    }
}
