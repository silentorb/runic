using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using runic.retreat.rhymes;

namespace runic.retreat
{
    public abstract class Grammar
    {
        public Dictionary<string, Rhyme> rhymes = new Dictionary<string, Rhyme>();
        public List<Rhyme> global_rhymes = new List<Rhyme>();

    }
}
