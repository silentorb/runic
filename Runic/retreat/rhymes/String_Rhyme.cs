using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace runic.retreat.rhymes
{
    public class String_Rhyme : Rhyme
    {
        public string pattern;

        public String_Rhyme(string pattern)
            : base(Rhyme_Type.text, pattern)
        {
            this.pattern = pattern;
        }

        public String_Rhyme(string name, string pattern)
            : base(Rhyme_Type.text, name)
        {
            this.pattern = pattern;
        }

        public override void initialize(Legend pattern, Loaded_Grammar grammar)
        {
            
        }

        public override Legend_Result match(Position position, Rhyme parent)
        {
            var slice = Parser.get_safe_substring(position.source, position.index, pattern.Length);
            if (slice == pattern)
            {
                var next = position.forward(pattern.Length);
                position.parser.add_entry(pattern, this, position, next);
                return new Legend_Result(new String_Legend(this, pattern), next);
            }
            return null;
        }

        public override IEnumerable<Rhyme> aggregate()
        {
            return new List<Rhyme>();
        }

        protected override List<Rhyme> get_single_type()
        {
            return new List<Rhyme>();
        }

        public override bool is_ghost
        {
            get { return true; }
        }
    }
}
