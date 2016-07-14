using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using imperative;
using imperative.render.artisan;
using imperative.render.artisan.targets;
using imperative.render.artisan.targets.cpp;
using imperative.summoner;
using imp_test.fixtures;
using NUnit.Framework;
using runic_imp;

namespace imp_test.tests
{
    [TestFixture]
    public class Runic_Imp_Test
    {
        [Test]
        public void browser_test()
        {
            var overlord = new Overlord("cpp");
            var target = (Cpp)overlord.target;
            Lexer_Generator.initialize(overlord);
            var dungeon = Lexer_Generator.generate("Imp_Lexer", Summoner.lexer, overlord, overlord.root);
            var strokes = new List<Stroke> { Source_File.generate_source_file(target,dungeon) };
            var passages = Painter.render_root(strokes).ToList();
            var segments = new List<Segment>();
            var output = Scribe.render(passages, segments);
            var goal = Utility.load_resource("imp_lexer.cpp");
            Utility.diff(goal, output);
        }

    }
}
