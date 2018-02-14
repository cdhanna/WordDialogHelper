using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace Dialog.Engine
{
    public class DialogEngine
    {

        private List<DialogAttribute> _attributes = new List<DialogAttribute>();
        private List<DialogRule> _rules = new List<DialogRule>();

        public DialogRule GetBestValidDialog()
        {
            
            var attrNameToValue = GetAttributeValues();
            var validRules = GetValidRules(attrNameToValue);
            var bestRule = GetBestRule(validRules);
            return bestRule;
        }

        public List<DialogRule> GetAllValidDialog()
        {
            var attrNameToValue = GetAttributeValues();
            var validRules = GetValidRules(attrNameToValue);
            return validRules;
        }

        public DialogEngine AddRule(DialogRule rule)
        {
            _rules.Add(rule);
            return this;
        }

        public DialogEngine AddAttribute(DialogAttribute attribute)
        {
            _attributes.Add(attribute);
            return this;
        }
        

       
        private Dictionary<string, long> GetAttributeValues()
        {
            var values = new Dictionary<string, long>(); // attribute name -> value
            for (var i = 0; i < _attributes.Count; i++)
            {
                var attribute = _attributes[i];
                attribute.Update();
                values.Add(attribute.Name.ToLower(), attribute.CurrentValue);
            }
            return values;
        }

        private List<DialogRule> GetValidRules(Dictionary<string, long> attrNameToValue)
        {
            var validRules = new List<DialogRule>();
            for (var i = 0; i < _rules.Count; i++)
            {
                var rule = _rules[i];
                var isValidRule = true;

                for (var j = 0; j < rule.Conditions.Length; j++)
                {
                    var condition = rule.Conditions[j];

                    var leftValue = attrNameToValue[condition.Left.ToLower()]; // assume that the attribute exists, I guess.
                    var rightValue = attrNameToValue[condition.Right.ToLower()];
                    var matched = false;
                    switch (condition.Op) // TODO add negation support
                    {
                        case "=":
                            matched = leftValue == rightValue;
                            break;
                        case ">":
                            matched = leftValue > rightValue;
                            break;
                        case "<":
                            matched = leftValue < rightValue;
                            break;
                        default:
                            throw new InvalidOperationException("Unknown operation type" + condition.Op);
                    }

                    isValidRule &= matched;
                }
                if (isValidRule == true)
                {
                    validRules.Add(rule);
                }

            }
            return validRules;
        }

        private DialogRule GetBestRule(List<DialogRule> rules)
        {
            var best = default(DialogRule);
            for (var i = 0; i < rules.Count; i++)
            {
                var rule = rules[i];
                if (best == null || rule.Conditions.Length > best?.Conditions.Length)
                {
                    best = rule;
                }
            }
            return best;
        }
    }
}
