using Antlr4.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DialogAddin.WordLang
{
    public class ParserErrorListener : BaseErrorListener
    {
        public List<GeneralError> Errors { get; set; } = new List<GeneralError>();

        public bool AnyErrors { get { return Errors.Count > 0; } }

        public override void SyntaxError(IRecognizer recognizer, IToken offendingSymbol, int line, int charPositionInLine, string msg, RecognitionException e)
        {
            
            Errors.Add(new GeneralError()
            {
                Line = line,
                CharPosition = charPositionInLine,
                Message = msg
            });
        }
    }

    public class LexerErrorListener : IAntlrErrorListener<int>
    {
        public List<GeneralError> Errors { get; set; } = new List<GeneralError>();

        public bool AnyErrors { get { return Errors.Count > 0; } }


        public void SyntaxError(IRecognizer recognizer, int offendingSymbol, int line, int charPositionInLine, string msg, RecognitionException e)
        {
            Errors.Add(new GeneralError()
            {
                Line = line,
                CharPosition = charPositionInLine,
                Message = msg
            });
        }
    }

    public struct GeneralError
    {
        public int Line;
        public int CharPosition;
        public string Message;
    }

}
