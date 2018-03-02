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
        public string GlobalName { get; private set; }

        public ConditionSetEvalHandler(string globalName)
        {
            GlobalName = globalName.ToLower();
        }

        public override void HandleNewConditionSet(DialogEngine engine, DialogConditionSet condition, List<string> extractedRefs)
        {
            // add an attribute for every condition that is set.

            engine.AddAttribute(DialogAttribute.New(GlobalName + "." + condition.Name,
                v => { }, // ignore set
                () =>
                {
                    //condition.Conditions.
                    return false;
                }
            ));

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
                    if (extractedRefs[i].StartsWith(_attribs[b].Name)
                        && (bestAttr == null
                            || _attribs[b].Name.Length >= bestAttr.Name.Length)
                            )
                    {
                        bestAttr = _attribs[b];
                    }
                }
                if (bestAttr != null)
                {
                    bestAttr.Add(engine, new TElem()
                    {
                        name = extractedRefs[i].Substring(bestAttr.Name.Length + 1),
                        value = bestAttr.DefaultValue
                    });
                }
            }
        }
    }
}
