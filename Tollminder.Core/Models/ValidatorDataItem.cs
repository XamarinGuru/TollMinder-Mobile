using System;
namespace Tollminder.Core.Models
{
    public class ValidatorDataItem
    {
        public string Name { get; set; }
        public string ValidationText { get; set; }

        public ValidatorDataItem(string name, string validationText)
        {
            Name = name;
            ValidationText = validationText;
        }
    }
}
