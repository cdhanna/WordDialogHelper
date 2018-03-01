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
        private ProgramToTree TreeVisitor = new ProgramToTree();
        public override string VisitProg([NotNull] WordLangParser.ProgContext context)
        {
            var rules = context.rule()
                .Select(ctx => Visit(ctx))
                .CombineWithCommas()
                ;
            return $"{{\"name\":\"test\",\"rules\":[{rules}],\"conditionSets\":null}}";

            //return $"[{rules}]";
        }

        public override string VisitRule([NotNull] WordLangParser.RuleContext context)
        {
            var title = Visit(context.ruleTitle());
            var displayAs = Visit(context.displayAs());
            var conditions = Visit(context.conditions());
            var dialogs = Visit(context.dialogs());
            var outcomes = Visit(context.outcomes());
            return $"{{\"name\":{title},\"displayAs\":{displayAs},\"conditions\":{conditions},\"dialog\":{dialogs},\"outcomes\":{outcomes}}}";
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

            if (context.outcomeSetter() != null)
            {
                return Visit(context.outcomeSetter());
            }
            if (context.outcomeFunction() != null)
            {
                return Visit(context.outcomeFunction());
            }
            throw new InvalidOperationException();
        }

        public override string VisitOutcomeSetter([NotNull] WordLangParser.OutcomeSetterContext context)
        {
            var target = Quotize(TreeVisitor.Visit(context.referance()));
            var expr = Quotize(TreeVisitor.Visit(context.expression()));
            return $"{{\"command\":\"set\",\"target\":{target},\"arguments\":{{\"\":{expr}}}}}";
        }

        public override string VisitOutcomeFunction([NotNull] WordLangParser.OutcomeFunctionContext context)
        {
            var target = Quotize(TreeVisitor.Visit(context.referance()));
            var args = "{}";
            if (context.outcomeFunctionNamedBindingList() != null)
            {
                args = Visit(context.outcomeFunctionNamedBindingList());
            }
            return $"{{\"command\":\"run\",\"target\":{target},\"arguments\":{args}}}";
        }

        public override string VisitOutcomeFunctionNamedBindingList([NotNull] WordLangParser.OutcomeFunctionNamedBindingListContext context)
        {
            var ctx = context.outcomeFunctionNamedBinding();
            var args = String.Join(",", ctx.Select(c =>
            {
                return $"\"{TreeVisitor.Visit(c.referance())}\":\"{TreeVisitor.Visit(c.expression())}\"";
            }).ToArray());
            return $"{{{args}}}";

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
            var line = Quotize(context.multilineText().GetText());
            var parts = Visit(context.multilineText());
            return $"{{\"speaker\":{speaker},\"content\":{line},\"parts\":{parts}}}";
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
           
            var left = Quotize(TreeVisitor.Visit(context.expression(0)));
            var right = Quotize(TreeVisitor.Visit(context.expression(1)));
            //var left = Quotize(Visit(context.text(0)));
            //var right = Quotize(Visit(context.text(1)));
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

            var children = context.children;
            var parts = new List<string>();
            for (var i = 0; i < children.Count; i++)
            {
                var child = children[i];
                var text = child.GetText();
                parts.Add(Visit(child));
            }

            return $"[{string.Join(",", parts)}]";
        }

        public override string VisitFreeText([NotNull] WordLangParser.FreeTextContext context)
        {
            return Quotize("'" + context.GetText() + "'");
        }

        public override string VisitTemplatedText([NotNull] WordLangParser.TemplatedTextContext context)
        {
            var expr = Quotize(TreeVisitor.Visit(context.expression()));
            return expr;
        }

        private string Quotize(string str)
        {
            return "\"" + str + "\"";
        }
    }
}
