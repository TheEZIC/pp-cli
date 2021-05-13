using System;
using System.IO;
using osu.Framework.Audio.Track;
using osu.Framework.Graphics.Textures;
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
        public RulesetFactory(Beatmap beatmap, int modeNumber = 0)
        {
            var workingBeatmap = new TestWorkingBeatmap(beatmap);
            var rulesetId = workingBeatmap.BeatmapInfo.RulesetID;
            
            Ruleset = GetRuleset(workingBeatmap.BeatmapInfo.RulesetID);
            workingBeatmap.BeatmapInfo.Ruleset = Ruleset.RulesetInfo;
            
            var playableBeatmap = workingBeatmap.GetPlayableBeatmap(Ruleset.RulesetInfo);

            //Convert beatmaps
            if (rulesetId == 0 && modeNumber != rulesetId)
            {
                Ruleset = GetRuleset(modeNumber);
                playableBeatmap = Ruleset.CreateBeatmapConverter(playableBeatmap).Convert();
            }
            
            workingBeatmap = new TestWorkingBeatmap(playableBeatmap);
            workingBeatmap.BeatmapInfo.Ruleset = Ruleset.RulesetInfo;
            
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