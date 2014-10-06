// <copyright file="MainClass.cs" company="John Colagioia">
//     John.Colagioia.net. Licensed under the GPLv3
// </copyright>
// <author>John Colagioia</author>
namespace Turwin
{
        using System;
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
                        var verd = new Verdigris();
                        string nl = Environment.NewLine;

                        if (args.Length == 0)
                        {
                                Console.WriteLine("Requires a parameter indicating grammar file.");
                                return;
                        }

                        var input = File.OpenText(args[0]);
                        var grammartext = input.ReadToEnd();

                        verd.ParseGrammar(grammartext);
                        string program = verd.GenerateProgram("Program");
                        Console.WriteLine(program.Replace(" ;", nl).Replace(nl + " ", nl));
                }
        }
}