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
    public class ToDialog
    {
        [TestMethod]
        public void One()
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
    }
}
