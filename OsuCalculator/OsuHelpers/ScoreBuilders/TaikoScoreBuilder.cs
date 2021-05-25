using System;
using System.Collections.Generic;
using System.Linq;
using osu.Game.Rulesets.Scoring;
using osu.Game.Rulesets.Taiko.Objects;

namespace OsuCalculator.OsuHelpers.ScoreBuilders
{
    public class TaikoScoreBuilder : ScoreBuilder
    {
        public TaikoScoreBuilder(Calculator calculator)
            : base(calculator)
        {
        }

        protected override int GetMaxCombo()
        {
            var hitObjects = Beatmap.HitObjects;
            return hitObjects.OfType<Hit>().Count();
        }

        protected override Dictionary<HitResult, int> GenerateHitResult(double accuracy, int countMiss, int countMeh = 0)
        {
            var totalResultCount = GetMaxCombo();
            var targetTotal = (int)Math.Round(accuracy * totalResultCount * 2);

            var great = targetTotal - (totalResultCount - countMiss);
            var good = totalResultCount - great - countMiss;

            return new Dictionary<HitResult, int>
            {
                { HitResult.Great, great },
                { HitResult.Good, good },
                { HitResult.Meh, 0 },
                { HitResult.Miss, countMiss }
            };
        }

        protected override double GetAccuracy(Dictionary<HitResult, int> hits)
        {
            var countGreat = hits[HitResult.Great];
            var countGood = hits[HitResult.Good];
            var countMiss = hits[HitResult.Miss];
            var total = countGreat + countGood + countMiss;

            return (double)((2 * countGreat) + countGood) / (2 * total);
        }
    }
}