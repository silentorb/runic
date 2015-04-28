using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using runic.parser;

namespace runic.lexer
{
    public class Tracker
    {
        [DebuggerDisplay("{debug_info}")]
        public class Entry
        {
            public Rhyme rhyme;
            public Rune rune;
            public bool success;

            public Entry(Rhyme rhyme, Rune rune, bool success)
            {
                this.rhyme = rhyme;
                this.rune = rune;
                this.success = success;
            }

            public string debug_info
            {
                get { return (success ? "* " : "! ") + rhyme.name + " " + rune.text; }
            }
        }

        public Entry furthest_success;
        public Legend_Result furthest_failure;
        public int furthest;
        public List<Entry> history = new List<Entry>();
        public List<Rune> runes;
        public string source;
        public string source_filename;
 
        public void add_entry(bool success, Rhyme rhyme, Rune rune)
        {
            var entry = new Entry(rhyme, rune, success);
            history.Add(entry);
            if (furthest_success == null || rune.index >= furthest_success.rune.index)
            {
                furthest_success = entry;
            }
        }

        public void update_failure(Legend_Result result, int substeps)
        {
            if (furthest_success != null && result.stone.current.index != furthest_success.rune.index)
                return;

            furthest_failure = result;
        }
    }
}
