using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace runic.retreat.rhymes
{
    public class Repetition_Rhyme : Rhyme
    {
        public int min; // min < 1 means this pattern is optional
        public int max; // max < 1 is infinite
        public Rhyme rhyme;
        public Rhyme divider;
        private bool has_variable_dividers = false;

        public Repetition_Rhyme(string name)
            : base(Rhyme_Type.repetition, name)
        {

        }

        public Repetition_Rhyme(Rhyme rhyme, Rhyme divider, int min, int max, string name = null)
            : base(Rhyme_Type.repetition, name)
        {
            this.rhyme = rhyme;
            this.divider = divider;
            this.min = min;
            this.max = max;
            has_variable_dividers = !divider.is_ghost;
        }

        public Repetition_Rhyme(Rhyme rhyme, int min, int max)
            : base(Rhyme_Type.repetition, null)
        {
            this.rhyme = rhyme;
            this.min = min;
            this.max = max;
        }

        public override void initialize(Legend pattern, Loaded_Grammar grammar)
        {
            if (pattern.type != "repetition")
                pattern = pattern.children[0];

            var patterns = pattern.children[0].children;
            rhyme = grammar.get_whisper_rhyme(patterns[0].text);
            if (patterns.Count == 3)
            {
                min = int.Parse(patterns[1].text);
                max = int.Parse(patterns[2].text);
            }
            else
            {
                divider = grammar.get_whisper_rhyme(patterns[1].text);
                min = int.Parse(patterns[2].text);
                max = int.Parse(patterns[3].text);
                //                has_variable_dividers = divider.aggregate().OfType<Or_Rhyme>().Any();
                has_variable_dividers = !divider.is_ghost;
            }
        }

        public override Legend_Result match(Position stone, Rhyme parent)
        {
            var original_stone = stone;
            var matches = new List<Legend>();
            var dividers = new List<Legend>();
            Legend last_divider = null;
            var final_stone = stone;
            var track_dividers = divider != null && has_variable_dividers;
            int match_count = -1;
            Legend_Result main_result;

            do
            {
                ++match_count;

                if (rhyme.name == "class_attribute")
                {
                    var x = 0;
                }

                main_result = stone.parser.match(stone, rhyme, this);
                if (!main_result.success)
                    break;

                matches.Add(main_result.legend);
                stone = final_stone = main_result.end;
                if (track_dividers && last_divider != null)
                    dividers.Add(last_divider);

                if (divider != null)
                {
                    var divider_result = stone.parser.match(stone, divider, this);

                    if (!divider_result.success)
                        break;

                    last_divider = divider_result.legend;
                    stone = divider_result.end;
                }
            }
            while (max == 0 || matches.Count < max);

            if (matches.Count < min)
            {
                if (matches.Count > 0)
                    stone.parser.update_failure(stone, this, matches.Count);

                return new Legend_Result(false, original_stone, this, matches.Count, main_result);
            }
            stone.parser.add_entry(null, this, original_stone, stone);

            // The equivalent of ? in a regex
            if (max == 1 && min == 0)
            {
                if (matches.Count == 1)
                    return new Legend_Result(true, matches[0]);

                return new Legend_Result(true, original_stone, this) { store_legend = true };
            }

            return new Legend_Result(true, new Group_Legend(this, matches, original_stone, final_stone, dividers));
        }

        override public IEnumerable<Rhyme> aggregate()
        {
            return divider != null
                ? new[] { rhyme, divider }
                : new[] { rhyme };
        }

        public override string debug_info
        {
            get { return "rep " + (name ?? rhyme.name); }
        }

        protected override List<Rhyme> get_single_type()
        {
            return rhyme.vertical_return_types;
        }

        public override Rhyme type_rhyme
        {
            get { return rhyme; }
        }
    }
}
