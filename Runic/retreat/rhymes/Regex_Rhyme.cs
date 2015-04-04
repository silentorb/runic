using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace runic.retreat.rhymes
{
    public class Regex_Rhyme : Rhyme
    {
        public Regex regex;
        public bool can_be_empty = false;

        public Regex_Rhyme(string pattern)
            : base(Rhyme_Type.regex, pattern)
        {
            regex = new Regex("\\G" + pattern);
        }

        public Regex_Rhyme(string name, string pattern)
            : base(Rhyme_Type.regex, name)
        {
            regex = new Regex("\\G" + pattern);
        }

        public override void initialize(Legend pattern, Loaded_Grammar grammar)
        {

        }

        public override Legend_Result match(Position position, Rhyme parent)
        {
            var match = regex.Match(position.source, position.index);
            if (!match.Success)
                return new Legend_Result(false, position, this);

            var value = match.Groups[match.Groups.Count - 1].Value;
            for (var i = match.Groups.Count - 1; i > 1; --i)
            {
                if (value == "")
                    value = match.Groups[match.Groups.Count - 2].Value;
            }

            if (value == "" && !can_be_empty)
                throw new Exception("Invalid regex: " + name + ".");

            if (value == "null")
            {
                value = value;
            }
            var next = position.forward(match.Length);
            position.parser.add_entry(value, this, position, next);
            return new Legend_Result(true, new String_Legend(this, value, position, next));
        }

        public override IEnumerable<Rhyme> aggregate()
        {
            return new List<Rhyme>();
        }

        protected override List<Rhyme> get_single_type()
        {
            return new List<Rhyme>();
        }
    }
}
