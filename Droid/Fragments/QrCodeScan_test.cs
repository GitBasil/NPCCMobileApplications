
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using ZXing.Mobile;

namespace NPCCMobileApplications.Droid
{
    public class QrCodeScan_test : Android.Support.V4.App.Fragment
    {
        LayoutInflater InflaterMain;
        View view;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            InflaterMain = inflater;
            view = inflater.Inflate(Resource.Layout.QrCodeScan_test, container, false);

            var needsPermissionRequest = ZXing.Net.Mobile.Android.PermissionsHandler.NeedsPermissionRequest(this.Activity);

            if (needsPermissionRequest)
                ZXing.Net.Mobile.Android.PermissionsHandler.RequestPermissionsAsync(this.Activity);

            if (!needsPermissionRequest)
                scanAsync();

            return view;
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            ZXing.Net.Mobile.Android.PermissionsHandler.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        async void scanAsync()
        {
            MobileBarcodeScanner.Initialize(Activity.Application);

            var scanner = new MobileBarcodeScanner();

            var result = await scanner.Scan();

            // The if was inverted.
            if (result == null)
            {
                return;
            }

            Console.WriteLine($"Scanned Barcode: {result}");

            // Using this you are sure it will run in the UI thread
            // as you will be updating an UI element.
            Activity.RunOnUiThread(() => {
                Activity.FindViewById<TextView>(Resource.Id.textView1).Text = result.Text;
                Activity.FindViewById<TextView>(Resource.Id.textView2).Text = result.Text;
            });
        }
    }
}
