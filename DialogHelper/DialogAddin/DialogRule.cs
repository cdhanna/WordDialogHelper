using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DialogAddin
{
    class DialogRule
    {
        public string Name { get; set; }
        public string DisplayAs { get; set; }
        public List<DialogRuleSection> DialogSections { get; set; }
    }

    class DialogRuleSection
    {
        public string ActorName { get; set; }
        public string Dialog { get; set; }
    }
}
