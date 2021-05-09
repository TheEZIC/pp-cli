using System;
using OsuCalculator.OsuHelpers.ScoreBuilders;

namespace OsuCalculator.OsuHelpers
{
    public class ScoreBuilderFactory
    {
        public ScoreBuilder GetScoreBuilder(Calculator calculator)
        {
            var modeNumber = calculator.GetBeatmap().BeatmapInfo.RulesetID;
            
            return modeNumber switch
            {
                0 => new OsuScoreBuilder(calculator),
                1 => new TaikoScoreBuilder(calculator),
                2 => new CatchScoreBuilder(calculator),
                3 => new ManiaScoreBuilder(calculator),
                _ => throw new Exception("Invalid mode")
            };
        }
    }
}