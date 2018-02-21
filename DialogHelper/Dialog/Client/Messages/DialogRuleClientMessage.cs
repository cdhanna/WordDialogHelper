using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dialog.Client.Messages
{
    public class DialogRuleClientMessage
    {
        [JsonProperty("type")]
        public string Type { get; set; }
        
        public DialogRuleClientMessage(string type)
        {
            Type = type;
        }


    }
    
    
    
}
