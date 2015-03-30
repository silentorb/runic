using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace runic.retreat
{
    public class Position
    {
        public int x { get; private set; }
        public int y { get; private set; }
        public int index { get; private set; }
        public Parser parser;

        // Make sure this is not modified or there will be some serious performance hits with large code strings.
        public string source { get; private set; }
        public static int tab_length = 4;

        public Position(string source, Parser parser)
        {
            x = 1;
            y = 1;
            this.source = source;
            this.parser = parser;
        }

        public Position clone()
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
            var end = index + steps;
            while (index < end)
            {
                var c = source[index];
                if (c == '\n')
                {
                    ++y;
                    x = 0;
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
    }
}
