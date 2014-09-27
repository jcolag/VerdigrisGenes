namespace VerdigrisGenes
{
        using System;
        using System.Collections.Generic;
        using System.Text.RegularExpressions;

        public class Grammar
        {
                private Dictionary<string, List<string>> productions;

                private Random rand;

                public Grammar()
                {
                        productions = new Dictionary<string, List<string>>();
                        rand = new Random();
                }

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
                        productions.Add(key, results);
                        Console.WriteLine("Added key " + key + " with " + results.Count.ToString() + " items.");
                        return results.Count;
                }

                public string Choose(string key)
                {
                        string k = key.ToUpper();

                        if (!this.productions.ContainsKey(k))
                        {
                                Console.WriteLine("Key " + key + " not found.");
                                return string.Empty;
                        }

                        List<string> options = productions[k];
                        int idx = rand.Next(options.Count);
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

                public string Fill(string original)
                {
                        char[] space = { ' ' };
                        string[] parts;
                        try
                        {
                                parts = original.Split(space);
                        }
                        catch (Exception e)
                        {
                                Console.WriteLine(e.Message);
                                return string.Empty;
                        }

                        string result = string.Empty;

                        try
                        {
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
                                                try
                                                {
                                                        fullpart = this.Fill(fullpart);
                                                        result += fullpart + " ";
                                                }
                                                catch
                                                {
                                                        result += "!!" + part + "!! ";
                                                }
                                        }
                                }
                        }
                        catch (Exception e)
                        {
                                Console.WriteLine(e.Message);
                        }

                        return result.Trim();
                }
        }
}