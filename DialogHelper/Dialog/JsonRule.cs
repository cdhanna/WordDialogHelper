using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dialog
{
    public class JsonRule
    {
        [JsonProperty("guid")]
        public Guid Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("displayAs")]
        public string DisplayAs { get; set; }

        [JsonProperty("conditions")]
        public string[] Conditions { get; set; }

        [JsonProperty("outcomes")]
        public string[] Outcomes { get; set; }

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
    }
}
