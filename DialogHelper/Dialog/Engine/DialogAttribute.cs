using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dialog.Engine
{
    public class DialogAttribute
    {
        public string Name { get; private set; }
        public long CurrentValue { get; private set; }

        public DialogAttribute(string name)
        {
            Name = name;
        }

        protected virtual long FetchValue()
        {
            return 0;
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
            return str.GetHashCode();
            
        }
    }

    public class ObjectDialogAttribute : DialogAttribute
    {

        private object _target;
        private string[] _path;

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
            }
            return DialogAttribute.ValueToLong(obj);
        }
    }
}
