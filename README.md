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

    Program ::= Declare Code
    
    Declare ::= define Varlist ;
    
    Varlist ::= @Declare
        | @Declare , Varlist
    
    Code ::= Statement ;
        | Statement ; Code
    
    Statement ::= input @Variable # # x
        | output @Initialized # init
        | let @Variable <- Expr # # x
        | Loop
        | Conditional
    
    Loop ::= while ( Comparison ) ; Code end while
    
    Conditional ::= if ( Comparison ) ; Code end if
    
    Comparison ::= Value Compare Value
    
    Compare ::= >
        | >=
        | =
        | <=
        | <
        | !=
    
    Expr ::= Value Op Value
    
    Op ::= +
        | -
        | *
        | /
        | %
    
    Value ::= @Number
        | @Initialized # init

The original version of this language had fuller expressions, but regularly bombed out with a stack overflow.  Eventually, I'll make the program non-recursive, but simpler expressions was more expedient.

It comes out pretty poorly-formatted, of course, but a reformatted result looks something like...

    define v1 v2 v3 v4 v5 v6 v7 v8 v9
    if ( 786335239 > 755784541 )
        while ( 605517595 <= 38486659 )
            input v8
            let v8 <- v8
            if ( 89742393 > v8 )
                if ( 673648782 >= 1434017890 )
                    output v8
                    let v1 <- 1348233004 + v8
                end if
            end if
            while ( 1951796450 >= v8 )
                let v2 <- 977605682 + v8
            end while
        end while
    end if
    if ( 81789079 <= v1 )
        output v8
        while ( v2 = 928878049 )
            output v2
            if ( v1 < 489661939 )
                if ( 2027429293 = v1 )
                    if ( 1695857986 > 21374349 )
                        if ( 1276753592 < v2 )
                            input v3
                            while ( v3 = v2 )
                                input v7
                                if ( 1084839086 > 103212587 )
                                    output v2
                                end if
                            end while
                            while ( v3 = 1409505925 )
                                while ( v7 >= v7 )
                                    input v3
                                end while
                            end while
                            while ( 1020645337 >= 217506125 )
                                output v8
                                output v2
                                if ( v3 >= 651753342 )
                                    let v7 <- v3 * 1516917728
                                end if
                                input v4
                            end while
                        end if
                        input v5
                        if ( 1022533319 <= 959818496 )
                            output v5
                        end if
                    end if
                    input v5
                end if
            end if
        end while
    end if
    if ( 2143841460 = 650882524 )
        let v1 <- v8
        input v5
    end if
    if ( 1796686360 <= v3 )
        if ( v5 <= 1498654852 )
            output v1
            output v3
        end if
        output v4
        if ( 1573723323 < v1 )
            while ( 1646100152 = 41144584 )
                input v5
                input v4
            end while
        let v3 <- 1931910887 - 763085573
        end if
    end if
    output v7
    while ( v2 >= 1621219584 )
        let v2 <- v2
    end while
    if ( 2069552253 <= v4 )
        input v5
        if ( v2 < 1441526126 )
            while ( v8 >= 636232699 )
                let v3 <- 1662628483
                while ( v3 > v7 )
                    output v4
                end while
            end while
        end if
        while ( v3 >= 37273055 )
            output v2
            output v2
            let v8 <- 976603404
        end while
        if ( v1 < 1947080969 )
            input v3
            input v9
            output v3
        end if
    end if
    while ( 364908657 <= 385470572 )
        input v2
    end while
    output v1

This is obviously nonsensical.  However, it has the virtue right off the bat of clearly being syntactically valid.

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

Right now, due to the architecture of the program, there are only four chromosomes.

 - Numerical constants

 - Variable references

 - _Initialized_ variable references

 - Choice of expression in a grammar production

This is primarily because the grammar currently doesn't know anything about what _kind_ of production it is, so the software can't easily determine whether we're looking at a statement-level choice or not.  By contrast, since the variables and numbers are handled separately, this already has a special code path.

The effect is that a genome file such as this...

    2 3 5 7 11 13 17 23 29 31 37
    0 3 6 0 3 6 0 3 6 0 3 6 0 3 6
    0 0 1 0 0 1 0 0 1 0 0 1 0 0 1
    0 0 1 1 1 1 1 1 1 1 0 1 3 0 0 0 1 0 0 1 0 0 0 3 0 0 0 1 1 1 4 0 0 1 3 0 1 2 1 0 2 1 0 4 0 0 0 3 0 0 1 0 4 0 0 0 0 1 1 0 0 1

...reliably produces...

    define v1 v2 v3 v4 v5 v6 v7 v8 v9
    if ( 2 >= 3 )
        let v1 <- 5
    end if
    while ( 7 >= v1 )
        if ( v1 <= 11 )
            let v4 <- 13 * v1
            if ( 17 <= 23 )
                output v1
            end if
        end if
        if ( 29 > v1 )
            input v7
            output v4
        end if
    end while

Or at least something similar, subject to the precise grammar used.  Change the grammar (which I have since writing this), and it becomes almost an entirely different program.

Future Path
-----------

I'll mostly go whereever there seems to be something interesting, but things that come to mind immediately include.

 - [X] Implement a symbol table and variable declaration.

 - [X] Hook the selection of program elements to genes.

 - [X] Write an interpreter for the language.

 - [ ] Increase the scope/context sensitivity of code generation, in particular loop bodies changing loop conditions.  Obviously, we can't evaluate the fitness of infinite loops.

 - [ ] In cases where infinite loops can't be avoided, a maximum number of instructions or iterations might be worth instituting.

 - [ ] "Pretty-print" the output programs.

 - [X] Build a test harness that can take input-output pairs to determine a fitness value.

 - [ ] Determine a reproductive model for the programs.  Single- or double-stranded?  Inherit by strand or by gene?  If double-stranded, a dominance model is necessary.

 - [ ] Build a mutation model that fits everything else.

So...a lot.
