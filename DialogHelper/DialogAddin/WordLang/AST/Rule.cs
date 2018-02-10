using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DialogAddin.WordLang.AST
{
    public class Rule : RuleTree
    {

        public FieldString Title { get; set; }
        public FieldString DisplayAs { get;set; }

        public List<Condition> Conditions { get; set; }
        public List<Dialog> Dialogs { get; set; }
        public List<Outcome> Outcomes { get; set; }

        public Rule()
        {

        }

        public bool Equals(Rule other)
        {
            if (other == null) return false;
            return other.Title.Equals(Title)
                && other.DisplayAs.Equals(DisplayAs)
                && other.Conditions.SequenceEqual(Conditions)
                && other.Dialogs.SequenceEqual(Dialogs)
                && other.Outcomes.SequenceEqual(Outcomes);
        }
        public override bool Equals(object obj)
        {
            return Equals(obj as Rule);
        }
        
    }
}
