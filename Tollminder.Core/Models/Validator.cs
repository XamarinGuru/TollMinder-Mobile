using System;
namespace Tollminder.Core.Models
{
    public class Validator
    {
        string Name { get; set; }
        string ValidationText { get; set; }
        Func<bool> Expression { get; set; }

        public Validator(string name, string validationText, Func<bool> expression)
        {
            Name = name;
            ValidationText = validationText;
            Expression = expression;
        }

        public ValidatorDataItem Validate()
        {
            if (Expression != null && Expression.Invoke())
                return new ValidatorDataItem(Name, ValidationText);
            else
                return new ValidatorDataItem(Name, null);
        }
    }
}
