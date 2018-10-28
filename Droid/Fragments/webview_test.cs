
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
using Android.Webkit;
using Android.Widget;

namespace NPCCMobileApplications.Droid
{
    public class Webview_test : Android.Support.V4.App.Fragment
    {
        LayoutInflater InflaterMain;
        WebView Webview;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            InflaterMain = inflater;
            View view = inflater.Inflate(Resource.Layout.webview_test, container, false);
            Webview = view.FindViewById<WebView>(Resource.Id.webView);

            Webview.SetWebViewClient(new ExtendWebviewClient());

            Webview.LoadUrl("http://www.npcc.ae");

            WebSettings webSettings = Webview.Settings;
            webSettings.JavaScriptEnabled = true;

            return view;
        }
    }

    internal class ExtendWebviewClient : WebViewClient
    {
        public override bool ShouldOverrideUrlLoading(WebView view, IWebResourceRequest request)
        {
            view.LoadUrl((string)request.Url);
            return true;
        }
    }
}
