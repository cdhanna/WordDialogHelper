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
doit
";
            var program = new WordLangResults(src);
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
doit
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
doit
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
doit
";
            var program = new WordLangResults(src);
            Assert.AreEqual(false, program.ParserErrors.AnyErrors);
        }

        [TestMethod]
        public void ValidNegation_IsNotNoSpace()
        {
            var src = @"A Nifty Rule
 dispLaYaS
monkey
conditions
x isnot y
dialogs
:plr
hello world
outcomes
doit
";
            var program = new WordLangResults(src);
            Assert.AreEqual(false, program.ParserErrors.AnyErrors);
        }

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
doit
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
doit
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
doit
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
doit
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
doit
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
doit
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
doit
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
doit
";
            var program = new WordLangResults(src);
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
doit
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
doit
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
doit
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
doit
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
doit
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
doit
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
doit";
            var program = new WordLangResults(src);
            Assert.AreEqual(false, program.ParserErrors.AnyErrors);

        }

        [TestMethod]
        public void ValidRule()
        {
            var src = @"a nifty rule
     displayAs 
   tunafish rep 
conditions    
yaday first is not blahblah some more   
    another line of fun = twice the pain 
dialogs
:player1        
speaking line text
multi line

:player2
    something meaningful
            outcomes
    doit
doit again
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
    doit
doit again
";
            // if thething is theotherthing:
            // muffin is true

            var program = new WordLangResults(src);
            Assert.AreEqual(false, program.ParserErrors.AnyErrors);

        }

    }
}
