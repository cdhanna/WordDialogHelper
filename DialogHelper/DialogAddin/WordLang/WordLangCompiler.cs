using Antlr4.Runtime;
using Dialog;
using DialogAddin.WordLang.AST.ErrorChecks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DialogAddin.WordLang
{
    public class WordLangCompiler
    {

        private List<ASTErrorChecker> _errorChecks = new List<ASTErrorChecker>();
        private ProgramToAST _toAST = new ProgramToAST();
        private ProgramToJson _toJSON = new ProgramToJson();

        public WordLangCompiler()
        {
            _errorChecks.Add(new DuplicateRuleTitle());
        }

        public WordLangCompilerResults Compile(string src, VariableCollection variabels)
        {
            try
            {
                var results = new WordLangCompilerResults();

                var lexerErrorListener = new LexerErrorListener();
                var parserErrorListener = new ParserErrorListener();
                src = src.Replace("‘", "'");
                src = src.Replace("’", "'");
                src = src.Replace("–", "-");


                var inputStream = new AntlrInputStream(src);

                var lexer = new WordLangLexer(inputStream);
                lexer.AddErrorListener(lexerErrorListener);

                var tokenStream = new CommonTokenStream(lexer);
                
                

                var parser = new WordLangParser(tokenStream);
                parser.AddErrorListener(parserErrorListener);

                var program = parser.prog();


                var errorVisitor = new ProgramToErrors(variabels);

                var generalErrors = new List<GeneralError>();
                generalErrors.AddRange(lexerErrorListener.Errors);
                generalErrors.AddRange(parserErrorListener.Errors);

                if (generalErrors.Count == 0)
                {
                    var detectedErrors = errorVisitor.Visit(program);
                    generalErrors.AddRange(detectedErrors);
                    var ast = _toAST.Visit(program) as AST.RuleSet;
                    if (ast == null)
                    {
                        generalErrors.Add(new GeneralError()
                        {
                            Message = "Fatal AST Parse Error."
                        });
                    }
                    else
                    {
                        _errorChecks.ForEach(errorChecker =>
                        {
                            generalErrors.AddRange(errorChecker.Check(ast));
                        });

                        results.AST = ast;
                        results.JSON = _toJSON.Visit(program);
                    }
                }
                results.Errors = generalErrors;

                return results;
            } catch (Exception ex)
            {
                Console.WriteLine("Error occured in compilation" + ex);
                throw ex;
            }
        }

    }

    public class WordLangCompilerResults
    {
        public List<GeneralError> Errors { get; set; }
        public AST.RuleSet AST { get; set; }
        public string JSON { get; set; }
    }
}
