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

            var attr = new ObjectDialogAttribute(dummy, "dummy", "X");

            attr.Update();

            var output = attr.CurrentValue;

            Assert.AreEqual(5, output);
        }

        [TestMethod]
        public void GlobalAttrib()
        {
            var c = true;
            var attr = GlobalDialogAttribute.New("c", val => c = val, () => c);

            attr.Update();

            var output = attr.CurrentValue;

            Assert.AreEqual(1, output);

            c = false;
            attr.Update();
            Assert.AreEqual(0, attr.CurrentValue);

            attr.Invoke(true);
            attr.Update();
            Assert.AreEqual(1, attr.CurrentValue);
        }

        [TestMethod]
        public void SimpleNestedTest()
        {
            var dc = new DummyCollection();

            var attr = new ObjectDialogAttribute(dc, "dummy", "d", "y");

            attr.Update();

            var output = attr.CurrentValue;

            Assert.AreEqual(DialogAttribute.ValueToLong("monkey"), output);
        }

        [TestMethod]
        public void StringToLong()
        {
            var str = "a";
            var str2 = "a";

            var code = str.ToLong();
            var code2 = str2.ToLong();

            Assert.AreEqual(code2, code);
        }
    }
}
