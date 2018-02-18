using Dialog;
using DialogAddin.WordLang;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
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
player.health + 50 > 100
dialogs
:player1        
speaking line text
:player2
something meaningful
 outcomes
    set x to y";

            var rules = new DialogRule[]
            {
                new DialogRule()
                {
                    
                    Name = "a nifty rule",
                    DisplayAs = "tunafish rep",
                    Conditions = new DialogRule.DialogCondition[]
                    {
                        new DialogRule.DialogCondition()
                        {
                            Op = ">",
                            Left = "(+ player.health 50)",
                            Right = "100"
                        }
                    },
                    Dialog = new DialogRule.DialogPart[]
                    {
                        new DialogRule.DialogPart()
                        {
                            Speaker = "player1",
                            Content = $"speaking line text"
                        },
                        new DialogRule.DialogPart()
                        {
                            Speaker = "player2",
                            Content = "something meaningful"
                        }
                    },
                    Outcomes = new DialogRule.DialogOutcome[]
                    {
                        //new DialogRule.DialogOutcome()
                        //{
                        //    Command = "set",
                        //    Target = "x",
                        //    Argument = "y"
                        //}
                    }
                    
                }
            };
            var expected = JsonConvert.SerializeObject(rules);
            //var expected = "[" +
            //    "{" +
            //        "\"title\":\"a nifty rule\"," +
            //        "\"displayAs\":\"tunafish rep\"," +
            //        "\"conditions\":[" +
            //            "{" +
            //                "\"op\":\"=!\"," +
            //                "\"left\":\"yaday first\"," +
            //                "\"right\":\"blahblah some more\"" +
            //            "}," +
            //            "{" +
            //                "\"op\":\"=\"," +
            //                "\"left\":\"another line of fun\"," +
            //                "\"right\":\"twice the pain\"" +
            //            "}" +
            //        "]," +
            //        "\"dialogs\":[" +
            //            "{" +
            //                "\"speaker\":\"player1\"," +
            //                "\"line\":\"speaking line text\r\nmulti line\"" +
            //            "}," +
            //            "{" +
            //                "\"speaker\":\"player2\"," +
            //                "\"line\":\"something meaningful\"" +
            //            "}" +
            //        "]," +
            //        "\"outcomes\":[" +
            //            "{" +
            //                "\"action\":\"doit\"" +
            //            "}," +
            //            "{" +
            //                "\"action\":\"doit again\"" +
            //            "}" +
            //        "]" +
            //    "}" +
            //    "" +
            //    "]";
            // if thething is theotherthing:
            // muffin is true

            var program = new WordLangResults(src);
            Assert.AreEqual(false, program.ParserErrors.AnyErrors);

            var v = new ProgramToJson();
            var output = v.Visit(program.ProgramContext);

            Assert.IsNotNull(output);


            Assert.AreEqual(expected, output);


            var parsed = JsonConvert.DeserializeObject<DialogRule[]>(output);

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
