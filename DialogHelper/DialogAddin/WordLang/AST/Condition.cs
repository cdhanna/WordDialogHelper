using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DialogAddin.WordLang.AST
{
    public class Condition : RuleTree
    {
        public FieldString LeftKey { get; set; }
        public FieldString RightKey { get; set; }
        public FieldComparisonOperator Operator { get; set; }
        public bool Negated { get; set; }

        public Condition()
        {

        }

        public bool Equals(Condition other)
        {
            if (other == null) return false;
            return LeftKey.Equals(other.LeftKey)
                && RightKey.Equals(other.RightKey)
                && Operator.Equals(other.Operator)
                && Negated.Equals(other.Negated);
        }
        public override bool Equals(object obj)
        {
            return Equals(obj as Condition);
        }
    }
}
