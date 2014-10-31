// <copyright file="Interpreter.cs" company="John Colagioia">
//     John.Colagioia.net. Licensed under the GPLv3
// </copyright>
// <author>John Colagioia</author>
namespace Interpreter
{
        using System;
        using System.Collections.Generic;

        /// <summary>
        /// The program interpreter.
        /// </summary>
        public class Interpreter
        {
                /// <summary>
                /// The program.
                /// </summary>
                private readonly List<Statement> program;

                /// <summary>
                /// The symbol table.
                /// </summary>
                private readonly Dictionary<string, int> symbolTable;

                /// <summary>
                /// The inputs.
                /// </summary>
                private readonly Queue<int> inputs;

                /// <summary>
                /// The outputs.
                /// </summary>
                private readonly List<int> outputs;

                /// <summary>
                /// Initializes a new instance of the <see cref="Interpreter"/> class.
                /// </summary>
                /// <param name="inputs">Expected program inputs.</param>
                public Interpreter(List<int> inputs)
                {
                        this.program = new List<Statement>();
                        this.symbolTable = new Dictionary<string, int>();
                        this.inputs = inputs == null ? null : new Queue<int>(inputs);
                        this.outputs = new List<int>();
                }

                /// <summary>
                /// Gets the outputs.
                /// </summary>
                /// <value>The outputs.</value>
                public List<int> Outputs
                {
                        get
                        {
                                return this.outputs;
                        }
                }

                /// <summary>
                /// Parse the specified program.
                /// </summary>
                /// <param name="program">Program text.</param>
                /// <returns><c>true</c> if the parse succeeds; otherwise, <c>false</c>.</returns>
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

                /// <summary>
                /// Execute the program.
                /// </summary>
                /// <returns><c>true</c> if the execution succeds; otherwise, <c>false</c>.</returns>
                public bool Go()
                {
                        return this.Go(null);
                }

                /// <summary>
                /// Execute the program.
                /// </summary>
                /// <param name="iterations">Maximum number of loop iterations.</param>
                /// <returns><c>true</c> if the execution succeds; otherwise, <c>false</c>.</returns>
                public bool Go(int? iterations)
                {
                        bool status = true;

                        this.outputs.Clear();
                        foreach (Statement s in this.program)
                        {
                                status &= s.Go(this.symbolTable, this.inputs, this.outputs, iterations);
                        }

                        return status;
                }
        }
}