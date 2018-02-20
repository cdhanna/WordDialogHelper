using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dialog.Engine
{
    public static class MathExtensions
    {
        public static long MathAdd(this long a, long b)
        {
            return 0;
        }

        public static string MathAdd(this string a, string b)
        {
            return a + b;
        }
        public static string MathAdd(this string a, int b)
        {
            return a + b;
        }
        public static string MathAdd(this int a, string b)
        {
            return a + b;
        }

        public static bool MathAdd(this bool a, bool b)
        {
            return a || b;
        }
        public static bool MathMultiply(this bool a, bool b)
        {
            return a && b;
        }


        public static int MathAdd(this int a, int b)
        {
            return a + b;
        }
        public static int MathMultiply(this int a, int b)
        {
            return a * b;
        }
        public static int MathDivide(this int a, int b)
        {
            return a / b;
        }
        public static int MathSubtract(this int a, int b)
        {
            return a - b;
        }
    }
}
