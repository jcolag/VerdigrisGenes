﻿// <copyright file="FitnessSelector.cs" company="John Colagioia">
//     John.Colagioia.net. Licensed under the GPLv3
// </copyright>
// <author>John Colagioia</author>
namespace Fitness
{
        using System;
        using System.Collections.Generic;
        using Interpreter;
        using VerdigrisGenes;

        /// <summary>
        /// Fitness selector.
        /// </summary>
        public class FitnessSelector: IComparable
        {
                /// <summary>
                /// The program inputs.
                /// </summary>
                private List<int> inputs;

                /// <summary>
                /// The expected outputs.
                /// </summary>
                private List<int> outputs;

                /// <summary>
                /// The program genome.
                /// </summary>
                private Verdigris genome;

                /// <summary>
                /// The interpreter.
                /// </summary>
                private Interpreter terp;

                /// <summary>
                /// The program.
                /// </summary>
                private string program;

                /// <summary>
                /// Whether the program has been executed.
                /// </summary>
                private bool executed;

                /// <summary>
                /// The program's fitness rating.
                /// </summary>
                private double rating = 0.0;

                /// <summary>
                /// Initializes a new instance of the <see cref="Fitness.FitnessSelector"/> class.
                /// </summary>
                /// <param name="grammar">The program grammar.</param>
                /// <param name="initialGenome">Initial genome.</param>
                /// <param name="inputs">Standard inputs.</param>
                /// <param name="outputs">Expected outputs.</param>
                public FitnessSelector(string grammar, string initialGenome, List<int> inputs, List<int> outputs)
                {
                        this.genome = new Verdigris();
                        this.genome.ParseGrammar(grammar);
                        if (!string.IsNullOrWhiteSpace(initialGenome))
                        {
                                this.genome.ReplaceChromosomes(initialGenome);
                        }

                        GenerateProgram(inputs, outputs);
                }

                public FitnessSelector(Verdigris verdigris, List<int> inputs, List<int> outputs)
                {
                        this.genome = verdigris;
                        GenerateProgram(inputs, outputs);
                }

                /// <summary>
                /// Gets the program's fitness rating.
                /// </summary>
                /// <value>The rating.</value>
                public double Rating
                {
                        get
                        {
                                return this.rating;
                        }
                }

                /// <summary>
                /// Gets the program.
                /// </summary>
                /// <value>The program.</value>
                public string Program
                {
                        get
                        {
                                return this.program;
                        }
                }

                /// <summary>
                /// Execute this instance's program.
                /// </summary>
                public void Execute()
                {
                        this.Execute(null);
                }

                /// <summary>
                /// Execute this instance's program.
                /// <param name="iterations">Maximum number of loop iterations.</param>
                /// </summary>
                public void Execute(int? iterations)
                {
                        this.executed = this.terp.Go(iterations);
                }

                /// <summary>
                /// Evaluate the program against the specified threshold.
                /// </summary>
                /// <param name="threshold">Threshold for success.</param>
                /// <returns>True if the program exceeds the threshold, otherwise false.</returns>
                public bool Evaluate(double threshold)
                {
                        var result = new List<int>(this.terp.Outputs);
                        double score = 1.0;

                        if (!this.executed)
                        {
                                this.Execute();
                        }

                        // Pad the lists to equal length
                        FitnessSelector.Pad(this.outputs, result.Count, 0);
                        FitnessSelector.Pad(result, this.outputs.Count, 0);

                        // Measure the difference between the expected results and program output
                        for (int i = 0; i < result.Count; i++)
                        {
                                score = FitnessSelector.Evaluate(result[i], this.outputs[i], score);
                        }

                        // It's a keeper if it's at least threshold-% "right"
                        this.rating = score * 100.0;
                        return this.rating >= threshold;
                }

                public FitnessSelector Mate(FitnessSelector partner)
                {
                        Verdigris partnerVerd = this.genome.Mate(partner.genome);
                        return new FitnessSelector(partnerVerd, this.inputs, this.outputs);
                }

                #region IComparable implementation

                /// <summary>
                /// Compares this object to another.
                /// </summary>
                /// <returns>The comparison.</returns>
                /// <param name="obj">The object to compare.</param>
                public int CompareTo(object obj)
                {
                        if (!(obj is FitnessSelector))
                        {
                                throw new NotImplementedException();
                        }

                        var b = (FitnessSelector)obj;
                        double thisval = this.Rating;
                        double thatval = b.Rating;
                        return thatval.CompareTo(thisval);
                }

                #endregion

                /// <summary>
                /// Evaluate the specified result, expected and score.
                /// </summary>
                /// <param name="result">Result value.</param>
                /// <param name="expected">Expected value.</param>
                /// <param name="score">Score to date.</param>
                /// <returns>Current score of coincidence with expected results.</returns>
                private static double Evaluate(int result, int expected, double score)
                {
                        double diff = Math.Abs((double)result - (double)expected);
                        double exp = Math.Max(Math.Abs((double)expected), Math.Abs((double)result));
                        double component = diff / exp;
                        if (component > 1.0)
                        {
                                component = 1.0 / component;
                        }

                        component = 1 - component;
                        return score * component;
                }

                /// <summary>
                /// Pad the specified list to max length with item.
                /// </summary>
                /// <param name="list">List to pad.</param>
                /// <param name="max">Maximum list length.</param>
                /// <param name="item">Item for padding.</param>
                /// <returns>Final (padded) length.</returns>
                private static int Pad(ICollection<int> list, int max, int item)
                {
                        int count = 0;
                        while (list.Count < max)
                        {
                                list.Add(item);
                                ++count;
                        }

                        return count;
                }

                /// <summary>
                /// Generates the program.
                /// </summary>
                /// <param name="invalues">Input values.</param>
                /// <param name="outvalues">Output values.</param>
                private void GenerateProgram(List<int> invalues, List<int> outvalues)
                {
                        string nl = Environment.NewLine;

                        this.inputs = new List<int>(invalues);
                        this.outputs = new List<int>(outvalues);
                        this.terp = new Interpreter(invalues);

                        this.program = this.genome.GenerateProgram("Program");
                        this.program = this.program.Replace(" ;", nl).Replace(nl + " ", nl);
                        this.terp.Parse(this.program);
                        this.executed = false;
                }
        }
}