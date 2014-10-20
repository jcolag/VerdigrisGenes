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

                private string text;

                private bool isNestable = false;

                private bool isLoop = false;

                private bool isEnd = false;

                private bool malformed = false;

                private StatementType type = StatementType.Unknown;

                private List<string> varList;

                private Dictionary<LexicalElement, string> elements;

                public Statement()
                {
                        this.nest = new List<Statement>();
                        this.varList = new List<string>();
                        this.elements = new Dictionary<LexicalElement, string>();
                        this.text = string.Empty;
                }

                public Statement(string line)
                {
                        this.nest = new List<Statement>();
                        this.Parse(line);
                        this.varList = new List<string>();
                        this.elements = new Dictionary<LexicalElement, string>();
                        this.text = line;
                }

                public string Text
                {
                        get
                        {
                                return this.text;
                        }
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

                public List<string> VarList
                {
                        get
                        {
                                return this.varList;
                        }
                }

                public bool Valid
                {
                        get
                        {
                                bool ok = !this.malformed;
                                switch (this.type)
                                {
                                case StatementType.Input:
                                case StatementType.Output:
                                        ok &= elements.ContainsKey(LexicalElement.Target);
                                        break;
                                case StatementType.Conditional:
                                case StatementType.Loop:
                                        ok &= elements.ContainsKey(LexicalElement.LeftOperand);
                                        ok &= elements.ContainsKey(LexicalElement.Operator);
                                        ok &= elements.ContainsKey(LexicalElement.RightOperand);
                                        break;
                                case StatementType.Assign:
                                        ok &= elements.ContainsKey(LexicalElement.Target);
                                        ok &= elements.ContainsKey(LexicalElement.LeftOperand);
                                        ok &= elements.ContainsKey(LexicalElement.Operator);
                                        ok &= elements.ContainsKey(LexicalElement.RightOperand);
                                        break;
                                }
                                return ok;
                        }
                }

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

                public void Nest(Statement item)
                {
                        this.nest.Add(item);
                }

                public bool Go(Dictionary<string, int> symbols, Queue<int> inputs, List<int> outputs)
                {
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
                                        }
                                }

                                break;
                        }

                        return status;
                }

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

                private int EvaluateExpression(Dictionary<string, int> symbols)
                {
                        string op1name, op2name, oper;
                        int op1, op2, val = 0;

                        op1name = this.elements[LexicalElement.LeftOperand];
                        op2name = this.elements[LexicalElement.RightOperand];
                        oper = this.elements[LexicalElement.Operator];
                        op1 = Statement.ValueFromName(op1name, symbols);
                        op2 = Statement.ValueFromName(op2name, symbols);

                        switch(oper)
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
                                val = op1 / op2;
                                break;
                        case "%":
                                val = op1 % op2;
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

                private static bool Match(string target, string input)
                {
                        return target == input;
                }

                private static bool MatchVariable(string name)
                {
                        bool goodname = true;
                        foreach (char c in name)
                        {
                                goodname &= char.IsLetterOrDigit(c);
                        }

                        return goodname;
                }

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

                private static bool MatchArithmetic(string op)
                {
                        var ops = new List<string>(){ "+", "-", "*", "/", "%" };
                        return ops.Contains(op);
                }

                private static bool MatchBoolean(string op)
                {
                        var ops = new List<string>(){ "<", "<=", "=", ">=", ">", "!=" };
                        return ops.Contains(op);
                }
        }
}