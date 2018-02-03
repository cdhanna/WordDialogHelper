//using Dialog;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Antlr4.Runtime.Misc;

//namespace DialogAddin.WordLang
//{
//    class RulesVisitor : WordLangBaseVisitor<List<JsonRule>>
//    {
//        public override List<JsonRule> VisitProg([NotNull] WordLangParser.ProgContext context)
//        {

//            var ruleVisitor = new RuleModelVisitor();

//            return context.rule().Select(ruleCtx => ruleVisitor.VisitRule(ruleCtx)).ToList();

//        }


//        class RuleModelVisitor : WordLangBaseVisitor<JsonRule>
//        {


//            public override JsonRule VisitRule([NotNull] WordLangParser.RuleContext context)
//            {
//                var rule = new JsonRule();
//                var strVisitor = new RuleGetStringVisitor();
//                var arrayVisitor = new RuleGetStringArrayVisitor(strVisitor);

//                var title = strVisitor.VisitRuleTitle(context.ruleTitle());
//                rule.Name = title;

//                var displayAs = strVisitor.VisitDisplayAs(context.displayAs());
//                rule.DisplayAs = displayAs;

//                var conditions = arrayVisitor.VisitConditions(context.conditions());
//                rule.Conditions = conditions;

//                var outcomes = arrayVisitor.VisitOutcomes(context.outcomes());
//                rule.Outcomes = outcomes;

//                var dialogVisitor = new RuleGetDialogPartVisitor();
//                rule.Dialog = dialogVisitor.VisitDialogs(context.dialogs());

//                return rule;
//            }

//        }

//        class RuleGetStringVisitor : WordLangBaseVisitor<string>
//        {
//            public override string VisitRuleTitle([NotNull] WordLangParser.RuleTitleContext context)
//            {
//                return context.expr().GetText();
//            }

//            public override string VisitDisplayAs([NotNull] WordLangParser.DisplayAsContext context)
//            {
//                return context.expr().GetText();
//            }

//            public override string VisitSingleCondition([NotNull] WordLangParser.SingleConditionContext context)
//            {
//                return VisitComparison(context.comparison());
//            }

//            public override string VisitComparison([NotNull] WordLangParser.ComparisonContext context)
//            {
//                var op = context.comparisonOp();

//                var split = false;
//                var left = "";
//                var right = "";
//                for (var i = 0; i < context.ChildCount; i++)
//                {

//                    if (context.GetChild(i) == op)
//                    {
//                        split = true;
//                    }
//                    else
//                    {
//                        if (!split)
//                        {
//                            left += context.GetChild(i).GetText();
//                        }
//                        else
//                        {
//                            right += context.GetChild(i).GetText();
//                        }
//                    }
//                }

//                return $"[{left}] {op} [{right}]";
//            }

//            public override string VisitSingleOutcome([NotNull] WordLangParser.SingleOutcomeContext context)
//            {
//                return context.GetText();
//            }
//        }

//        class RuleGetStringArrayVisitor : WordLangBaseVisitor<string[]>
//        {
//            public RuleGetStringVisitor StringVisitor { get; set; }

//            public RuleGetStringArrayVisitor(RuleGetStringVisitor strVisitor)
//            {
//                strVisitor = StringVisitor;
//            }

//            public override string[] VisitConditions([NotNull] WordLangParser.ConditionsContext context)
//            {
//                return context.singleCondition()
//                    .Select(conditionCtx => StringVisitor.VisitSingleCondition(conditionCtx))
//                    .ToArray();

//            }

//            public override string[] VisitOutcomes([NotNull] WordLangParser.OutcomesContext context)
//            {
//                return context.singleOutcome()
//                    .Select(singleOutcomeCtx => StringVisitor.VisitSingleOutcome(singleOutcomeCtx))
//                    .ToArray();
//            }
//        }

//        class RuleGetDialogPartVisitor : WordLangBaseVisitor<JsonRule.DialogPart[]>
//        {
//            public override JsonRule.DialogPart[] VisitDialogs([NotNull] WordLangParser.DialogsContext context)
//            {
//                return context.dialogLine().ToList().Select(line => new JsonRule.DialogPart()
//                {
//                    Speaker = line.dialogLineSpeaker().NAME().GetText(),
//                    Content = line.dialogLineText().Select(lineCtx => lineCtx.GetText()).Aggregate("", (agg, curr) => agg + curr)
//                })
//                .ToArray();

//            }
//        }
//    }
//}
