using System;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Widget;

namespace NPCCMobileApplications.Droid
{
    [Service]
    public class PDFLongRunningThread : Service
    {

        volatile bool isRunning;
        string documentPath;
        Uri uri;
        WebClient client;

        public override IBinder OnBind(Intent intent)
        {
            return null;
        }

        public override void OnDestroy()
        {
            isRunning = false;
            client.Dispose();
            base.OnDestroy();
        }

        public override StartCommandResult OnStartCommand(Intent intent, [GeneratedEnum] StartCommandFlags flags, int startId)
        {
            isRunning = true;
            Task.Run(() =>
            {
                if (isRunning == true)
                {
                    string fileToDownload = intent.GetStringExtra("file_to_download");
                    SavePdf("pdfDoc.pdf", fileToDownload);
                }
                if (isRunning == true) 
                    this.StopSelf();
                    
            });
            return StartCommandResult.Sticky;
        }

        private void SavePdf(string filename, string link)
        {
            documentPath = Path.Combine((Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDownloads)).Path, filename);

            client = new WebClient();
            uri = new Uri(link);
            client.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");
            client.DownloadProgressChanged += Client_DownloadProgressChanged;
            client.DownloadFileAsync(uri, documentPath);
        }

        void Client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            WebClient wc = (WebClient)sender;
            long totalSize = long.Parse(wc.ResponseHeaders["Content-Length"]);
            if(Pdfview_test.getInstace().Activity == null){
                client.CancelAsync();
                return;
            }
            Pdfview_test.getInstace().ProgressBar(totalSize, e.BytesReceived);
            if(e.TotalBytesToReceive == e.BytesReceived)
            {
                Intent message = new Intent("PDFDownloading");
                message.PutExtra("file_to_view", documentPath);
                SendBroadcast(message);
            }

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
