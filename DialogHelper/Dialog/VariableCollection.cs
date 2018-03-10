using Dialog.Engine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dialog
{
    public class VariableCollection : IEnumerable<Variable>
    {
        private List<Variable> _variables = new List<Variable>();

        public List<Variable> Source { get { return _variables; } }

        public VariableCollection Clear()
        {
            _variables.Clear();
            return this;
        }

        public VariableCollection Add(string typeName, string fullName)
        {
            
            return Add(new Variable()
            {
                Type = typeName.ToLower(),
                Path = fullName.Split(new char[] { ' ', '.' }, StringSplitOptions.RemoveEmptyEntries),
            });
        }

        public VariableCollection Add(Variable v)
        {
            
            _variables.Add(v);
            return this;
        }

        public bool Exists(string name)
        {
            var normalized = name.NormalizeAttributeName();

            var parts = normalized.Split('.');

            var variable = _variables.FirstOrDefault(v =>
            {
                var fullName = v.FullName;
                if (v.IsBag)
                {
                    return normalized.StartsWith(fullName);
                } else
                {
                    return fullName.Equals(normalized);
                }
            });


            return variable != null;
        }

        public Variable GetFromPath(string[] path)
        {
            return _variables.FirstOrDefault(v => v.Path.SequenceEqual(path));
        }

        public IEnumerator<Variable> GetEnumerator()
        {
            
            return _variables.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _variables.GetEnumerator();
        }
    }
}
