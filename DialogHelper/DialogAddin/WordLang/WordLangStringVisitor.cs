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

        //public override object VisitRuleSet([NotNull] WordLangParser.RuleSetContext context)
        //{
        //    var rules = context.rule();
        //    return base.VisitRuleSet(context);
        //}

        public override string VisitRule([NotNull] WordLangParser.RuleContext context)
        {
            var text = context.GetText().Replace("\r", "").Replace("\n", "");

            var title = Visit(context.ruleTitle());
            var displayAs = Visit(context.displayAs());

            VisitConditions(context.conditions());

            //context.ruleTitle();
            return $"(rule title={title}, disp={displayAs})";
        }

        public override string VisitRuleTitle([NotNull] WordLangParser.RuleTitleContext context)
        {
            return context.TEXT().GetText();
        }

        public override string VisitDisplayAs([NotNull] WordLangParser.DisplayAsContext context)
        {
            return context.TEXT().GetText();
        }

        public override string VisitConditions([NotNull] WordLangParser.ConditionsContext context)
        {
            return base.VisitConditions(context);
        }

        public override string VisitSingleCondition([NotNull] WordLangParser.SingleConditionContext context)
        {

            context.TE

            return base.VisitSingleCondition(context);
        }

    }
}
