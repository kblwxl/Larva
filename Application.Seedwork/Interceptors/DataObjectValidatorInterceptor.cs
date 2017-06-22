
using Castle.DynamicProxy;
using Infrastructure.DataObjects;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Application.Seedwork.Interceptors
{
    public class DataObjectValidatorInterceptor : IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            foreach(var paramInfo in invocation.Method.GetParameters())
            {
                if(typeof(IDataObject).IsAssignableFrom(paramInfo.ParameterType))
                {
                    DataAnnotationValidator validator = new DataAnnotationValidator();
                    IDataObject item = invocation.Arguments[paramInfo.Position] as IDataObject;
                    if (!validator.IsValid(item))
                    {
                        
                        throw new ApplicationValidationErrorsException(validator.GetInvalidMessages(item));
                    }
                }
            }
            invocation.Proceed();
            
            //CheckRetValue(invocation);
        }
        private void CheckRetValue(IInvocation invocation)
        {
            var retValue = invocation.ReturnValue;
            if (retValue == null)
            {
                return;
            }
            if (typeof(Infrastructure.DataObjects.DataObjectBase).IsAssignableFrom(retValue.GetType()))
            {
                PropertyInfo[] publicProperties = retValue.GetType().GetProperties();
                foreach (var pi in publicProperties)
                {
                    if (pi.PropertyType.Equals(typeof(string)))
                    {
                        object v = pi.GetValue(retValue);
                        if (v == null)
                        {
                            pi.SetValue(retValue, string.Empty);
                        }
                    }
                    else if (pi.PropertyType.IsGenericType && typeof(IList).IsAssignableFrom(pi.PropertyType))
                    {
                        IList list = pi.GetValue(retValue) as IList;
                        if (list != null && list.Count > 0)
                        {
                            var itemPis = list[0].GetType().GetProperties();
                            foreach (var item in list)
                            {
                                foreach (var itemPi in itemPis)
                                {
                                    if (itemPi.PropertyType.Equals(typeof(string)))
                                    {
                                        object v = itemPi.GetValue(item);
                                        if (v == null)
                                        {
                                            itemPi.SetValue(item, string.Empty);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }


            }

        }
    }
    
    internal class DataAnnotationValidator
    {
        void SetValidationAttributeErrors(IDataObject item, List<string> errors) 
        {
            var result = from property in TypeDescriptor.GetProperties(item).Cast<PropertyDescriptor>()
                         from attribute in property.Attributes.OfType<ValidationAttribute>()
                         where !attribute.IsValid(property.GetValue(item))
                         select attribute.FormatErrorMessage(string.Empty);

            if (result != null
                &&
                result.Any())
            {
                errors.AddRange(result);
            }
        }
        void SetValidatableObjectErrors(IDataObject item, List<string> errors) 
        {
            if (typeof(IValidatableObject).IsAssignableFrom(item.GetType()))
            {
                var validationContext = new ValidationContext(item, null, null);

                var validationResults = ((IValidatableObject)item).Validate(validationContext);

                errors.AddRange(validationResults.Select(vr => vr.ErrorMessage));
            }
        }
        public bool IsValid(IDataObject item)
        {
            if(item == null)
            {
                return false;
            }
            List<string> validationErrors = new List<string>();

            SetValidatableObjectErrors(item, validationErrors);
            SetValidationAttributeErrors(item, validationErrors);

            return !validationErrors.Any();

        }
        public IEnumerable<string> GetInvalidMessages(IDataObject item) 
        {
            if (item == null)
                return null;

            List<string> validationErrors = new List<string>();

            SetValidatableObjectErrors(item, validationErrors);
            SetValidationAttributeErrors(item, validationErrors);


            return validationErrors;
        }
    }
}
