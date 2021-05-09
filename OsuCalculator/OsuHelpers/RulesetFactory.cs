using System;
using osu.Game.Beatmaps;
using osu.Game.Rulesets;
using osu.Game.Rulesets.Catch;
using osu.Game.Rulesets.Mania;
using osu.Game.Rulesets.Osu;
using osu.Game.Rulesets.Taiko;
using osu.Game.Tests.Beatmaps;

namespace OsuCalculator.OsuHelpers
{
    public class RulesetFactory
    {
        public Ruleset Ruleset { get; }
        public WorkingBeatmap WorkingBeatmap { get; }
        public RulesetFactory(WorkingBeatmap workingBeatmap, int modeNumber = 0)
        {
            var rulesetId = workingBeatmap.BeatmapInfo.RulesetID;
            Ruleset = GetRuleset(rulesetId);
            workingBeatmap.BeatmapInfo.Ruleset = Ruleset.RulesetInfo;
            var playableBeatmap = workingBeatmap.GetPlayableBeatmap(Ruleset.RulesetInfo);

            //Convert beatmmaps
            if (rulesetId == 0 && modeNumber != rulesetId)
            {
                Ruleset = GetRuleset(modeNumber);
                rulesetId = modeNumber;
                playableBeatmap = Ruleset.CreateBeatmapConverter(playableBeatmap).Convert();
                workingBeatmap = new TestWorkingBeatmap(playableBeatmap);
                workingBeatmap.BeatmapInfo.Ruleset = Ruleset.RulesetInfo;
            }

            WorkingBeatmap = workingBeatmap;
        }

        private Ruleset GetRuleset(int modeNumber)
        {
            return modeNumber switch
            {
                0 => new OsuRuleset(),
                1 => new TaikoRuleset(),
                2 => new CatchRuleset(),
                3 => new ManiaRuleset(),
                _ => throw new Exception("Invalid mode")
            };
        }
    }
}