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
        public void ValidRule()
        {
            var src = @"a nifty rule
displayAs 
tunafish rep
conditions    
yaday first is blahblah some more   
another line of fun = twice the pain 

party House
displayAs
toothe paste time
conditions
x is y

";
            // if thething is theotherthing:
            // muffin is true

            var program = new WordLangResults(src);
            var visitor = new WordLangStringVisitor();

            var tree = program.ProgramContext.ToStringTree();

            var result = visitor.Visit(program.ProgramContext);

        }
        

        [TestMethod]
        public void Test()
        {
            var src = "ccccc";

            var parseErrorHandler = new ParserErrorListener();
            var lexerErrorHandler = new LexerErrorListener();

            var inputStream = new AntlrInputStream(src);
            var lexer = new WordLangLexer(inputStream);
            lexer.AddErrorListener(lexerErrorHandler);
            
            var tokenStream = new CommonTokenStream(lexer);

            

            var parser = new WordLangParser(tokenStream);
            parser.AddErrorListener(parseErrorHandler);


            var topContext = parser.prog();
            var v = new WordLangStringVisitor();
            v.Visit(topContext);
            var str = topContext.ToStringTree();

        }
    }
}
