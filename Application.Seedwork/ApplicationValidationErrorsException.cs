using Application.Seedwork.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Seedwork
{
    public class ApplicationValidationErrorsException : Exception
    {
        IEnumerable<string> _validationErrors;
        public IEnumerable<string> ValidationErrors
        {
            get
            {
                return _validationErrors;
            }
        }
        public ApplicationValidationErrorsException(IEnumerable<string> validationErrors)
            : base(Messages.exception_ApplicationValidationExceptionDefaultMessage)
        {
            _validationErrors = validationErrors;
        }
        public override string Message
        {
            get
            {
                if(_validationErrors!=null)
                {
                    return string.Join(Environment.NewLine, _validationErrors);
                }
                return string.Empty;
            }
        }
    }
}
