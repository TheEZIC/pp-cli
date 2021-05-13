using System;
using System.IO;
using System.Net;
using osu.Framework.Audio;
using osu.Framework.Audio.Track;
using osu.Framework.Graphics.Textures;
using osu.Game.Beatmaps;
using osu.Game.Beatmaps.Formats;
using osu.Game.IO;
using osu.Game.Rulesets;
using osu.Game.Rulesets.Osu.Beatmaps;
using osu.Game.Tests.Beatmaps;
using OsuCalculator.OsuHelpers.ScoreBuilders;

namespace OsuCalculator.OsuHelpers
{
    public class BeatmapParser
    {
        public BeatmapParserResult ParseId(int id, int mode = 0)
        {
            try
            {
                var stream = new WebClient().OpenRead(@$"https://osu.ppy.sh/osu/{id}");
                var reader = new LineBufferedReader(stream);
                
                return CreateResult(reader, mode);
            }
            catch (Exception e)
            {
                System.Console.WriteLine(e);
                throw;
            }
        }

        public BeatmapParserResult ParseFile(string path, int mode = 0)
        {
            try
            {
                var stream = File.OpenRead(path);
                var reader = new LineBufferedReader(stream);

                return CreateResult(reader, mode);
            }
            catch (Exception e)
            {
                System.Console.WriteLine(e);
                throw;
            }
        }

        private BeatmapParserResult CreateResult(LineBufferedReader reader, int mode = 0)
        {
            var beatmap = Decoder.GetDecoder<Beatmap>(reader).Decode(reader);
            var rulesetFactory = new RulesetFactory(beatmap, mode);
            var calculator = new Calculator(rulesetFactory);
            var scoreBuilder = new ScoreBuilderFactory().GetScoreBuilder(calculator);
            
            return new BeatmapParserResult()
            {
                Ruleset = rulesetFactory.Ruleset,
                WorkingBeatmap = rulesetFactory.WorkingBeatmap,
                Calculator = calculator,
                ScoreBuilder = scoreBuilder,
            };
        }

        public class BeatmapParserResult
        {
            public Ruleset Ruleset { get; set; }
            public WorkingBeatmap WorkingBeatmap { get; set; }
            public Calculator Calculator { get; set; }
            public ScoreBuilder ScoreBuilder { get; set; }
        }
    }
}