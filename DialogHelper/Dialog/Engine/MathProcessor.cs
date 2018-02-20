using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace Dialog.Engine
{
    
    public static class MathProcessor
    {
        private const string METHOD_ADD = "MathAdd";
        private const string METHOD_MULTIPLY = "MathMultiply";
        private const string METHOD_DIVIDE = "MathDivide";
        private const string METHOD_SUBTRACT = "MathSubtract";
       
        static IEnumerable<MethodInfo> GetExtensionMethods(Assembly assembly, Type extendedType)
        {
            List<MethodInfo> extension_methods = new List<MethodInfo>();

            foreach (Type t in assembly.GetTypes())
            {
                if (t.IsDefined(typeof(ExtensionAttribute), false))
                {
                    foreach (MethodInfo mi in t.GetMethods())
                    {
                        if (mi.IsDefined(typeof(ExtensionAttribute), false))
                        {
                            if (mi.GetParameters()[0].ParameterType == extendedType)
                                extension_methods.Add(mi);
                        }
                    }
                }
            }
            return extension_methods;
        }

        private static object CombineObjects(string method, object a, object b)
        {
            if (a == null || b == null)
            {
                throw new Exception($"Cannot {method} a null value");
            }
            try
            {
                var e = GetExtensionMethods(Assembly.GetExecutingAssembly(), a.GetType());
                if (e.Count() == 0)
                {
                    throw new Exception("No extensions method exist for type");
                }
                var mi = e.FirstOrDefault(m => m.Name.Equals(method) && m.GetParameters().Length == 2 && m.GetParameters()[0].ParameterType == a.GetType() && m.GetParameters()[1].ParameterType == b.GetType());

                return mi.Invoke(null, new object[] { a, b });
            } catch (Exception ex)
            {
                throw new Exception($"Failed to {method} objects. ", ex);
            }
        }

        public static object ProcessAsPrefixMathTyped(this string expression, Dictionary<string, object> variables = null)
        {
            if (variables == null)
            {
                variables = new Dictionary<string, object>();
            }

            

            var buildingSym = default(string);
            var stack = new Stack<object>();
            var left = default(object);
            var right = default(object);

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
                        // Divide!
                        left = stack.Pop();
                        right = stack.Pop();
                        stack.Push(CombineObjects(METHOD_DIVIDE, left, right));
                        break;
                    case '*':
                        // Multiply!
                        left = stack.Pop();
                        right = stack.Pop();
                        stack.Push(CombineObjects(METHOD_MULTIPLY, left, right));
                        break;
                    case '-':
                        // Subtract!
                        left = stack.Pop();
                        right = stack.Pop();
                        stack.Push(CombineObjects(METHOD_SUBTRACT, left, right));
                        break;
                    case '+':
                        // Add!
                        left = stack.Pop();
                        right = stack.Pop();
                        stack.Push(CombineObjects(METHOD_ADD, left, right));
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
                            stack.Push(true);
                        }
                        else if (buildingSym.Equals("false"))
                        {
                            stack.Push(false);
                        }
                        else if (buildingSym.EndsWith("'") && buildingSym.StartsWith("'"))
                        {
                            stack.Push(buildingSym.Substring(1, buildingSym.Length - 2));
                        }
                        else if (numbers.Contains(buildingSym[0]))
                        {
                            stack.Push(int.Parse(buildingSym));
                        }
                        else
                        {
                            buildingSym = buildingSym.ToLower();
                            if (!variables.ContainsKey(buildingSym))
                            {
                                throw new Exception($"Cannot evaluate expression, because {buildingSym} did not exist");
                            }
                            stack.Push(variables[buildingSym]);
                        }


                        break;
                }

            }

            return stack.Pop();
        }

        public static long ProcessAsPrefixMath(this string expression, Dictionary<string, long> variables=null)
        {
            if (variables == null)
            {
                variables = new Dictionary<string, long>();
            }
            var buildingSym = default(string);
            var stack = new Stack<long>();
            var left = default(long);
            var right = default(long);

            var numbers = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
            

            for (var i = expression.Length - 1; i > -1; i --)
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
                        // Divide!
                        left = stack.Pop();
                        right = stack.Pop();
                        stack.Push(left / right);
                        break;
                    case '*':
                        // Multiply!
                        left = stack.Pop();
                        right = stack.Pop();
                        stack.Push(left * right);
                        break;
                    case '-':
                        // Subtract!
                        left = stack.Pop();
                        right = stack.Pop();
                        stack.Push(left - right);
                        break;
                    case '+':
                        // Add!
                        left = stack.Pop();
                        right = stack.Pop();
                        stack.Push(left + right);
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
                            stack.Push(1);
                        }
                        else if (buildingSym.Equals("false"))
                        {
                            stack.Push(0);
                        } else if (buildingSym.EndsWith("'") && buildingSym.StartsWith("'"))
                        {
                            stack.Push(buildingSym.Substring(1, buildingSym.Length - 2).ToLong());
                        } else if (numbers.Contains(buildingSym[0]))
                        {
                            stack.Push(int.Parse(buildingSym));
                        }
                        else
                        {
                            stack.Push(variables[buildingSym]);
                        }


                        break;
                }

            }
            
            return stack.Pop();
        }
    }
}
