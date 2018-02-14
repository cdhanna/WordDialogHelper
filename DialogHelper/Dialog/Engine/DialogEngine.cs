using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace Dialog.Engine
{
    public class DialogEngine
    {

        private List<DialogAttribute> _attributes = new List<DialogAttribute>();

        public void RunDialog()
        {

        }

        public DialogEngine AddAttribute(DialogAttribute attribute)
        {
            _attributes.Add(attribute);
            return this;
        }
        
       
    }
}
