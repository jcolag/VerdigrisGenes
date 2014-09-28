VerdigrisGenes
==============

_VerdigrisGenes_ is for experiments with genetic programming.

I've had a minor interest in genetic programming for a long time, but have never been impressed by the examples.  Demonstrations always seem to conflate their "DNA" with the program, neglecting the fact that programs are the _organisms_, in that both have a _structure_ without which the result is non-viable.

That is, I have yet to see a genetic programming example that started with the assumption that all programs would be valid.  Most (maybe I'm behind the times) appear to just encode program elements as a DNA-like string and randomly generating the strings.  So, it takes forever to print `Hello, World!` and that's about the end of the example, because most of the programs end up looking like random garbage.  (It's worse when the program is encoded as machine instructions or a pseudo-assembler, where structure can't even be sensibly tested.)

Therefore, _VerdigrisGenes_ will attempt a more structured form, starting from the premise that programs __will be__ valid.

Status
------

At this time, the program can read a grammar file (in a pseudo-BNF) and randomly generate a program.  Well, it can do so provided that the language is relatively simple.  If there's too much complexity, the algorithm is recursive-descent, so it can easily overflow the stack.

The next step is going to remove the variables from syntax and build a symbol table, so that we can guarantee a minimum level of _semantic_ correctness as well as syntax.

Example
-------

To reiterate the status, _VerdigrisGenes_ only generates a random program, at this point, so there isn't all that much to show.  However, given a stripped-down imperative programming language like this:

    Program ::= Statement
              | Statement
                Program
    
    Statement ::= input v Number
                | output v Number
                | let v Number <- Expr
                | Loop
                | Conditional
    
    Loop ::= while Comparison
               Program
             end while
    
    Conditional ::= if Comparison
                      Program
                    end if
    
    Comparison ::= Value Compare Value
    
    Compare ::= >
              | >=
              | =
              | <=
              | <
    
    Expr ::= Value + Value
           | Value - Value
           | Value * Value
           | Value / Value
    
    Value ::= Number
            | v Number
    
    Number ::= Digit
             | Digit Number
    
    Digit ::= 1
            | 2
            | 3
            | 4
            | 5
            | 6
            | 7
            | 8
            | 9
            | 0

The original version of this language had fuller expressions, but regularly bombed out with a stack overflow.  Eventually, I'll make the program non-recursive, but simpler expressions was more expedient.

It comes out pretty poorly-formatted, of course, but a reformatted result looks something like...

    while v62985 = v7
        if v2 < 01
            while v4 > 1339
                let v1 <- 0 * v50
            end while
            while v7 > 9
                input v61
            end while
            while 4436 <= v8
                output v4
                input v2765
            end while
            let v3 <- 7 - v5
        end if
        output v6
        while v6 = 780
            if v8 > v07
                input v407
                if 0 = v8
                    output v86
                    let v8 <- v1085 * 7
                end if
            end if
        end while
    end while

Because we have no sense of variables, this is obviously nonsense.  However, it has the virtue right off the bat of clearly being syntactically valid.  (Technically, it isn't, because the numbers are generated one character at a time and have spaces in them, but the goal is to move the values into the genes.)

Genetics
--------

The plan will undoubtedly evolve as I fiddle with the idea, but my current plan is to use six "strands" of genetic information, each responsible for a different part of the program.

 - Choice of statements, likely modified by whether there are (initialized) variables available

 - Navigation through grammar inside each statement, possibly one for each _kind_ of statement

 - Choice of operations in expressions

 - Numerical constants, rather than generating them syntactically

 - Variable references, again, rather than generating syntactically

 - Block lengths, which aren't strictly necessary, but seem like a potentially-useful thing to track

I suspect these will each be sequences of numbers cut somehow (modulus seems too unpredictable) to the range that makes sense in context.

Future Path
-----------

I'll mostly go whereever there seems to be something interesting, but things that come to mind immediately include.

 - [ ] Implement a symbol table and variable declaration.

 - [ ] Hook the selection of program elements to genes.

 - [ ] Write an interpreter for the language.

 - [ ] "Pretty-print" the output programs.

 - [ ] Build a test harness that can take input-output pairs to determine a fitness value.

 - [ ] Determine a reproductive model for the programs.  Single- or double-stranded?  Inherit by strand or by gene?  If double-stranded, a dominance model is necessary.

 - [ ] Build a mutation model that fits everything else.

So...a lot.
