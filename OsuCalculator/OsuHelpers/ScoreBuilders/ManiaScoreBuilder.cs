using System.Collections.Generic;
using osu.Game.Rulesets.Scoring;

namespace OsuCalculator.OsuHelpers.ScoreBuilders
{
    public class ManiaScoreBuilder : ScoreBuilder
    {
        public ManiaScoreBuilder(Calculator calculator)
            : base(calculator)
        {
        }
        protected override int GetMaxCombo() => 0;
        protected override Dictionary<HitResult, int> GenerateHitResult(double accuracy, int countMiss)
        {
            var totalHits = Beatmap.HitObjects.Count;

            return new Dictionary<HitResult, int>
            {
                { HitResult.Perfect, totalHits },
                { HitResult.Great, 0 },
                { HitResult.Ok, 0 },
                { HitResult.Good, 0 },
                { HitResult.Meh, 0 },
                { HitResult.Miss, 0 }
            };
        }
        protected override double GetAccuracy(Dictionary<HitResult, int> hits) => 0;
    }
}