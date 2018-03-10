using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using Dialog;

namespace DialogAddin.WordLang
{
    public class ProgramToVariableErrors : WordLangBaseVisitor<List<GeneralError>>
    {

        public VariableCollection Variables { get; private set; }

        public ProgramToVariableErrors(VariableCollection truth)
        {
            Variables = truth;
        }

        //public override List<GeneralError> Visit(IParseTree tree)
        //{
        //    var errs = new List<GeneralError>();

        //    for (var i = 0; i < tree.ChildCount; i++)
        //    {
        //        var child = tree.GetChild(i);

        //        var childErrs = child.Accept(this);
        //        if (childErrs != null)
        //        {
        //            errs.AddRange(childErrs);
        //        }
        //        //Visit(child);
        //    }


        //    return errs;
        //}

        public override List<GeneralError> VisitChildren(IRuleNode node)
        {
            var errs = new List<GeneralError>();

            for (var i = 0; i < node.ChildCount; i++)
            {
                var child = node.GetChild(i);

                var childErrs = child.Accept(this);
                if (childErrs != null)
                {
                    errs.AddRange(childErrs);
                }
                //Visit(child);
            }


            return errs;
        }



        public override List<GeneralError> VisitReferance([NotNull] WordLangParser.ReferanceContext context)
        {
            var text = context.GetText().Trim();
            if (!Variables.Exists(text))
            {
                return new GeneralError[] {
                    context.NewError($"Variable wasn't listed. {text}")
                }.ToList();
            }
            else return null;
        }

    }
}
