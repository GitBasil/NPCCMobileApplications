
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

            Webview.LoadUrl("http://npcc.ae");

            WebSettings webSettings = Webview.Settings;
            webSettings.JavaScriptEnabled = true;

            return view;
        }
        public override void OnDestroy()
        {
            base.OnDestroy();
            destroyWebView();
        }

        public void destroyWebView()
        {

            // Make sure you remove the WebView from its parent view before doing anything.
            Webview.RemoveAllViews();

            Webview.ClearHistory();

            // NOTE: clears RAM cache, if you pass true, it will also clear the disk cache.
            // Probably not a great idea to pass true if you have other WebViews still alive.
            Webview.ClearCache(true);

            // Loading a blank page is optional, but will ensure that the WebView isn't doing anything when you destroy it.
            Webview.LoadUrl("about:blank");

            Webview.OnPause();
            Webview.RemoveAllViews();
            Webview.DestroyDrawingCache();

            // NOTE: This pauses JavaScript execution for ALL WebViews, 
            // do not use if you have other WebViews still alive. 
            // If you create another WebView after calling this, 
            // make sure to call mWebView.resumeTimers().
            Webview.PauseTimers();

            // NOTE: This can occasionally cause a segfault below API 17 (4.2)
            Webview.Destroy();

            // Null out the reference so that you don't end up re-using it.
            Webview = null;
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
