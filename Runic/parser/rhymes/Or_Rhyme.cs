using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using runic.lexer;

namespace runic.parser.rhymes
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

        public override void initialize(Legend pattern, Parser parser)
        {
            rhymes = pattern.children.Select(parser.create_child).ToList();
        }

        public override Legend_Result match(Runestone stone, Rhyme parent)
        {
            Legend_Result failure = null;
            foreach (var rhyme in rhymes)
            {
                var result = rhyme.match(stone, this);
                if (result.success)
                    return result;

                if (failure == null || result.steps >= failure.steps)
                    failure = result;
            }

            stone.tracker.update_failure(failure, 0);
            return Legend_Result.failure(this, stone, failure, 0);
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
