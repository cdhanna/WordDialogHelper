using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DialogAddin.WordLang.AST
{
    public class RuleSet : RuleTree
    {
        public List<Rule> Rules { get; set; } = new List<Rule>();

        public RuleSet()
        {

        }

        public bool Equals(RuleSet other)
        {
            if (other == null) return false;
            return Rules.SequenceEqual(other.Rules);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as RuleSet);
        }
        
    }
}
