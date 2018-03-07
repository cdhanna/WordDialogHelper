using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antlr4.Runtime.Misc;
using Dialog;

namespace DialogAddin.WordLang
{
    class ProgramToErrors : WordLangBaseVisitor<List<GeneralError>>
    {

        private VariableCollection _variables;

        public ProgramToErrors(VariableCollection knownVariableCollection)
        {
            _variables = knownVariableCollection;
        }

        public override List<GeneralError> VisitProg([NotNull] WordLangParser.ProgContext context)
        {
            var errs = new List<GeneralError>();

            context.rule().Select(ctx => Visit(ctx))
                .ToList()
                .ForEach(errSet => errs.AddRange(errSet));

            return errs;
        }

        public override List<GeneralError> VisitRule([NotNull] WordLangParser.RuleContext context)
        {
            var errs = new List<GeneralError>();



            var conditions = context.conditions();
            if (conditions.ChildCount == 0)
            {
                errs.Add(context.NewError("Missing the Conditions header"));
            } else
            {
                errs.AddRange(Visit(conditions));
            }

            var outcomes = context.outcomes();
            if (outcomes.ChildCount == 0)
            {
                errs.Add(context.NewError("Missing the Outcomes header"));
            } else
            {
                errs.AddRange(Visit(outcomes));
            }

            return errs;
        }

        public override List<GeneralError> VisitOutcomes([NotNull] WordLangParser.OutcomesContext context)
        {
            var errs = new List<GeneralError>();

            var outcomeCtxs = context.singleOutcome();

            if (outcomeCtxs == null || outcomeCtxs.Count() == 0)
            {
                errs.Add(context.NewError("You must have at least one Outcome."));
            } else
            {
                outcomeCtxs.Select(ctx => Visit(ctx))
                    .ToList()
                    .ForEach(set => errs.AddRange(set));
            }


            return errs;
        }

        public override List<GeneralError> VisitConditions([NotNull] WordLangParser.ConditionsContext context)
        {
            var errs = new List<GeneralError>();
            var booleanExprCtxs = context.booleanExpr();

            if (booleanExprCtxs == null || booleanExprCtxs.Count() == 0)
            {
               // errs.Add(context.NewError("You must have at least one Condition."));
            } else
            {
                booleanExprCtxs.Select(ctx => Visit(ctx))
                    .ToList()
                    .ForEach(set => errs.AddRange(set));
            }

            return errs;
        }

        public override List<GeneralError> VisitSingleOutcome([NotNull] WordLangParser.SingleOutcomeContext context)
        {
            var errs = new List<GeneralError>();

            //var modifierErrs = context.outcomeModifier()?.Accept(this);
            //if (modifierErrs != null)
            //{
            //    errs.AddRange(modifierErrs);
            //}

            var setterErrs = context.outcomeSetter()?.Accept(this);
            if (setterErrs != null)
            {
                errs.AddRange(setterErrs);
            }


            return errs;
        }

        public override List<GeneralError> VisitBooleanExpr([NotNull] WordLangParser.BooleanExprContext context)
        {
            var errs = new List<GeneralError>();
            context.expression().Select(ctx => Visit(ctx))
                .ToList()
                .ForEach(set => errs.AddRange(set));
            
            return errs;
        }

        public override List<GeneralError> VisitExpression([NotNull] WordLangParser.ExpressionContext context)
        {
            var errs = new List<GeneralError>();

            errs.AddRange(Visit(context.additiveExpr()));

            //context.term

            //var refCtx = context.referance();

            //if (refCtx != null)
            //{
            //    errs.AddRange(Visit(refCtx));
            //}
           
            return errs;
        }

        public override List<GeneralError> VisitAdditiveExpr([NotNull] WordLangParser.AdditiveExprContext context)
        {
            var errs = new List<GeneralError>();

            errs.AddRange(Visit(context.multiplicitiveExpr()));
            if (context.additiveExpr() != null)
            {
                errs.AddRange(Visit(context.additiveExpr()));
            }

            return errs;
        }

        public override List<GeneralError> VisitMultiplicitiveExpr([NotNull] WordLangParser.MultiplicitiveExprContext context)
        {
            var errs = new List<GeneralError>();
            errs.AddRange(Visit(context.parenableExpr()));
            if (context.multiplicitiveExpr() != null)
            {
                errs.AddRange(Visit(context.multiplicitiveExpr()));
            }
            return errs;
        }

        public override List<GeneralError> VisitParenableExpr([NotNull] WordLangParser.ParenableExprContext context)
        {
            var errs = new List<GeneralError>();

            if (context.term() != null)
            {
                errs.AddRange(Visit(context.term()));
            } else
            {
                errs.AddRange(Visit(context.expression()));
            }

            return errs;
        }

        public override List<GeneralError> VisitTerm([NotNull] WordLangParser.TermContext context)
        {
            var errs = new List<GeneralError>();
            if (context.referance() != null)
            {
                errs.AddRange(Visit(context.referance()));
            }
            return errs;
        }

        public override List<GeneralError> VisitReferance([NotNull] WordLangParser.ReferanceContext context)
        {
            var errs = new List<GeneralError>();
            var path = ResolveReference(context);

            //var variable = _variables.GetFromPath(path);

            //if (variable == null)
            //{
            //    errs.Add(context.NewError("Unknown variable: " + String.Join(".", path)));
            //}

            return errs;
        }

        public string[] ResolveReference(WordLangParser.ReferanceContext context)
        {
            try
            {
                var full = new List<string>();
                var part = context.allowedReferenceWords()?.GetText()?.ToLower();
                if (part != null)
                {
                    full.Add(part);

                }
                else
                {
                    return full.ToArray();
                }

                if (context.referance() != null)
                {
                    var other = ResolveReference(context.referance());
                    full.AddRange(other);
                }
                return full.ToArray();
            } catch (Exception ex)
            {
                Console.WriteLine("error in prog->errs ");
                throw ex;
            }
        }

    }
}
