// <copyright file="FitnessSelector.cs" company="John Colagioia">
//     John.Colagioia.net. Licensed under the GPLv3
// </copyright>
// <author>John Colagioia</author>
namespace Fitness
{
        using System;
        using System.Collections.Generic;
        using VerdigrisGenes;
        using Interpreter;

        public class FitnessSelector
        {
                private readonly List<int> outputs;

                private readonly Verdigris genome;

                private readonly Interpreter terp;

                private readonly string program;

                private bool executed;

                private double rating = 0.0;

                public FitnessSelector(string grammar, string initialGenome, List<int> inputs, List<int> outputs)
                {
                        string nl = Environment.NewLine;

                        this.outputs = new List<int>(outputs);
                        this.terp = new Interpreter(inputs);
                        this.genome = new Verdigris();
                        this.genome.ParseGrammar(grammar);
                        if (!string.IsNullOrWhiteSpace(initialGenome))
                        {
                                this.genome.ReplaceChromosomes(initialGenome);
                        }

                        this.program = this.genome.GenerateProgram("Program");
                        this.program = this.program.Replace(" ;", nl).Replace(nl + " ", nl);
                        this.terp.Parse(program);

                        this.executed = false;
                }

                public double Rating
                {
                        get
                        {
                                return this.rating;
                        }
                }

                public void Execute()
                {
                        this.executed = this.terp.Go();
                }

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

                private static double Evaluate(int result, int expected, double score)
                {
                        double diff = Math.Abs((double)result - (double)expected);
                        double exp = Math.Abs((double)expected);
                        double component = diff / exp;
                        if (component > 1.0)
                        {
                                component = 1.0 / component;
                        }

                        component = 1 - component;
                        return score * component;
                }

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
        }
}