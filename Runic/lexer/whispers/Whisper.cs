using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace runic.lexer
{
    public enum Whisper_Type
    {
        regex,
        text,
        group
    }

    public abstract class Whisper
    {
        public enum Attribute
        {
            ignore,
            optional,
            tween
        }

        public string name;
        public Attribute[] attributes;
        public Whisper_Type type;
        public List<Whisper> targets;

        protected Whisper(Whisper_Type type, string name)
        {
            this.type = type;
            this.name = name;
        }

        public abstract Rune match(string input, Position position, int max = 0);

        public bool has_attribute(Attribute attribute)
        {
            return attributes != null && attributes.Contains(attribute);
        }

        public void add_target(Whisper target)
        {
            if (targets == null)
                targets = new List<Whisper>();

            targets.Add(target);
        }
    }
}
