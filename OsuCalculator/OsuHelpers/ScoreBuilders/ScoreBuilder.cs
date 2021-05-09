using System;
using System.Collections.Generic;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Scoring;
using osu.Game.Scoring;

namespace OsuCalculator.OsuHelpers.ScoreBuilders
{
    public abstract class ScoreBuilder
    {
        protected Calculator Calculator;
        protected IBeatmap Beatmap;
        public ScoreInfo ScoreInfo { get; } = new ScoreInfo();

        protected ScoreBuilder(Calculator calculator)
        {
            Calculator = calculator;
            Beatmap = calculator.GetBeatmap();
        }

        public ScoreBuilder AddCombo(int combo = -1)
        {
            if (combo == -1)
                combo = GetMaxCombo();
            
            ScoreInfo.MaxCombo = Math.Min(combo, GetMaxCombo());;

            return this;
        }

        public ScoreBuilder AddScore(int score)
        {
            ScoreInfo.TotalScore = score;
            return this;
        }

        public ScoreBuilder AddMods(string modsString = "")
        {
            ScoreInfo.Mods = Calculator.GetModsFromString(modsString).ToArray();
            return this;
        }

        public ScoreBuilder AddAccuracyAndHits(double accuracy, int countMiss = 0)
        {
            var hits = GenerateHitResult(accuracy, countMiss);
            ScoreInfo.Accuracy = GetAccuracy(hits);
            ScoreInfo.Statistics = hits;
            return this;
        }

        protected abstract int GetMaxCombo();
        protected abstract Dictionary<HitResult, int> GenerateHitResult(double accuracy, int countMiss);
        protected abstract double GetAccuracy(Dictionary<HitResult, int> hits);
    }
}