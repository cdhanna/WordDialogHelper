using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dialog.Client.Messages
{
    public class ClearVariablesMessage : DialogRuleClientMessage
    {
        public const string TYPE = "CLEAR_VARIABLES";
        
        public ClearVariablesMessage() : base(TYPE) { }
    }
}
