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
    public class LongRunningThread : Service
    {

        volatile bool isRunning;
        public override StartCommandResult OnStartCommand(Intent intent, [GeneratedEnum] StartCommandFlags flags, int startId)
        {
            isRunning = true;
            Task.Run(() =>
            {
                if (isRunning == true)
                {
                    string fileToDownload = intent.GetStringExtra("file_to_download");
                    string file_to_view = SavePdf("pdfDoc.pdf", fileToDownload);

                    Intent message = new Intent("PDFDownloading");
                    // If desired, pass some values to the broadcast receiver.
                    message.PutExtra("file_to_view", file_to_view);
                    SendBroadcast(message);

                }
                if (isRunning == true) 
                    this.StopSelf();
                    
            });
            return StartCommandResult.Sticky;
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

        public override IBinder OnBind(Intent intent)
        {
            return null;
        }

        public override void OnDestroy()
        {
            isRunning = false;
            base.OnDestroy();
        }
    }
}
