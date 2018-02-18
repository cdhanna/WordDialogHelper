using System;
using Dialog;
using Dialog.Engine;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace DialogTests.AttributeFetch
{
    [TestClass]
    public class BasicEngine
    {
        class Actor
        {
            public int Health = 50;
            public int MaxHealth = 50;
            public int Ammo = 15;
        }

        [TestMethod]
        public void SimpleEngine()
        {
            var player = new Actor();

            var rules = new DialogRule[]
            {
                new DialogRule()
                {
                    Name = "I have full health!",
                    Conditions = new DialogRule.DialogCondition[]
                    {
                        new DialogRule.DialogCondition()
                        {
                            Left = "player.health",
                            Op = "=",
                            Right = "player.maxHealth"
                        }
                    }
                },
                new DialogRule()
                {
                   
                    Name = "I am the king of health and ammo",
                    Conditions = new DialogRule.DialogCondition[]
                    {
                        new DialogRule.DialogCondition()
                        {
                            Left = "player.health",
                            Op = "=",
                            Right = "player.maxHealth"
                        },
                        new DialogRule.DialogCondition()
                        {
                            Left = "player.ammo",
                            Op = ">",
                            Right = "25"
                        }
                    }
                }
            };

            var attributes = new ObjectDialogAttribute[]
            {
                new ObjectDialogAttribute(player, "player", "health"),
                new ObjectDialogAttribute(player, "player", "maxHealth"),
                new ObjectDialogAttribute(player, "player", "ammo"),
            };

            var engine = new DialogEngine();
            rules.ToList().ForEach(r => engine.AddRule(r));
            attributes.ToList().ForEach(a => engine.AddAttribute(a));

            var best = engine.GetBestValidDialog();
            Assert.IsNotNull(best);
            Assert.AreEqual("I have full health!", best.Name);
        }

        [TestMethod]
        public void SimpleEngine2()
        {
            var player = new Actor();
            player.Ammo = 26;

            var rules = new DialogRule[]
            {
                new DialogRule()
                {
                    Name = "I have full health!",
                    Conditions = new DialogRule.DialogCondition[]
                    {
                        new DialogRule.DialogCondition()
                        {
                            Left = "player.health",
                            Op = "=",
                            Right = "player.maxHealth"
                        }
                    }
                },
                new DialogRule()
                {

                    Name = "I am the king of health and ammo",
                    Conditions = new DialogRule.DialogCondition[]
                    {
                        new DialogRule.DialogCondition()
                        {
                            Left = "player.health",
                            Op = "=",
                            Right = "player.maxHealth"
                        },
                        new DialogRule.DialogCondition()
                        {
                            Left = "player.ammo",
                            Op = ">",
                            Right = "25"
                        }
                    }
                }
            };

            var attributes = new ObjectDialogAttribute[]
            {
                new ObjectDialogAttribute(player, "player", "health"),
                new ObjectDialogAttribute(player, "player", "maxHealth"),
                new ObjectDialogAttribute(player, "player", "ammo"),
            };

            var engine = new DialogEngine();
            rules.ToList().ForEach(r => engine.AddRule(r));
            attributes.ToList().ForEach(a => engine.AddAttribute(a));

            var best = engine.GetBestValidDialog();
            Assert.IsNotNull(best);
            Assert.AreEqual("I am the king of health and ammo", best.Name);
        }

        [TestMethod]
        public void SimpleEngineConMath()
        {
            var player = new Actor();
            player.Ammo = 3;

            var rules = new DialogRule[]
            {
                new DialogRule()
                {
                    Name = "I have more than half health",
                    Conditions = new DialogRule.DialogCondition[]
                    {
                        new DialogRule.DialogCondition()
                        {
                            Left = "player.health",
                            Op = ">",
                            Right = "(/ player.maxHealth 2)"
                        }
                    }
                },
                new DialogRule()
                {

                    Name = "I am the king of health and ammo",
                    Conditions = new DialogRule.DialogCondition[]
                    {
                        new DialogRule.DialogCondition()
                        {
                            Left = "player.health",
                            Op = "=",
                            Right = "player.maxHealth"
                        },
                        new DialogRule.DialogCondition()
                        {
                            Left = "player.ammo",
                            Op = ">",
                            Right = "25"
                        }
                    }
                }
            };

            var attributes = new ObjectDialogAttribute[]
            {
                new ObjectDialogAttribute(player, "player", "health"),
                new ObjectDialogAttribute(player, "player", "maxHealth"),
                new ObjectDialogAttribute(player, "player", "ammo"),
            };

            var engine = new DialogEngine();
            rules.ToList().ForEach(r => engine.AddRule(r));
            attributes.ToList().ForEach(a => engine.AddAttribute(a));

            var best = engine.GetBestValidDialog();
            Assert.IsNotNull(best);
            Assert.AreEqual("I have more than half health", best.Name);
        }
    }
}
