using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DialogAddin.WordLang.AST
{
    public class FieldValue<T> : RuleTree
    {
        public T Value { get; set; }
        public int StartIndex { get; set; }
        public int StopIndex { get; set; }

        public bool Equals(FieldValue<T> other)
        {
            if (other == null) return false;
            return other.StartIndex.Equals(StartIndex)
                && other.StopIndex.Equals(StopIndex)
                && other.Value.Equals(Value);
        }
        public override bool Equals(object obj)
        {
            return Equals(obj as FieldValue<T>);
        }
    }
    public class FieldString : FieldValue<string>
    {
        
        public FieldString()
        {

        }

    }

    public class FieldComparisonOperator : FieldValue<ComparisonOperator>
    {

    }
}
