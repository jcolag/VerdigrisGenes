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
                /// The entry point of the program, where the program control starts and ends.
                /// </summary>
                /// <param name="args">The command-line arguments.</param>
                public static void Main(string[] args)
                {
                        var stdin = new List<int>() { 720 };
                        var stdout = new List<int>() { 2, 2, 2, 2, 3, 3, 5 };
                        FitnessSelector fit;
                        string nl = Environment.NewLine;
                        string genometext = string.Empty;

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

                        fit = new FitnessSelector(grammartext, genometext, stdin, stdout);
                        fit.Execute();
                        bool accept = fit.Evaluate(90.0);
                        Console.WriteLine(accept.ToString() + ": " + fit.Rating.ToString() + "%");
                }
        }
}