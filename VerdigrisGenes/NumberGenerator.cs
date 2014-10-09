namespace VerdigrisGenes
{
        using System;
        using System.Collections.Generic;

        public class NumberGenerator
        {
                private Random rand;

                private Dictionary<string, Chromosome> chromosomes;

                public NumberGenerator()
                {
                        rand = new Random();
                        chromosomes = new Dictionary<string, Chromosome>();
                }

                public bool Load(string name, List<int> genes)
                {
                        var chr = new Chromosome();
                        bool loaded = chr.Load(genes);
                        if (chromosomes.ContainsKey(name))
                        {
                                chromosomes.Remove(name);
                        }

                        chromosomes.Add(name, chr);
                        return loaded;
                }

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

                public int Next(string name)
                {
                        if (!chromosomes.ContainsKey(name))
                        {
                                return this.rand.Next();
                        }

                        Chromosome chr = chromosomes[name];
                        int value = chr.Next();
                        if (value == int.MinValue)
                        {
                                return this.rand.Next();
                        }

                        return value;
                }

                public int Next(string name, int max)
                {
                        return this.Next(name) % max;
                }
        }
}