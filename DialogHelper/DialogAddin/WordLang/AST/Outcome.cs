using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DialogAddin.WordLang.AST
{
    public class Outcome : RuleTree
    {
        public FieldString Action { get; set; }

        public bool Equals(Outcome other)
        {
            return other != null
                && other.Action.Equals(Action);
        }
        public override bool Equals(object obj)
        {
            return Equals(obj as Outcome);
        }
    }
}
