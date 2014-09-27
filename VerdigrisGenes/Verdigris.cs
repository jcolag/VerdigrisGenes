namespace VerdigrisGenes
{
        using System;
        using System.Text.RegularExpressions;

        public class Verdigris
        {
                private Grammar grammar;

                public Verdigris()
                {
                        grammar = new Grammar();
                }

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

                public string GenerateProgram(string startstate)
                {
                        return this.grammar.Fill(startstate);
                }
        }
}