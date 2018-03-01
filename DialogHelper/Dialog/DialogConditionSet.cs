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

        public class DialogCondition
        {
            [JsonProperty("op")]
            public string Op { get; set; }
            [JsonProperty("left")]
            public string Left { get; set; }

            [JsonProperty("right")]
            public string Right { get; set; }

        }
    }
}
