using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dialog.Validation
{
    public static class ValidationCommon
    {
        public const string VALID_CHARACTERS = "\nabcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789:._ -()[];\"'><=";

        public static void CheckForBadCharacters(this string str, List<ValidationError> errors, Guid ruleId, int offset, bool offsetOverride)
        {
            

            for (var i = 0; i < str.Length; i++)
            {
                if (VALID_CHARACTERS.Contains(str[i]) == false)
                {
                    errors.Add(new ValidationError()
                    {
                        Message = $"Invalid Character {str[i]}.",
                        Offset = offsetOverride ? -1 : offset + i,
                        Length = 1,
                        ErrorType = ValidationErrorType.BAD_CHARACTER,
                        RuleId = ruleId
                    });
                    break;
                }
            }
        }

    }
}
