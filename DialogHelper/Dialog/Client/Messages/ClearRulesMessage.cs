using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dialog.Client.Messages
{
    public class ClearRulesMessage : DialogRuleClientMessage
    {
        public const string TYPE = "CLEAR_RULES";
        public ClearRulesMessage() : base(TYPE) { }
    }
}
