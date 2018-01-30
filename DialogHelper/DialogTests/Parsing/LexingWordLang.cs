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
