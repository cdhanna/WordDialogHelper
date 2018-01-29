using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dialog.Validation
{
    public class RuleValidator
    {

        public const string VALID_CHARACTERS = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789:._ -()[];\"'><=";

        public ValidationResults ValidateRules(List<JsonRule> rules)
        {
            var results = new ValidationResults();
            results.NameErrors = ValidateRuleNames(rules);
            return results;
        }


        public List<ValidationError> ValidateRuleNames(List<JsonRule> rules)
        {
            var errors = new List<ValidationError>();

            var names = new Dictionary<string, bool>();

            for (var ruleIndex = 0; ruleIndex < rules.Count; ruleIndex++)
            {
                var rule = rules[ruleIndex];

                #region CHECK FOR VALID CHARACTERS
                for (var i = 0; i < rule.Name.Length; i++)
                {
                    if (VALID_CHARACTERS.Contains(rule.Name[i]) == false)
                    {
                        errors.Add(new ValidationError()
                        {
                            Message = $"Invalid Character {rule.Name[i]}.",
                            Offset = i,
                            Length = 1,
                            ErrorType = ValidationErrorType.BAD_CHARACTER,
                            RuleId = rule.Id
                        });
                        break;
                    }
                }
                #endregion

                #region CHECK FOR SAME NAME
                if (names.ContainsKey(rule.Name))
                {
                    errors.Add(new ValidationError()
                    {
                        Message = "This name already exists",
                        Offset = 0,
                        Length = rule.Name.Length,
                        ErrorType = ValidationErrorType.DUPLICATE,
                        RuleId = rule.Id
                    });
                } else
                {
                    names.Add(rule.Name, true);
                }
                #endregion
            }
            return errors;
        }

        public List<ValidationError> ValidateConditions(List<JsonRule> rules)
        {
            var errors = new List<ValidationError>();

            for (var ruleIndex = 0; ruleIndex < rules.Count; ruleIndex++)
            {
                var rule = rules[ruleIndex];

                #region CHECK FOR FORM

                // trigger StartTalkingToSomeone
                var allowedFormat = "VAL OP VAL, FUNC";

                #endregion

            }
            
            return errors;
        }

    }
}
