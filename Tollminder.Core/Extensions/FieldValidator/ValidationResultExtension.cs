using System;
using MvvmValidation;

namespace Tollminder.Core.Extensions.FieldValidator
{
    public static class ValidationResultExtension
    {
        public static ObservableDictionary<string, string> AsObservableDictionary(this ValidationResult result)
        {
            var dictionary = new ObservableDictionary<string, string>();
            foreach (var item in result.ErrorList)
            {
                var key = item.Target.ToString();
                var text = item.ErrorText;
                if (dictionary.ContainsKey(key))
                {
                    dictionary[key] = dictionary.Keys + Environment.NewLine + text;
                }
                else
                {
                    dictionary[key] = text;
                }
            }
            return dictionary;
        }
    }
}
