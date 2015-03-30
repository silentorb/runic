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
        public int start;
        public int end;
        public Parser parser;
        public string value;

        public Entry(Rhyme rhyme, int start, int end, string value, Parser parser)
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
                var suffix = value != null
                    ? " " + value
                    : " NO MATCH (" + Parser.get_safe_substring(parser.code, start, 10) + ")";

                return start + "-" + end + " " + rhyme.name + " " + suffix;
            }
        }
    }
}
