// <copyright file="Statement.cs" company="John Colagioia">
//     John.Colagioia.net. Licensed under the GPLv3
// </copyright>
// <author>John Colagioia</author>
namespace Interpreter
{
        using System;
        using System.Collections.Generic;

        /// <summary>
        /// Program statement.
        /// </summary>
        public class Statement
        {
                /// <summary>
                /// The nested statements.
                /// </summary>
                private List<Statement> nest;

                /// <summary>
                /// The statement text.
                /// </summary>
                private string text;

                /// <summary>
                /// Whether this kind of statement is nestable.
                /// </summary>
                private bool isNestable = false;

                /// <summary>
                /// Whether this kind of (nestable) statement is a loop.
                /// </summary>
                private bool isLoop = false;

                /// <summary>
                /// Whether this kind of statement is a nesting endpoint.
                /// </summary>
                private bool isEnd = false;

                /// <summary>
                /// Whether the statement was malformed.
                /// </summary>
                private bool malformed = false;

                /// <summary>
                /// The type of statement.
                /// </summary>
                private StatementType type = StatementType.Unknown;

                /// <summary>
                /// The variable list.
                /// </summary>
                private List<string> varList;

                /// <summary>
                /// The lexical elements.
                /// </summary>
                private Dictionary<LexicalElement, string> elements;

                /// <summary>
                /// Initializes a new instance of the <see cref="Statement"/> class.
                /// </summary>
                public Statement()
                {
                        this.nest = new List<Statement>();
                        this.varList = new List<string>();
                        this.elements = new Dictionary<LexicalElement, string>();
                        this.text = string.Empty;
                }

                /// <summary>
                /// Initializes a new instance of the <see cref="Statement"/> class.
                /// </summary>
                /// <param name="line">Line of program.</param>
                public Statement(string line)
                {
                        this.nest = new List<Statement>();
                        this.Parse(line);
                        this.varList = new List<string>();
                        this.elements = new Dictionary<LexicalElement, string>();
                        this.text = line;
                }

                /// <summary>
                /// Gets the text.
                /// </summary>
                /// <value>The text.</value>
                public string Text
                {
                        get
                        {
                                return this.text;
                        }
                }

                /// <summary>
                /// Gets a value indicating whether this instance is loop.
                /// </summary>
                /// <value><c>true</c> if this instance is a loop; otherwise, <c>false</c>.</value>
                public bool IsLoop
                {
                        get
                        {
                                return this.isLoop;
                        }
                }

                /// <summary>
                /// Gets a value indicating whether this instance is an end.
                /// </summary>
                /// <value><c>true</c> if this instance is end; otherwise, <c>false</c>.</value>
                public bool IsEnd
                {
                        get
                        {
                                return this.isEnd;
                        }
                }

                /// <summary>
                /// Gets the type of statement.
                /// </summary>
                /// <value>The type.</value>
                public StatementType Type
                {
                        get
                        {
                                return this.type;
                        }
                }

                /// <summary>
                /// Gets the variable list.
                /// </summary>
                /// <value>The variable list.</value>
                public List<string> VarList
                {
                        get
                        {
                                return this.varList;
                        }
                }

                /// <summary>
                /// Gets a value indicating whether this <see cref="Statement"/> is valid.
                /// </summary>
                /// <value><c>true</c> if valid; otherwise, <c>false</c>.</value>
                public bool Valid
                {
                        get
                        {
                                bool ok = !this.malformed;
                                switch (this.type)
                                {
                                case StatementType.Input:
                                case StatementType.Output:
                                        ok &= this.elements.ContainsKey(LexicalElement.Target);
                                        break;
                                case StatementType.Conditional:
                                case StatementType.Loop:
                                        ok &= this.elements.ContainsKey(LexicalElement.LeftOperand);
                                        ok &= this.elements.ContainsKey(LexicalElement.Operator);
                                        ok &= this.elements.ContainsKey(LexicalElement.RightOperand);
                                        break;
                                case StatementType.Assign:
                                        ok &= this.elements.ContainsKey(LexicalElement.Target);
                                        ok &= this.elements.ContainsKey(LexicalElement.LeftOperand);
                                        ok &= this.elements.ContainsKey(LexicalElement.Operator);
                                        ok &= this.elements.ContainsKey(LexicalElement.RightOperand);
                                        break;
                                }

                                return ok;
                        }
                }

                /// <summary>
                /// Parse the specified line.
                /// </summary>
                /// <param name="line">Line of program.</param>
                /// <returns><c>true</c> if the parse succeeded; otherwise, <c>false</c>.</returns>
                public bool Parse(string line)
                {
                        string[] space = { Environment.NewLine, " ", "\t" };
                        string[] tokens = line.ToLower().Split(space, StringSplitOptions.RemoveEmptyEntries);
                        bool block = false;
                        int index = 1;

                        if (tokens.Length == 0)
                        {
                                return false;
                        }

                        switch (tokens[0])
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

                                        this.varList.Add(tokens[index++]);
                                        cont = tokens.Length > index && Statement.Match(",", tokens[index++]);
                                }

                                break;
                        case "input":
                                this.type = StatementType.Input;
                                if (tokens.Length > 1 && Statement.MatchVariable(tokens[1]))
                                {
                                        this.elements.Add(LexicalElement.Target, tokens[1]);
                                }

                                break;
                        case "output":
                                this.type = StatementType.Output;
                                if (tokens.Length > 1 && Statement.MatchVariableOrNumber(tokens[1]))
                                {
                                        this.elements.Add(LexicalElement.Target, tokens[1]);
                                }

                                break;
                        case "let":
                                this.type = StatementType.Assign;
                                if (tokens.Length > 1 && Statement.MatchVariable(tokens[1]))
                                {
                                        this.elements.Add(LexicalElement.Target, tokens[1]);
                                }

                                this.malformed &= tokens.Length < 6 || !Statement.Match("<-", tokens[2]);
                                if (tokens.Length > 3 && Statement.MatchVariableOrNumber(tokens[3]))
                                {
                                        this.elements.Add(LexicalElement.LeftOperand, tokens[3]);
                                }

                                if (tokens.Length > 4 && Statement.MatchArithmetic(tokens[4]))
                                {
                                        this.elements.Add(LexicalElement.Operator, tokens[4]);
                                }

                                if (tokens.Length > 5 && Statement.MatchVariableOrNumber(tokens[5]))
                                {
                                        this.elements.Add(LexicalElement.RightOperand, tokens[5]);
                                }

                                break;
                        case "while":
                                this.type = StatementType.Loop;
                                block = true;
                                this.isLoop = true;
                                this.malformed &= tokens.Length < 6
                                        || !Statement.Match("(", tokens[1])
                                        || !Statement.Match(")", tokens[5]);
                                if (Statement.MatchVariableOrNumber(tokens[2]))
                                {
                                        this.elements.Add(LexicalElement.LeftOperand, tokens[2]);
                                }

                                if (Statement.MatchBoolean(tokens[3]))
                                {
                                        this.elements.Add(LexicalElement.Operator, tokens[3]);
                                }

                                if (Statement.MatchVariableOrNumber(tokens[4]))
                                {
                                        this.elements.Add(LexicalElement.RightOperand, tokens[4]);
                                }

                                break;
                        case "if":
                                this.type = StatementType.Conditional;
                                block = true;
                                this.malformed &= tokens.Length < 6
                                        || !Statement.Match("(", tokens[1])
                                        || !Statement.Match(")", tokens[5]);
                                if (tokens.Length > 2 && Statement.MatchVariableOrNumber(tokens[2]))
                                {
                                        this.elements.Add(LexicalElement.LeftOperand, tokens[2]);
                                }

                                if (tokens.Length > 3 && Statement.MatchBoolean(tokens[3]))
                                {
                                        this.elements.Add(LexicalElement.Operator, tokens[3]);
                                }

                                if (tokens.Length > 4 && Statement.MatchVariableOrNumber(tokens[4]))
                                {
                                        this.elements.Add(LexicalElement.RightOperand, tokens[4]);
                                }

                                break;
                        case "end":
                                this.type = StatementType.End;
                                this.isEnd = true;
                                this.isLoop = tokens[1].ToLower() == "while";
                                this.malformed &= !this.isLoop && tokens[1].ToLower() != "if";
                                break;
                        case "#":
                                this.type = StatementType.Comment;
                                break;
                        }

                        this.isNestable = block;
                        return this.isNestable;
                }

                /// <summary>
                /// Nest the specified statement.
                /// </summary>
                /// <param name="item">Statement to insert.</param>
                public void Nest(Statement item)
                {
                        this.nest.Add(item);
                }

                /// <summary>
                /// Execute this statement, given a symbol table and (optional) inputs.
                /// </summary>
                /// <param name="symbols">Symbol table.</param>
                /// <param name="inputs">Program inputs, optional.</param>
                /// <param name="outputs">Program outputs, used if inputs provided.</param>
                /// <returns><c>true</c> if successful; otherwise, <c>false</c>.</returns>
                public bool Go(Dictionary<string, int> symbols, Queue<int> inputs, List<int> outputs)
                {
                        return this.Go(symbols, inputs, outputs, null);
                }

                /// <summary>
                /// Execute this statement, given a symbol table and (optional) inputs.
                /// </summary>
                /// <param name="symbols">Symbol table.</param>
                /// <param name="inputs">Program inputs, optional.</param>
                /// <param name="outputs">Program outputs, used if inputs provided.</param>
                /// <param name="max">Maximum iterations in loops.</param> 
                        string varname;
                        int val;
                        bool status = true;

                        switch (this.type)
                        {
                        case StatementType.Output:
                                varname = this.elements[LexicalElement.Target];
                                if (outputs == null)
                                {
                                        Console.WriteLine(symbols[varname].ToString());
                                }
                                else
                                {
                                        outputs.Add(symbols[varname]);
                                }

                                break;
                        case StatementType.Input:
                                varname = this.elements[LexicalElement.Target];
                                if (inputs == null)
                                {
                                        Console.Write(varname + ": ");
                                        string input = Console.ReadLine();
                                        status = int.TryParse(input, out val);
                                }
                                else if (inputs.Count > 0)
                                {
                                        val = inputs.Dequeue();
                                }
                                else
                                {
                                        val = 0;
                                        status = false;
                                }

                                symbols[varname] = val;
                                break;
                        case StatementType.Assign:
                                varname = this.elements[LexicalElement.Target];
                                symbols[varname] = this.EvaluateExpression(symbols);
                                break;
                        case StatementType.Conditional:
                                if (this.EvaluateExpression(symbols) == 1)
                                {
                                        foreach (Statement s in this.nest)
                                        {
                                                status &= s.Go(symbols, inputs, outputs);
                                        }
                                }

                                break;
                        case StatementType.Loop:
                                while (this.EvaluateExpression(symbols) == 1)
                                {
                                        foreach (Statement s in this.nest)
                                        {
                                                status &= s.Go(symbols, inputs, outputs);
                                                status &= s.Go(symbols, inputs, outputs, max);
                                        }
                                }

                                break;
                        }

                        return status;
                }

                /// <summary>
                /// Determines the appropriate value to use based on the input string.
                /// </summary>
                /// <returns>The value.</returns>
                /// <param name="varname">Variable name or string representing an integer.</param>
                /// <param name="symbols">Symbol table.</param>
                private static int ValueFromName(string varname, IDictionary<string, int> symbols)
                {
                        int result = 0;

                        if (char.IsLetter(varname[0]))
                        {
                                result = symbols[varname];
                        }
                        else
                        {
                                int.TryParse(varname, out result);
                        }

                        return result;
                }

                /// <summary>
                /// Match the specified target with input.
                /// </summary>
                /// <param name="target">Target string.</param>
                /// <param name="input">Input string.</param>
                /// <returns>><c>true</c> if the strings match; otherwise, <c>false</c>.</returns>
                private static bool Match(string target, string input)
                {
                        return target == input;
                }

                /// <summary>
                /// Matches that the input string is a valid variable name.
                /// </summary>
                /// <returns><c>true</c>, if variable name was matched, <c>false</c> otherwise.</returns>
                /// <param name="name">Candidate variable name.</param>
                private static bool MatchVariable(string name)
                {
                        bool goodname = true;
                        foreach (char c in name)
                        {
                                goodname &= char.IsLetterOrDigit(c);
                        }

                        return goodname;
                }

                /// <summary>
                /// Matches that the input string is a variable or number.
                /// </summary>
                /// <returns><c>true</c>, if variable or number was matched, <c>false</c> otherwise.</returns>
                /// <param name="name">Candidate variable name or numerical string.</param>
                private static bool MatchVariableOrNumber(string name)
                {
                        bool goodname = true;

                        if (char.IsLetter(name[0]))
                        {
                                return Statement.MatchVariable(name);
                        }

                        foreach (char c in name)
                        {
                                goodname &= char.IsDigit(c);
                        }

                        return goodname;
                }

                /// <summary>
                /// Matches an arithmetic operator.
                /// </summary>
                /// <returns><c>true</c>, if arithmetic operator was matched, <c>false</c> otherwise.</returns>
                /// <param name="op">The operation.</param>
                private static bool MatchArithmetic(string op)
                {
                        var ops = new List<string>() { "+", "-", "*", "/", "%" };
                        return ops.Contains(op);
                }

                /// <summary>
                /// Matches a boolean operator.
                /// </summary>
                /// <returns><c>true</c>, if boolean operator was matched, <c>false</c> otherwise.</returns>
                /// <param name="op">The operation.</param>
                private static bool MatchBoolean(string op)
                {
                        var ops = new List<string>() { "<", "<=", "=", ">=", ">", "!=" };
                        return ops.Contains(op);
                }

                /// <summary>
                /// Evaluates the expression.
                /// </summary>
                /// <returns>The expression.</returns>
                /// <param name="symbols">Symbol table.</param>
                private int EvaluateExpression(Dictionary<string, int> symbols)
                {
                        string op1name, op2name, oper;
                        int op1, op2, val = 0;

                        op1name = this.elements[LexicalElement.LeftOperand];
                        op2name = this.elements[LexicalElement.RightOperand];
                        oper = this.elements[LexicalElement.Operator];
                        op1 = Statement.ValueFromName(op1name, symbols);
                        op2 = Statement.ValueFromName(op2name, symbols);

                        switch (oper)
                        {
                        case "+":
                                val = op1 + op2;
                                break;
                        case "-":
                                val = op1 - op2;
                                break;
                        case "*":
                                val = op1 * op2;
                                break;
                        case "/":
                                if (op2 == 0)
                                {
                                        val = 0;
                                }
                                else
                                {
                                        val = op1 / op2;
                                }
                                break;
                        case "%":
                                if (op2 == 0)
                                {
                                        val = 0;
                                }
                                else
                                {
                                        val = op1 % op2;
                                }
                                break;
                        case "<":
                                val = op1 < op2 ? 1 : 0;
                                break;
                        case "<=":
                                val = op1 <= op2 ? 1 : 0;
                                break;
                        case "=":
                                val = op1 == op2 ? 1 : 0;
                                break;
                        case ">=":
                                val = op1 >= op2 ? 1 : 0;
                                break;
                        case ">":
                                val = op1 > op2 ? 1 : 0;
                                break;
                        case "!=":
                                val = op1 != op2 ? 1 : 0;
                                break;
                        }

                        return val;
                }
        }
}