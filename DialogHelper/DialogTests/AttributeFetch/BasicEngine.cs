using System;
using Dialog;
using Dialog.Engine;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Collections.Generic;

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

            public void SetAll(int health, int max, int ammo)
            {
                Health = health;
                MaxHealth = max;
                Ammo = ammo;
            }
        }

        [TestMethod]
        public void SimpleTemplateInterp()
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
                    },
                    Dialog = new DialogRule.DialogPart[]
                    {
                        new DialogRule.DialogPart()
                        {
                            Speaker ="player",
                            Content = "I have health of {player health} which and {player maxHealth + player health}",
                            ContentParts = new string[]
                            {
                                "'I have health of '",
                                "player.health",
                                "' which and '",
                                "(+ player.maxHealth player.health)"
                            }
                        }
                    }
                },
                
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

            var dialogs = engine.ExecuteRuleDialogs(best);
            Assert.AreEqual(1, dialogs.Length);
            Assert.AreEqual("I have health of 50 which and 100", dialogs[0]);

        }

        [TestMethod]
        public void TemplateInterpStrings()
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
                    },
                    Dialog = new DialogRule.DialogPart[]
                    {
                        new DialogRule.DialogPart()
                        {
                            Speaker ="player",
                            Content = "my health is <color='red'> great </color>",
                            ContentParts = new string[]
                            {
                                "'my health is <color='red'> great </color>'",
                            }
                        }
                    }
                },

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

            var dialogs = engine.ExecuteRuleDialogs(best);
            Assert.AreEqual(1, dialogs.Length);
            Assert.AreEqual("my health is <color='red'> great </color>", dialogs[0]);

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

        [TestMethod]
        public void SimpleEngineOutcomeSetter()
        {
            var player = new Actor();
            player.MaxHealth = 100;
            player.Health = 100;
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
                    },
                    Outcomes = new DialogRule.DialogOutcome[]
                    {
                        new DialogRule.DialogOutcome()
                        {
                            Command = "set",
                            Target = "player.health",
                            Arguments = new Dictionary<string, string>()
                            {
                                { "", "(/ player.maxHealth 2)" }
                            }
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

            engine.ExecuteRuleOutcomes(best);

            Assert.AreEqual(50, player.Health);

        }

        [TestMethod]
        public void SimpleEngineOutcomeRunner()
        {
            var player = new Actor();
            player.MaxHealth = 100;
            player.Health = 100;
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
                    },
                    Outcomes = new DialogRule.DialogOutcome[]
                    {
                        new DialogRule.DialogOutcome()
                        {
                            Command = "run",
                            Target = "player.sampleFunc",
                            Arguments = new Dictionary<string, string>()
                            {
                                { "h", "(/ player.maxHealth 2)" },
                                
                                { "ammo", "55" }
                            }
                        }
                    }
                }
            };

            // new OFDA(player, "setAll", new Dictionary<string, string>(){

            var attributes = new DialogAttribute[]
            {
                new ObjectDialogAttribute(player, "player", "health"),
                new ObjectDialogAttribute(player, "player", "maxHealth"),
                new ObjectDialogAttribute(player, "player", "ammo"),
                new ObjectFunctionDialogAttribute("player.sampleFunc", new Action<Dictionary<string, object>>(vars =>
                {
                    var health = (int)vars["h"];
                    var maxHealth = (int)vars["mx"];
                    var ammo = (int)vars["ammo"];

                    player.SetAll(health, maxHealth, ammo);

                }), new Dictionary<string, object>{
                    { "mx", 200 }
                })
            };

            var engine = new DialogEngine();
            rules.ToList().ForEach(r => engine.AddRule(r));
            attributes.ToList().ForEach(a => engine.AddAttribute(a));

            var best = engine.GetBestValidDialog();
            Assert.IsNotNull(best);
            Assert.AreEqual("I have full health!", best.Name);

            engine.ExecuteRuleOutcomes(best);

            Assert.AreEqual(50, player.Health);
            Assert.AreEqual(200, player.MaxHealth);
            Assert.AreEqual(55, player.Ammo);

        }
    }
}
