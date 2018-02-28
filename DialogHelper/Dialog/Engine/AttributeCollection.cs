using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dialog.Engine
{
    public class AttributeCollection
    {

        //private Dictionary<string, DialogAttribute>

        public AttributeCollection AddFullName(DialogAttribute attribute)
        {
            // add the attribute
            return this;
        }

        public AttributeCollection AddPrefix(DialogAttribute attribute)
        {


            return this;
        }

        public void Find(string name)
        {

        }

    }
}
