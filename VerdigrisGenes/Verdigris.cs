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
                /// Initializes a new instance of the <see cref="VerdigrisGenes.Verdigris"/> class.
                /// </summary>
                /// <param name="g">The grammar component.</param>
                public Verdigris(Grammar g)
                {
                        this.grammar = g;
                }

                /// <summary>
                /// Gets the grammar.
                /// </summary>
                /// <value>The grammar.</value>
                protected Grammar Grammar
                {
                        get
                        {
                                return this.grammar;
                        }
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
                        return this.grammar.ParseGrammar(productions);
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

                /// <summary>
                /// Dumps the chromosomes.
                /// </summary>
                /// <returns>The chromosomes.</returns>
                public string DumpChromosomes()
                {
                        return this.grammar.DumpChromosomes();
                }

                /// <summary>
                /// Mate with the specified programmer.
                /// </summary>
                /// <param name="mate">The mate.</param>
                /// <returns>The new programmer.</returns>
                public Verdigris Mate(Verdigris mate)
                {
                        Grammar g = this.grammar.Mate(mate.Grammar);
                        return new Verdigris(g);
                }
        }
}