using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dialog.Engine
{
    public static class MathProcessor
    {
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
