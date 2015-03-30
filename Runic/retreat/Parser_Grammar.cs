using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using runic.retreat.rhymes;

namespace runic.retreat
{
    class Parser_Grammar:Grammar
    {
        public Parser_Grammar()
        {
            rhymes["start"] = generate();
        }

        public Rhyme generate()
        {
            var id = new Regex_Rhyme("id", @"[\$a-zA-Z0-9_]+");
            var or = new String_Rhyme("or", "|");
            var string_value = new Regex_Rhyme("string", "\"([^\"]*)\"|\\G'([^']*)'");
            var start_rep = new String_Rhyme("start_rep", "@(");
            var end_rep = new String_Rhyme("end_rep", ")");
            var equals = new String_Rhyme("equals", "=");
            var comma = new String_Rhyme("comma", ",");
            var regex = new Regex_Rhyme("regex", @"/([^/]+)/");
            var spaces = new Regex_Rhyme("spaces", @"[ \t]+");
            var newlines = new Regex_Rhyme("newlines", @"(\s*\n)+\s*");

            global_rhymes.Add(spaces);
            global_rhymes.Add(newlines);

            var repetition = new And_Rhyme("repetition", new List<Rhyme>
            {
                new Regex_Rhyme(@"@\("),
                new Repetition_Rhyme(id, comma, 3, 4),
                end_rep
            });

            var option = new Or_Rhyme("option", new List<Rhyme>
            {
                id,
                repetition
            });

            var group = new Or_Rhyme("group", new List<Rhyme>
            {
                new Repetition_Rhyme(option, or, 2, 0) { name = "or" },
                new Repetition_Rhyme(option, spaces, 2, 0) { name = "and" },
                option
            });

            var rule = new And_Rhyme("rule", new List<Rhyme>
            {
                id,
                equals,
                group
            });

            return new Repetition_Rhyme(rule, newlines, 1, 0);
        }
    }
}
