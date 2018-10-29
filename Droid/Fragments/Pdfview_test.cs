
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Com.Joanzapata.Pdfview;

namespace NPCCMobileApplications.Droid
{
    public class Pdfview_test : Android.Support.V4.App.Fragment
    {
        LayoutInflater InflaterMain;
        View view;
        PDFView pdfView;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            InflaterMain = inflater;
            view = inflater.Inflate(Resource.Layout.Pdfview_test, container, false);
            pdfView = view.FindViewById<PDFView>(Resource.Id.pdfview);

            pdfView.FromAsset("sample.pdf").Load();

            return view;
        }

    }
}
