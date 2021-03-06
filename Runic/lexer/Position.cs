﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace runic.lexer
{
    public class Position
    {
        public int x { get; private set; }
        public int y { get; private set; }
        public int index { get; private set; }

        // Make sure this is not modified or there will be some serious performance hits with large code strings.
        public Meadow meadow;
        public static int tab_length = 4;
        public Position previous;

        public Position(Meadow meadow)
        {
            x = 1;
            y = 1;
            this.meadow = meadow;
        }

        private Position clone()
        {
            return new Position(meadow)
            {
                x = x,
                y = y,
                index = index
            };
        }

        public Position forward(int steps)
        {
            var result = clone()._forward(steps);
            result.previous = this;
            return result;
        }

        private Position _forward(int steps)
        {
            var end = index + steps;
            while (index < end)
            {
                var c = meadow.content[index];
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
    }
}
