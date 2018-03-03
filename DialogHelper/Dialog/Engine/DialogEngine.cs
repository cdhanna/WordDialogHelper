using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Dialog.Engine
{
    public class DialogEngine
    {

        private List<DialogAttribute> _attributes = new List<DialogAttribute>();
        private List<DialogRule> _rules = new List<DialogRule>();
        private List<DialogConditionSet> _conditionSets = new List<DialogConditionSet>();
        private List<EngineAdditionHandler> _onAdditionHandlers = new List<EngineAdditionHandler>();


        private Dictionary<Regex, Func<string>> _transformers = new Dictionary<Regex, Func<string>>();

        public string[] GetHandlerNames
        {
            get { return _onAdditionHandlers.Select(s => s.GetType().Name).ToArray(); }
        }

        public List<string> GetAttributeNames()
        {
            return _attributes.Select(a => a.Name).ToList();
        }

        public DialogRule GetBestValidDialog()
        {
            var transformer = BuildTransformFunction();
            var attrNameToValue = GetAttributeValueCodes();
            var validRules = GetValidRules(attrNameToValue, transformer);
            var bestRule = GetBestRule(validRules);
            return bestRule;
        }

        public DialogRule GetBestValidDialogForPlayer(string speaker="player")
        {
            var attrNameToValue = GetAttributeValueCodes();
            var transformer = BuildTransformFunction();

            var validRules = GetValidRules(attrNameToValue, transformer);
            var bestRule = GetBestRuleForPlayer(validRules, speaker);
            return bestRule;
        }

        public List<DialogRule> GetAllValidDialog()
        {
            var attrNameToValue = GetAttributeValueCodes();
            var transformer = BuildTransformFunction();
            var validRules = GetValidRules(attrNameToValue, transformer);
            return validRules;
        }

        public List<DialogRule> GetAllValidDialogForPlayer(string speaker="player")
        {
            var transformer = BuildTransformFunction();
            var attrNameToValue = GetAttributeValueCodes();
            var validRules = GetValidRules(attrNameToValue, transformer);
            return validRules
                .Where(r => r.Dialog.Count() > 0 && r.Dialog.First().Speaker.Equals(speaker))
                .ToList();
        }

        public DialogEngine AddHandler(EngineAdditionHandler ruleHandler)
        {
            _onAdditionHandlers.Add(ruleHandler);
            return this;
        }

        public DialogEngine AddConditionSet(DialogConditionSet condition)
        {
            var refs = ExtractReferencesFromCondition(condition);
            for (var i = 0; i < _onAdditionHandlers.Count; i++)
            {
                _onAdditionHandlers[i].HandleNewConditionSet(this, condition, refs);
            }
            _conditionSets.Add(condition);
            return this;
        }

        public DialogEngine AddRule(DialogRule rule)
        {

            var refs = ExtractReferencesFromRule(rule);
            for (var i = 0; i < _onAdditionHandlers.Count; i++)
            {
                _onAdditionHandlers[i].HandleNewRule(this, rule, refs);
            }

            // extract any references

            // TODO add optimization; if we have already seen a reference go by, never find it again. It has already been placed into the correct bag.

            // if any refs match bag prefix, 
            
            //var bagAttrs = _attributes
            //    .OfType<BagDialogAttribute>()
            //    .ToList();
            //for (var i = 0; i < refs.Count; i++)
            //{
            //    var bestAttr = default(BagDialogAttribute);
            //    for (var b = 0; b < bagAttrs.Count; b ++)
            //    {
            //        if (refs[i].StartsWith(bagAttrs[b].Name) && (bestAttr == null || bagAttrs[b].Name.Length >= bestAttr.Name.Length))
            //        {
            //            bestAttr = bagAttrs[b];
            //        }
            //    }
            //    if (bestAttr != null)
            //    {
            //        bestAttr.Add(this, new BagElement()
            //        {
            //            name = refs[i].Substring(bestAttr.Name.Length + 1),
            //            value = false
            //        });
            //    }
            //}

            _rules.Add(rule);
            return this;
        }

        public List<string> ExtractReferencesFromRule(DialogRule rule)
        {
            var references = new List<string>();

            for (var i = 0; i < rule.Conditions?.Length; i++)
            {
                references.AddRange(rule.Conditions[i].Left.ExtractReferences());
                references.AddRange(rule.Conditions[i].Right.ExtractReferences());
            }

            //for (var i = 0; i < rule.Dialog?.Length; i++)
            //{
            //    rule.Dialog[i].ContentParts.Select(p => p.ExtractReferences()).ToList()
            //        .ForEach(refs => references.AddRange(refs));
            //    //references.AddRange(rule.Dialog[i].Content.ExtractReferences());
            //}

            for (var i = 0; i < rule?.Outcomes?.Length; i++)
            {
                references.AddRange(rule.Outcomes[i].Target.ExtractReferences());
                rule.Outcomes[i].Arguments.Values.Select(v => v.ExtractReferences())
                    .ToList().ForEach(refs => references.AddRange(refs)) ;
                //references.AddRange(rule.Outcomes[i].Arguments.)
            }

            return references.Distinct().ToList();
        }

        public List<string> ExtractReferencesFromCondition(DialogConditionSet condition)
        {
            var references = new List<string>();

            for (var i = 0; i < condition.Conditions?.Length; i++)
            {
                references.AddRange(condition.Conditions[i].Left.ExtractReferences());
                references.AddRange(condition.Conditions[i].Right.ExtractReferences());
            }
            
            return references.Distinct().ToList();
        }

        public DialogEngine AddAttribute(DialogAttribute attribute)
        {
            for (var i = 0; i < _onAdditionHandlers.Count; i++)
            {
                _onAdditionHandlers[i].HandleNewAttribute(this, attribute);
            }
            _attributes.Add(attribute);
            return this;
        }
        
        public DialogEngine AddTransform(string replace, Func<string> to)
        {
            _transformers.Add(new Regex("^" + replace), to);
            return this;
        }

        public Func<string, string> BuildTransformFunction()
        {
            return (input) =>
            {
                var output = input;

                foreach (var kv in _transformers)
                {
                    output = kv.Key.Replace(output, kv.Value());
                }
                //Console.WriteLine("Querying " + output);
                return output;
            };
        }

        public string[] ExecuteRuleDialogs(DialogRule rule)
        {

            var values = GetAttributeActualValues();
            var transform = BuildTransformFunction();
            return rule.Dialog
                .Select(d => String.Join("", d.ContentParts.Select(p => p.ProcessAsPrefixMathTyped(values, transform))))
                .ToArray();
        }

        public string[] ExecuteRuleSpeakers(DialogRule rule)
        {
            var values = GetAttributeActualValues();
            var transform = BuildTransformFunction();

            return rule.Dialog
                .Select(d => String.Join("", d.Speaker.ProcessAsPrefixMathTyped(values, transform)))
                .ToArray();
        }

        public void ExecuteRuleOutcomes(DialogRule rule)
        {
            var exceptions = new List<Exception>();
            for (var i = 0; i < rule.Outcomes.Length; i++)
            {
                try
                {
                    var transform = BuildTransformFunction();
                    var outcome = rule.Outcomes[i];
                    //var targetAttribute = _attributes.g
                    var transformedTargetName = transform(outcome.Target.ToLower()).ToLower();
                    var targetAttribute = _attributes.FirstOrDefault(a => a.Name.ToLower().Equals(transformedTargetName));
                    if (targetAttribute == null)
                    {
                        throw new Exception("Couldnt execute rule, because target attribute couldnt be found. " + outcome.Target + " as " + transformedTargetName);
                    }
                    var values = GetAttributeActualValues();

                    switch (outcome.Command)
                    {
                        case "set":
                            var value = outcome.Arguments[""].ProcessAsPrefixMathTyped(values, transform);
                            targetAttribute.Invoke(value);
                            break;
                        case "run":
                            var outputValues = new Dictionary<string, object>();
                            foreach (var kv in outcome.Arguments)
                            {
                                outputValues.Add(kv.Key, kv.Value.ProcessAsPrefixMathTyped(values, transform));
                            }
                            targetAttribute.Invoke(outputValues);
                            break;
                        default:
                            throw new NotImplementedException("Unknown outcome command " + outcome.Command);
                    }
                } catch (Exception ex)
                {
                    exceptions.Add(ex);
                }

            }

            if (exceptions.Count > 0)
            {
                var allMessages = string.Join("\n", exceptions.Select(e => e.Message));
                throw new Exception($"Failed to run all outcomes! {allMessages}");
            }
        }

        private Dictionary<string, object> GetAttributeActualValues()
        {
            var values = new Dictionary<string, object>(); // attribute name -> value
            for (var i = 0; i < _attributes.Count; i++)
            {
                var attribute = _attributes[i];
                attribute.Update();
                values.Add(attribute.Name.ToLower(), attribute.GetRealValue());
            }
            return values;
        }
        private Dictionary<string, long> GetAttributeValueCodes()
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

        private List<DialogRule> GetValidRules(Dictionary<string, long> attrNameToValue, Func<string, string> transformer)
        {
            var exceptions = new List<Exception>();
            var validRules = new List<DialogRule>();
            for (var i = 0; i < _rules.Count; i++)
            {
                var rule = _rules[i];
                var isValidRule = true;

                for (var j = 0; j < rule.Conditions.Length; j++)
                {
                    var condition = rule.Conditions[j];
                    var matched = false;

                    try
                    {
                        var leftValue = condition.Left.ToLower().ProcessAsPrefixMath(attrNameToValue, transformer);
                        var rightValue = condition.Right.ToLower().ProcessAsPrefixMath(attrNameToValue, transformer);

                        switch (condition.Op) // TODO add negation support
                        {
                            case "=":
                                matched = leftValue == rightValue;
                                break;
                            case "!=":
                            case "=!":
                                matched = leftValue != rightValue;
                                break;
                            case ">":
                            case "!<":
                                matched = leftValue > rightValue;
                                break;
                            case "<":
                            case ">!":
                                matched = leftValue < rightValue;
                                break;
                            default:
                                throw new InvalidOperationException("Unknown operation type" + condition.Op);
                        }
                    } catch (Exception ex)
                    {
                        exceptions.Add(ex);
                    }

                    isValidRule &= matched;
                }
                if (isValidRule == true)
                {
                    validRules.Add(rule);
                }

            }

            for (var i = 0; i < exceptions.Count; i++)
            {
                var allMessages = String.Join("\n", exceptions.Select(e => e.Message));
                Console.Error.WriteLine($"dialog rule EXCEPTION! {allMessages}");
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

        private DialogRule GetBestRuleForPlayer(List<DialogRule> rules, string speaker)
        {
            var best = default(DialogRule);
            for (var i = 0; i < rules.Count; i++)
            {
                var rule = rules[i];
                if (best != null 
                    && rule.Conditions.Length > best?.Conditions.Length 
                    && rule.Dialog.Length > 0
                    && rule.Dialog[0].Speaker.ToLower().Equals(speaker))
                {
                    best = rule;
                }
            }
            return best;
        }
    }
}
