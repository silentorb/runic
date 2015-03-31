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
            : base(Rhyme_Type.regex, pattern)
        {
            this.pattern = pattern;
        }

        public String_Rhyme(string name, string pattern)
            : base(Rhyme_Type.regex, name)
        {
            this.pattern = pattern;
        }

        public override void initialize(Legend pattern, Loaded_Grammar grammar)
        {
            throw new NotImplementedException();
        }

        public override Legend_Result match(Position position, Rhyme parent)
        {
            var slice = Parser.get_safe_substring(position.source, position.index, pattern.Length);
            if (slice == pattern)
                return new Legend_Result(new String_Legend(this, pattern), position.forward(pattern.Length));

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
