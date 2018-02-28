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
      

            /*
             * var b = new BagAttr('player.flags', typeof(bool))
             * b.Match('player.health') --> false
             * b.Match('player.flags') --> false
             * b.Match('player.flags.x') --> true
             * 
             * 
             * create a dictionary
             * when an attribute has the same prefix as a bag, it will add it to the bag's dictionary
             * but how does the system know about the prefix match ?
             * 
             * set
             * 
             */

        [TestMethod]
        public void SimpleEngine()
        {
            var player = new
            {
                health = 10,
                flags = new BagElement[]
                {
                    new BagElement()
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

            var engine = new DialogEngine();
            var attributes = new DialogAttribute[]
            {
                new ObjectDialogAttribute(player, "player", "health"),
                new BagDialogAttribute("player.flags", player.flags).UpdateElements(engine)
            };

            attributes.ToList().ForEach(a => engine.AddAttribute(a));



            rules.ToList().ForEach(r => engine.AddRule(r));

            var best = engine.GetBestValidDialog();
            Assert.IsNotNull(best);
            Assert.AreEqual("I have full health!", best.Name);
        }

    }
    
}
