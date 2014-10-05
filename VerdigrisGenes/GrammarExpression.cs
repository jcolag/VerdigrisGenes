using System;

namespace VerdigrisGenes
{
        public class GrammarExpression
        {
                private readonly string expression;

                private readonly bool reqValue;

                private readonly bool initialize;

                public GrammarExpression(string expr, bool needValue, bool init)
                {
                        this.expression = expr;
                        this.reqValue = needValue;
                        this.initialize = init;
                }

                public string Expression
                {
                        get
                        {
                                return expression;
                        }
                }

                public bool ReqValue
                {
                        get
                        {
                                return reqValue;
                        }
                }

                public bool Initialize
                {
                        get
                        {
                                return initialize;
                        }
                }
        }
}

