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
    public class ConditionSets
    {

        [TestMethod]
        public void ConditoinalBlockBasic()
        {
            var src = @"
TedToMelvin
conditions
e speaker is 'ted'
e target is 'melvin'

A Nifty Rule
 dispLaYaS 
conditions
x is y
use tedToMelvin
dialogs
:plr
hello world
outcomes
set a to b
";
            var program = new WordLangResults(src);
            Assert.AreEqual(false, program.LexerErrors.AnyErrors);
            Assert.AreEqual(false, program.ParserErrors.AnyErrors);

            
        }

    }
}
