using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace runic.lexer
{
    public class Whisper_Group : Whisper
    {
        public Whisper[] whispers;

        public Whisper_Group(string name)
            : base(Whisper_Type.group, name)
        {
        }

        public Whisper_Group(string name, IEnumerable<Whisper> whispers)
            : base(Whisper_Type.group, name)
        {
            this.whispers = whispers.ToArray();
            foreach (var whisper in this.whispers)
            {
                whisper.parent = this;
            }
        }

        public override Rune match(string input, Position position, int max = 0)
        {
            foreach (var whisper in whispers)
            {
                var rune = whisper.match(input, position);
                if (rune != null)
                    return rune;
            }

            return null;
        }
    }
}
