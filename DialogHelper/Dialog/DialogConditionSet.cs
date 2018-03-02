using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dialog
{
    public class DialogConditionSet
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("conditions")]
        public DialogCondition[] Conditions { get; set; }


        public override bool Equals(object obj)
        {
            return Equals(obj as DialogConditionSet);
        }

        public bool Equals(DialogConditionSet other)
        {
            if (other == null) return false;
            return other.Name.Equals(Name)
                && other.Conditions.SequenceEqual(Conditions);
        }

        public class DialogCondition
        {
            [JsonProperty("op")]
            public string Op { get; set; }
            [JsonProperty("left")]
            public string Left { get; set; }

            [JsonProperty("right")]
            public string Right { get; set; }

            public override bool Equals(object obj)
            {
                return Equals(obj as DialogCondition);
            }

            public bool Equals(DialogCondition other)
            {
                if (other == null) return false;
                return other.Op.Equals(Op)
                    && other.Left.Equals(Left)
                    && other.Right.Equals(Right);
            }

        }
    }
}
