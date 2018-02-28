using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dialog.Engine
{
    public static class AttributeExtensions
    {

        public static object ExtractAttribute(this Dictionary<string, object> attrs, string key)
        {
            if (attrs.ContainsKey(key))
            {
                return attrs[key];
            } else
            {
                throw new Exception($"Attribute didnt exist. {key}");
            }
        }

        public static long ExtractAttribute(this Dictionary<string, long> attrs, string key)
        {
            if (attrs.ContainsKey(key))
            {
                return attrs[key];
            }
            else
            {
                throw new Exception($"Attribute didnt exist. {key}");
            }
        }


        public static List<string> ExtractReferences(this string expression)
        {
            var output = new List<string>();
            var buildingSym = default(string);
           
            var numbers = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };


            for (var i = expression.Length - 1; i > -1; i--)
            {
                // read input stream looking for symbols, number, ), (, +, -, *, /
                var c = expression[i];

                switch (c)
                {
                    case '(':
                    case ')':
                    case ' ':
                        // ignore it, yo
                        break;

                    case '/':
                    case '*':
                    case '-':
                    case '+':
                        // keep ignoring it, yo
                        break;
                    default:
                        // Symbol time!
                        buildingSym = "";
                        var j = i;
                        var v = c;
                        while (j > -1 && v != ' ' && v != ')' && v != '(' && v != '+' && v != '*' && v != '/' && v != '-')
                        {
                            buildingSym = v + buildingSym;
                            j--;
                            v = j > -1 ? expression[j] : ' ';
                        }
                        i = j + 1;

                        if (buildingSym.Equals("true"))
                        {
                            
                        }
                        else if (buildingSym.Equals("false"))
                        {
                            
                        }
                        else if (buildingSym.EndsWith("'") && buildingSym.StartsWith("'"))
                        {
                            
                        }
                        else if (numbers.Contains(buildingSym[0]))
                        {

                        }
                        else
                        {
                            output.Add(buildingSym.ToLower());
                        }


                        break;
                }

            }

            return output.Distinct().ToList();
        }

    }
}
