using DialogAddin.WordLang;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DialogTests.Parsing
{
    [TestClass]
    public class OutcomeTests
    {
        [TestMethod]
        public void SimpleOutcome_RunWithWords()
        {
            var src = @"a nifty rule
displayAs 
tunafish rep 
conditions    
a is b
dialogs
:player 1        
speaking line text
multi line
outcomes
run a 
    with b as 5 
    with c as 2
";
            var program = new WordLangResults(src);
            Assert.AreEqual(false, program.ParserErrors.AnyErrors);


            var v = new ProgramToTree();
            var result = v.Visit(program.ProgramContext.rule(0).outcomes().singleOutcome(0));

            Assert.AreEqual("(run target=[a] args=[(arg name=[b] expr=[5]),(arg name=[c] expr=[2])])", result);

        }

        [TestMethod]
        public void SimpleOutcome_RunWithWordsNoArgs()
        {
            var src = @"a nifty rule
displayAs 
tunafish rep 
conditions    
a is b
dialogs
:player 1        
speaking line text
multi line
outcomes
run a 
";
            var program = new WordLangResults(src);
            Assert.AreEqual(false, program.ParserErrors.AnyErrors);


            var v = new ProgramToTree();
            var result = v.Visit(program.ProgramContext.rule(0).outcomes().singleOutcome(0));

            Assert.AreEqual("(run target=[a] args=[])", result);

        }

       

        [TestMethod]
        public void SimpleOutcome_SetWithWords()
        {
            var src = @"a nifty rule
displayAs 
tunafish rep 
conditions    
a is b
dialogs
:player 1        
speaking line text
multi line
outcomes
set a to b
";
            var program = new WordLangResults(src);
            Assert.AreEqual(false, program.ParserErrors.AnyErrors);


            var v = new ProgramToTree();
            var result = v.Visit(program.ProgramContext.rule(0).outcomes().singleOutcome(0));

            Assert.AreEqual("(set target=[a] expr=[b])", result);

        }

        [TestMethod]
        public void SimpleOutcome_SetWithExpr()
        {
            var src = @"a nifty rule
displayAs 
tunafish rep 
conditions    
a is b
dialogs
:player 1        
speaking line text
multi line
outcomes
a = b
";
            var program = new WordLangResults(src);
            Assert.AreEqual(false, program.ParserErrors.AnyErrors);
            
            var v = new ProgramToTree();
            var result = v.Visit(program.ProgramContext.rule(0).outcomes().singleOutcome(0));

            Assert.AreEqual("(set target=[a] expr=[b])", result);

        }

        [TestMethod]
        public void SimpleOutcome_SetWithExprMath()
        {
            var src = @"a nifty rule
displayAs 
tunafish rep 
conditions    
a is b
dialogs
:player 1        
speaking line text
multi line
outcomes
a = (b + c) * 2
";
            var program = new WordLangResults(src);
            Assert.AreEqual(false, program.ParserErrors.AnyErrors);

            var v = new ProgramToTree();
            var result = v.Visit(program.ProgramContext.rule(0).outcomes().singleOutcome(0));

            Assert.AreEqual("(set target=[a] expr=[(* (+ b c) 2)])", result);

        }

    }
}
