using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace runic.retreat
{
    public class Legend_Result
    {
        public Legend legend;
        public Position stone;
        public bool store_legend;

        public Legend_Result(Legend legend, Position stone)
        {
            this.legend = legend;
            this.stone = stone;
            store_legend = legend != null;
            if (stone.index > stone.parser.furthest)
            {
                stone.parser.furthest = stone.index;
                stone.parser.furthest_legend_result = this;
            }
        }
    }

}
