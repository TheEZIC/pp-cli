using System;
using System.Collections.Generic;
using System.Linq;
using osu.Game.Rulesets.Osu.Objects;
using osu.Game.Rulesets.Scoring;

namespace OsuCalculator.OsuHelpers.ScoreBuilders
{
    public class OsuScoreBuilder : ScoreBuilder
    {
        public OsuScoreBuilder(Calculator calculator)
            : base(calculator)
        {
        }
        protected override int GetMaxCombo()
        {
            var hitObjects = Beatmap.HitObjects;
            return hitObjects.Count + hitObjects.OfType<Slider>().Sum(s => s.NestedHitObjects.Count - 1);
        }
        protected override Dictionary<HitResult, int> GenerateHitResult(double accuracy, int countMiss)
        {
            var hitObjects = Beatmap.HitObjects;
            var totalResultCount = hitObjects.Count;
            var targetTotal = (int)Math.Round(accuracy * totalResultCount * 6);
            var delta = targetTotal - (totalResultCount - countMiss);
            
            var great = delta / 5;
            var good = delta % 5;
            var meh = totalResultCount - great - good - countMiss;
            
            return new Dictionary<HitResult, int>
            {
                { HitResult.Great, great },
                { HitResult.Good, good },
                { HitResult.Meh, meh },
                { HitResult.Miss, countMiss },
            };
        }
        protected override double GetAccuracy(Dictionary<HitResult, int> hits)
        {
            var countGreat = hits[HitResult.Great];
            var countGood = hits[HitResult.Good];
            var countMeh = hits[HitResult.Meh];
            var countMiss = hits[HitResult.Miss];
            var total = countGreat + countGood + countMeh + countMiss;

            return (double)((6 * countGreat) + (2 * countGood) + countMeh) / (6 * total);
        }
    }
}