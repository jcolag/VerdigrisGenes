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
                /// The grammar productions.
                /// </summary>
                private Dictionary<string, List<string>> productions;

                /// <summary>
                /// The random number generator.
                /// </summary>
                private Random rand;

                /// <summary>
                /// The number of declared variables.
                /// </summary>
                private int declaredVariables = 0;

                /// <summary>
                /// Initializes a new instance of the <see cref="VerdigrisGenes.Grammar"/> class.
                /// </summary>
                public Grammar()
                {
                        this.productions = new Dictionary<string, List<string>>();
                        this.rand = new Random();
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
                        string[] values = kv[1].Split(pipe);

                        for (int idx = 0; idx < values.Length; idx++)
                        {
                                values[idx] = values[idx].Trim();
                        }

                        var results = new List<string>(values);
                        this.productions.Add(key, results);
                        Console.WriteLine("Added key " + key + " with " + results.Count.ToString() + " items.");
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
                                switch (key)
                                {
                                case "@Declare":
                                        ++this.declaredVariables;
                                        return "v" + this.declaredVariables.ToString();
                                case "@Variable":
                                        return "v" + (this.rand.Next(this.declaredVariables) + 1).ToString();
                                case "@Number":
                                        return this.rand.Next().ToString();
                                }
                        }

                        if (!this.productions.ContainsKey(k))
                        {
                                Console.WriteLine("Key " + key + " not found.");
                                return string.Empty;
                        }

                        List<string> options = this.productions[k];
                        int idx = this.rand.Next(options.Count);
                        string result = "//";
                        try
                        {
                                result = options[idx].Trim();
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
                                        Console.WriteLine("terminal '" + part + "'");
                                        result += part + " ";
                                }
                                else
                                {
                                        string fullpart = this.Choose(part.Trim());
                                        Console.WriteLine("non-terminal '" + part + "' => '" + fullpart + "'");
                                        fullpart = this.Fill(fullpart);
                                        result += fullpart + " ";
                                }
                        }

                        return result.Trim();
                }
        }
}