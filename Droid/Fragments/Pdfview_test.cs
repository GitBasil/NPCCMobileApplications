
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

            pdfView.FromFile(ReadPdfStreamFromExternalStorage("P&ID-SFNY-STP-18-VER-00001.pdf", "https://edms.npcc.ae/NDMS/PublicDocuPreview.aspx?ORIG=P&ID-SFNY-STP-18-VER-00001.pdf&DocuVersID=5057633&SID=O")).Load();

            return view;
        }

        private string SavePdf(string filename, string link) 
        {
            string documentPath = Path.Combine((Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDownloads)).Path, filename);

            using (WebClient client = new WebClient())
            {
                client.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");
                client.DownloadFile(link, documentPath);
            }

            return documentPath;

        }

        private Java.IO.File ReadPdfStreamFromExternalStorage(string filename, string link)
        {
            //Get the path of external storage directory. Here we used download directory to read PDF document 
            String path = SavePdf(filename,link);
            //Read the specific PDF document from the download directory 
            Java.IO.File filePath = new Java.IO.File(path);
            // Check whether the file is exist in the download directory 
            if (filePath.Exists())
            {
                return filePath;
            }
            else
            {
                // throw exception if the file is not found in the appropriate directory 
                throw new FileNotFoundException("File not found" + filePath.AbsolutePath.ToString());
            }
        }

    }
}
