// <copyright file="Grammar.cs" company="John Colagioia">
//     John.Colagioia.net. Licensed under the GPLv3
// </copyright>
// <author>John Colagioia</author>
namespace VerdigrisGenes
{
        using System;
        using System.Collections.Generic;
        using System.Text.RegularExpressions;

        /// <summary>
        /// Language Grammar.
        /// </summary>
        public class Grammar
        {
                /// <summary>
                /// The chromosome names.
                /// </summary>
                private readonly string[] chromosomeNames =
                {
                        "Number",
                        "Variable",
                        "Initialized",
                        "Production"
                };

                /// <summary>
                /// The symbol table.
                /// </summary>
                private readonly List<Variable> symbolTable = new List<Variable>();

                /// <summary>
                /// The number generator.
                /// </summary>
                private NumberGenerator ng;

                /// <summary>
                /// The grammar productions.
                /// </summary>
                private Dictionary<string, List<GrammarExpression>> productions;

                /// <summary>
                /// The number of declared variables.
                /// </summary>
                private int declaredVariables = 0;

                /// <summary>
                /// Initializes a new instance of the <see cref="VerdigrisGenes.Grammar"/> class.
                /// </summary>
                public Grammar()
                {
                        this.productions = new Dictionary<string, List<GrammarExpression>>();
                        this.ng = new NumberGenerator();
                }

                /// <summary>
                /// Replaces the chromosomes.
                /// </summary>
                /// <returns>The chromosomes.</returns>
                /// <param name="genes">The genes to parse into chromosomes.</param>
                /// <returns>Number of chromosomes parsed.</returns>
                public int ReplaceChromosomes(string genes)
                {
                        char[] nl = { '\n', '\r' };
                        string[] lines = genes.Split(nl);
                        int index = 0;

                        foreach (string name in this.chromosomeNames)
                        {
                                string line = lines[index];
                                ++index;
                                this.ng.Load(name, line);
                        }

                        return lines.Length;
                }

                /// <summary>
                /// Add the specified production.
                /// </summary>
                /// <param name="production">The grammar production.</param>
                /// <returns>Count of grammar productions added.</returns>
                public int Add(string production)
                {
                        string newline = Environment.NewLine;
                        string separation = newline + newline;

                        if (string.IsNullOrWhiteSpace(production))
                        {
                                return 0;
                        }

                        if (production.Contains(separation))
                        {
                                int count = 0;
                                string[] parts = Regex.Split(production, separation);

                                foreach (string p in parts)
                                {
                                        count += this.Add(p);
                                }

                                return count;
                        }

                        if (!production.Contains("::="))
                        {
                                Console.WriteLine("Malformed production:" + newline + production);
                                return -1;
                        }

                        string[] kv = Regex.Split(production, "::=");
                        string key = kv[0].Trim().ToUpper();
                        if (kv.Length == 1)
                        {
                                Console.WriteLine("Mismatch for " + key);
                        }

                        char[] pipe = { '|' };
                        char[] hash = { '#' };
                        string[] values = kv[1].Split(pipe);
                        var results = new List<GrammarExpression>();

                        for (int idx = 0; idx < values.Length; idx++)
                        {
                                string[] parts = values[idx].Split(hash);
                                string expr = parts[0].Trim();
                                bool req = false;
                                bool init = false;

                                if (parts.Length > 1)
                                {
                                        req = parts[1].Contains("init");
                                }

                                init |= parts.Length > 2;
                                results.Add(new GrammarExpression(expr, req, init));
                        }

                        this.productions.Add(key, results);
                        return results.Count;
                }

                /// <summary>
                /// Choose syntax based on the specified key.
                /// </summary>
                /// <param name="key">Non-terminal key.</param>
                /// <returns>Replacement string.</returns>
                public string Choose(string key)
                {
                        string k = key.ToUpper();

                        if (key.StartsWith("@", StringComparison.CurrentCulture))
                        {
                                int which;
                                Variable v;

                                switch (key)
                                {
                                case "@Declare":
                                        ++this.declaredVariables;
                                        string name = "v" + this.declaredVariables.ToString();
                                        this.symbolTable.Add(new Variable(name));
                                        return name;
                                case "@Variable":
                                        which = this.ng.Next("Variable", this.symbolTable.Count);
                                        v = this.symbolTable[which];
                                        v.ToInitialize = true;
                                        return v.Name;
                                case "@Initialized":
                                        List<Variable> initialized = this.symbolTable.FindAll(x => x.Initialized);
                                        if (initialized.Count == 0)
                                        {
                                                return "novariable";
                                        }

                                        which = this.ng.Next("Initialized", initialized.Count);
                                        v = initialized[which];
                                        return v.Name;
                                case "@Number":
                                        return this.ng.Next("Number").ToString();
                                }
                        }

                        if (!this.productions.ContainsKey(k))
                        {
                                Console.WriteLine("Key " + key + " not found.");
                                return string.Empty;
                        }

                        List<GrammarExpression> options = this.productions[k];
                        if (this.symbolTable.FindAll(var => var.Initialized).Count == 0)
                        {
                                options = options.FindAll(ge => !ge.ReqValue);
                        }

                        int idx = this.ng.Next("Production", options.Count);
                        string result = "//";
                        try
                        {
                                result = options[idx].Expression.Trim();
                        }
                        catch
                        {
                                Console.WriteLine("Choose failed!");
                        }

                        return result == "//" ? string.Empty : result;
                }

                /// <summary>
                /// Fill the specified grammar non-terminal.
                /// </summary>
                /// <param name="original">Original non-terminal.</param>
                /// <returns>Replacement string.</returns>
                public string Fill(string original)
                {
                        string[] parts;
                        parts = Regex.Split(original, @"\s+");

                        string result = string.Empty;

                        foreach (string part in parts)
                        {
                                if (part.ToLower() == part)
                                {
                                        if (part == ";")
                                        {
                                                foreach (Variable v in this.symbolTable)
                                                {
                                                        v.Update();
                                                }
                                        }

                                        result += part + " ";
                                }
                                else
                                {
                                        string fullpart = this.Choose(part.Trim());
                                        fullpart = this.Fill(fullpart);
                                        result += fullpart + " ";
                                }
                        }

                        return result.Trim();
                }
        }
}