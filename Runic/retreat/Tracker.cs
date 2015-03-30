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
        public int index;
        public Parser parser;
        public string value;

        public Entry(Rhyme rhyme, int index, string value, Parser parser)
        {
            this.rhyme = rhyme;
            this.index = index;
            this.value = value;
            this.parser = parser;
        }

        public string debug_info
        {
            get
            {
                return value != null
                    ? rhyme.name + " " + value
                    : "No match";
            }
        }
    }
}
