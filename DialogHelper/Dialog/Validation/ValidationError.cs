using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dialog.Validation
{
    public class ValidationError
    {
        public Guid RuleId { get; set; }
        public string Message { get; set; }
        public int Offset { get; set; }
        public int Length { get; set; }
        public ValidationErrorType ErrorType { get; set; }

    }
}
