using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dialog
{
    public class DialogRule
    {
        [JsonProperty("guid")]
        public Guid Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("displayAs")]
        public string DisplayAs { get; set; }

        [JsonProperty("conditions")]
        public DialogCondition[] Conditions { get; set; }

        [JsonProperty("outcomes")]
        public DialogOutcome[] Outcomes { get; set; }

        [JsonProperty("dialog")]
        public DialogPart[] Dialog { get; set; }

        public class DialogPart
        {
            [JsonProperty("guid")]
            public Guid Id { get; set; }

            [JsonProperty("speaker")]
            public string Speaker { get; set; }

            [JsonProperty("text")]
            public string Content { get; set; }
        }

        public class DialogCondition
        {
            [JsonProperty("left")]
            public string Left { get; set; }

            [JsonProperty("right")]
            public string Right { get; set; }

            [JsonProperty("op")]
            public string Op { get; set; }
        }

        public class DialogOutcome
        {
            [JsonProperty("command")]
            public string Command { get; set; }

            [JsonProperty("target")]
            public string Target { get; set; }

            [JsonProperty("argument")]
            public string Argument { get; set; }
        }
    }
}
