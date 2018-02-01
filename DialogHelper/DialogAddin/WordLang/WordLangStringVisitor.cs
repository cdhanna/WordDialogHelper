using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antlr4.Runtime.Misc;

namespace DialogAddin.WordLang
{
    public class WordLangStringVisitor : WordLangBaseVisitor<string>
    {

        public override string VisitProg([NotNull] WordLangParser.ProgContext context)
        {
            var rules = context.rule();
            var ruleStrings = rules.ToList().Select(r => Visit(r)).ToList();

            var str = "(prog ";
            ruleStrings.ForEach(r => str += r);
            str += ")";

            return str;
        }

        public override string VisitDisplayAs([NotNull] WordLangParser.DisplayAsContext context)
        {
            return context.expr().GetText();
        }

        ////public override object VisitRuleSet([NotNull] WordLangParser.RuleSetContext context)
        ////{
        ////    var rules = context.rule();
        ////    return base.VisitRuleSet(context);
        ////}

        public override string VisitRule([NotNull] WordLangParser.RuleContext context)
        {
            var text = context.GetText().Replace("\r", "").Replace("\n", "");

            var title = Visit(context.ruleTitle());
            var displayAs = Visit(context.displayAs());

            var conditions = VisitConditions(context.conditions());

            //context.ruleTitle();
            return $"(rule title={title}, disp={displayAs}, conds={conditions})";
        }

        public override string VisitRuleTitle([NotNull] WordLangParser.RuleTitleContext context)
        {
            return context.expr().GetText();
        }

        //public override string VisitDisplayAs([NotNull] WordLangParser.DisplayAsContext context)
        //{
        //    return context.TEXT().GetText();
        //}

        public override string VisitConditions([NotNull] WordLangParser.ConditionsContext context)
        {

            //return "go away";
            var condStrs = context.singleCondition().Select(c => VisitSingleCondition(c));
            return condStrs.Aggregate("[", (agg, curr) => agg + " " + curr) + " ]";
        }

        public override string VisitSingleCondition([NotNull] WordLangParser.SingleConditionContext context)
        {
            return Visit(context.comparison());
        }

        public override string VisitComparison([NotNull] WordLangParser.ComparisonContext context)
        {
            var op = context.comparisonOp();

            var split = false;
            var left = "";
            var right = "";
            for (var i = 0; i < context.ChildCount; i++)
            {
                
                if (context.GetChild(i) == op)
                {
                    split = true;
                } else
                {
                    if (!split)
                    {
                        left += context.GetChild(i).GetText();
                    } else
                    {
                        right += context.GetChild(i).GetText();
                    }
                }
            }

            var opText = op.GetText();
            //context.
            return $"({opText}, {left}, {right})";

            return base.VisitComparison(context);
        }

        //public override string VisitSingleCondition([NotNull] WordLangParser.SingleConditionContext context)
        //{



        //    //var left = context.TEXT(0).GetText();
        //    //var right = context.TEXT(1).GetText();

        //    //var op = context.EQUALTO().GetText();
        //    ////var text = context.TEXT();

        //    ////var str = text.GetText();

        //    //return $"({op} , {left}, {right})";
        //}



    }
}
