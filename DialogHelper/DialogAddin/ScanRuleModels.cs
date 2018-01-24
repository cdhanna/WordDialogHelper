using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DialogAddin
{
    public class ScannedRule
    {
        public string Name { get; set; }
        public List<ScannedRuleSection> Sections { get; set; } = new List<ScannedRuleSection>();
    }

    public class ScannedRuleSection
    {
        public string Name { get; set; }
        public string Content { get; set; }
    }
}
