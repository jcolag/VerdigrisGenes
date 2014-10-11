// <copyright file="Chromosome.cs" company="John Colagioia">
//     John.Colagioia.net. Licensed under the GPLv3
// </copyright>
// <author>John Colagioia</author>
namespace VerdigrisGenes
{
        using System;
        using System.Collections.Generic;

        /// <summary>
        /// Chromosome-like data.
        /// </summary>
        public class Chromosome
        {
                /// <summary>
                /// The genes.
                /// </summary>
                private List<int> genes;

                /// <summary>
                /// The index counting which genes have been returned.
                /// </summary>
                private int index;

                /// <summary>
                /// Whether the chromosome has changed.
                /// </summary>
                private bool isChanged = false;

                /// <summary>
                /// Initializes a new instance of the <see cref="VerdigrisGenes.Chromosome"/> class.
                /// </summary>
                public Chromosome()
                {
                        this.genes = new List<int>();
                        this.index = 0;
                }

                /// <summary>
                /// Gets a value indicating whether this instance has changed.
                /// </summary>
                /// <value><c>true</c> if this instance has changed; otherwise, <c>false</c>.</value>
                public bool IsChanged
                {
                        get
                        {
                                return this.isChanged;
                        }
                }

                /// <summary>
                /// Load the specified genes.
                /// </summary>
                /// <param name="genes">The genes.</param>
                /// <returns>Whether the load was successful.</returns>
                public bool Load(List<int> genes)
                {
                        this.genes = genes;
                        this.isChanged = false;
                        return true;
                }

                /// <summary>
                /// Return the next gene for this chromosome.
                /// </summary>
                /// <returns>The next gene.</returns>
                public int Next()
                {
                        return this.genes.Count <= this.index ? int.MinValue : this.genes[this.index++];
                }

                /// <summary>
                /// Add the specified next value to the chromosome.
                /// </summary>
                /// <param name="next">The gene to add.</param>
                public void Add(int next)
                {
                        this.genes.Add(next);
                        this.isChanged = true;
                }

                /// <summary>
                /// Dump this instance's contents.
                /// </summary>
                /// <returns>A space-delimited gene list.</returns>
                public string Dump()
                {
                        string chr = string.Empty;
                        foreach (int gene in this.genes)
                        {
                                chr += gene.ToString() + " ";
                        }

                        return chr.Trim();
                }
        }
}