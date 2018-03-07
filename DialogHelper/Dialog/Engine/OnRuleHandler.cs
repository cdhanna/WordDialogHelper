using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dialog.Engine
{
    public class EngineAdditionHandler
    {

        public virtual void HandleNewAttribute(DialogEngine engine, DialogAttribute attrib)
        {

        }

        public virtual void HandleNewRule(DialogEngine engine, DialogRule rule, List<string> extractedRefs)
        {

        }

        public virtual void HandleNewConditionSet(DialogEngine engine, DialogConditionSet condition, List<string> extractedRefs)
        {

        }

    }


    public class ConditionSetEvalHandler : EngineAdditionHandler
    {
        private Dictionary<string, List<DialogRule.DialogCondition>> _nameToSet = new Dictionary<string, List<DialogRule.DialogCondition>>();

        public ConditionSetEvalHandler()
        {
           // GlobalName = globalName.ToLower();
        }

        public override void HandleNewRule(DialogEngine engine, DialogRule rule, List<string> extractedRefs)
        {

            var conds = rule.Conditions.ToList();
            var alreadyReplaced = new List<string>();
            for (var i = 0; i < conds.Count; i++)
            {
                var cond = conds[i];
                // try replacing left side...
                var leftRefs = cond.Left.ExtractReferences().ToList();
                if (leftRefs.Count == 1 && _nameToSet.ContainsKey(leftRefs[0]))
                {
                    if (alreadyReplaced.Contains(leftRefs[0]))
                    {
                        throw new Exception($"Invalid condition set usage. The condition set, {leftRefs[0]} has already been process for {rule.Name}, and doing so again would cause an infinite loop of darkness and despair.");
                    }

                    alreadyReplaced.Add(leftRefs[0]);
                    conds.RemoveAt(i);
                    _nameToSet[leftRefs[0]].ForEach(c => conds.Add(c));
                    
                    i = 0;
                }
                
            }

            rule.Conditions = conds.ToArray();
            
            base.HandleNewRule(engine, rule, extractedRefs);
        }

        public override void HandleNewConditionSet(DialogEngine engine, DialogConditionSet condition, List<string> extractedRefs)
        {
            _nameToSet.Add("__.conditions." + condition.Name.ToLower(),
                condition.Conditions.Select(c => new DialogRule.DialogCondition() { Left = c.Left, Op = c.Op, Right = c.Right }).ToList());
            base.HandleNewConditionSet(engine, condition, extractedRefs);
        }
    }

    public class BagIntHandler : BagAttributesRuleAddedHandler<int, BagIntElement>
    {

    }
    public class BagBoolHandler : BagAttributesRuleAddedHandler<bool, BagBoolElement>
    {

    }

    public class BagStringHandler : BagAttributesRuleAddedHandler<string, BagStringElement>
    {

    }


    public class BagAttributesRuleAddedHandler<TData, TElem> : EngineAdditionHandler
        where TElem : BagElement<TData>, new()
    {

        private List<BagDialogAttribute<TData, TElem>> _attribs = new List<BagDialogAttribute<TData, TElem>>();

        public override void HandleNewAttribute(DialogEngine engine, DialogAttribute attrib)
        {
            if (attrib is BagDialogAttribute<TData, TElem>)
            {
                //Console.WriteLine($"ADDING BAG ATTRIBUTE {typeof(TData).Name}, {typeof(TElem).Name}");
                _attribs.Add(attrib as BagDialogAttribute<TData, TElem>);
            }
            base.HandleNewAttribute(engine, attrib);
        }

        public override void HandleNewConditionSet(DialogEngine engine, DialogConditionSet condition, List<string> extractedRefs)
        {
            HandleRefs(engine, extractedRefs);
            base.HandleNewConditionSet(engine, condition, extractedRefs);
        }

        public override void HandleNewRule(DialogEngine engine, DialogRule rule, List<string> extractedRefs)
        {

            //if (rule.Name.Equals("Rule Introduce Yourself Chief Alchemist Vol"))
            //{
            //    Console.WriteLine("FOUND SPECIAL RULE");
            //    Console.WriteLine(rule.Name);
            //    Console.WriteLine("Extracted Refs ");
            //    extractedRefs.ForEach(r => Console.WriteLine("\t" + r));
            //}

            HandleRefs(engine, extractedRefs);
            base.HandleNewRule(engine, rule, extractedRefs);
        }

        private void HandleRefs(DialogEngine engine, List<string> extractedRefs)
        {
            for (var i = 0; i < extractedRefs.Count; i++)
            {
                var bestAttr = default(BagDialogAttribute<TData, TElem>);
                for (var b = 0; b < _attribs.Count; b++)
                {
                    if (extractedRefs[i].ToLower().StartsWith(_attribs[b].Name)
                        && (bestAttr == null
                            || _attribs[b].Name.Length >= bestAttr.Name.Length)
                            )
                    {
                        bestAttr = _attribs[b];
                    }
                }
                if (bestAttr != null)
                {

                    Console.WriteLine($"Found Ref {extractedRefs[i]} for {bestAttr.Name}");

                    bestAttr.Add(engine, new TElem()
                    {
                        name = extractedRefs[i].Substring(bestAttr.Name.Length + 1).ToLower(),
                        value = bestAttr.DefaultValue
                    });
                }
            }
        }
    }
    
}
