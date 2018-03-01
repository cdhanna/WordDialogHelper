using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dialog
{
    public class DialogBundle
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("rules")]
        public DialogRule[] Rules { get; set; } 

        [JsonProperty("conditionSets")]
        public DialogConditionSet[] ConditionSets { get; set; } 

        public override bool Equals(object obj)
        {
            return Equals(obj as DialogBundle);
        }

        public bool Equals(DialogBundle other)
        {
            if (other == null) return false;
            return other.Name.Equals(Name)
                && (other.Rules != null && other.Rules.SequenceEqual(Rules))
                && ((other.ConditionSets == null && ConditionSets == null) || (other.ConditionSets != null && other.ConditionSets.SequenceEqual(ConditionSets)));
        }
    }
}
