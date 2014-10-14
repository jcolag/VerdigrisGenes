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

                public Statement()
                {
                        this.nest = new List<Statement>();
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

                public bool Parse(string line)
                {
                        string[] nl = { Environment.NewLine };
                        string[] tokens = line.ToLower().Split(nl, StringSplitOptions.RemoveEmptyEntries);
                        bool block = false;

                        switch(tokens[0])
                        {
                        case "define":
                                break;
                        case "input":
                                break;
                        case "output":
                                break;
                        case "let":
                                break;
                        case "while":
                                block = true;
                                this.isLoop = true;
                                break;
                        case "if":
                                block = true;
                                break;
                        case "end":
                                this.isEnd = true;
                                break;
                        case "#":
                                break;
                        }

                        this.isNestable = block;
                        return this.isNestable;
                }

                public void Nest(Statement item)
                {
                        this.nest.Add(item);
                }
        }
}

