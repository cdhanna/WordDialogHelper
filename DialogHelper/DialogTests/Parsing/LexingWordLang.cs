using Antlr4.Runtime;
using DialogAddin.WordLang;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DialogTests.Parsing
{
    using L = WordLangLexer;
    [TestClass]
    public class LexingWordLang
    {

        public LexerErrorListener GetLexerErrors(string src)
        {
            var lexerErrorHandler = new LexerErrorListener();

            var inputStream = new AntlrInputStream(src);
            var lexer = new WordLangLexer(inputStream);
            lexer.AddErrorListener(lexerErrorHandler);

            var tokenStream = new CommonTokenStream(lexer);
            var tokens = lexer.GetAllTokens();
            return lexerErrorHandler;
        }

        public Tuple<ParserErrorListener, LexerErrorListener> GetParseErrors(string src)
        {
            var lexerErrorHandler = new LexerErrorListener();

            var inputStream = new AntlrInputStream(src);
            var lexer = new WordLangLexer(inputStream);
            lexer.AddErrorListener(lexerErrorHandler);

            var tokenStream = new CommonTokenStream(lexer);

            var parseErrorHandler = new ParserErrorListener();
            var parser = new WordLangParser(tokenStream);
            parser.AddErrorListener(parseErrorHandler);

            var ctx = parser.prog();

            return new Tuple<ParserErrorListener, LexerErrorListener>(parseErrorHandler, lexerErrorHandler);
        }

        [TestMethod]
        public void ValidWordSimple()
        {
            var src = @"sample
";
            var listener = GetLexerErrors(src);

            Assert.IsFalse(listener.AnyErrors);
        }

        [TestMethod]
        public void InvalidWord_BadChar()
        {
            var src = @"sample&";
            var listener = GetLexerErrors(src);

            Assert.IsTrue(listener.AnyErrors);
        }

        [TestMethod]
        public void TemplateDialog()
        {
            var src = @"A Nifty Rule
 dispLaYaS 
conditions
x is y
dialogs
:plr
hello world, my name is {'Mr. ' + actor.name}
outcomes
set a to b
";
            var program = new WordLangResults(src);
            Assert.AreEqual(false, program.LexerErrors.AnyErrors);
            Assert.AreEqual(false, program.ParserErrors.AnyErrors);
            var v = new ProgramToTree();

            //var line = v.Visit(program.ProgramContext.rule(0).dialogs().dialogLine(0));

            //Assert.AreEqual("(dialog speaker=[plr] parts=[(freetext line=[hello world, my name is ]),(templ expr=[(+ 'Mr. ' actor.name)])])", line);

        }

        [TestMethod]
        public void OptionalDisplayAs()
        {
            var src = @"A Nifty Rule
 dispLaYaS 
conditions
x is y
dialogs
:plr
hello world
outcomes
set a to b
";
            var program = new WordLangResults(src);
            Assert.AreEqual(false, program.ParserErrors.AnyErrors);

        }

        [TestMethod]
        public void DialogHasNumbers()
        {
            var src = @"A Nifty Rule
 dispLaYaS 
conditions
x is y
dialogs
:plr
this is the 5th time I ate 3 apple pies
outcomes
set a to b
";
            var program = new WordLangResults(src);
            Assert.AreEqual(false, program.LexerErrors.AnyErrors);
            Assert.AreEqual(false, program.ParserErrors.AnyErrors);

        }

        [TestMethod]
        public void OutcomesCanPass()
        {
            var src = @"A Nifty Rule
 dispLaYaS 
conditions
x is y
dialogs
:plr
this is the 5th time I ate 3 apple pies
outcomes
pass
";
            var program = new WordLangResults(src);
            Assert.AreEqual(false, program.LexerErrors.AnyErrors);
            Assert.AreEqual(false, program.ParserErrors.AnyErrors);

        }


        [TestMethod]
        public void ValidNegation_IsNot()
        {
            var src = @"A Nifty Rule
 dispLaYaS
monkey
conditions
x is not y
dialogs
:plr
hello world
outcomes
set a to b
";
            var program = new WordLangResults(src);
            Assert.AreEqual(false, program.ParserErrors.AnyErrors);



        }

        [TestMethod]
        public void ValidNegation_NotIs()
        {
            var src = @"A Nifty Rule
 dispLaYaS
monkey
conditions
x not is y
dialogs
:plr
hello world not
outcomes
set a to b
";
            var program = new WordLangResults(src);
            Assert.AreEqual(false, program.ParserErrors.AnyErrors);
        }

        [TestMethod]
        public void ValidNegation_NotIsNoSpace()
        {
            var src = @"A Nifty Rule
 dispLaYaS
monkey
conditions
x notis y
dialogs
:plr
hello world
outcomes
set a to b
";
            var program = new WordLangResults(src);
            Assert.AreEqual(false, program.ParserErrors.AnyErrors);
        }

//        [TestMethod]
//        public void ValidNegation_IsNotNoSpace()
//        {
//            var src = @"A Nifty Rule
// dispLaYaS
//monkey
//conditions
//x isnot y
//dialogs
//:plr
//hello world
//outcomes
//set a to b
//";
//            var program = new WordLangResults(src);
//            Assert.AreEqual(false, program.ParserErrors.AnyErrors);
//        }

        [TestMethod]
        public void ValidNegation_EqualNot()
        {
            var src = @"A Nifty Rule
 dispLaYaS
monkey
conditions
x =! y
dialogs
:plr
hello world
outcomes
set a to b
";
            var program = new WordLangResults(src);
            Assert.AreEqual(false, program.ParserErrors.AnyErrors);
        }

        [TestMethod]
        public void ValidNegation_EqualNotWSpace()
        {
            var src = @"A Nifty Rule
 dispLaYaS
monkey
conditions
x = ! y
dialogs
:plr
hello world
outcomes
set a to b
";
            var program = new WordLangResults(src);
            Assert.AreEqual(false, program.ParserErrors.AnyErrors);
        }

        [TestMethod]
        public void ValidNegation_GreaterThan()
        {
            var src = @"A Nifty Rule
 dispLaYaS
monkey
conditions
x > y
dialogs
:plr
hello world
outcomes
set a to b
";
            var program = new WordLangResults(src);
            Assert.AreEqual(false, program.ParserErrors.AnyErrors);
        }

        [TestMethod]
        public void ValidNegation_NotGreaterThan()
        {
            var src = @"A Nifty Rule
 dispLaYaS
monkey
conditions
x !> y
dialogs
:plr
hello world
outcomes
set a to b
";
            var program = new WordLangResults(src);
            Assert.AreEqual(false, program.ParserErrors.AnyErrors);
        }
        [TestMethod]
        public void ValidNegation_NotGreaterThanWord()
        {
            var src = @"A Nifty Rule
 dispLaYaS
monkey
conditions
x not > y
dialogs
:plr
hello world
outcomes
set a to b
";
            var program = new WordLangResults(src);
            Assert.AreEqual(false, program.ParserErrors.AnyErrors);
        }

        [TestMethod]
        public void ValidNegation_NotGreaterThanWordNoSpace()
        {
            var src = @"A Nifty Rule
 dispLaYaS
monkey
conditions
x not> y
dialogs
:plr
hello world
outcomes
set a to b
";
            var program = new WordLangResults(src);
            Assert.AreEqual(false, program.ParserErrors.AnyErrors);
        }
        [TestMethod]
        public void ValidNegation_NotGreaterThanWordNoSpaceFlipped()
        {
            var src = @"A Nifty Rule
 dispLaYaS
monkey
conditions
x >not y
dialogs
:plr
hello world
outcomes
set a to b
";
            var program = new WordLangResults(src);
            Assert.AreEqual(false, program.ParserErrors.AnyErrors);
        }

        [TestMethod]
        public void ConditionGreatherThan0()
        {
            var src = @"A Nifty Rule
 dispLaYaS
monkey
conditions
x > 0
dialogs
:plr
hello world
outcomes
set a to b
";
            var program = new WordLangResults(src);
            Assert.AreEqual(false, program.ParserErrors.AnyErrors);
            Assert.AreEqual(false, program.LexerErrors.AnyErrors);
        }

        [TestMethod]
        public void DialogCanHaveBoldMarkup()
        {
            var src = @"A Nifty Rule
 dispLaYaS
monkey
conditions
x > 0
dialogs
:plr
hello <b> world </b>
outcomes
set a to b
";
            var program = new WordLangResults(src);
            Assert.AreEqual(false, program.LexerErrors.AnyErrors);
            Assert.AreEqual(false, program.ParserErrors.AnyErrors);
        }

        [TestMethod]
        public void CanDialogHaveEllipses()
        {
            var src = @"A Nifty Rule
 dispLaYaS
monkey
conditions
x > 0
dialogs
:plr
...hello... there...
outcomes
set a to b
";
            var program = new WordLangResults(src);
            Assert.AreEqual(false, program.LexerErrors.AnyErrors);
            Assert.AreEqual(false, program.ParserErrors.AnyErrors);
        }



        [TestMethod]
        public void DialogCanHaveColorMarkup()
        {
            var src = @"A Nifty Rule
 dispLaYaS
monkey
conditions
x > 0
dialogs
:plr
hello <color='red'> world </color>
outcomes
set a to b
";
            var program = new WordLangResults(src);
            Assert.AreEqual(false, program.LexerErrors.AnyErrors);
            Assert.AreEqual(false, program.ParserErrors.AnyErrors);
        }

        [TestMethod]
        public void DialogCanHaveColorHexCode()
        {
            var src = @"A Nifty Rule
 dispLaYaS
monkey
conditions
x > 0
dialogs
:plr
hello <color=#ff000000> world </color>
outcomes
set a to b
";
            var program = new WordLangResults(src);
            Assert.AreEqual(false, program.LexerErrors.AnyErrors);
            Assert.AreEqual(false, program.ParserErrors.AnyErrors);
        }



        [TestMethod]
        public void ValidLiteralBool()
        {
            var src = @"A Nifty Rule
 dispLaYaS
monkey
conditions
x = true
dialogs
:plr
hello world
outcomes
set a to b
";
            var program = new WordLangResults(src);
            // Assert.AreEqual(L.TRUE, program.Tokens[14].Type);
            Assert.AreEqual(false, program.LexerErrors.AnyErrors);
            Assert.AreEqual(false, program.ParserErrors.AnyErrors);
        }

        [TestMethod]
        public void ValidLiteralString()
        {
            var src = @"A Nifty Rule
 dispLaYaS
monkey
conditions
x = 'brainz'
dialogs
:plr
hello world
outcomes
set a to b
";
            var program = new WordLangResults(src);
            Assert.AreEqual(false, program.LexerErrors.AnyErrors);
            Assert.AreEqual(false, program.ParserErrors.AnyErrors);
        }

        [TestMethod]
        public void ValidLiteralStringWithSpace()
        {
            var src = @"A Nifty Rule
 dispLaYaS
monkey
conditions
x = ' '
dialogs
:plr
hello world
outcomes
set a to b
";
            var program = new WordLangResults(src);
            Assert.AreEqual(false, program.LexerErrors.AnyErrors);
            Assert.AreEqual(false, program.ParserErrors.AnyErrors);
        }

        [TestMethod]
        public void ValidLiteralStringWithSpaces()
        {
            var src = @"A Nifty Rule
 dispLaYaS
monkey
conditions
x = 'I Am Cool'
dialogs
:plr
hello world
outcomes
set a to b
";
            var program = new WordLangResults(src);
            Assert.AreEqual(false, program.LexerErrors.AnyErrors);
            Assert.AreEqual(false, program.ParserErrors.AnyErrors);
        }

        [TestMethod]
        public void ValidLiteralStringEmpty()
        {
            var src = @"A Nifty Rule
 dispLaYaS
monkey
conditions
x = ''
dialogs
:plr
hello world
outcomes
set a to b
";
            var program = new WordLangResults(src);
            Assert.AreEqual(false, program.LexerErrors.AnyErrors);
            Assert.AreEqual(false, program.ParserErrors.AnyErrors);
        }

        [TestMethod]
        public void ValidRefs()
        {
            var src = @"A Nifty Rule
 dispLaYaS
monkey
conditions
player.name is enemy.archNemesis
dialogs
:plr
hello world
outcomes
set a to b
";
            var program = new WordLangResults(src);
            Assert.AreEqual(false, program.ParserErrors.AnyErrors);
        }

        [TestMethod]
        public void ValidNegation_Less()
        {
            var src = @"A Nifty Rule
 dispLaYaS
monkey
conditions
x < y
dialogs
:plr
hello world
outcomes
set a to b
";
            var program = new WordLangResults(src);

            var names = L.tokenNames;

            var expectedTokenTypes = new int[]
            {
                L.NAME, L.WHITESPACE, L.NAME, L.WHITESPACE, L.NAME, L.NEWLINE,
                L.DISPLAYAS, L.NEWLINE,
                L.NAME, L.NEWLINE,
                L.CONDITIONS, L.NEWLINE,
                L.NAME,L.WHITESPACE, L.LESSTHAN, L.WHITESPACE, L.NAME
            };
            for (var i = 0; i < Math.Min(expectedTokenTypes.Length, program.Tokens.Count); i++)
            {
                var expectedType = expectedTokenTypes[i];
                var actualType = program.Tokens[i].Type;
               
                Assert.AreEqual(expectedType, actualType);
            }


            Assert.AreEqual(false, program.ParserErrors.AnyErrors);
        }

        [TestMethod]
        public void ValidOutcomeSetter()
        {
            var src = @"A Nifty Rule
 dispLaYaS
monkey
conditions
x < y
dialogs
:plr
hello world
outcomes
sEt  Egg.NoG To 'peanuts'
";
            var program = new WordLangResults(src);

            var names = L.tokenNames;

            var expectedTokenTypes = new int[]
            {
                L.NAME, L.WHITESPACE, L.NAME, L.WHITESPACE, L.NAME, L.NEWLINE,
                L.DISPLAYAS, L.NEWLINE,
                L.NAME, L.NEWLINE,
                L.CONDITIONS, L.NEWLINE,
                L.NAME,L.WHITESPACE, L.LESSTHAN, L.WHITESPACE, L.NAME, L.NEWLINE,
                L.DIALOGS, L.NEWLINE,
                L.COLON, L.NAME, L.NEWLINE,
                L.NAME, L.WHITESPACE, L.NAME, L.NEWLINE,
                L.OUTCOMES, L.NEWLINE,
                L.SET, L.NAME, L.DOT, L.NAME, L.TO, L.QUOTE, L.NAME, L.QUOTE
            };
            for (var i = 0; i < Math.Min(expectedTokenTypes.Length, program.Tokens.Count); i++)
            {
                var expectedType = expectedTokenTypes[i];
                var actualType = program.Tokens[i].Type;

                Assert.AreEqual(expectedType, actualType);
            }

            Assert.AreEqual(false, program.ParserErrors.AnyErrors);
        }

        [TestMethod]
        public void ValidOutcomeSetterBool()
        {
            var src = @"A Nifty Rule
 dispLaYaS
monkey
conditions
x < y
dialogs
:plr
hello world
outcomes
sEt  Egg.NoG To fAlSE
";
            var program = new WordLangResults(src);

            var names = L.tokenNames;

            var expectedTokenTypes = new int[]
            {
                L.NAME, L.WHITESPACE, L.NAME, L.WHITESPACE, L.NAME, L.NEWLINE,
                L.DISPLAYAS, L.NEWLINE,
                L.NAME, L.NEWLINE,
                L.CONDITIONS, L.NEWLINE,
                L.NAME,L.WHITESPACE, L.LESSTHAN, L.WHITESPACE, L.NAME, L.NEWLINE,
                L.DIALOGS, L.NEWLINE,
                L.COLON, L.NAME, L.NEWLINE,
                L.NAME, L.WHITESPACE, L.NAME, L.NEWLINE,
                L.OUTCOMES, L.NEWLINE,
                L.SET, L.NAME, L.DOT, L.NAME, L.TO, L.FALSE
            };
            for (var i = 0; i < Math.Min(expectedTokenTypes.Length, program.Tokens.Count); i++)
            {
                var expectedType = expectedTokenTypes[i];
                var actualType = program.Tokens[i].Type;

                Assert.AreEqual(expectedType, actualType);
            }

            Assert.AreEqual(false, program.ParserErrors.AnyErrors);
        }
        [TestMethod]
        public void ValidOutcomeModifier()
        {
            var src = @"A Nifty Rule
 dispLaYaS
monkey
conditions
x < y
dialogs
:plr
hello world
outcomes
 set tuna.toast to 5
";
            var program = new WordLangResults(src);

            var names = L.tokenNames;

            //var expectedTokenTypes = new int[]
            //{
            //    L.NAME, L.WHITESPACE, L.NAME, L.WHITESPACE, L.NAME, L.NEWLINE,
            //    L.DISPLAYAS, L.NEWLINE,
            //    L.NAME, L.NEWLINE,
            //    L.CONDITIONS, L.NEWLINE,
            //    L.NAME,L.WHITESPACE, L.LESSTHAN, L.WHITESPACE, L.NAME, L.NEWLINE,
            //    L.DIALOGS, L.NEWLINE,
            //    L.COLON, L.NAME, L.NEWLINE,
            //    L.NAME, L.WHITESPACE, L.NAME, L.NEWLINE,
            //    L.OUTCOMES, L.NEWLINE,
            //    L.MODIFY, L.NAME, L.DOT, L.NAME, L.BY, L.INTEGER
            //};
            //for (var i = 0; i < Math.Min(expectedTokenTypes.Length, program.Tokens.Count); i++)
            //{
            //    var expectedType = expectedTokenTypes[i];
            //    var actualType = program.Tokens[i].Type;

            //    Assert.AreEqual(expectedType, actualType);
            //}

            Assert.AreEqual(false, program.ParserErrors.AnyErrors);
        }
        [TestMethod]
        public void ValidOutcomeModifierRef()
        {
            var src = @"A Nifty Rule
 dispLaYaS
monkey
conditions
x < y
dialogs
:plr
hello world
outcomes
 set toast.count to player.hunger
";
            var program = new WordLangResults(src);

            var names = L.tokenNames;

          

            Assert.AreEqual(false, program.ParserErrors.AnyErrors);
        }

        [TestMethod]
        public void ValidOutcomeModifierNegative()
        {
            var src = @"A Nifty Rule
 dispLaYaS
monkey
conditions
x < y
dialogs
:plr
hello world
outcomes
 set toast.count to -12
";
            var program = new WordLangResults(src);

            var names = L.tokenNames;

            

            Assert.AreEqual(false, program.ParserErrors.AnyErrors);
        }

        [TestMethod]
        public void ValidOutcomeModifierNegativeRef()
        {
            var src = @"A Nifty Rule
 dispLaYaS
monkey
conditions
x < y
dialogs
:plr
hello world
outcomes
 set globalToast.count to -player.toastCount
";
            var program = new WordLangResults(src);

            var names = L.tokenNames;

            
            Assert.AreEqual(false, program.ParserErrors.AnyErrors);
        }

        [TestMethod]
        public void TestingTemplate()
        {
            var src = @"Rule
DisplayAs
brainz
Conditions
a is b
Dialogs
Outcomes
set a to b
";

            var program = new WordLangResults(src);

            var names = L.tokenNames;

            //var expectedTokenTypes = new int[]
            //{
            //    L.NAME, L.WHITESPACE, L.NAME, L.WHITESPACE, L.NAME, L.NEWLINE,
            //    L.DISPLAYAS, L.NEWLINE,
            //    L.NAME, L.NEWLINE,
            //    L.CONDITIONS, L.NEWLINE,
            //    L.NAME,L.WHITESPACE, L.LESSTHAN, L.WHITESPACE, L.NAME, L.NEWLINE,
            //    L.DIALOGS, L.NEWLINE,
            //    L.COLON, L.NAME, L.NEWLINE,
            //    L.NAME, L.WHITESPACE, L.NAME, L.NEWLINE,
            //    L.OUTCOMES, L.NEWLINE,
            //    L.MODIFY, L.NAME, L.DOT, L.NAME, L.BY, L.MINUS, L.NAME, L.DOT, L.NAME
            //};
            //for (var i = 0; i < Math.Min(expectedTokenTypes.Length, program.Tokens.Count); i++)
            //{
            //    var expectedType = expectedTokenTypes[i];
            //    var actualType = program.Tokens[i].Type;

            //    Assert.AreEqual(expectedType, actualType);
            //}

            Assert.AreEqual(false, program.ParserErrors.AnyErrors);

        }

        [TestMethod]
        public void ValidNegation_NotLess()
        {
            var src = @"A Nifty Rule
 dispLaYaS
monkey
conditions
x !< y
dialogs
:plr
hello world
outcomes
set a to b
";
            var program = new WordLangResults(src);
            Assert.AreEqual(false, program.ParserErrors.AnyErrors);
        }

        [TestMethod]
        public void ValidNegation_NotLessSpace()
        {
            var src = @"A Nifty Rule
 dispLaYaS
monkey
conditions
x ! < y
dialogs
:plr
hello world
outcomes
set a to b
";
            var program = new WordLangResults(src);
            Assert.AreEqual(false, program.ParserErrors.AnyErrors);
        }

        [TestMethod]
        public void ValidNegation_NotLessSpaceWord()
        {
            var src = @"A Nifty Rule
 dispLaYaS
monkey
conditions
x not < y
dialogs
:plr
hello world
outcomes
set a to b
";
            var program = new WordLangResults(src);
            Assert.AreEqual(false, program.ParserErrors.AnyErrors);
        }

        [TestMethod]
        public void ValidNegation_NotLessWord()
        {
            var src = @"A Nifty Rule
 dispLaYaS
monkey
conditions
x not< y
dialogs
:plr
hello world
outcomes
set a to b
";
            var program = new WordLangResults(src);
            Assert.AreEqual(false, program.ParserErrors.AnyErrors);
        }

        [TestMethod]
        public void ValidNegation_NotLessWordFlipped()
        {
            var src = @"A Nifty Rule
 dispLaYaS
monkey
conditions
x <not y
dialogs
:plr
hello world
outcomes
set a to b
";
            var program = new WordLangResults(src);
            Assert.AreEqual(false, program.ParserErrors.AnyErrors);
        }

        [TestMethod]
        public void ValidNegation_NotLessWordSpaceFlipped()
        {
            var src = @"A Nifty Rule
 dispLaYaS
monkey
conditions
x < not y
dialogs
:plr
hello world
outcomes
set a to b
";
            var program = new WordLangResults(src);
            Assert.AreEqual(false, program.ParserErrors.AnyErrors);
        }


        [TestMethod]
        public void ValidNoEndOfLine()
        {
            var src = @"A Nifty Rule
 dispLaYaS 
test
conditions 
x is y
dialogs
:plr
hello world
outcomes
set a to b";
            var program = new WordLangResults(src);
            Assert.AreEqual(false, program.ParserErrors.AnyErrors);

        }

        [TestMethod]
        public void ValidRule()
        {
            var src = @"a nifty to set rule
     displayAs 
   tunafish to set bell tolls rep 
conditions    
yaday.first is not blahblah some more   
    another line of fun = twice the pain 
dialogs
:player 1        
speaking line text set to bell tolls
multi line

:player2
    something meaningful
            outcomes
    set a to b
set a to b again
";
            // if thething is theotherthing:
            // muffin is true

            var program = new WordLangResults(src);
            Assert.AreEqual(false, program.ParserErrors.AnyErrors);


        }

        [TestMethod]
        public void ValidRuleWithNotInText()
        {
            var src = @"a nifty rule
     displayAs 
   tunafish rep 
conditions    
yaday first is blahblah some more   
    another line of fun = twice the pain 
dialogs
:player1        
I am not the villain
multi line
:player2

is this what you think a villain is not



            outcomes
    set a to b
set a to b again
";
            // if thething is theotherthing:
            // muffin is true

            var program = new WordLangResults(src);
            Assert.AreEqual(false, program.ParserErrors.AnyErrors);

        }

    }
}
