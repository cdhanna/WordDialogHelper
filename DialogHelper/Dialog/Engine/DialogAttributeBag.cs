using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dialog.Engine
{
    //[Serializable]
    //public class BagElement
    //{
    //    public string name;
    //    public bool value;
    //}

    

    [Serializable]
    public class BagElement<T>
    {
        public string name;
        public T value;
    }

    [Serializable]
    public class BagBoolElement : BagElement<bool> { }
    [Serializable]
    public class BagIntElement : BagElement<int> { }
    [Serializable]
    public class BagStringElement : BagElement<string> { }


    public class BagDialogAttribute<TData, TElem> : DialogAttribute
        where TElem : BagElement<TData>
    {
        public List<TElem> Elements { get; private set; }

        //private Dictionary<string, TElem> _nameToElement = new Dictionary<string, TElem>();
        //private Dictionary<string, DialogAttribute> _nameToAttribute = new Dictionary<string, DialogAttribute>();
        public TData DefaultValue { get; set; }

        public BagDialogAttribute(string baseName, TData defaultValue, List<TElem> elements) : base(baseName)
        {
            Elements = elements;
            DefaultValue = defaultValue;
            if (Elements == null)
            {
                Elements = new List<TElem>();
            }

        }

        public void Add(DialogEngine dEngine, TElem element)
        {
            element.name = element.name.Replace(" ", ".");
            element.name = element.name.ToLower();
            var fullyQualifiedName = Name + "." + element.name;



            Console.WriteLine($"ADDING BAG ELEMENT. NAME {fullyQualifiedName} TYPE {typeof(TElem).Name}");
            if (!dEngine.HasAttribute(fullyQualifiedName))
            {
                if (Elements.Contains(element) == false)
                {
                    Elements.Add(element);

                }

                dEngine.AddAttribute(DialogAttribute.New(Name + '.' + element.name, v => element.value = v, () => element.value));

            }
        }

        protected override long FetchValue()
        {

            
            return base.FetchValue();
        }

        public override void Invoke(Dictionary<string, object> values)
        {
            throw new Exception($"This is a bag attribute, and can never be invoked directly {Name}");
        }

        public BagDialogAttribute<TData, TElem> UpdateElements(DialogEngine dEngine)
        {
            Elements.ForEach(e => Add(dEngine, e));
            return this;
        }

    }
}
