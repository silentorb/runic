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

        public override void initialize(Legend pattern, Parser parser)
        {
            throw new NotImplementedException();
        }

        public override Legend_Result match(Position position, Rhyme parent)
        {
            var slice = Parser.get_safe_substring(position.source, position.index, pattern.Length);
            return slice == pattern
                ? new Legend_Result(new String_Legend(this, pattern), position)
                : null;
        }

        public override IEnumerable<Rhyme> aggregate()
        {
            throw new NotImplementedException();
        }

        protected override List<Rhyme> get_single_type()
        {
            throw new NotImplementedException();
        }
    }
}
