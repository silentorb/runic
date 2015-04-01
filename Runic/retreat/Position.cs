using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace runic.retreat
{
    [DebuggerDisplay("{debug_string}")]
    public class Position
    {
        public int x { get; private set; }
        public int y { get; private set; }
        public int index { get; private set; }
        public Parser parser;

        // Make sure this is not modified or there will be some serious performance hits with large code strings.
        public string source { get; private set; }
        public static int tab_length = 4;

        public string debug_string
        {
            get { return index + " " + get_sample(); }
        }

        public Position(string source, Parser parser)
        {
            x = 1;
            y = 1;
            this.source = source;
            this.parser = parser;
        }

        private Position clone()
        {
            return new Position(source, parser)
                {
                    x = x,
                    y = y,
                    index = index
                };
        }

        public Position forward(int steps)
        {
            return clone()._forward(steps);
        }

        private Position _forward(int steps)
        {
            var end = index + steps;
            while (index < end)
            {
                var c = source[index];
                if (c == '\n')
                {
                    ++y;
                    x = 1;
                }
                else if (c == '\t')
                {
                    x += tab_length;
                }
                else
                {
                    ++x;
                }
                ++index;
            }

            return this;
        }

        public string get_position_string()
        {
            return y + ":" + x;
        }

        public string get_sample(int length = 10)
        {
            return Parser.get_safe_substring(source, index, length);
        }
    }
}
