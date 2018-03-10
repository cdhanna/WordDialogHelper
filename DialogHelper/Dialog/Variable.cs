using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dialog
{
    public class Variable
    {
        public string[] Path { get; set; }
        public string Type { get; set; }
        public string FullName { get { return String.Join(".", Path).ToLower(); } }
        public bool IsBag { get { return Type.EndsWith("*"); } }

    }
}
