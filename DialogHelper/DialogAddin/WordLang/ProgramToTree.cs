using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antlr4.Runtime.Misc;

namespace DialogAddin.WordLang
{
    public class ProgramToTree : WordLangBaseVisitor<string>
    {
        public override string VisitProg([NotNull] WordLangParser.ProgContext context)
        {
            var rules = context.rule()
                .Select(ctx => Visit(ctx))
                .Aggregate("", (a, c) => a + c);
           
            return $"(program rules=[{rules}])";
        }

        public override string VisitRule([NotNull] WordLangParser.RuleContext context)
        {
            var title = Visit(context.ruleTitle());
            var displayAs = Visit(context.displayAs());
            var conditions = Visit(context.conditions());
            var dialogs = Visit(context.dialogs());
            var outcomes = Visit(context.outcomes());

            return $"(rule title=[{title}] displayAs=[{displayAs}] conditions=[{conditions}] dialogs=[{dialogs}] outcomes=[{outcomes}])";
        }

        public override string VisitOutcomes([NotNull] WordLangParser.OutcomesContext context)
        {
            return context.singleOutcome()
                .Select(ctx => Visit(ctx))
                .Aggregate("", (c, a) => c + a);
        }

        public override string VisitSingleOutcome([NotNull] WordLangParser.SingleOutcomeContext context)
        {
            //var action =  Visit(context.text());
            var action = "";
            return $"(outcome action=[{action}])";
        }

        public override string VisitDialogs([NotNull] WordLangParser.DialogsContext context)
        {
            return context.dialogLine()
                .Select(ctx => Visit(ctx))
                .Aggregate("", (a, c) => a + c);
        }

        public override string VisitDialogLine([NotNull] WordLangParser.DialogLineContext context)
        {
            var speaker = Visit(context.text());
            var line = Visit(context.multilineText());
            return $"(dialog speaker=[{speaker}] line=[{line}])";
        }

        public override string VisitConditions([NotNull] WordLangParser.ConditionsContext context)
        {
            return context.booleanExpr()
                .Select(ctx => Visit(ctx))
                .Aggregate("", (a, c) => a + c);
        }

        public override string VisitBooleanExpr([NotNull] WordLangParser.BooleanExprContext context)
        {
            //var left = Visit(context.text(0));
            //var right = Visit(context.text(1));
            var left = "";
            var right = "";
            var op = Visit(context.booleanOp());
            return $"(cond op=[{op}] left=[{left}] right=[{right}])";
        }

        public override string VisitBooleanOp([NotNull] WordLangParser.BooleanOpContext context)
        {
            var neg = context.NEGATION() != null;
            var op = "";
            if (context.booleanOpMain()!=null) op += Visit(context.booleanOpMain());

            var result = (neg ? "!" : "") + op;

            //var other = context.booleanOp();
            //if (other != null)
            //{
            //    result += Visit(other);
            //}

            return result;
        }

        public override string VisitBooleanOpMain([NotNull] WordLangParser.BooleanOpMainContext context)
        {
            var eq = context.EQUALTO();
            if (eq != null)
            {
                return "=";
            }
            var lt = context.LESSTHAN();
            if (lt != null)
            {
                return "<";
            }
            var gt = context.GREATERTHAN();
            if (gt != null)
            {
                return ">";
            }
            throw new InvalidOperationException("Unrecognized boolean main operation." + context.GetText());
        }

        public override string VisitDisplayAs([NotNull] WordLangParser.DisplayAsContext context)
        {
            return Visit(context.text());
        }

        public override string VisitRuleTitle([NotNull] WordLangParser.RuleTitleContext context)
        {
            return Visit(context.text());
        }

        public override string VisitText([NotNull] WordLangParser.TextContext context)
        {
            return context.NAME().CombineTokens();
        }

        public override string VisitMultilineText([NotNull] WordLangParser.MultilineTextContext context)
        {
            return context.GetText();
        }

    }
}
