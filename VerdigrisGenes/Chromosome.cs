namespace VerdigrisGenes
{
        using System;
        using System.Collections.Generic;

        public class Chromosome
        {
                private List<int> genes;

                private int index;

                public Chromosome()
                {
                        this.genes = new List<int>();
                        this.index = 0;
                }

                public bool Load(List<int> genes)
                {
                        this.genes = genes;
                        return true;
                }

                public int Next()
                {
                        return this.genes.Count <= this.index ? int.MinValue : this.genes[this.index++];
                }
        }
}