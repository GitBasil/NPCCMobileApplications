
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Com.Joanzapata.Pdfview;
using Dmax.Dialog;

namespace NPCCMobileApplications.Droid
{
    public class Pdfview_test : Android.Support.V4.App.Fragment
    {
        LayoutInflater InflaterMain;
        View view;
        PDFView pdfView;
        AlertDialog dialog;

        DocBroadcastReceiver receiver;
        private static Pdfview_test ins;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            receiver = new DocBroadcastReceiver();
            ins = this;
        }

        public static Pdfview_test getInstace()
        {
            return ins;
        }

        public void ShowPDFDoc(String path)
        {
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
            dialog.Dismiss();
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            InflaterMain = inflater;
            view = inflater.Inflate(Resource.Layout.Pdfview_test, container, false);
            pdfView = view.FindViewById<PDFView>(Resource.Id.pdfview);

            Intent downloadIntent = new Intent(this.Activity, typeof(LongRunningThread));

            downloadIntent.PutExtra("file_to_download", "https://edms.npcc.ae/NDMS/PublicDocuPreview.aspx?ORIG=P&ID-SFNY-STP-18-VER-00001.pdf&DocuVersID=5057633&SID=O");

            dialog = new SpotsDialog(ins.Context, Resource.Style.CustomDialog);
            dialog.SetMessage("Downloading PDF...");
            dialog.SetCancelable(false);
            dialog.Show();

            this.Activity.StartService(downloadIntent);

            return view;
        }

        public override void OnResume()
        {
            base.OnResume();
            this.Activity.RegisterReceiver(receiver, new IntentFilter("PDFDownloading"));
        }

        public override void OnPause()
        {
            base.OnPause();
            this.Activity.UnregisterReceiver(receiver);
        }

    }

    [BroadcastReceiver(Enabled = true)]
    [IntentFilter(new[] { "PDFDownloading" })]
    public class DocBroadcastReceiver : BroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {
            string file_to_view = intent.GetStringExtra("file_to_view");
            Pdfview_test.getInstace().ShowPDFDoc(file_to_view);
        }
    }
}
