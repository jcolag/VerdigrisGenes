// <copyright file="GrammarExpression.cs" company="John Colagioia">
//     John.Colagioia.net. Licensed under the GPLv3
// </copyright>
// <author>John Colagioia</author>
namespace VerdigrisGenes
{
        using System;

        public class GrammarExpression
        {
                /// <summary>
                /// The expression.
                /// </summary>
                private readonly string expression;

                /// <summary>
                /// Whether the expression will require an initialized variable.
                /// </summary>
                private readonly bool reqValue;

                /// <summary>
                /// Whether the expression will initialize a variable.
                /// </summary>
                private readonly bool initialize;

                /// <summary>
                /// Initializes a new instance of the <see cref="VerdigrisGenes.GrammarExpression"/> class.
                /// </summary>
                /// <param name="expr">Expr.</param>
                /// <param name="needValue">If set to <c>true</c> need value.</param>
                /// <param name="init">If set to <c>true</c> init.</param>
                public GrammarExpression(string expr, bool needValue, bool init)
                {
                        this.expression = expr;
                        this.reqValue = needValue;
                        this.initialize = init;
                }

                /// <summary>
                /// Gets the expression.
                /// </summary>
                /// <value>The expression.</value>
                public string Expression
                {
                        get
                        {
                                return this.expression;
                        }
                }

                /// <summary>
                /// Gets a value indicating whether this <see cref="VerdigrisGenes.GrammarExpression"/> req value.
                /// </summary>
                /// <value><c>true</c> if req value; otherwise, <c>false</c>.</value>
                public bool ReqValue
                {
                        get
                        {
                                return this.reqValue;
                        }
                }

                /// <summary>
                /// Gets a value indicating whether this <see cref="VerdigrisGenes.GrammarExpression"/> is initialize.
                /// </summary>
                /// <value><c>true</c> if initialize; otherwise, <c>false</c>.</value>
                public bool Initialize
                {
                        get
                        {
                                return this.initialize;
                        }
                }
        }
}