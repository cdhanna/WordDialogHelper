using Dialog;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DialogTests.AttributeFetch
{
    [TestClass]
    public class VariableValidationTests
    {

        [TestMethod]
        public void Simple()
        {
            var c = new VariableCollection();
            c.Add("int", "x");
            Assert.IsTrue(c.Exists("x"));
        }

        [TestMethod]
        public void NestedCapital()
        {
            var c = new VariableCollection();
            c.Add("int", "x Y z");
            Assert.IsTrue(c.Exists("x y Z"));
        }

        [TestMethod]
        public void BagInt()
        {
            var c = new VariableCollection();
            c.Add("int*", "x ints");
            Assert.IsTrue(c.Exists("x ints a"));
        }

        [TestMethod]
        public void NoExist()
        {
            var c = new VariableCollection();
            c.Add("int", "x ints");
            Assert.IsFalse(c.Exists("x ints a"));
        }
    }
}
