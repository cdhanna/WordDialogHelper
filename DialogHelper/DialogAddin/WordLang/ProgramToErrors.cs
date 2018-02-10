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
 

            return errs;
        }

        public override List<GeneralError> VisitConditions([NotNull] WordLangParser.ConditionsContext context)
        {
            var errs = new List<GeneralError>();
            var booleanExprCtxs = context.booleanExpr();

            if (booleanExprCtxs == null || booleanExprCtxs.Count() == 0)
            {
                errs.Add(context.NewError("You Must have at least one Condition, in the form of a SET x TO y, or MODIFY a BY b"));
            } else
            {
                booleanExprCtxs.Select(ctx => Visit(ctx))
                    .ToList()
                    .ForEach(set => errs.AddRange(set));
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

            var refCtx = context.referance();

            if (refCtx != null)
            {
                errs.AddRange(Visit(refCtx));
            }
           
            return errs;
        }

        public override List<GeneralError> VisitReferance([NotNull] WordLangParser.ReferanceContext context)
        {
            var errs = new List<GeneralError>();
            var path = ResolveReference(context);

            var variable = _variables.GetFromPath(path);

            if (variable == null)
            {
                errs.Add(context.NewError("Unknown variable: " + String.Join(".", path)));
            }

            return errs;
        }

        public string[] ResolveReference(WordLangParser.ReferanceContext context)
        {
            var full = new List<string>();
            var part = context.NAME().GetText().ToLower();
            full.Add(part);
            if (context.referance() != null)
            {
                var other = ResolveReference(context.referance());
                full.AddRange(other);
            }
            return full.ToArray();
        }

    }
}
