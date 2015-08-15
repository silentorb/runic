using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using imperative;
using imperative.expressions;
using imperative.schema;
using runic.lexer;

namespace runic_imp
{
    public static class Lexer_Generator
    {
        public static Dungeon lexer_dungeon;
        static Dungeon whisper_dungeon;
        static Dungeon string_whisper_dungeon;
        static Dungeon regex_whisper_dungeon;
        static Dungeon whisper_group_dungeon;
        public static Dictionary<Whisper_Type, Dungeon> whisper_types = new Dictionary<Whisper_Type, Dungeon>();

        public static void initialize(Overlord overlord)
        {
            var code = Utility.load_resource("lexer.imp");
            overlord.summon(code, "lexer.imp", true);

            var realm = overlord.root.dungeons["runic"].dungeons["lexer"];

            lexer_dungeon = realm.dungeons["Lexer"];
            whisper_dungeon = realm.dungeons["Whisper"];
            string_whisper_dungeon = realm.dungeons["String_Whisper"];
            regex_whisper_dungeon = realm.dungeons["Regex_Whisper"];
            whisper_group_dungeon = realm.dungeons["Whisper_Group"];

            whisper_types[Whisper_Type.text] = realm.dungeons["String_Whisper"];
            whisper_types[Whisper_Type.regex] = realm.dungeons["Regex_Whisper"];
            whisper_types[Whisper_Type.group] = realm.dungeons["Whisper_Group"];
        }

        public static Dungeon generate(string name, Lexer lexer, Overlord overlord, Dungeon realm)
        {
            var dungeon = new Dungeon(name, overlord, realm, lexer_dungeon);
            var constructor = new Minion("constructor", dungeon);
            constructor.parameters = new List<Parameter>();
            dungeon.minions.Add("constructor", constructor);

            foreach (var whisper in lexer.whispers.Values)
            {
                if (whisper.parent == null || whisper.type == Whisper_Type.@group)
                    convert_whisper(whisper, dungeon);
            }

            return dungeon;
        }

        public static void convert_whisper(Whisper whisper, Dungeon dungeon)
        {
            var constructor = dungeon.minions["constructor"];
            var add_minion = dungeon.summon_minion("add_whisper", true);

            var type = whisper_types[whisper.type];
            //            var portal = new Portal(whisper.name, new Profession(type), dungeon);
            //            dungeon.add_portal(portal);
            if (whisper.type == Whisper_Type.@group)
            {
                var list_profession = Profession.create(Professions.List, true,
                    new List<Profession> { Profession.create(whisper_dungeon) });

                constructor.expressions.Add(new Method_Call(add_minion, null,
                   new Instantiate(type, new Expression[]
                {
                    new Literal(whisper.name),
                    new Instantiate(list_profession,((Whisper_Group)whisper).whispers
                        .Select(w => new_or_existing_whisper(w, dungeon)))
                })));
            }
            else
            {
                var addition = new Method_Call(add_minion, null, create_whisper_instantiation(whisper));
                constructor.expressions.Add(addition);
            }
        }

        static Expression new_or_existing_whisper(Whisper whisper, Dungeon dungeon)
        {
            return whisper.type == Whisper_Type.@group
                ? new Portal_Expression(dungeon.all_portals["whispers"]) { index = new Literal(whisper.name) }
                : create_whisper_instantiation(whisper);
        }

        static Expression create_whisper_instantiation(Whisper whisper)
        {
            var pattern = whisper.type == Whisper_Type.text
            ? ((String_Whisper)whisper).text
            : ((Regex_Whisper)whisper).regex.ToString();

            var type = whisper_types[whisper.type];
            return new Instantiate(type, new Expression[]
                {
                    new Literal(whisper.name),
                    new Literal(pattern)
                });
        }
    }
}
