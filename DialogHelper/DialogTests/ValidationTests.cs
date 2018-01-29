using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Dialog;
using Dialog.Validation;
using System.Linq;

namespace DialogTests
{
    [TestClass]
    public class ValidationTests
    {
        [TestMethod]
        public void HasInvalidCharacterInName()
        {
            var rules = new JsonRule[] {
                new JsonRule()
                {
                    Name = "Has*InvalidCharacter"
                }
            }.ToList();

            var validator = new RuleValidator();
            var results = validator.ValidateRules(rules);

            Assert.AreEqual(1, results.NameErrors.Count);
            Assert.AreEqual(3, results.NameErrors[0].Offset);
            Assert.AreEqual(1, results.NameErrors[0].Length);
            Assert.AreEqual(ValidationErrorType.BAD_CHARACTER, results.NameErrors[0].ErrorType);

            Assert.AreEqual(true, results.HasErrors);

        }

        [TestMethod]
        public void HasOkayName()
        {
            var rules = new JsonRule[] {
                new JsonRule()
                {
                    Name = "HasValidCharacter"
                }
            }.ToList();

            var validator = new RuleValidator();
            var results = validator.ValidateRules(rules);

            Assert.AreEqual(false, results.HasErrors);
        }

        [TestMethod]
        public void DuplicateNamesError()
        {
            var rules = new JsonRule[] {
                new JsonRule()
                {
                    Name = "A"
                },
                new JsonRule()
                {
                    Name = "A"
                }
            }.ToList();

            var validator = new RuleValidator();
            var results = validator.ValidateRules(rules);

            Assert.AreEqual(true, results.HasErrors);
        }
    }
}
