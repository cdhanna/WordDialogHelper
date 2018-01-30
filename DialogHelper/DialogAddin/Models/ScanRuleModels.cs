using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Office = Microsoft.Office.Core;
using Word = Microsoft.Office.Interop.Word;
namespace DialogAddin.Models
{
    public class ScannedRule
    {
        public string Name { get; set; }
        public List<ScannedRuleSection> Sections { get; set; } = new List<ScannedRuleSection>();
        public Word.Document Document { get; set; }

        public Guid Id { get; set; } = Guid.NewGuid();
        public int Start { get; set; }

    }

    public class ScannedRuleSection
    {
        public string Name { get; set; }
        public string Content { get; set; }
        public int Start { get; set; }

    }
}
