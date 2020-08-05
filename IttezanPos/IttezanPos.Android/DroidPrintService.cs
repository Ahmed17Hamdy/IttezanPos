using System;

using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Android.Print;
using Android.Content;
using IttezanPos.Droid;
using IttezanPos.DependencyServices;
using System.IO;

[assembly: Dependency(typeof(PrintService))]
namespace IttezanPos.Droid
{
    class PrintService : IPrintService
    {
        public void Print(Stream inputStream, string fileName)
        {
            if (inputStream.CanSeek)
                //Reset the position of PDF document stream to be printed
                inputStream.Position = 0;
            //Create a new file in the Personal folder with the given name
            string createdFilePath = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), fileName);
            //Save the stream to the created file
            using (var dest = System.IO.File.OpenWrite(createdFilePath))
                inputStream.CopyTo(dest);
            string filePath = createdFilePath;
            var activity = Xamarin.Essentials.Platform.CurrentActivity;
            PrintManager printManager = (PrintManager)activity.GetSystemService(Context.PrintService);
            PrintDocumentAdapter pda = new CustomPrintDocumentAdapter(filePath);
            //Print with null PrintAttributes
            printManager.Print(fileName, pda, null);
        }
    }
    //public class DroidPrintService : IPrintService
    //{
    //    public DroidPrintService()
    //    {
    //    }

    //    public void Print(WebView viewToPrint)
    //    {
    //        var droidViewToPrint = Platform.CreateRenderer(viewToPrint).ViewGroup.GetChildAt(0) as Android.Webkit.WebView;

    //        if (droidViewToPrint != null)
    //        {
    //            // Only valid for API 19+
    //            var version = Android.OS.Build.VERSION.SdkInt;

    //            if (version >= Android.OS.BuildVersionCodes.Kitkat)
    //            {
    //                var printMgr = (PrintManager)Forms.Context.GetSystemService(Context.PrintService);

    //                printMgr.Print("Forms-EZ-Print", droidViewToPrint.CreatePrintDocumentAdapter(), null);
    //            }
    //        }
    //    }
    //}
}