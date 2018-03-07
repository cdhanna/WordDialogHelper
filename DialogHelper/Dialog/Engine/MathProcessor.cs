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
                var methods = typeof(MathExtensions).GetMethods();
                //var e = GetExtensionMethods(Assembly.GetAssembly(typeof(MathExtensions)), a.GetType());
                if (methods.Count() == 0)
                {
                    throw new Exception("No extensions method exist for type");
                }
                var mi = methods.FirstOrDefault(m => m.Name.Equals(method) && m.GetParameters().Length == 2 && m.GetParameters()[0].ParameterType == a.GetType() && m.GetParameters()[1].ParameterType == b.GetType());

                return mi.Invoke(null, new object[] { a, b });
            } catch (Exception ex)
            {
                throw new Exception($"Failed to {method} objects. ", ex);
            }
        }

        public static object ProcessAsPrefixMathTyped(this string expression, Dictionary<string, object> variables = null, Func<string, string> transformer=null)
        {
            if (variables == null)
            {
                variables = new Dictionary<string, object>();
            }
            if (transformer== null)
            {
                transformer = (s) => s;
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
                        bool inString = false;
                        while (j > -1 && (v != ' '||inString) ) //&& v != ')' && v != '(' && v != '+' && v != '*' && v != '/' && v != '-')
                        {
                            if (v == '\'' || v == '"')
                            {
                                inString = !inString;
                            }
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
                        else if (buildingSym.EndsWith("'") && buildingSym.StartsWith("'") && buildingSym.Length > 1)
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
                            buildingSym = transformer(buildingSym);

                            var value = variables.ExtractAttribute(buildingSym);
                            stack.Push(value);
                            //if (!variables.ContainsKey(buildingSym))
                            //{
                            //    throw new Exception($"Cannot evaluate expression, because {buildingSym} did not exist");
                            //}
                            //stack.Push(variables[buildingSym]);
                        }


                        break;
                }

            }

            return stack.Pop();
        }

        public static long ProcessAsPrefixMath(this string expression, Dictionary<string, long> variables=null, Func<string, string> transformer = null)
        {
            if (variables == null)
            {
                variables = new Dictionary<string, long>();
            }
            if (transformer == null)
            {
                transformer = s => s;
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
                        bool inString = false;

                        buildingSym = "";
                        var j = i;
                        var v = c;
                        while (j > -1 && (v != ' ' || inString)) //(j > -1 && v != ' ' && v != ')' && v != '(' && v != '+' && v != '*' && v != '/' && v != '-')
                        {
                            if (v == '\'' || v == '"')
                            {
                                inString = !inString;
                            }
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
                        }
                        else if (buildingSym.EndsWith("'") && buildingSym.StartsWith("'") && buildingSym.Length > 1)

                        //} else if (buildingSym.EndsWith("'") && buildingSym.StartsWith("'"))
                        {
                            stack.Push(buildingSym.Substring(1, buildingSym.Length - 2).ToLong());
                        }
                        else if (numbers.Contains(buildingSym[0]))
                        {
                            stack.Push(int.Parse(buildingSym));
                        }
                        else
                        {
                            buildingSym = buildingSym.ToLower();
                            buildingSym = transformer(buildingSym);
                            var value = variables.ExtractAttribute(buildingSym);

                            stack.Push(value);
                        }


                        break;
                }

            }
            
            return stack.Pop();
        }
    }
}
