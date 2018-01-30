using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dialog.Validation
{
    public class RuleValidator
    {


        public ValidationResults ValidateRules(List<JsonRule> rules)
        {
            var results = new ValidationResults();
            results.NameErrors = ValidateRuleNames(rules);
            results.DialogErrors = ValidateDialogs(rules);
            results.DisplayAsErrors = ValidateDisplays(rules);
            return results;
        }

        public List<ValidationError> ValidateDisplays(List<JsonRule> rules)
        {
            var errors = new List<ValidationError>();

            var names = new Dictionary<string, bool>();

            for (var ruleIndex = 0; ruleIndex < rules.Count; ruleIndex++)
            {
                var rule = rules[ruleIndex];

                #region CHECK FOR VALID CHARACTERS
                rule.DisplayAs.CheckForBadCharacters(errors, rule.Id, 0, false);
                #endregion

                #region CHECK FOR SAME NAME
                if (names.ContainsKey(rule.DisplayAs))
                {
                    errors.Add(new ValidationError()
                    {
                        Message = "This DisplayAs already exists",
                        Offset = 0,
                        Length = rule.DisplayAs.Length,
                        ErrorType = ValidationErrorType.DUPLICATE,
                        RuleId = rule.Id
                    });
                }
                else if (rule.DisplayAs.Length > 0)
                {
                    names.Add(rule.DisplayAs, true);
                }
                #endregion
            }
            return errors;
        }

        public List<ValidationError> ValidateRuleNames(List<JsonRule> rules)
        {
            var errors = new List<ValidationError>();

            var names = new Dictionary<string, bool>();

            for (var ruleIndex = 0; ruleIndex < rules.Count; ruleIndex++)
            {
                var rule = rules[ruleIndex];

                #region CHECK FOR VALID CHARACTERS
                rule.Name.CheckForBadCharacters(errors, rule.Id, 0, true);
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

        public List<ValidationError> ValidateDialogs(List<JsonRule> rules)
        {
            var errors = new List<ValidationError>();

            for (var ruleIndex = 0; ruleIndex < rules.Count; ruleIndex++)
            {
                var rule = rules[ruleIndex];

                #region CHECK FOR AT LEAST ONE SPEAKER
                if (rule.Dialog.Length == 0)
                {
                    errors.Add(new ValidationError()
                    {
                        ErrorType = ValidationErrorType.EMPTY,
                        Offset = 0,
                        Length = 1,
                        Message = "Must have at least one dialog speaker.",
                        RuleId = rule.Id
                    });
                }
                #endregion

                #region CHECK EACH DIALOG PART FOR ERRORS
                var offset = 0;
                for (var i = 0; i < rule.Dialog.Length; i++)
                {
                    var part = rule.Dialog[i];
                    var offsetPlusSpeaker = offset + (string.IsNullOrEmpty(part.Speaker) ? 0 : part.Speaker.Length) + 1;
                    var speaker = part.Speaker.Substring(1, part.Speaker.Length - 1);

                    #region CHECK FOR EMPTY SPEAKER
                    if (string.IsNullOrEmpty(speaker))
                    {
                        errors.Add(new ValidationError()
                        {
                            ErrorType = ValidationErrorType.EMPTY,
                            Offset = offset,
                            Length = 1,
                            Message = "Speaker cannot be empty",
                            RuleId = rule.Id
                        });
                    }
                    #endregion

                    #region CHECK FOR BAD CHARACTERS
                    part.Speaker.CheckForBadCharacters(errors, rule.Id, offset, false);
                    part.Content.CheckForBadCharacters(errors, rule.Id, offsetPlusSpeaker, false);
                    #endregion

                    #region CHECK FOR EMPTY LINES
                    if (string.IsNullOrEmpty(part.Content) || "\n".Equals(part.Content))
                    {
                        errors.Add(new ValidationError()
                        {
                            ErrorType = ValidationErrorType.EMPTY,
                            Offset = offset,
                            Length = 1,
                            Message = "Speaker must say something.",
                            RuleId = rule.Id
                        });
                    }
                    #endregion

                    offset += (string.IsNullOrEmpty(part.Speaker) ? 0 : part.Speaker.Length) + 1;
                    offset += (string.IsNullOrEmpty(part.Content) ? 0 : part.Content.Length) + 1;
                }
                #endregion




                //#region CHECK FOR FORM

                //// trigger StartTalkingToSomeone
                //var allowedFormat = "VAL OP VAL, FUNC";

                //#endregion

            }

            return errors;
        }

    }
}
