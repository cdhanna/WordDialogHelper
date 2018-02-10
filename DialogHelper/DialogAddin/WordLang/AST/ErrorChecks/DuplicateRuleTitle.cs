using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DialogAddin.WordLang.AST.ErrorChecks
{
    public abstract class ASTErrorChecker
    {
        public abstract List<GeneralError> Check(RuleSet rules);
    }



    public class DuplicateRuleTitle : ASTErrorChecker
    {

        public override List<GeneralError> Check(RuleSet ruleSet)
        {
            var errs = new List<GeneralError>();
            var seen = new Dictionary<string, bool>();
            foreach(var rule in ruleSet.Rules)
            {
                if (seen.ContainsKey(rule.Title.Value))
                {
                    errs.Add(new GeneralError()
                    {
                        Message = "Duplicate Rule Name Cannot Exist.",
                        Line = 1,
                        CharPosition = rule.Title.StartIndex,
                        EndLine = 1,
                        EndCharPosition = rule.Title.StopIndex
                    });
                } else
                {
                    seen.Add(rule.Title.Value, true);
                }
            }

            return errs;
        }
    }
}
