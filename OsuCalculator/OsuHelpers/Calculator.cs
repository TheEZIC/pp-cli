using System;
using System.Collections.Generic;
using System.Linq;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Difficulty;
using osu.Game.Rulesets.Mods;
using osu.Game.Scoring;

namespace OsuCalculator.OsuHelpers
{
    public class Calculator
    {
        private RulesetFactory _rulesetFactory;
        public Calculator(RulesetFactory rulesetFactory)
        {
            _rulesetFactory = rulesetFactory;
        }
        
        public List<Mod> GetModsFromString(string modsString)
        {
            var mods = modsString.Split(",");
            var avaibleMods = _rulesetFactory.Ruleset.GetAllMods().ToList();

            return mods
                .Select(mod => avaibleMods.FirstOrDefault(m =>
                    string.Equals(m.Acronym, mod, StringComparison.CurrentCultureIgnoreCase)))
                .Where(modClass => modClass != null)
                .ToList();
        }

        public DifficultyAttributes CalculateAttributes(string modsString)
        {
            var mods = GetModsFromString(modsString);
            var diff = _rulesetFactory
                .Ruleset
                .CreateDifficultyCalculator(_rulesetFactory.WorkingBeatmap)
                .Calculate(mods.ToArray());

            return diff;
        }
        
        public double CalculatePP(DifficultyAttributes difficultyAttributes, ScoreInfo score)
        {
            var pp = _rulesetFactory
                .Ruleset
                .CreatePerformanceCalculator(difficultyAttributes, score)
                .Calculate();

            return pp;
        }
        
        public IBeatmap GetBeatmap()
        {
            return _rulesetFactory.WorkingBeatmap.Beatmap;
        }
    }
}