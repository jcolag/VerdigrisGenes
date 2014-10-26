// <copyright file="LexicalElement.cs" company="John Colagioia">
//     John.Colagioia.net. Licensed under the GPLv3
// </copyright>
// <author>John Colagioia</author>
namespace Interpreter
{
        /// <summary>
        /// Lexical elements.
        /// </summary>
        public enum LexicalElement
        {
                /// <summary>
                /// The target of an assignment.
                /// </summary>
                Target,

                /// <summary>
                /// The left operand.
                /// </summary>
                LeftOperand,

                /// <summary>
                /// The right operand.
                /// </summary>
                RightOperand,

                /// <summary>
                /// The operator.
                /// </summary>
                Operator
        }
}