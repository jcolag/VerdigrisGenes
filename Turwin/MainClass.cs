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
                private const int maxiterations = 1;

                /// <summary>
                /// The entry point of the program, where the program control starts and ends.
                /// </summary>
                /// <param name="args">The command-line arguments.</param>
                public static void Main(string[] args)
                {
                        var stdin = new List<int>() { 720 };
                        var stdout = new List<int>() { 2, 2, 2, 2, 3, 3, 5 };
                        var fit = new List<FitnessSelector>();
                        string nl = Environment.NewLine;
                        string genometext = string.Empty;
                        double threshold = double.Epsilon;

                        if (args.Length == 0)
                        {
                                Console.WriteLine("Requires a parameter indicating a grammar file and a genome file.");
                                return;
                        }

                        var input = File.OpenText(args[0]);
                        var grammartext = input.ReadToEnd();
                        input.Close();

                        if (args.Length > 1)
                        {
                                input = File.OpenText(args[1]);
                                genometext = input.ReadToEnd();
                                input.Close();
                        }

                        for (int i = 0; i < MainClass.population; i++)
                        {
                                var f = new FitnessSelector(grammartext, genometext, stdin, stdout);
                                genometext = string.Empty;
                                f.Execute(maxiterations);
                                f.Evaluate(threshold);
                                fit.Add(f);
                                bool accept = f.Rating >= threshold;
                                Console.WriteLine(i.ToString() + " - " + accept.ToString() + ": " + f.Rating.ToString() + "%");
                        }

                        fit.Sort();
                        for (int i = 0; i < fit.Count; i++)
                        {
                                if (fit[i].Rating < threshold)
                                {
                                        fit.RemoveRange(i, fit.Count - i);
                                        break;
                                }
                        }

                        Console.WriteLine("Number of runs that exceed the threshold of " + threshold.ToString()
                                + ":  " + fit.Count.ToString());
                }
        }
}