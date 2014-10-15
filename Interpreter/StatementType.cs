// <copyright file="Interpreter.cs" company="John Colagioia">
//     John.Colagioia.net. Licensed under the GPLv3
// </copyright>
// <author>John Colagioia</author>
namespace Interpreter
{
        using System;

        public enum StatementType
        {
                Unknown,
                Define,
                Input,
                Output,
                Assign,
                Loop,
                Conditional,
                End,
                Comment
        }
}