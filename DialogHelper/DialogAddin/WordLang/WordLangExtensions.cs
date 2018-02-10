using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DialogAddin.WordLang
{
    static class WordLangExtensions
    {
        public static string CombineTokens(this ITerminalNode[] nodes)
        {
            return String.Join(" ", nodes.Select(n => n.GetText()));
        }
        public static string CombineWithCommas(this IEnumerable<string> collection)
        {
            return String.Join(",", collection);
        }
        
        public static GeneralError NewError(this ParserRuleContext ctx, string message)
        {
            return GeneralError.New(ctx.Start.StartIndex, ctx.Stop.StopIndex, message);
        }
    }
}
