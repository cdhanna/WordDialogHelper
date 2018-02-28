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
    public class ComplicatedParse
    {

        [TestMethod]
        public void PunctuationInDialog()
        {
            var src = @"A Nifty Rule
 dispLaYaS 
conditions
x is y
dialogs
:plr
hello world! I am here, to give you a lesson on rythm anatomy.
outcomes
set a to b
";
            var program = new WordLangResults(src);
            Assert.AreEqual(false, program.LexerErrors.AnyErrors);
            Assert.AreEqual(false, program.ParserErrors.AnyErrors);

        }

        [TestMethod]
        public void AsInDialog()
        {
            var src = @"A Nifty Rule
 dispLaYaS 
conditions
x is y
dialogs
:plr
I am as cool as sliced break
outcomes
set a to b
";
            var program = new WordLangResults(src);
            Assert.AreEqual(false, program.LexerErrors.AnyErrors);
            Assert.AreEqual(false, program.ParserErrors.AnyErrors);

        }

        [TestMethod]
        public void WithInDialog()
        {
            var src = @"A Nifty Rule
 dispLaYaS 
conditions
x is y
dialogs
:plr
come with me to the market
outcomes
set a to b
";
            var program = new WordLangResults(src);
            Assert.AreEqual(false, program.LexerErrors.AnyErrors);
            Assert.AreEqual(false, program.ParserErrors.AnyErrors);

        }


        [TestMethod]
        public void ApostrophieInDialog()
        {
            var src = @"A Nifty Rule
 dispLaYaS 
conditions
x is y
dialogs
:plr
This isn't yours
outcomes
set a to b
";
            var program = new WordLangResults(src);
            Assert.AreEqual(false, program.LexerErrors.AnyErrors);
            Assert.AreEqual(false, program.ParserErrors.AnyErrors);

        }

        [TestMethod]
        public void ToInVariable()
        {
            var src = @"A Nifty Rule
 dispLaYaS 
conditions
plr flags talkedTo is true
dialogs
:plr
This isn't yours
outcomes
set a to b
";
            var program = new WordLangResults(src);
            Assert.AreEqual(false, program.LexerErrors.AnyErrors);
            Assert.AreEqual(false, program.ParserErrors.AnyErrors);

        }
    }
}
