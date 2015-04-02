using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace runic.retreat.rhymes
{
    public class Or_Rhyme : Rhyme
    {
        public List<Rhyme> rhymes;

        public Or_Rhyme(string name)
            : base(Rhyme_Type.or, name)
        {

        }

        public Or_Rhyme(string name, List<Rhyme> rhymes)
            : base(Rhyme_Type.or, name)
        {
            this.rhymes = rhymes;
        }

        public override void initialize(Legend pattern, Loaded_Grammar grammar)
        {
            rhymes = pattern.children.Select(p => grammar.create_child(p)).ToList();
        }

        public override Legend_Result match(Position stone, Rhyme parent)
        {
            Legend_Result failure = null;
            foreach (var rhyme in rhymes)
            {
                var result = rhyme.match(stone, this);
                if (result.success)
                {
                    stone.parser.add_entry(null, this, stone, result.end);
                    return result;
                }
                if (failure == null || result.steps >= failure.steps)
                    failure = result;
            }

            return new Legend_Result(false, stone, this, 0, failure);
        }

        override public IEnumerable<Rhyme> aggregate()
        {
            return rhymes;
        }

        protected override List<Rhyme> get_single_type()
        {
            var result = rhymes[0].vertical_return_types;
            result = rhymes.Skip(1).Aggregate(result, 
                (current, rhyme) => rhyme.vertical_return_types.Where(current.Contains).ToList());

            if (!result.Contains(this))
                result.Add(this);

            return result;
        }
    }
}
