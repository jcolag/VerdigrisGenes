using System;

namespace VerdigrisGenes
{
        public class GrammarExpression
        {
                private readonly string expression;

                private readonly string condition;

                private readonly int initialize;

                public GrammarExpression(string expr, string cond, int init)
                {
                        this.expression = expr;
                        this.condition = cond;
                        this.initialize = init;
                }

                public string Expression
                {
                        get
                        {
                                return expression;
                        }
                }

                public string Condition
                {
                        get
                        {
                                return condition;
                        }
                }

                public int Initialize
                {
                        get
                        {
                                return initialize;
                        }
                }
        }
}

