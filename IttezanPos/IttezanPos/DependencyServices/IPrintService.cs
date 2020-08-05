using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xamarin.Forms;

namespace IttezanPos.DependencyServices
{
    public interface IPrintService
    {
        //  void Print(WebView viewToPrint);
        void Print(Stream inputStream, string fileName);
    }
}
