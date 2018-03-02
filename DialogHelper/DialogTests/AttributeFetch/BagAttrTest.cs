using System;
using System.Collections.Generic;
using System.Linq;
using Dialog;
using Dialog.Engine;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DialogTests.AttributeFetch
{
    [TestClass]
    public class BagAttrTest
    {
      

         
        [TestMethod]
        public void BoolBag()
        {
            var player = new
            {
                health = 10,
                flags = new BagBoolElement[]
                {
                    new BagBoolElement()
                    {
                        name = "a",
                        value = true
                    }
                }.ToList()
            };

            var rules = new DialogRule[]
            {
                new DialogRule()
                {
                    Name = "I have full health!",
                    Conditions = new DialogRule.DialogCondition[]
                    {
                        new DialogRule.DialogCondition()
                        {
                            Left = "player.flags.a",
                            Op = "=",
                            Right = "true"
                        },
                        new DialogRule.DialogCondition()
                        {
                            Left = "player.flags.b",
                            Op = "=",
                            Right = "false"
                        }
                    }
                },
                
            };

            var engine = new DialogEngine()
                .AddHandler(new BagAttributesRuleAddedHandler<bool, BagBoolElement>());
            var attributes = new DialogAttribute[]
            {
                new ObjectDialogAttribute(player, "player", "health"),
                DialogAttribute.New("player.flags", false, player.flags ).UpdateElements(engine)
                //new BagDialogAttribute<bool>("player.flags", player.flags).UpdateElements(engine)
            };

            attributes.ToList().ForEach(a => engine.AddAttribute(a));



            rules.ToList().ForEach(r => engine.AddRule(r));

            var best = engine.GetBestValidDialog();
            Assert.IsNotNull(best);
            Assert.AreEqual("I have full health!", best.Name);
        }

        [TestMethod]
        public void BoolBag2()
        {
            var player = new
            {
                health = 10,
                flags = new BagBoolElement[]
                {
                    new BagBoolElement()
                    {
                        name = "a.part",
                        value = true
                    }
                }.ToList()
            };

            var rules = new DialogRule[]
            {
                new DialogRule()
                {
                    Name = "I have full health!",
                    Conditions = new DialogRule.DialogCondition[]
                    {
                        new DialogRule.DialogCondition()
                        {
                            Left = "player.flags.a.part",
                            Op = "=",
                            Right = "true"
                        },
                        new DialogRule.DialogCondition()
                        {
                            Left = "player.flags.b",
                            Op = "=",
                            Right = "false"
                        }
                    }
                },

            };

            var engine = new DialogEngine()
                .AddHandler(new BagBoolHandler());
            var attributes = new DialogAttribute[]
            {
                new ObjectDialogAttribute(player, "player", "health"),
                DialogAttribute.New("player.flags", false, player.flags ).UpdateElements(engine)
                //new BagDialogAttribute<bool>("player.flags", player.flags).UpdateElements(engine)
            };

            attributes.ToList().ForEach(a => engine.AddAttribute(a));



            rules.ToList().ForEach(r => engine.AddRule(r));

            var best = engine.GetBestValidDialog();
            Assert.IsNotNull(best);
            Assert.AreEqual("I have full health!", best.Name);
        }


        [TestMethod]
        public void IntBag()
        {
            var player = new
            {
                health = 10,
                flags = new BagIntElement[]
                {
                    new BagIntElement()
                    {
                        name = "a",
                        value = 5
                    }
                }.ToList()
            };

            var rules = new DialogRule[]
            {
                new DialogRule()
                {
                    Name = "I have full health!",
                    Conditions = new DialogRule.DialogCondition[]
                    {
                        new DialogRule.DialogCondition()
                        {
                            Left = "player.flags.a",
                            Op = "=",
                            Right = "5"
                        },
                        new DialogRule.DialogCondition()
                        {
                            Left = "player.flags.b",
                            Op = ">",
                            Right = "6"
                        }
                    }
                },

            };

            var engine = new DialogEngine()
                .AddHandler(new BagIntHandler());
            var attributes = new DialogAttribute[]
            {
                new ObjectDialogAttribute(player, "player", "health"),
                DialogAttribute.New("player.flags", 10, player.flags ).UpdateElements(engine)
                //new BagDialogAttribute<bool>("player.flags", player.flags).UpdateElements(engine)
            };

            attributes.ToList().ForEach(a => engine.AddAttribute(a));



            rules.ToList().ForEach(r => engine.AddRule(r));

            var best = engine.GetBestValidDialog();
            Assert.IsNotNull(best);
            Assert.AreEqual("I have full health!", best.Name);
        }

        [TestMethod]
        public void StringAndIntBags()
        {
            var player = new
            {
                health = 10,
                nums = new BagIntElement[]
                {
                    new BagIntElement()
                    {
                        name = "a",
                        value = 5
                    }
                }.ToList(),
                strs = new BagStringElement[]
                {
                    new BagStringElement()
                    {
                        name = "a",
                        value = "tuna"
                    }
                }.ToList()
            };

            var rules = new DialogRule[]
            {
                new DialogRule()
                {
                    Name = "I have full health!",
                    Conditions = new DialogRule.DialogCondition[]
                    {
                        new DialogRule.DialogCondition()
                        {
                            Left = "player.nums.a",
                            Op = "=",
                            Right = "5"
                        },
                        new DialogRule.DialogCondition()
                        {
                            Left = "player.nums.b",
                            Op = ">",
                            Right = "6"
                        },
                        new DialogRule.DialogCondition()
                        {
                            Left = "player.strs.a",
                            Op = "=!",
                            Right = "player.strs.b"
                        }
                    }
                },

            };

            var engine = new DialogEngine()
                .AddHandler(new BagIntHandler())
                .AddHandler(new BagStringHandler()) ;
            var attributes = new DialogAttribute[]
            {
                new ObjectDialogAttribute(player, "player", "health"),
                DialogAttribute.New("player.nums", 10, player.nums ).UpdateElements(engine),
                DialogAttribute.New("player.strs", "eggs", player.strs ).UpdateElements(engine)
                //new BagDialogAttribute<bool>("player.flags", player.flags).UpdateElements(engine)
            };

            attributes.ToList().ForEach(a => engine.AddAttribute(a));



            rules.ToList().ForEach(r => engine.AddRule(r));

            var best = engine.GetBestValidDialog();
            Assert.IsNotNull(best);
            Assert.AreEqual("I have full health!", best.Name);
        }
    }
    
}
