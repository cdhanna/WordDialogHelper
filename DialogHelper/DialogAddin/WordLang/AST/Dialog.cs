using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DialogAddin.WordLang.AST
{
    public class Dialog : RuleTree
    {
        public FieldString Speaker { get; set; }
        public FieldString Line { get; set; }

        public bool Equals(Dialog other)
        {
            return other != null
                && other.Speaker.Equals(Speaker)
                && other.Line.Equals(Line);
        }
        public override bool Equals(object obj)
        {
            return Equals(obj as Dialog);
        }
    }
}
