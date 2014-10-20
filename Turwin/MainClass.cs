// <copyright file="MainClass.cs" company="John Colagioia">
//     John.Colagioia.net. Licensed under the GPLv3
// </copyright>
// <author>John Colagioia</author>
namespace Turwin
{
        using System;
        using System.Collections.Generic;
        using System.IO;
        using VerdigrisGenes;

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
                        var verd = new Verdigris();
                        var terp = new Interpreter.Interpreter(stdin);
                        string nl = Environment.NewLine;

                        if (args.Length == 0)
                        {
                                Console.WriteLine("Requires a parameter indicating a grammar file and a genome file.");
                                return;
                        }

                        var input = File.OpenText(args[0]);
                        var grammartext = input.ReadToEnd();
                        input.Close();
                        verd.ParseGrammar(grammartext);

                        if (args.Length > 1)
                        {
                                input = File.OpenText(args[1]);
                                var genometext = input.ReadToEnd();
                                input.Close();
                                verd.ReplaceChromosomes(genometext);
                        }

                        string program = verd.GenerateProgram("Program");
                        program = program.Replace(" ;", nl).Replace(nl + " ", nl);
                        Console.WriteLine(program);
                        Console.WriteLine(verd.DumpChromosomes());

                        terp.Parse(program);
                        terp.Go();
                        foreach (int x in terp.Outputs)
                        {
                                Console.WriteLine(x);
                        }
                }
        }
}