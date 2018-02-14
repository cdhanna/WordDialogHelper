using System;
using Dialog.Engine;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DialogTests.AttributeFetch
{
    [TestClass]
    public class SimpleAttrTests
    {
        class Dummy
        {
            public int x = 5;
            public string y = "monkey";

        }
        class DummyCollection
        {
            public Dummy d = new Dummy();
            public Dummy d2 = new Dummy();
        }

        [TestMethod]
        public void SimpleAttributeTest()
        {
            var dummy = new Dummy();

            var attr = new ObjectDialogAttribute(dummy, "X");

            attr.Update();

            var output = attr.CurrentValue;

            Assert.AreEqual(5, output);
        }

        [TestMethod]
        public void SimpleNestedTest()
        {
            var dc = new DummyCollection();

            var attr = new ObjectDialogAttribute(dc, "d", "y");

            attr.Update();

            var output = attr.CurrentValue;

            Assert.AreEqual(DialogAttribute.ValueToLong("monkey"), output);
        }
    }
}
