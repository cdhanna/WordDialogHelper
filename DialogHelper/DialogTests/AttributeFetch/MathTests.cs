﻿using Dialog.Engine;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DialogTests.AttributeFetch
{
    [TestClass]
    public class MathTests
    {

        [TestMethod]
        public void NumericalAdd()
        {
            var math = "(+ 206 14)";
            var result = math.ProcessAsPrefixMath();
            Assert.AreEqual(220, result);
        }

        [TestMethod]
        public void NumericalAddChain()
        {
            var math = "(+ 5 (+ 1 2))";
            var result = math.ProcessAsPrefixMath();
            Assert.AreEqual(8, result);
        }

        [TestMethod]
        public void NumericalSubtract()
        {
            var math = "(- 10 5)";
            var result = math.ProcessAsPrefixMath();
            Assert.AreEqual(5, result);
        }

        [TestMethod]
        public void NumericalMult()
        {
            var math = "(* 10 5)";
            var result = math.ProcessAsPrefixMath();
            Assert.AreEqual(50, result);
        }

        [TestMethod]
        public void NumericalDivide()
        {
            var math = "(/ 10 5)";
            var result = math.ProcessAsPrefixMath();
            Assert.AreEqual(2, result);
        }

        [TestMethod]
        public void NumericalChains()
        {
            var math = "(+ (* (/ 6(+ 1 2)) 4) 2)";
            var result = math.ProcessAsPrefixMath();
            Assert.AreEqual(10, result);
        }

        [TestMethod]
        public void NoExprssion()
        {
            var math = "5";
            var result = math.ProcessAsPrefixMath();
            Assert.AreEqual(5, result);
        }

        [TestMethod]
        public void NoExprssionBool()
        {
            var math = "true";
            var result = math.ProcessAsPrefixMath();
            Assert.AreEqual(1, result);
        }

        [TestMethod]
        public void BoolAddChain()
        {
            var math = "(+ true (+ false true))";
            var result = math.ProcessAsPrefixMath();
            Assert.AreEqual(2, result);
        }

        [TestMethod]
        public void NoExpressionStr()
        {
            var math = "'egg'";
            var result = math.ProcessAsPrefixMath();
            Assert.AreEqual("egg".ToLong(), result);
        }

        [TestMethod]
        public void StrAddChain()
        {
            var math = "(+ 'egg' (+ 2 'a'))";
            var result = math.ProcessAsPrefixMath();
            Assert.AreEqual("egg".ToLong() + 2 + "a".ToLong(), result);
        }

        [TestMethod]
        public void NoExpressionVar()
        {
            var math = "plr.health";
            var result = math.ProcessAsPrefixMath(new Dictionary<string, long>
            {
                { "plr.health", 50 }
            });
            Assert.AreEqual(50, result);
        }

        [TestMethod]
        public void VarAddChain()
        {
            var math = "(+ plr.health (+ 3 medkit))";
            var result = math.ProcessAsPrefixMath(new Dictionary<string, long>
            {
                { "plr.health", 50 },
                { "medkit", 10 }
            });
            Assert.AreEqual(63, result);
        }

        [TestMethod]
        public void RocketDamage()
        {
            // (50 * damage) / 100
            var math = "(- plr.health (/ (* rocket.damage plr.shield) 100))";
            var result = math.ProcessAsPrefixMath(new Dictionary<string, long>
            {
                { "plr.health", 100 },
                { "plr.shield", 10 },
                { "rocket.damage" , 50 }
            });
            Assert.AreEqual(95, result);
        }

        [TestMethod]
        public void StringSubs()
        {
            var math = "'mr vol'";
            var result = math.ProcessAsPrefixMath();
            Assert.AreEqual("mr vol".ToLong(), result);
        }

        [TestMethod]
        public void TypedSimpleAdd()
        {
            var math = "(+ 12 5)";
            var result = math.ProcessAsPrefixMathTyped();
            Assert.AreEqual(17, result);
        }
        [TestMethod]
        public void TypedSimpleMultiply()
        {
            var math = "(* 5 5)";
            var result = math.ProcessAsPrefixMathTyped();
            Assert.AreEqual(25, result);
        }
        [TestMethod]
        public void TypedSimpleDivide()
        {
            var math = "(/ 5 5)";
            var result = math.ProcessAsPrefixMathTyped();
            Assert.AreEqual(1, result);
        }
        [TestMethod]
        public void TypedSimpleSubtract()
        {
            var math = "(- 5 5)";
            var result = math.ProcessAsPrefixMathTyped();
            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public void TypedStringAdd()
        {
            var math = "(+ 'eggnog' 'tuna')";
            var result = math.ProcessAsPrefixMathTyped();
            Assert.AreEqual("eggnogtuna", result);
        }

        [TestMethod]
        public void TypedStringAddNumber()
        {
            var math = "(+ 'eggnog' 6)";
            var result = math.ProcessAsPrefixMathTyped();
            Assert.AreEqual("eggnog6", result);
        }

        [TestMethod]
        public void TypedStringAddNumberReversed()
        {
            var math = "(+ 6 'a')";
            var result = math.ProcessAsPrefixMathTyped();
            Assert.AreEqual("6a", result);
        }

        [TestMethod]
        public void TypedStringAddWithVar()
        {
            var math = "(+ 'eggnog' x)";
            var result = math.ProcessAsPrefixMathTyped(new Dictionary<string, object>(){
                { "x", "tuna"}
            });
            Assert.AreEqual("eggnogtuna", result);
        }

        [TestMethod]
        public void TypedBoolAddIsOr()
        {
            var math = "(+ true true)";
            var result = math.ProcessAsPrefixMathTyped();
            Assert.AreEqual(true, result);
        }
        [TestMethod]
        public void TypedBoolAddIsOr2()
        {
            var math = "(+ false true)";
            var result = math.ProcessAsPrefixMathTyped();
            Assert.AreEqual(true, result);
        }

        [TestMethod]
        public void TypedBoolMultIsAnd()
        {
            var math = "(* true true)";
            var result = math.ProcessAsPrefixMathTyped();
            Assert.AreEqual(true, result);
        }
        [TestMethod]
        public void TypedBoolMultIsAnd2()
        {
            var math = "(* true false)";
            var result = math.ProcessAsPrefixMathTyped();
            Assert.AreEqual(false, result);
        }

    }
}
