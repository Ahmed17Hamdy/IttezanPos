using System;
using System.Collections.Generic;
using System.Text;

namespace IttezanPos.Models.BModel
{
    public class RequiredValidator : IValidator
    {
        public string Message { get; set; } = AppResources.FieldReq;

        public bool Check(string value)
        {
            return !string.IsNullOrWhiteSpace(value);
        }
    }
}
