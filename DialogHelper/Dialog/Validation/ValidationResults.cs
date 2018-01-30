using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dialog.Validation
{
    public class ValidationResults
    {

        public List<ValidationError> NameErrors { get; set; } = new List<ValidationError>();
        public List<ValidationError> DisplayAsErrors { get; set; } = new List<ValidationError>();
        public List<ValidationError> ConditionErrors { get; set; } = new List<ValidationError>();
        public List<ValidationError> DialogErrors { get; set; } = new List<ValidationError>();

        public bool HasErrors
        {
            get
            {
                return NameErrors.Count > 0
                    || ConditionErrors.Count > 0
                    || DialogErrors.Count > 0
                    || DisplayAsErrors.Count > 0;
            }
        }
        public bool IsCorrect
        {
            get
            {
                return NameErrors.Count == 0
                    && ConditionErrors.Count == 0
                    && DialogErrors.Count == 0
                    && DisplayAsErrors.Count == 0;
            }
        }


        public ValidationResults()
        {

        }

    }
}
