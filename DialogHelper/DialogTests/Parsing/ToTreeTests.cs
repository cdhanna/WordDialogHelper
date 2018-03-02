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
            var src = @"
tunafish 
conditions
a is b

a nifty rule 
     displayAs 
   tunafish rep 
conditions    
player health + 50 > 100
dialogs
:player1        
speaking {player health + 12} line text
:player2
something <b>meaningful</b>
:player3
this is <color='red'> red text </color>
 outcomes
    set x to y
run actions.fart with power as 100 * player.fart
with target as enemy";

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
                            Content = "speaking {player health + 12} line text",
                            ContentParts = new string[]
                            {
                                "'speaking '",
                                "(+ player.health 12)",
                                "' line text'"
                            }
                        },
                        new DialogRule.DialogPart()
                        {
                            Speaker = "player2",
                            Content = "something <b>meaningful</b>",
                            ContentParts = new string[]
                            {
                                "'something <b>meaningful</b>'"
                            }
                        },
                        new DialogRule.DialogPart()
                        {
                            Speaker = "player3",
                            Content = "this is <color='red'> red text </color>",
                            ContentParts = new string[]
                            {
                                "'this is <color='red'> red text </color>'"
                            }
                        }
                    },
                    Outcomes = new DialogRule.DialogOutcome[]
                    {
                        new DialogRule.DialogOutcome()
                        {
                            Command = "set",
                            Target = "x",
                            Arguments = new Dictionary<string, string>()
                            {
                                { "", "y" }
                            }
                        },
                        new DialogRule.DialogOutcome()
                        {
                            Command = "run",
                            Target = "actions.fart",
                            Arguments = new Dictionary<string, string>()
                            {
                                { "power", "(* 100 player.fart)" },
                                { "target", "enemy" }
                            }
                        }
                    }
                    
                }
            };

            var bundle = new DialogBundle()
            {
                Name = "test",
                Rules = rules,
                ConditionSets = new DialogConditionSet[]
                {
                    new DialogConditionSet()
                    {
                        Name = "tunafish",
                        Conditions = new DialogConditionSet.DialogCondition[]
                        {
                            new DialogConditionSet.DialogCondition()
                            {
                                Left = "a",
                                Op = "=",
                                Right = "b"
                            }
                        }
                    }
                }
            };
            
            var expected = JsonConvert.SerializeObject(bundle, Formatting.None, new JsonSerializerSettings()
            {

                StringEscapeHandling = StringEscapeHandling.Default
            });
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

            var backwards = JsonConvert.DeserializeObject<DialogBundle>(output);
            Assert.AreEqual(backwards, bundle);
            Assert.AreEqual(expected, output);

            
        }

       

    }
}
