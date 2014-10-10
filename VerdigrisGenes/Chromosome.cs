namespace VerdigrisGenes
{
        using System;
        using System.Collections.Generic;

        public class Chromosome
        {
                private List<int> genes;

                private int index;

                private bool isChanged = false;

                public Chromosome()
                {
                        this.genes = new List<int>();
                        this.index = 0;
                }

                public bool IsChanged
                {
                        get
                        {
                                return isChanged;
                        }
                }

                public bool Load(List<int> genes)
                {
                        this.genes = genes;
                        this.isChanged = false;
                        return true;
                }

                public int Next()
                {
                        return this.genes.Count <= this.index ? int.MinValue : this.genes[this.index++];
                }

                public void Add(int next)
                {
                        this.genes.Add(next);
                        this.isChanged = true;
                }

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