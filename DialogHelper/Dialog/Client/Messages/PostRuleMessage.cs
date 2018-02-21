using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dialog.Client.Messages
{
    public class PostRuleMessage : DialogRuleClientMessage
    {
        public const string TYPE = "POST_RULE";

        [JsonProperty("rules")]
        public DialogRule[] Rules;

        public PostRuleMessage() : base(TYPE) { }
    }
}
