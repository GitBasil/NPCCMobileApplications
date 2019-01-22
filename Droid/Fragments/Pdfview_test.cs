
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Util;
using Android.Views;
using Android.Widget;
using Com.Joanzapata.Pdfview;
using Dmax.Dialog;
using NPCCMobileApplications.Library;
using SupportToolbar = Android.Support.V7.Widget.Toolbar;

namespace NPCCMobileApplications.Droid
{
    public class Pdfview_test : Android.Support.V4.App.Fragment
    {
        LayoutInflater InflaterMain;
        View view;
        PDFView pdfView;
        ProgressBar progress_bar;
        RelativeLayout ProgressCont;
        TextView proTextView;
        string pdfLink;
        AppCompatActivity act;
        SupportToolbar mToolbar;

        DocBroadcastReceiver receiver;
        private static Pdfview_test ins;

        private string _link;
        public Pdfview_test(string link)
        {
            _link = link;
        }
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            receiver = new DocBroadcastReceiver();
            ins = this;
            HasOptionsMenu = true;
        }

        public static Pdfview_test getInstace()
        {
            return ins;
        }

        public void ShowPDFDoc(String path)
        {
            //this.Activity.UnregisterReceiver(receiver);
            ProgressCont.Visibility = ViewStates.Gone;

            ins.Activity.RunOnUiThread(() => {
                Java.IO.File filePath = new Java.IO.File(path);
                // Check whether the file is exist in the download directory 
                if (filePath.Exists())
                {
                    pdfView.FromFile(filePath).Load();
                }
                else
                {
                    // throw exception if the file is not found in the appropriate directory 
                    throw new FileNotFoundException("File not found" + filePath.AbsolutePath.ToString());
                }
            });
        }

        public void ProgressBar(long totalSize, long BytesReceived)
        {

            ins.Activity.RunOnUiThread(() => {
                if(progress_bar.Max == 100)
                progress_bar.Max = (int)totalSize;
                progress_bar.Progress =(int)BytesReceived;
                proTextView.Text = npcc_services.npcc_SizeSuffix(BytesReceived) + "/" + npcc_services.npcc_SizeSuffix(totalSize);
            });
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            InflaterMain = inflater;
            view = inflater.Inflate(Resource.Layout.Pdfview_test, container, false);

            act = (AppCompatActivity)this.Activity;

            mToolbar = act.FindViewById<SupportToolbar>(Resource.Id.toolbar);
            act.SetSupportActionBar(mToolbar);

            act.SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            mToolbar.NavigationIcon.SetColorFilter(Color.ParseColor("#FFFFFF"), PorterDuff.Mode.SrcAtop);

            pdfView = view.FindViewById<PDFView>(Resource.Id.pdfview);

            progress_bar = view.FindViewById<ProgressBar>(Resource.Id.progress_bar);

            ProgressCont = view.FindViewById<RelativeLayout>(Resource.Id.ProgressCont);

            proTextView = view.FindViewById<TextView>(Resource.Id.proTextView);

            Intent downloadIntent = new Intent(this.Activity, typeof(PDFLongRunningThread));
            downloadIntent.PutExtra("file_to_download", _link);
        
            this.Activity.RegisterReceiver(receiver, new IntentFilter("PDFDownloading"));
            this.Activity.StartService(downloadIntent);

            return view;
        }


        public override void OnDestroyView()
        {
            base.OnDestroyView();
            this.Activity.UnregisterReceiver(receiver);
            act.SupportActionBar.SetDisplayHomeAsUpEnabled(false);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            act.SupportFragmentManager.PopBackStack();
            return base.OnOptionsItemSelected(item);
        }
    }
}
