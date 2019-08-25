using System;
using System.Collections.Generic;
using System.Text;

namespace IttezanPos.Models.BModel
{
    public interface IValidator
    {
        string Message { get; set; }
        bool Check(string value);
    }
}
