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


                    /*
                     * the Left and Right will always be expressions.
                     * 
                     * At the moment, an expression is 
                         * expr: literal | reference
                         * literal: NUMBER | STRING | TRUE | FALSE
                         * reference: NAME | NAME SEPARATOR reference
                     * 
                     * So we can write a cheap parser to handle that, with these hacky rules...
                     * 1. if the first character is 0-9, its a literal NUMBER
                     * 2. if the first character is a " or ', its a literal STRING
                     * 3. if the expr is false or true, its a literal BOOL
                     * 4. otherwise, its a reference
                     * 
                     */

                    //var leftValue = HackyParse(condition.Left.ToLower(), attrNameToValue);
                    //var rightValue = HackyParse(condition.Right.ToLower(), attrNameToValue);
                    var leftValue = condition.Left.ToLower().ProcessAsPrefixMath(attrNameToValue);
                    var rightValue = condition.Right.ToLower().ProcessAsPrefixMath(attrNameToValue);

                    //var leftValue = attrNameToValue[condition.Left.ToLower()]; // assume that the attribute exists, I guess.
                    //var rightValue = attrNameToValue[condition.Right.ToLower()];
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

        private long HackyParse(string expr, Dictionary<string, long> attrNameToValue)
        {
            
            if (expr.Length > 0 &&
                (expr[0].Equals("\"")
                || expr[0].Equals("'")))
            {
                // strip quotes
                return expr.Substring(1, expr.Length - 2).ToLong();
            }
            else if (expr.Length > 0 &&
              (expr[0].Equals('0')
              || expr[0].Equals('1')
              || expr[0].Equals('2')
              || expr[0].Equals('3')
              || expr[0].Equals('4')
              || expr[0].Equals('5')
              || expr[0].Equals('6')
              || expr[0].Equals('7')
              || expr[0].Equals('8')
              || expr[0].Equals('9')
              ))
            {
                return long.Parse(expr);
            }
            else if (expr.Equals("false") || expr.Equals("true"))
            {
                return (expr.Equals("false") ? 0 : 1);
            } else
            {
                return attrNameToValue[expr];
            }
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
