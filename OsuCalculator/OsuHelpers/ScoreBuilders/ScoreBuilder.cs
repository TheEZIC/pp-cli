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
            ScoreInfo.Ruleset = calculator.GetBeatmap().BeatmapInfo.Ruleset;
        }

        public ScoreBuilder AddCombo(int? combo = null)
        {
            combo ??= GetMaxCombo();
            ScoreInfo.MaxCombo = Math.Min(combo.GetValueOrDefault(), GetMaxCombo());
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

        public ScoreBuilder AddAccuracyAndHits(double accuracy, int countMiss = 0, int countMeh = 0)
        {
            var hits = GenerateHitResult(accuracy, countMiss, countMeh);
            ScoreInfo.Accuracy = GetAccuracy(hits);
            ScoreInfo.Statistics = hits;
            return this;
        }

        protected abstract int GetMaxCombo();
        protected abstract Dictionary<HitResult, int> GenerateHitResult(double accuracy, int countMiss, int countMeh = 0);
        protected abstract double GetAccuracy(Dictionary<HitResult, int> hits);
    }
}