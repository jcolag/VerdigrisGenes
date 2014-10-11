// <copyright file="Verdigris.cs" company="John Colagioia">
//     John.Colagioia.net. Licensed under the GPLv3
// </copyright>
// <author>John Colagioia</author>
namespace VerdigrisGenes
{
        using System;
        using System.Text.RegularExpressions;

        /// <summary>
        /// Verdigris Genes main class.
        /// </summary>
        public class Verdigris
        {
                /// <summary>
                /// The grammar to use for generating programs.
                /// </summary>
                private Grammar grammar;

                /// <summary>
                /// Initializes a new instance of the <see cref="VerdigrisGenes.Verdigris"/> class.
                /// </summary>
                public Verdigris()
                {
                        this.grammar = new Grammar();
                }

                /// <summary>
                /// Replaces the chromosomes.
                /// </summary>
                /// <returns>The chromosomes.</returns>
                /// <param name="genes">The genes.</param>
                /// <returns>Number of chromosomes parsed.</returns>
                public int ReplaceChromosomes(string genes)
                {
                        return this.grammar.ReplaceChromosomes(genes);
                }

                /// <summary>
                /// Parses the grammar.
                /// </summary>
                /// <returns>The grammar.</returns>
                /// <param name="productions">The grammar productions.</param>
                public int ParseGrammar(string productions)
                {
                        string newline = Environment.NewLine;
                        string separation = newline + newline;
                        string[] prods = Regex.Split(productions, separation);
                        int count = 0;

                        foreach (string p in prods)
                        {
                                this.grammar.Add(p);
                                ++count;
                        }

                        return count;
                }

                /// <summary>
                /// Generates a program.
                /// </summary>
                /// <returns>The program.</returns>
                /// <param name="startstate">The starting state.</param>
                public string GenerateProgram(string startstate)
                {
                        return this.grammar.Fill(startstate);
                }
        }
}