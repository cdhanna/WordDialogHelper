using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antlr4.Runtime.Misc;

namespace DialogAddin.WordLang
{
    public class WordLangStringVisitor : WordLangBaseVisitor<object>
    {

        //public override object VisitRuleSet([NotNull] WordLangParser.RuleSetContext context)
        //{
        //    var rules = context.rule();
        //    return base.VisitRuleSet(context);
        //}

        public override object VisitRule([NotNull] WordLangParser.RuleContext context)
        {
            var text = context.GetText();
            //context.ruleTitle();
            return base.VisitRule(context);
        }

        //public override object VisitRuleTitle([NotNull] WordLangParser.RuleTitleContext context)
        //{
        //    var title = context.GetText();

        //    return base.VisitRuleTitle(context);
        //}

    }
}
