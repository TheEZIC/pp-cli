using System;
using System.Collections.Generic;
using System.Linq;
using osu.Game.Rulesets.Objects;
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
        protected override Dictionary<HitResult, int> GenerateHitResult(double accuracy, int countMiss, int countMeh = 0)
        {
            var nObjects = Beatmap.HitObjects.Count;

            var s = nObjects - countMiss - countMeh;
            var countGood = (int)Math.Round(-((accuracy * 6 * nObjects - 6 * s - countMeh) / 4));
            var countGreat = nObjects - countGood - countMeh - countMiss;

            return new Dictionary<HitResult, int>
            {
                { HitResult.Great, countGreat },
                { HitResult.Ok, countGood },
                { HitResult.Meh, countMeh },
                { HitResult.Miss, countMiss }
            };
        }
        protected override double GetAccuracy(Dictionary<HitResult, int> hits)
        {
            var countGreat = hits[HitResult.Great];
            var countGood = hits[HitResult.Ok];
            var countMeh = hits[HitResult.Meh];
            var countMiss = hits[HitResult.Miss];
            var total = countGreat + countGood + countMeh + countMiss;

            return (double)((6 * countGreat) + (2 * countGood) + countMeh) / (6 * total);
        }
    }
}