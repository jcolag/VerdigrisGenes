// <copyright file="Interpreter.cs" company="John Colagioia">
//     John.Colagioia.net. Licensed under the GPLv3
// </copyright>
// <author>John Colagioia</author>
namespace Interpreter
{
        using System;
        using System.Collections.Generic;

        public class Interpreter
        {
                private List<Statement> program;

                public Interpreter()
                {
                        program = new List<Statement>();
                }

                public bool Parse(string program)
                {
                        var nesting = new Stack<Statement>();
                        string[] nl = { Environment.NewLine };
                        string[] lines = program.Split(nl, StringSplitOptions.RemoveEmptyEntries);

                        foreach (string line in lines)
                        {
                                var s = new Statement();
                                bool nest = s.Parse(line);
                                if (nesting.Count == 0)
                                {
                                        this.program.Add(s);
                                }
                                else
                                {
                                        Statement ctrl = nesting.Peek();
                                        ctrl.Nest(s);
                                }

                                if (nest)
                                {
                                        nesting.Push(s);
                                }
                                else if (s.IsEnd)
                                {
                                        if (s.IsLoop == nesting.Peek().IsLoop)
                                        {
                                                nesting.Pop();
                                        }
                                        else
                                        {
                                                return false;
                                        }
                                }
                        }

                        return true;
                }

                public bool Go()
                {
                        return true;
                }
        }
}

