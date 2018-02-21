using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dialog.Client.Messages
{
    public class PostVariableMessage : DialogRuleClientMessage
    {
        public const string TYPE = "POST_VARIABLE";

        [JsonProperty("variable")]
        public Variable[] Variables { get; set; }

        public PostVariableMessage() : base(TYPE) { }
    }
}
