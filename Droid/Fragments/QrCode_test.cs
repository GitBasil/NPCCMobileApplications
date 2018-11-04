
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace NPCCMobileApplications.Droid
{
    public class QrCode_test : Android.Support.V4.App.Fragment
    {
        LayoutInflater InflaterMain;
        View view;
        ImageView imageBarcode;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            InflaterMain = inflater;
            view = inflater.Inflate(Resource.Layout.QrCode_test, container, false);

            imageBarcode = view.FindViewById<ImageView>(Resource.Id.imageBarcode);

            var barcodeWriter = new ZXing.Mobile.BarcodeWriter
            {
                Format = ZXing.BarcodeFormat.QR_CODE,
                Options = new ZXing.Common.EncodingOptions
                {
                    Width = 300,
                    Height = 300
                }
            };
            var barcode = barcodeWriter.Write("Basel Abubaker");

            imageBarcode.SetImageBitmap(barcode);

            return view;
        }
    }

   
}
