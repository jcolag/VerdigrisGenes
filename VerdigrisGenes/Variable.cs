using System;

namespace VerdigrisGenes
{
        public class Variable
        {
                private readonly string name;

                private bool initialized = false;

                private bool toInitialize = false;

                public Variable(string name)
                {
                        this.name = name;
                }

                public string Name
                {
                        get
                        {
                                return this.name;
                        }
                }

                public bool Initialized
                {
                        get
                        {
                                return this.initialized;
                        }

                        set
                        {
                                this.initialized = value;
                        }
                }

                public bool ToInitialize
                {
                        set
                        {
                                toInitialize = value;
                        }
                }

                public void Update()
                {
                        if (this.toInitialize)
                        {
                                this.initialized = true;
                                this.toInitialize = false;
                        }
                }
        }
}

