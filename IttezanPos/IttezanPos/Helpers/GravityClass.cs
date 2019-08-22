using Android.Views;
using System;
using System.Collections.Generic;
using System.Text;

namespace IttezanPos.Helpers
{
    public class GravityClass
    {
        public static int Grav()
        {
            if (Settings.LastUserGravity == "Arabic")
            {
                return (int)GravityFlags.Right;
            }
            else
            {
                return (int)GravityFlags.Left;
            }
        }
    }
}
