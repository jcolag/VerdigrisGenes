// <copyright file="MainClass.cs" company="John Colagioia">
//     John.Colagioia.net. Licensed under the GPLv3
// </copyright>
// <author>John Colagioia</author>
namespace Turwin
{
        using System;
        using System.Collections.Generic;
        using System.IO;
        using Fitness;

        /// <summary>
        /// Main class.
        /// </summary>
        public class MainClass
        {
                /// <summary>
                /// The population size.
                /// </summary>
                private const int population = 10000;

                /// <summary>
                /// The maximum number of loop iterations for the interpreter.
                /// </summary>
                private const int maxiterations = 1000;

                /// <summary>
                /// The maximum number of generations.
                /// </summary>
                private const int maxgenerations = 100;

                /// <summary>
                /// The entry point of the program, where the program control starts and ends.
                /// </summary>
                /// <param name="args">The command-line arguments.</param>
                public static void Main(string[] args)
                {
                        var rng = new Random();
                        var stdin = new List<int>() { 5 };
                        var stdout = new List<int>() { 6 };
                        var fit = new List<FitnessSelector>();
                        double threshold = double.Epsilon;
                        int generation = 0;

                        if (args.Length == 0)
                        {
                                Console.WriteLine("Requires a parameter indicating a grammar file.");
                                return;
                        }

                        var input = File.OpenText(args[0]);
                        var grammartext = input.ReadToEnd();
                        input.Close();

                        do
                        {
                                EvaluateNextGeneration(stdin, stdout, fit, threshold, grammartext);

                                int parents = fit.Count;
                                for (int i = parents; i < MainClass.population - 1000; i++)
                                {
                                        int mother = rng.Next(parents);
                                        int father = rng.Next(parents);
                                        FitnessSelector child = fit[mother].Mate(fit[father]);
                                        fit.Add(child);
                                }

                                threshold = fit[parents - 1].Rating;
                                generation += 1;
                        }
                        while (generation < MainClass.maxgenerations && fit.Count >= 1 && fit[0].Rating < 100);

                        Console.WriteLine("# Best program");
                        Console.WriteLine(fit[0].Program);
                }

                /// <summary>
                /// Evaluates the next generation.
                /// </summary>
                /// <param name="stdin">Input values.</param>
                /// <param name="stdout">Expected output values.</param>
                /// <param name="programs">List of programs.</param>
                /// <param name="threshold">Threshold rating.</param>
                /// <param name="grammartext">Grammar text.</param>
                static void EvaluateNextGeneration(List<int> stdin, List<int> stdout, List<FitnessSelector> programs, double threshold, string grammartext)
                {
                        for (int i = programs.Count; i < MainClass.population; i++)
                        {
                                var f = new FitnessSelector(grammartext, string.Empty, stdin, stdout);
                                f.Execute(maxiterations);
                                f.Evaluate(threshold);
                                programs.Add(f);
                                // bool accept = f.Rating >= threshold;
                                // Console.WriteLine(i.ToString() + " - " + accept.ToString() + ": " + f.Rating.ToString() + "%");
                        }

                        programs.Sort();
                        for (int i = 0; i < programs.Count; i++)
                        {
                                if (programs[i].Rating < threshold)
                                {
                                        programs.RemoveRange(i, programs.Count - i);
                                        break;
                                }
                        }

                        Console.WriteLine("#Runs over threshold of " + threshold.ToString() + "%: "
                                + programs.Count.ToString()
                                + "; best rating is "
                                + programs[0].Rating
                                + "%");
                }
        }
}