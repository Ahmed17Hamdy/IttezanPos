using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace IttezanPos.DependencyServices
{
    public interface IPrintService
    {
        void Print(WebView viewToPrint);
    }
}
