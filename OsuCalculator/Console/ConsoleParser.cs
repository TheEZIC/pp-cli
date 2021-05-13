using System;
using System.IO;
using McMaster.Extensions.CommandLineUtils;
using OsuCalculator.OsuHelpers;

namespace OsuCalculator.Console
{
    public class ConsoleParser
    {
        private CommandLineApplication _app;
        private BeatmapParser _beatmapParser = new BeatmapParser();
        public ConsoleParser(string[] args)
        {
            _app = new CommandLineApplication();
            _app.HelpOption();

            //global options
            var optMode = _app.Option<int>("-m|--mode <MODE>", "Set osu! gamemode", CommandOptionType.SingleValue);
            var optScore = _app.Option<int>("-s|--score <SCORE>", "Set score", CommandOptionType.SingleValue);
            var optAccuracy = _app.Option<float>("-a|--accuracy <ACCURACY>", "Set accuracy", CommandOptionType.SingleValue);
            var optCombo = _app.Option<int>("-c|--combo <COMBO>", "Set combo", CommandOptionType.SingleValue);
            var optCountMiss = _app.Option<int>("-miss|--miss <MISS>", "Set count of misses", CommandOptionType.SingleValue);
            var optMods = _app.Option<string>("-mods|--mods <MODS>", "Set comma divided mods acronyms", CommandOptionType.SingleValue);
            
            optMode.DefaultValue = 0;
            optScore.DefaultValue = 1000000;
            optAccuracy.DefaultValue = 1;
            optCountMiss.DefaultValue = 0;
            optMods.DefaultValue = "";

            //id command
            _app.Command("id", command =>
            {
                command.Description = "Get beatmap from osu API";
                command.HelpOption();

                var argId = command.Argument<int>("id", "Beatmap id").IsRequired();
                
                command.OnExecute(() =>
                {
                    var id =  Int32.Parse(argId.Value);
                    
                    //options values
                    var mode = Int32.Parse(optMode.Value());
                    var score = Int32.Parse(optScore.Value());
                    var accuracy = float.Parse(optAccuracy.Value());
                    var combo = optCombo.HasValue() ? Int32.Parse(optCombo.Value()) : (int?) null;
                    var countMiss = Int32.Parse(optCountMiss.Value());
                    var mods = optMods.Value();

                    var beatmap = _beatmapParser.ParseId(id, mode);
                    var difficultyAttributes = beatmap.Calculator.CalculateAttributes(mods);
                    var scoreBuilder = beatmap.ScoreBuilder;

                    var scoreInfo = scoreBuilder
                        .AddCombo(combo)
                        .AddScore(score)
                        .AddAccuracyAndHits(accuracy, countMiss)
                        .AddMods(mods)
                        .ScoreInfo;
                        
                    var pp = beatmap.Calculator.CalculatePP(difficultyAttributes, scoreInfo);
                    
                    System.Console.WriteLine(pp);
                });
            });
            
            //file command
            _app.Command("file", command =>
            {
                command.Description = "Get beatmap from .osu file";
                command.HelpOption();
                
                var path = command.Argument<string>("path", "Beatmap id").IsRequired();

                command.OnExecute(() =>
                {
                    if (!File.Exists(path.Value))
                        throw new FileNotFoundException(".osu file not found at specified path");
                    
                    //options values
                    var mode = Int32.Parse(optMode.Value());
                    var score = Int32.Parse(optScore.Value());
                    var accuracy = float.Parse(optAccuracy.Value());
                    var combo = optCombo.HasValue() ? Int32.Parse(optCombo.Value()) : (int?) null;
                    var countMiss = Int32.Parse(optCountMiss.Value());
                    var mods = optMods.Value();

                    var beatmap = _beatmapParser.ParseFile(path.Value, mode);
                    var difficultyAttributes = beatmap.Calculator.CalculateAttributes(mods);
                    var scoreBuilder = beatmap.ScoreBuilder;

                    var scoreInfo = scoreBuilder
                        .AddCombo(combo)
                        .AddScore(score)
                        .AddAccuracyAndHits(accuracy, countMiss)
                        .AddMods(mods)
                        .ScoreInfo;
                        
                    var pp = beatmap.Calculator.CalculatePP(difficultyAttributes, scoreInfo);
                    
                    System.Console.WriteLine(pp);
                });
            });

            if (args.Length == 0)
            {
                _app.ShowHelp();
            }
            else
            {
                _app.Execute(args);
            }
        }
    }
}