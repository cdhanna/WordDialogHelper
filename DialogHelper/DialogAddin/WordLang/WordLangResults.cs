using Antlr4.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DialogAddin.WordLang
{
    public class WordLangResults
    {

        public ParserErrorListener ParserErrors { get; set; } = new ParserErrorListener();
        public LexerErrorListener LexerErrors { get; set; } = new LexerErrorListener();

        public WordLangParser.ProgContext ProgramContext { get; set; }
        public IList<IToken> Tokens { get; private set; }
        public WordLangResults(string src)
        {
            var inputStream = new AntlrInputStream(src);
            var lexer = new WordLangLexer(inputStream);
            lexer.AddErrorListener(LexerErrors);

            var tokenInputStream = new AntlrInputStream(src);
            var lexerForTokens = new WordLangLexer(tokenInputStream);
            Tokens = lexerForTokens.GetAllTokens();

            var tokenStream = new CommonTokenStream(lexer);

            var parser = new WordLangParser(tokenStream);
            parser.AddErrorListener(ParserErrors);

            ProgramContext = parser.prog();
        }

    }
}
