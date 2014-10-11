// <copyright file="Variable.cs" company="John Colagioia">
//     John.Colagioia.net. Licensed under the GPLv3
// </copyright>
// <author>John Colagioia</author>
namespace VerdigrisGenes
{
        using System;

        /// <summary>
        /// Program Variable.
        /// </summary>
        public class Variable
        {
                /// <summary>
                /// The variable name.
                /// </summary>
                private readonly string name;

                /// <summary>
                /// Has the variable been initialized?
                /// </summary>
                private bool initialized = false;

                /// <summary>
                /// Are we running a statement that will initialize the variable?
                /// </summary>
                private bool toInitialize = false;

                /// <summary>
                /// Initializes a new instance of the <see cref="VerdigrisGenes.Variable"/> class.
                /// </summary>
                /// <param name="name">Name of the variable.</param>
                public Variable(string name)
                {
                        this.name = name;
                }

                /// <summary>
                /// Gets the variable's name.
                /// </summary>
                /// <value>The name.</value>
                public string Name
                {
                        get
                        {
                                return this.name;
                        }
                }

                /// <summary>
                /// Gets or sets a value indicating whether this <see cref="VerdigrisGenes.Variable"/> is initialized.
                /// </summary>
                /// <value><c>true</c> if initialized; otherwise, <c>false</c>.</value>
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

                /// <summary>
                /// Sets a value indicating whether this <see cref="VerdigrisGenes.Variable"/> will be initialized.
                /// </summary>
                /// <value><c>true</c> if to initialize; otherwise, <c>false</c>.</value>
                public bool ToInitialize
                {
                        set
                        {
                                this.toInitialize = value;
                        }
                }

                /// <summary>
                /// Update this instance.
                /// </summary>
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