using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dialog.Engine
{
    [Serializable]
    public class BagElement
    {
        public string name;
        public bool value;
    }

    public class BagDialogAttribute : DialogAttribute
    {
        public List<BagElement> Elements { get; private set; }

        private Dictionary<string, BagElement> _nameToElement = new Dictionary<string, BagElement>();
        private Dictionary<string, DialogAttribute> _nameToAttribute = new Dictionary<string, DialogAttribute>();


        public BagDialogAttribute(string baseName, List<BagElement> elements) : base(baseName)
        {
            Elements = elements;
            if (Elements == null)
            {
                Elements = new List<BagElement>();
            }

        }

        public void Add(DialogEngine dEngine, BagElement element)
        {
            if (!_nameToElement.ContainsKey(element.name))
            {
                if (Elements.Contains(element) == false)
                {
                    Elements.Add(element);

                }
                _nameToElement.Add(element.name, element);

                dEngine.AddAttribute(GlobalDialogAttribute.New(Name + '.' + element.name, v => element.value = v, () => element.value));

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

        public BagDialogAttribute UpdateElements(DialogEngine dEngine)
        {
            Elements.ForEach(e => Add(dEngine, e));
            return this;
        }

    }
}
