using Antlr4.Runtime.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AST = DialogAddin.WordLang.AST;

namespace DialogAddin.WordLang
{
    public class ProgramToAST : WordLangBaseVisitor<AST.RuleTree>
    {

        public override AST.RuleTree VisitProg([NotNull] WordLangParser.ProgContext context)
        {
            var ruleSet = new AST.RuleSet();

            //ruleSet.Rules = context.rule()
            //    .Select(ctx => Visit(ctx))
            //    .OfType<AST.Rule>()
            //    .ToList();

            return ruleSet;
        }

        public override AST.RuleTree VisitRule([NotNull] WordLangParser.RuleContext context)
        {
            var rule = new AST.Rule();

            var titleContext = context.ruleTitle();
            rule.Title = new AST.FieldString()
            {
                Value = titleContext.text().NAME().CombineTokens(),
                StartIndex = titleContext.text().NAME().First().Symbol.StartIndex,
                StopIndex = titleContext.text().NAME().Last().Symbol.StopIndex
            };

            var displayAsCtx = context.displayAs().text();
            if (displayAsCtx != null)
            {
                rule.DisplayAs = new AST.FieldString()
                {
                    Value = displayAsCtx.NAME().CombineTokens(),
                    StartIndex = displayAsCtx.Start.StartIndex,
                    StopIndex = displayAsCtx.Stop.StopIndex
                };
            } else
            {
                rule.DisplayAs = new AST.FieldString()
                {
                    Value = "",
                    StartIndex = context.displayAs().Start.StartIndex,
                    StopIndex = context.displayAs().Stop.StopIndex
                };
            }

            rule.Conditions = context.conditions().booleanExpr()
                .Select(ctx => Visit(ctx))
                .OfType<AST.Condition>()
                .ToList();

            rule.Dialogs = context.dialogs().dialogLine()
                .Select(ctx => Visit(ctx))
                .OfType<AST.Dialog>()
                .ToList();

            rule.Outcomes = context.outcomes().singleOutcome()
                .Select(ctx => Visit(ctx))
                .OfType<AST.Outcome>()
                .ToList();

            return rule;
        }

        public override AST.RuleTree VisitSingleOutcome([NotNull] WordLangParser.SingleOutcomeContext context)
        {
            var outcome = new AST.Outcome();
            //outcome.Action = new AST.FieldString()
            //{
            //    Value = context.text().NAME().CombineTokens(),
            //    StartIndex = context.text().NAME().First().Symbol.StartIndex,
            //    StopIndex = context.text().NAME().Last().Symbol.StopIndex
            //};
            return outcome;
        }

        public override AST.RuleTree VisitDialogLine([NotNull] WordLangParser.DialogLineContext context)
        {
            var dialog = new AST.Dialog();

            
            dialog.Speaker = new AST.FieldString()
            {
                Value = context.text().NAME().CombineTokens(),
                StartIndex = context.text().NAME().First().Symbol.StartIndex,
                StopIndex = context.text().NAME().Last().Symbol.StopIndex
            };
            dialog.Line = new AST.FieldString()
            {
                Value = context.multilineText().GetText(),
                StartIndex = context.multilineText().Start.StartIndex,
                StopIndex = context.multilineText().Stop.StopIndex
            };

            return dialog;
        }

        public override AST.RuleTree VisitBooleanExpr([NotNull] WordLangParser.BooleanExprContext context)
        {
            var condition = new AST.Condition();

            //var leftCtx = context.text(0);
            //var rightCtx = context.text(1);

            //var rightName = rightCtx.NAME().CombineTokens();

            //condition.LeftKey = new AST.FieldString()
            //{
            //    Value = leftCtx.NAME().CombineTokens(),
            //    StartIndex = leftCtx.NAME().First().Symbol.StartIndex,
            //    StopIndex = leftCtx.NAME().Last().Symbol.StopIndex
            //};
            //condition.RightKey = new AST.FieldString()
            //{
            //    Value = rightCtx.NAME().CombineTokens(),
            //    StartIndex = rightCtx.NAME().First().Symbol.StartIndex,
            //    StopIndex = rightCtx.NAME().Last().Symbol.StopIndex
            //};

            var opMainCtx = context.booleanOp().booleanOpMain();

            condition.Negated = context.booleanOp().NEGATION() != null;
            
            condition.Operator = new AST.FieldComparisonOperator()
            {
                Value = (opMainCtx.EQUALTO() != null ? AST.ComparisonOperator.EQUALS :
                    (opMainCtx.LESSTHAN() != null ? AST.ComparisonOperator.LESS : AST.ComparisonOperator.GREATER)),
                StartIndex = context.booleanOp().Start.StartIndex,
                StopIndex = context.booleanOp().Stop.StopIndex

            };
            

            return condition;
        }




    }

    //static class ProgramToASTExtensions
    //{
    //    public static string Combine(this WordLangParser.TextContext ctx)
    //    {
    //        return ctx.NAME().CombineTokens();
    //    }
    //}
}
