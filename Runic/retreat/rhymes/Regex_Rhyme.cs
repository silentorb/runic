﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace runic.retreat.rhymes
{
    public class Regex_Rhyme : Rhyme
    {
        public Regex regex;

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
            throw new NotImplementedException();
        }

        public override Legend_Result match(Position position, Rhyme parent)
        {
            var match = regex.Match(position.source, position.index);
            if (!match.Success)
                return null;

            var value = match.Groups[match.Groups.Count - 1].Value;
            for (var i = match.Groups.Count - 1; i > 1; --i)
            {
                if (value == "")
            {
                value = match.Groups[match.Groups.Count - 2].Value;
            }
            }
            
            if (value == "")
                throw new Exception("Invalid regex: " + name + ".");

            return new Legend_Result(new String_Legend(this, value), position.forward(match.Length));
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
