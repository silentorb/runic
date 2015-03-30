﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace runic.retreat.rhymes
{
    public class And_Rhyme : Rhyme
    {
        public List<Rhyme> rhymes;
        private Rhyme single_non_ghost;

        public And_Rhyme(string name)
            : base(Rhyme_Type.and, name)
        {

        }

        internal And_Rhyme(string name, List<Rhyme> rhymes)
            : base(Rhyme_Type.and, name)
        {
            this.rhymes = rhymes;
            if (rhymes.Count(r => !r.is_ghost) == 1)
                single_non_ghost = rhymes.First(r => !r.is_ghost);
        }

        public override void initialize(Legend pattern, Loaded_Grammar grammar)
        {
            rhymes = pattern.children.Select(grammar.create_child).ToList();

            if (rhymes.Count(r => !r.is_ghost) == 1)
                single_non_ghost = rhymes.First(r => !r.is_ghost);
        }

        public override Legend_Result match(Position stone, Rhyme parent)
        {
            var results = new List<Legend>();
            int match_count = 0;
            foreach (var rhyme in rhymes)
            {
                var result = rhyme.match(stone, this);
                if (result == null)
                {
                    if (match_count == 0)
                        return null;

                    result = stone.parser.check_globals(stone);
                    if (result == null)
                        return null;

                    stone = result.stone;
                    result = rhyme.match(stone, this);
                    if (result == null)
                        return null;
                }

                if (result.store_legend && !rhyme.is_ghost)
                    results.Add(result.legend);

                ++match_count;
                stone = result.stone;
            }

            var legend = should_return_single(results, parent)
                ? results[0]
                : new Group_Legend(this, results);

            return new Legend_Result(legend, stone);
        }

        private static bool should_return_single(List<Legend> results, Rhyme parent)
        {
            if (results.Count != 1)
                return false;

            if (parent == null)
                return true;

            if (parent.type != Rhyme_Type.or)
                return true;

            return results[0] != null && parent.returns(results[0].rhyme.type_rhyme);
        }

        override public IEnumerable<Rhyme> aggregate()
        {
            return rhymes;
        }

        protected override List<Rhyme> get_single_type()
        {
            var result = new List<Rhyme> { this };
            var considered = rhymes.Where(r => !r.is_ghost).ToArray();
            if (considered.Count() != 1)
                return result;

            var types = considered.First().vertical_return_types;

            foreach (var rhyme in types)
            {
                if (!result.Contains(rhyme))
                    result.Add(rhyme);
            }

            return result;
        }
    }
}
