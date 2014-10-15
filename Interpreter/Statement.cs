// <copyright file="Interpreter.cs" company="John Colagioia">
//     John.Colagioia.net. Licensed under the GPLv3
// </copyright>
// <author>John Colagioia</author>
namespace Interpreter
{
        using System;
        using System.Collections.Generic;

        public class Statement
        {
                private List<Statement> nest;

                private bool isNestable = false;

                private bool isLoop = false;

                private bool isEnd = false;

                private StatementType type = StatementType.Unknown;

                private List<string> varList;

                public Statement()
                {
                        this.nest = new List<Statement>();
                        this.varList = new List<string>();
                }

                public Statement(string line)
                {
                        this.nest = new List<Statement>();
                        this.Parse(line);
                }

                public bool IsLoop
                {
                        get
                        {
                                return this.isLoop;
                        }
                }

                public bool IsEnd
                {
                        get
                        {
                                return this.isEnd;
                        }
                }

                public StatementType Type
                {
                        get
                        {
                                return this.type;
                        }
                }

                public bool Parse(string line)
                {
                        string[] nl = { Environment.NewLine };
                        string[] tokens = line.ToLower().Split(nl, StringSplitOptions.RemoveEmptyEntries);
                        bool block = false;
                        int index = 1;

                        switch(tokens[0])
                        {
                        case "define":
                                bool cont = true;

                                this.type = StatementType.Define;
                                while (cont)
                                {
                                        if (!char.IsLetter(tokens[index][0]))
                                        {
                                                break;
                                        }

                                        varList.Add(tokens[index++]);
                                        cont = Statement.Match(",", tokens[index++]);
                                }
                                break;
                        case "input":
                                this.type = StatementType.Input;
                                break;
                        case "output":
                                this.type = StatementType.Output;
                                break;
                        case "let":
                                this.type = StatementType.Assign;
                                break;
                        case "while":
                                this.type = StatementType.Loop;
                                block = true;
                                this.isLoop = true;
                                break;
                        case "if":
                                this.type = StatementType.Conditional;
                                block = true;
                                break;
                        case "end":
                                this.type = StatementType.End;
                                this.isEnd = true;
                                break;
                        case "#":
                                this.type = StatementType.Comment;
                                break;
                        }

                        this.isNestable = block;
                        return this.isNestable;
                }

                public void Nest(Statement item)
                {
                        this.nest.Add(item);
                }

                private static bool Match(string target, string input)
                {
                        return target == input;
                }
        }
}

