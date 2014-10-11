// <copyright file="NumberGenerator.cs" company="John Colagioia">
//     John.Colagioia.net. Licensed under the GPLv3
// </copyright>
// <author>John Colagioia</author>
namespace VerdigrisGenes
{
        using System;
        using System.Collections.Generic;

        /// <summary>
        /// Number generator combining the chromosome data with random numbers.
        /// </summary>
        public class NumberGenerator
        {
                /// <summary>
                /// The random number generator.
                /// </summary>
                private Random rand;

                /// <summary>
                /// The chromosomes.
                /// </summary>
                private Dictionary<string, Chromosome> chromosomes;

                /// <summary>
                /// Initializes a new instance of the <see cref="VerdigrisGenes.NumberGenerator"/> class.
                /// </summary>
                public NumberGenerator()
                {
                        this.rand = new Random();
                        this.chromosomes = new Dictionary<string, Chromosome>();
                }

                /// <summary>
                /// Load the specified genes for the named chromosome.
                /// </summary>
                /// <param name="name">Name of the chromosome.</param>
                /// <param name="genes">List of genes.</param>
                /// <returns>Whether the load was successful.</returns>
                public bool Load(string name, List<int> genes)
                {
                        var chr = new Chromosome();
                        bool loaded = chr.Load(genes);
                        if (this.chromosomes.ContainsKey(name))
                        {
                                this.chromosomes.Remove(name);
                        }

                        this.chromosomes.Add(name, chr);
                        return loaded;
                }

                /// <summary>
                /// Load the specified genes for the named chromosome.
                /// </summary>
                /// <param name="name">Name of the chromosome.</param>
                /// <param name="genes">List of genes as a space-delimited string.</param>
                /// <returns>Whether the load was successful.</returns>
                public bool Load(string name, string genes)
                {
                        char[] space = { ' ' };
                        string[] elements = genes.Split(space);
                        var l = new List<int>();
                        foreach (string s in elements)
                        {
                                int i = Convert.ToInt32(s);
                                l.Add(i);
                        }

                        return this.Load(name, l);
                }

                /// <summary>
                /// Get the next gene for the named chromosome.
                /// </summary>
                /// <param name="name">Name of the chromosome.</param>
                /// <returns>The next gene.</returns>
                public int Next(string name)
                {
                        if (!this.chromosomes.ContainsKey(name))
                        {
                                return this.rand.Next();
                        }

                        Chromosome chr = this.chromosomes[name];
                        int value = chr.Next();
                        if (value == int.MinValue)
                        {
                                int next = this.rand.Next();
                                chr.Add(next);
                                return next;
                        }

                        return value;
                }

                /// <summary>
                /// Get the next gene for the named chromosome, subject to a maximum value.
                /// </summary>
                /// <param name="name">Name of the chromosome.</param>
                /// <param name="max">Maximum value.</param>
                /// <returns>The next gene.</returns>
                public int Next(string name, int max)
                {
                        return this.Next(name) % max;
                }
        }
}