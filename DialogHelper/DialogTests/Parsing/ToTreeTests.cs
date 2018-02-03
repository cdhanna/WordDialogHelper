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
    public class ConversionTests
    {
        [TestMethod]
        public void SimpleJSON()
        {
            var src = @"  a nifty rule 
     displayAs 
   tunafish rep 
conditions    
yaday first is not blahblah some more   
    another line of fun = twice the pain
dialogs
:player1        
speaking line text
multi line

:player2
something meaningful
 outcomes
    doit
doit again
";
            var expected = "[" +
                "{" +
                    "\"title\":\"a nifty rule\"," +
                    "\"displayAs\":\"tunafish rep\"," +
                    "\"conditions\":[" +
                        "{" +
                            "\"op\":\"=!\"," +
                            "\"left\":\"yaday first\"," +
                            "\"right\":\"blahblah some more\"" +
                        "}," +
                        "{" +
                            "\"op\":\"=\"," +
                            "\"left\":\"another line of fun\"," +
                            "\"right\":\"twice the pain\"" +
                        "}" +
                    "]," +
                    "\"dialogs\":[" +
                        "{" +
                            "\"speaker\":\"player1\"," +
                            "\"line\":\"speaking line text\r\nmulti line\"" +
                        "}," +
                        "{" +
                            "\"speaker\":\"player2\"," +
                            "\"line\":\"something meaningful\"" +
                        "}" +
                    "]," +
                    "\"outcomes\":[" +
                        "{" +
                            "\"action\":\"doit\"" +
                        "}," +
                        "{" +
                            "\"action\":\"doit again\"" +
                        "}" +
                    "]" +
                "}" +
                "" +
                "]";
            // if thething is theotherthing:
            // muffin is true

            var program = new WordLangResults(src);
            Assert.AreEqual(false, program.ParserErrors.AnyErrors);

            var v = new ProgramToJson();
            var output = v.Visit(program.ProgramContext);

            Assert.IsNotNull(output);


            Assert.AreEqual(expected, output);
        }

        [TestMethod]
        public void SimpleTree()
        {
            var src = @"  a nifty rule 
     displayAs 
   tunafish rep 
conditions    
yaday first is not blahblah some more   
    another line of fun = twice the pain
dialogs
:player1        
speaking line text
multi line

:player2
something meaningful
 outcomes
    doit
doit again
";
            var expected = "(program rules=[" +
                "(rule title=[a nifty rule]" +
                     " displayAs=[tunafish rep]" +
                     " conditions=[" +
                        "(cond op=[!=] left=[yaday first] right=[blahblah some more])" +
                        "(cond op=[=] left=[another line of fun] right=[twice the pain])" +
                       "]" +
                     " dialogs=[" +
                        "(dialog speaker=[player1] line=[speaking line text\r\nmulti line])" +
                        "(dialog speaker=[player2] line=[something meaningful])" +
                       "]" +
                     " outcomes=[" +
                        "(outcome action=[doit])" +
                        "(outcome action=[doit again])" +
                     "]" +
                     ")" +
                "])";
            // if thething is theotherthing:
            // muffin is true

            var program = new WordLangResults(src);
            Assert.AreEqual(false, program.ParserErrors.AnyErrors);

            var v = new ProgramToTree();
            var output = v.Visit(program.ProgramContext);

            Assert.IsNotNull(output);

            
            Assert.AreEqual(expected, output);
        }

    }
}
