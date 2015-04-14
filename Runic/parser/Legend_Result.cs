using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using runic.lexer;

namespace runic.parser
{
    public class Legend_Result
    {
        public Legend legend;
        public Runestone stone;
        public bool store_legend;
        public bool success;

        public Rhyme rhyme;
        public int steps;
        public Legend_Result child;

        public Legend_Result(Legend legend, Runestone stone)
        {
            this.legend = legend;
            this.stone = stone;
            store_legend = legend != null;
            success = true;
        }

        public Runestone next()
        {
            return stone.next();
        }

        public static Legend_Result failure(Rhyme rhyme, Runestone stone, Legend_Result child, int steps)
        {
            return new Legend_Result(null, stone)
                {
                    rhyme = rhyme,
                    steps = steps,
                    child = child,
                    success = false
                };
        }

        public Legend_Result get_endpoint()
        {
            return child != null
                ? child.get_endpoint()
                : this;
        }
    }

}
