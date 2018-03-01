using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dialog.Engine
{
    public static class GlobalDialogAttribute
    {
        
    }

    public class GlobalDialogAttribute<T> : DialogAttribute
    {
        public string Name { get; private set; }
        public Action<T> Setter { get; private set; }
        public Func<T> Getter { get; private set; }

        public GlobalDialogAttribute(string name, Action<T> setter, Func<T> getter) : base(name)
        {
            Name = name;
            Setter = setter;
            Getter = getter;
        }

        protected override long FetchValue()
        {
            var obj = Getter();
            return DialogAttribute.ValueToLong(obj);
        }

        public override object GetRealValue()
        {
            var obj = Getter();
            return obj;
        }

        public override void Invoke(Dictionary<string, object> values)
        {
            var val = values[""];
            if (val is T)
            {
                Setter((T)values[""]);
            } else
            {
                throw new Exception("You have an incorrect typing in the global setter");

            }
        }
    }
}
