using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace runic.lexer
{
    public class Regex_Whisper : Whisper
    {
        public Regex regex;

        public Regex_Whisper(string name, string pattern)
            : base(Whisper_Type.regex, name)
        {
            regex = new Regex("\\G" + pattern);
        }

        public override Rune match(string input, Position position, int max = 0)
        {
            var match = regex.Match(input, position.index);
            if (!match.Success)
                return null;

//            var value = match.Groups[match.Groups.Count - 1].Value;
//            if (value == "")
//            {
//                if (match.Groups.Count == 2)
//                    throw new Exception("Invalid regex");
//
//                value = match.Groups[match.Groups.Count - 2].Value;
//            }
            var value = match.Groups[match.Groups.Count - 1].Value;
            for (var i = match.Groups.Count - 1; i > 1; --i)
            {
                if (value == "")
                    value = match.Groups[match.Groups.Count - 2].Value;
            }

            if (value == "")
                throw new Exception("Invalid regex: " + name + ".");

            if (targets != null)
            {
//                var child_position = new Position(value);
                foreach (var target in targets)
                {
                    var child_match = target.match(input, position, value.Length);
                    if (child_match != null && child_match.length == value.Length)
                    {
                        position.forward(value.Length);
                        return child_match;
                    }
                }
            }

            return new Rune(this, value, position, position.forward(match.Length));
        }
    }
}
