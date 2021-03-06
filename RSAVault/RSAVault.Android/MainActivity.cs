using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
using Plugin.CurrentActivity;
using Plugin.Fingerprint;

namespace RSAVault.Droid
{
    [Activity(Label = "RSAVault", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize )]
    public class MainActivity :Kit.Droid.Services.MainActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            LoadApplication(new App());
            CrossFingerprint.SetCurrentActivityResolver(()=>CrossCurrentActivity.Current.Activity);
        }
    }
}