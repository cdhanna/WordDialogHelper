using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dialog
{
    public class Variable
    {
        public string[] Path { get; set; }
        public string Type { get; set; }
        public string FullName { get { return String.Join(".", Path); } }


    }
}
