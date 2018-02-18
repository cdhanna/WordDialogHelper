using DialogAddin.WordLang;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DialogTests.Parsing
{
    using L = WordLangLexer;

    [TestClass]
    public class LexingMath
    {
        [TestMethod]
        public void ConditionalMath_AddRefRight()
        {
            var src = @"A Nifty Rule
 dispLaYaS 
conditions
x is y + z
dialogs
:plr
hello world
outcomes
set a to b
";
            var program = new WordLangResults(src);
            Assert.AreEqual(false, program.ParserErrors.AnyErrors);

        }

        [TestMethod]
        public void ConditionalMath_AddRefLeft()
        {
            var src = @"A Nifty Rule
 dispLaYaS 
conditions
x + z is y
dialogs
:plr
hello world
outcomes
set a to b
";
            var program = new WordLangResults(src);
            Assert.AreEqual(false, program.ParserErrors.AnyErrors);

        }

        [TestMethod]
        public void ConditionalMath_AddLiteralRight()
        {
            var src = @"A Nifty Rule
 dispLaYaS 
conditions
x is y + 2
dialogs
:plr
hello world
outcomes
set a to b
";
            var program = new WordLangResults(src);
            Assert.AreEqual(false, program.ParserErrors.AnyErrors);

        }

        [TestMethod]
        public void ConditionalMath_AddLiteralLeft()
        {
            var src = @"A Nifty Rule
 dispLaYaS 
conditions
x + 25 is y
dialogs
:plr
hello world
outcomes
set a to b
";
            var program = new WordLangResults(src);
            Assert.AreEqual(false, program.ParserErrors.AnyErrors);

        }

        [TestMethod]
        public void ConditionalMath_AddLiteralLeftSeveral()
        {
            var src = @"A Nifty Rule
 dispLaYaS 
conditions
x + 25 + z + 1205 is y
dialogs
:plr
hello world
outcomes
set a to b
";
            var program = new WordLangResults(src);
            Assert.AreEqual(false, program.ParserErrors.AnyErrors);

        }

        [TestMethod]
        public void ConditionalMath_AddLotsOfSpaces()
        {
            var src = @"A Nifty Rule
 dispLaYaS 
conditions
x  + 25  + z +  1205 is y   +  12
dialogs
:plr
hello world
outcomes
set a to b
";
            var program = new WordLangResults(src);
            Assert.AreEqual(false, program.ParserErrors.AnyErrors);
            var v = new ProgramToTree();
            var firstExpr = program.ProgramContext.rule(0).conditions().booleanExpr(0).expression(0);
            var output = v.VisitExpression(firstExpr);
            Assert.AreEqual("(+ x (+ 25 (+ z 1205)))", output);
        }

        [TestMethod]
        public void ConditionalMath_SubtractRight()
        {
            var src = @"A Nifty Rule
 dispLaYaS 
conditions
x - 5 is y
dialogs
:plr
hello world
outcomes
set a to b
";
            var program = new WordLangResults(src);
            Assert.AreEqual(false, program.ParserErrors.AnyErrors);
            var v = new ProgramToTree();
            var firstExpr = program.ProgramContext.rule(0).conditions().booleanExpr(0).expression(0);
            var output = v.VisitExpression(firstExpr);
            Assert.AreEqual("(- x 5)", output);
        }


      

        [TestMethod]
        public void ConditionalMath_PEMDAS_AddThenMult()
        {
            var srcA = @"A Nifty Rule
 dispLaYaS 
conditions
x + 5 * a is y 
dialogs
:plr
hello world
outcomes
set a to b
"; // (+ x (* 5 a))
            var progA = new WordLangResults(srcA);
            Assert.AreEqual(false, progA.ParserErrors.AnyErrors);

           
            var v = new ProgramToTree();
            var firstExpr = progA.ProgramContext.rule(0).conditions().booleanExpr(0).expression(0);
            var output = v.VisitExpression(firstExpr);
            Assert.AreEqual("(+ x (* 5 a))", output);

        }

        [TestMethod]
        public void ConditionalMath_PEMDAS_MultThenAdd()
        {
            
            var srcB = @"A Nifty Rule
 dispLaYaS 
conditions
x * a + 5 is y
dialogs
:plr
hello world
outcomes
set a to b
";
            var progB = new WordLangResults(srcB);
            Assert.AreEqual(false, progB.ParserErrors.AnyErrors);

            var v = new ProgramToTree();

            var firstExpr = progB.ProgramContext.rule(0).conditions().booleanExpr(0).expression(0);
            var output = v.VisitExpression(firstExpr);
            Assert.AreEqual("(+ (* x a) 5)", output);

        }

        [TestMethod]
        public void ConditionalMath_Complicated()
        {
            var srcA = @"A Nifty Rule
 dispLaYaS 
conditions
x * y + z * w * a + 1 is y 
dialogs
:plr
hello world
outcomes
set a to b
";
            
            var progA = new WordLangResults(srcA);
            Assert.AreEqual(false, progA.ParserErrors.AnyErrors);
            var firstExpr = progA.ProgramContext.rule(0).conditions().booleanExpr(0).expression(0);
            var v = new ProgramToTree();
            var output = v.VisitExpression(firstExpr);
            Assert.AreEqual("(+ (* x y) (+ (* z (* w a)) 1))", output);

        }

        [TestMethod]
        public void ConditionalMath_PEMDAS_MultDivide()
        {
            var srcA = @"A Nifty Rule
 dispLaYaS 
conditions
a / b * c is y 
dialogs
:plr
hello world
outcomes
set a to b
";

            var progA = new WordLangResults(srcA);
            Assert.AreEqual(false, progA.ParserErrors.AnyErrors);
            var firstExpr = progA.ProgramContext.rule(0).conditions().booleanExpr(0).expression(0);
            var v = new ProgramToTree();
            var output = v.VisitExpression(firstExpr);
            Assert.AreEqual("(/ a (* b c))", output);

        }

        [TestMethod]
        public void ConditionalMath_PEMDAS_Parens()
        {
            var srcA = @"A Nifty Rule
 dispLaYaS 
conditions
2 * (3 + 4) is y 
dialogs
:plr
hello world
outcomes
set a to b
";

            var progA = new WordLangResults(srcA);
            Assert.AreEqual(false, progA.ParserErrors.AnyErrors);
            var firstExpr = progA.ProgramContext.rule(0).conditions().booleanExpr(0).expression(0);
            var v = new ProgramToTree();
            var output = v.VisitExpression(firstExpr);
            Assert.AreEqual("(* 2 (+ 3 4))", output);

        }

        [TestMethod]
        public void ConditionalMath_PEMDAS_ParensHarder()
        {
            var srcA = @"A Nifty Rule
 dispLaYaS 
conditions
2 * (3 + 4) - (5 - 2) * 4 is y 
dialogs
:plr
hello world
outcomes
set a to b
";

            var progA = new WordLangResults(srcA);
            Assert.AreEqual(false, progA.ParserErrors.AnyErrors);
            var firstExpr = progA.ProgramContext.rule(0).conditions().booleanExpr(0).expression(0);
            var v = new ProgramToTree();
            var output = v.VisitExpression(firstExpr);
            Assert.AreEqual("(- (* 2 (+ 3 4)) (* (- 5 2) 4))", output);

        }

        [TestMethod]
        public void ConditionalMath_PEMDAS_ParensNested()
        {
            var srcA = @"A Nifty Rule
 dispLaYaS 
conditions
2 * (  ((3 + 4) - (5 - 2)) + 4) is y 
dialogs
:plr
hello world
outcomes
set a to b
";

            var progA = new WordLangResults(srcA);
            Assert.AreEqual(false, progA.ParserErrors.AnyErrors);
            var firstExpr = progA.ProgramContext.rule(0).conditions().booleanExpr(0).expression(0);
            var v = new ProgramToTree();
            var output = v.VisitExpression(firstExpr);
            Assert.AreEqual("(* 2 (+ (- (+ 3 4) (- 5 2)) 4))", output);

        }
    }
}
