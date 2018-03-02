using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Dialog.Engine
{
    public class DialogAttribute
    {

        public static BagDialogAttribute<TData, TElem> New<TData, TElem>(string name, TData defaultValue, List<TElem> elements)
            where TElem : BagElement<TData>
        {
            return new BagDialogAttribute<TData, TElem>(name, defaultValue, elements);
        }
        public static GlobalDialogAttribute<T> New<T>(string name, Action<T> setter, Func<T> getter)
        {
            return new GlobalDialogAttribute<T>(name, setter, getter);
        }


        public string Name { get; private set; }
        public long CurrentValue { get; private set; }
        

        public DialogAttribute(string name)
        {
            Name = name.ToLower();
        }

        protected virtual long FetchValue()
        {
            return 0;
        }

        public virtual object GetRealValue()
        {
            return 0;
        }


        public void Invoke(object value)
        {
            Invoke(new Dictionary<string, object>()
            {
                {"", value }
            });
        }
        public virtual void Invoke(Dictionary<string, object> values)
        {
            // 
        }

        public void Update()
        {
            CurrentValue = FetchValue();
        }

       

        public static long ValueToLong(object obj)
        {

            if (obj is int)
            {
                return (long)(int)(obj);
            }

            if (obj is string)
            {
                return ((string)obj).ToLong();
            }

            if (obj is bool)
            {
                return obj.Equals(true) ? 1 : 0;
            }

            throw new InvalidOperationException("Unknown type");
        }

    }

    public static class TypeExtensions
    {
        public static long ToLong(this string str)
        {

            /*
             * a -> 01
             * b -> 02
             * c -> 03
             * ..
             * z -> 26
             * 
             * abc -> 010203
             * 
             */

            return str.GetHashCode();
            
        }
    }

    public class ObjectFunctionDialogAttribute : DialogAttribute
    {
        private Action<Dictionary<string, object>> _action;
        private Dictionary<string, object> _defaults;
        public ObjectFunctionDialogAttribute(string name, Action<Dictionary<string, object>> method, Dictionary<string, object> defaults=null) : base(name)
        {
            _action = method;
            _defaults = defaults;
        }

        public override void Invoke(Dictionary<string, object> values)
        {
            foreach(var kv in _defaults)
            {
                if (values.ContainsKey(kv.Key) == false)
                {
                    values.Add(kv.Key, kv.Value);
                }
            }
            _action(values);
        }
    }

    public class ObjectDialogAttribute : DialogAttribute
    {

        private object _target;
        private string[] _path;

        private FieldInfo _lastFieldInfo;

        public ObjectDialogAttribute(object target, string baseName, params string[] path)
            : base(baseName + "." + String.Join(".", path))
        {
            _target = target;
            _path = path;
        }

        protected override long FetchValue()
        {
            var obj = _target;
            foreach (var part in _path)
            {
                if (obj == null) return default(long);
                var type = obj.GetType();
                var info = type.GetFields().FirstOrDefault(f => f.Name.ToLower().Equals(part.ToLower()));
                if (info == null) return default(long);
                obj = info.GetValue(obj);
                _lastFieldInfo = info;

            }
            return DialogAttribute.ValueToLong(obj);
        }

        public override object GetRealValue()
        {
            var obj = _target;
            foreach (var part in _path)
            {
                if (obj == null) return default(long);
                var type = obj.GetType();
                var info = type.GetFields().FirstOrDefault(f => f.Name.ToLower().Equals(part.ToLower()));
                if (info == null) return default(long);
                obj = info.GetValue(obj);
                _lastFieldInfo = info;
            }
            return obj;
        }

        public override void Invoke(Dictionary<string, object> values)
        {
            if (_lastFieldInfo == null)
            {
                throw new Exception("Attribute Never found matching field for " + Name);
            }
            _lastFieldInfo.SetValue(_target, values[""]);
        }
    }
}
