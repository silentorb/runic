using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using runic.retreat.rhymes;

namespace runic.retreat
{
    public class Legend_Result
    {
        public Legend legend;
        public Position start;
        public bool store_legend;
        public bool success;
        public string stack_trace;
        public Legend_Result child;
        public Rhyme rhyme;
        public int steps;

        public Position end
        {
            get { return legend != null ? legend.end : start; }
        }

        public Legend_Result(bool success, Legend legend)
        {
            stack_trace = Environment.StackTrace;
            this.legend = legend;
            store_legend = legend != null;
            this.success = success;
            start = legend.start;
        }

        public Legend_Result(bool success, Position start, Rhyme rhyme, int steps = 0, Legend_Result child = null)
        {
            stack_trace = Environment.StackTrace;
            this.success = success;
            this.start = start;
            this.child = child;
            this.rhyme = rhyme;
            this.steps = steps;
        }

        public Legend_Result get_endpoint()
        {
            return child != null
                ? child.get_endpoint()
                : this;
        }
    }

}
