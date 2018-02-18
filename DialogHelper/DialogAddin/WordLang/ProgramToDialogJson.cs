using Dialog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antlr4.Runtime.Misc;

namespace DialogAddin.WordLang
{
    public class ProgramToDialogJson : WordLangBaseVisitor<List<DialogRule>>
    {
        private ProgramToTree TreeVisitor = new ProgramToTree();

        public override List<DialogRule> VisitProg([NotNull] WordLangParser.ProgContext context)
        {

            var rules = context.rule();

            return base.VisitProg(context);
        }
    }
}
