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
                private readonly List<Statement> program;

                private readonly Dictionary<string, int> symbolTable;

                private readonly Queue<int> inputs;

                private readonly List<int> outputs;

                public Interpreter(List<int> inputs)
                {
                        program = new List<Statement>();
                        symbolTable = new Dictionary<string, int>();
                        this.inputs = inputs == null ? null : new Queue<int>(inputs);
                        this.outputs = new List<int>();
                }

                public List<int> Outputs
                {
                        get
                        {
                                return this.outputs;
                        }
                }

                public bool Parse(string program)
                {
                        var nesting = new Stack<Statement>();
                        string[] nl = { Environment.NewLine };
                        string[] lines = program.Split(nl, StringSplitOptions.RemoveEmptyEntries);
                        int linenumber = 0;

                        foreach (string line in lines)
                        {
                                var s = new Statement();
                                bool nest = s.Parse(line);

                                ++linenumber;
                                if (!s.Valid)
                                {
                                        Console.WriteLine("Syntax error on line #" + linenumber.ToString());
                                        Console.WriteLine("\t" + s.Text);
                                }

                                if (s.Type != StatementType.Comment
                                        && s.Type != StatementType.Define
                                        && s.Type != StatementType.End
                                        && s.Valid)
                                {
                                        if (nesting.Count == 0)
                                        {
                                                this.program.Add(s);
                                        }
                                        else
                                        {
                                                Statement ctrl = nesting.Peek();
                                                ctrl.Nest(s);
                                        }
                                }

                                if (s.Type == StatementType.Define)
                                {
                                        foreach (string name in s.VarList)
                                        {
                                                this.symbolTable.Add(name, 0);
                                        }
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
                        bool status = true;

                        this.outputs.Clear();
                        foreach (Statement s in program)
                        {
                                status &= s.Go(this.symbolTable, this.inputs, this.outputs);
                        }

                        return status;
                }
        }
}