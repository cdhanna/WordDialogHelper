using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dialog.Client.Messages
{
    public class PostValueMessage : DialogRuleClientMessage
    {
        public const string TYPE = "POST_VALUES";
        [JsonProperty("values")]
        public Dictionary<string, object> Values { get; set; }
        public PostValueMessage() : base(TYPE) { }
    }
}
