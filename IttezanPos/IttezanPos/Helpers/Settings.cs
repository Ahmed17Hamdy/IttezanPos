using Plugin.Settings;
using Plugin.Settings.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace IttezanPos.Helpers
{
    public static class Settings
    {
        private static ISettings AppSettings
        {
            get
            {
                return CrossSettings.Current;
            }
        }
        private const string LastGravity = "last_Gravity_key";
        private static readonly string GravitySettings = string.Empty;
        public static string LastUserGravity
        {
            get
            =>
                 AppSettings.GetValueOrDefault(LastGravity, GravitySettings);
            set
            =>
                AppSettings.AddOrUpdateValue(LastGravity, value);
        }
    }
}
