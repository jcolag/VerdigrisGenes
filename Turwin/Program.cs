namespace Turwin
{
        using System;
        using System.IO;
        using VerdigrisGenes;

        class MainClass
        {
                public static void Main(string[] args)
                {
                        var verd = new Verdigris();

                        if (args.Length == 0)
                        {
                                Console.WriteLine("Requires a parameter indicating grammar file.");
                                return;
                        }

                        var input = File.OpenText(args[0]);
                        var grammartext = input.ReadToEnd();

                        verd.ParseGrammar(grammartext);
                        string program = verd.GenerateProgram("Program");
                        Console.WriteLine(program);
                }
        }
}