using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antlr4.Runtime.Misc;

namespace DialogAddin.WordLang
{
    public class ProgramToJson : WordLangBaseVisitor<string>
    {

        public override string VisitProg([NotNull] WordLangParser.ProgContext context)
        {
            var rules = context.rule()
                .Select(ctx => Visit(ctx))
                .CombineWithCommas()
                ;
            return $"[{rules}]";
        }

        public override string VisitRule([NotNull] WordLangParser.RuleContext context)
        {
            var title = Visit(context.ruleTitle());
            var displayAs = Visit(context.displayAs());
            var conditions = Visit(context.conditions());
            var dialogs = Visit(context.dialogs());
            var outcomes = Visit(context.outcomes());
            return $"{{\"title\":{title},\"displayAs\":{displayAs},\"conditions\":{conditions},\"dialogs\":{dialogs},\"outcomes\":{outcomes}}}";
        }

        public override string VisitOutcomes([NotNull] WordLangParser.OutcomesContext context)
        {
            var outcomes = context.singleOutcome()
                .Select(ctx => Visit(ctx))
                .CombineWithCommas();
            return $"[{outcomes}]";
        }
        public override string VisitSingleOutcome([NotNull] WordLangParser.SingleOutcomeContext context)
        {
            //var action = Quotize(Visit(context.text()));
            var action = "";
            return $"{{\"action\":{action}}}";
        }

        public override string VisitDialogs([NotNull] WordLangParser.DialogsContext context)
        {
            var dialogs = context.dialogLine()
                .Select(ctx => Visit(ctx))
                .CombineWithCommas();
            return $"[{dialogs}]";
        }

        public override string VisitDialogLine([NotNull] WordLangParser.DialogLineContext context)
        {
            var speaker = Quotize(Visit(context.text()));
            var line = Quotize(Visit(context.multilineText()));
            return $"{{\"speaker\":{speaker},\"line\":{line}}}";
        }

        public override string VisitConditions([NotNull] WordLangParser.ConditionsContext context)
        {
            var conds = context.booleanExpr()
                .Select(ctx => Visit(ctx))
                .CombineWithCommas();
            return $"[{conds}]";
        }

        public override string VisitBooleanExpr([NotNull] WordLangParser.BooleanExprContext context)
        {
            var op = Visit(context.booleanOp());
            //var left = Quotize(Visit(context.text(0)));
            //var right = Quotize(Visit(context.text(1)));
            var left = ""; var right = "";
            return $"{{\"op\":{op},\"left\":{left},\"right\":{right}}}";
        }

        public override string VisitBooleanOp([NotNull] WordLangParser.BooleanOpContext context)
        {
            var op = Visit(context.booleanOpMain());
            op += (context.NEGATION() == null ? "" : "!");

            return Quotize(op);
        }

        public override string VisitBooleanOpMain([NotNull] WordLangParser.BooleanOpMainContext context)
        {
            if (context.EQUALTO() != null)
            {
                return "=";
            } else if (context.GREATERTHAN() != null)
            {
                return ">";
            } else if (context.LESSTHAN() != null)
            {
                return "<";
            } else
            {
                return "?";
            }

        }

        public override string VisitRuleTitle([NotNull] WordLangParser.RuleTitleContext context)
        {
            return Quotize(Visit(context.text()));
        }
        public override string VisitDisplayAs([NotNull] WordLangParser.DisplayAsContext context)
        {
            var txt = context.text();
            if (txt == null)
            {
                return Quotize("");
            } else
            {
                return Quotize(Visit(context.text()));
            
            }
        }

        public override string VisitText([NotNull] WordLangParser.TextContext context)
        {
            return context.NAME().CombineTokens();
        }

        public override string VisitMultilineText([NotNull] WordLangParser.MultilineTextContext context)
        {
            return context.GetText();
        }

        private string Quotize(string str)
        {
            return "\"" + str + "\"";
        }
    }
}
