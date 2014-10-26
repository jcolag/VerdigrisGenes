// <copyright file="StatementType.cs" company="John Colagioia">
//     John.Colagioia.net. Licensed under the GPLv3
// </copyright>
// <author>John Colagioia</author>
namespace Interpreter
{
        /// <summary>
        /// Statement type.
        /// </summary>
        public enum StatementType
        {
                /// <summary>
                /// Unknown statements.
                /// </summary>
                Unknown,

                /// <summary>
                /// Variable definition statements.
                /// </summary>
                Define,

                /// <summary>
                /// Input statements.
                /// </summary>
                Input,

                /// <summary>
                /// Output statements.
                /// </summary>
                Output,

                /// <summary>
                /// Assignment statements.
                /// </summary>
                Assign,

                /// <summary>
                /// Looping (WHILE) statements.
                /// </summary>
                Loop,

                /// <summary>
                /// Conditional (IF) statements.
                /// </summary>
                Conditional,

                /// <summary>
                /// End (IF and WHILE) statements.
                /// </summary>
                End,

                /// <summary>
                /// Program comments.
                /// </summary>
                Comment
        }
}