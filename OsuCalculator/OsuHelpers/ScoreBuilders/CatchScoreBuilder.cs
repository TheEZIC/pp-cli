using System;
using System.Collections.Generic;
using System.Linq;
using osu.Game.Rulesets.Catch.Objects;
using osu.Game.Rulesets.Scoring;

namespace OsuCalculator.OsuHelpers.ScoreBuilders
{
    public class CatchScoreBuilder : ScoreBuilder
    {
        public CatchScoreBuilder(Calculator calculator)
            : base(calculator)
        {
        }
        protected override int GetMaxCombo()
        {
            var hitObjects = Beatmap.HitObjects;
            
            return hitObjects.Count 
                + hitObjects
                    .OfType<JuiceStream>()
                    .Sum(s => s.NestedHitObjects.Count - s.NestedHitObjects.OfType<TinyDroplet>().Count() - 1) 
                - hitObjects
                    .OfType<BananaShower>()
                    .Count();
        }
        protected override Dictionary<HitResult, int> GenerateHitResult(double accuracy, int countMiss, int countMeh = 0)
        {
            var combo = GetMaxCombo();
            var fruitsHit = combo - countMiss;
            var tinyTickMiss = fruitsHit / accuracy - countMiss - fruitsHit;

            return new Dictionary<HitResult, int>
            {
                { HitResult.Great, fruitsHit },
                { HitResult.SmallTickMiss, Convert.ToInt32(tinyTickMiss) },
                { HitResult.Miss, countMiss }
            };
        }
        protected override double GetAccuracy(Dictionary<HitResult, int> hits) => 0;
    }
}