using System;
using System.Linq;
using DialogAddin.WordLang;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using AST = DialogAddin.WordLang.AST;

namespace DialogTests.Parsing
{
    [TestClass]
    public class ToASTTests
    {
        [TestMethod]
        public void SimpleToAST()
        {
            //            var src = @"  a nifty rule 
            //     displayAs 
            //   tunafish rep 
            //conditions    
            //yaday first is not blahblah some more   
            //    another line of fun = twice the pain
            //dialogs
            //:player1        
            //speaking line text
            //multi line

            //:player2
            //something meaningful
            // outcomes
            //    doit
            //doit again
            //";
            var src = "  a nifty rule  " + // ends on 15
                "\r\n  displayAs  " + // ends on 30
                "\r\n   tunafish rep  " + // ends on 49
                "\r\n conditions" + // ends on 62
                "\r\n a is not b   " + // ends on 78
                "\r\n dialogs" + // ends on 88
                "\r\n :a" + // ends on 93
                "\r\n hello  \r\n there" + // ends on 111
                "\r\n :b " + // ends on 117
                "\r\n goodbye " + // end son 128
                "\r\n outcomes " + // ends on 140
                "\r\n doit" +
                "\r\n";
            var expected = new AST.RuleSet()
            {
                Rules = new AST.Rule[]
                {
                    new AST.Rule()
                    {
                        Title = new AST.FieldString()
                        {
                            Value = "a nifty rule",
                            StartIndex = 2,
                            StopIndex = 13
                        },
                        DisplayAs = new AST.FieldString()
                        {
                            Value = "tunafish rep",
                            StartIndex = 36,
                            StopIndex = 47
                        },
                        Conditions = new AST.Condition[]
                        {
                            new AST.Condition()
                            {
                                LeftKey = new AST.FieldString()
                                {
                                    StartIndex = 66,
                                    StopIndex = 66,
                                    Value = "a"
                                },
                                RightKey = new AST.FieldString()
                                {
                                    Value = "b",
                                    StartIndex = 75,
                                    StopIndex = 75
                                },
                                Operator = new AST.FieldComparisonOperator()
                                {
                                    Value = AST.ComparisonOperator.EQUALS,
                                    StartIndex = 67,
                                    StopIndex = 74,
                                },
                                Negated = true
                            }
                        }.ToList(),
                        Dialogs = new AST.Dialog[]
                        {
                            new AST.Dialog()
                            {
                                Speaker = new AST.FieldString()
                                {
                                    Value = "a",
                                    StartIndex = 93,
                                    StopIndex = 93
                                },
                                Line = new AST.FieldString()
                                {
                                    Value = "hello  \r\n there",
                                    StartIndex = 97,
                                    StopIndex = 111
                                }
                            },
                            new AST.Dialog()
                            {
                                Speaker = new AST.FieldString()
                                {
                                    Value = "b",
                                    StartIndex = 116,
                                    StopIndex = 116
                                },
                                Line = new AST.FieldString()
                                {
                                    Value = "goodbye",
                                    StartIndex = 121,
                                    StopIndex = 127
                                }
                            }
                        }.ToList(),
                        Outcomes = new AST.Outcome[]
                        {
                            new AST.Outcome()
                            {
                                Action = new AST.FieldString()
                                {
                                    Value = "doit",
                                    StartIndex = 144,
                                    StopIndex = 147
                                }
                            }
                        }.ToList()
                        //DisplayAs = "tunafish rep",
                        //Line = 1,
                        //Char = 0,
                    }
                }.ToList()
            };
            // if thething is theotherthing:
            // muffin is true

            var program = new WordLangResults(src);
            Assert.AreEqual(false, program.ParserErrors.AnyErrors);

            var v = new ProgramToAST();
            var output = v.Visit(program.ProgramContext);

            Assert.IsNotNull(output);


            Assert.AreEqual(expected, output);

        }
    }
}
