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
        public void WordApostrophie()
        {
            var src = @"A Nifty Rule
 dispLaYaS 
conditions
x is y
dialogs
:plr
hello world! he’s I am here, to give you a lesson on rythm anatomy.
outcomes
set a to b
";
            var program = new WordLangResults(src);
            Assert.AreEqual(false, program.LexerErrors.AnyErrors);
            Assert.AreEqual(false, program.ParserErrors.AnyErrors);

        }


        [TestMethod]
        public void SpeakerAsEpxression()
        {
            var src = @"A Nifty Rule
 dispLaYaS 
conditions
x is y
dialogs
:{player.name}
My name is a myster
outcomes
set a to b
";
            var program = new WordLangResults(src);
            Assert.AreEqual(false, program.LexerErrors.AnyErrors);
            Assert.AreEqual(false, program.ParserErrors.AnyErrors);

        }

        [TestMethod]
        public void DotDotDot()
        {
            var src = @"A Nifty Rule
 dispLaYaS 
conditions
x is y
dialogs
:plr
hello world… 
outcomes
set a to b
";
            var program = new WordLangResults(src);
            Assert.AreEqual(false, program.LexerErrors.AnyErrors);
            Assert.AreEqual(false, program.ParserErrors.AnyErrors);

        }

        [TestMethod]
        public void AsInDisplayAs()
        {
            var src = @"A Nifty Rule
 dispLaYaS 
Im as cool as you
conditions
x is y
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

        [TestMethod]
        public void CommaInName()
        {
            var src = @"A Nifty Rule
 dispLaYaS 
conditions
x is y
dialogs
:plr, the great
hello world! I am here, to give you a lesson on rythm anatomy.
outcomes
set a to b
";
            var program = new WordLangResults(src);
            Assert.AreEqual(false, program.LexerErrors.AnyErrors);
            Assert.AreEqual(false, program.ParserErrors.AnyErrors);

        }


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
        public void ConditionsInName()
        {
            var src = @"A Nifty Rule
 dispLaYaS 
conditions
conditions x is y
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
        public void ConditionsInName2()
        {
            var src = @"A Nifty Rule
 dispLaYaS 
conditions
conditions x is y
b is y
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
        public void ConditionsInName3()
        {
            var src = @"A Nifty Rule
 dispLaYaS 
conditions
x is y
conditions x is y
b is y
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
        public void AsInName()
        {
            var src = @"A Nifty Rule
 dispLaYaS 
conditions
xas is y
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
        public void IsInname()
        {
            var src = @"A Nifty Rule
 dispLaYaS 
conditions
isto is x
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
        public void StupidQuotes()
        {
            var src = @"A Nifty Rule
 dispLaYaS 
conditions
x is ‘okay’
dialogs
:plr
This is okay
outcomes
set a to b
";
            var program = new WordLangResults(src);
            Assert.AreEqual(false, program.LexerErrors.AnyErrors);
            Assert.AreEqual(false, program.ParserErrors.AnyErrors);

        }

        [TestMethod]
        public void NumbersInConditions()
        {
            var src = @"A Nifty Rule
 dispLaYaS 
conditions
player item 5 is 'cool'
dialogs
:plr
This is okay
outcomes
set a to b
";
            var program = new WordLangResults(src);
            Assert.AreEqual(false, program.LexerErrors.AnyErrors);
            Assert.AreEqual(false, program.ParserErrors.AnyErrors);

        }

        [TestMethod]
        public void AsInTheRuleName()
        {
            var src = @"Astronomy Assist
 dispLaYaS 
conditions
player item 5 is 'cool'
dialogs
:plr
This is okay
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

        [TestMethod]
        public void IsInVariable()
        {
            var src = @"A Nifty Rule
 dispLaYaS 
conditions
plr flags isCool is true
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
