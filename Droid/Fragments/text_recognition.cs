using System.Text.RegularExpressions;
using Android;
using Android.Content.PM;
using Android.Gms.Vision;
using Android.Gms.Vision.Texts;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Util;
using Android.Views;
using Android.Widget;
using Java.Lang;
using static Android.Gms.Vision.Detector;

namespace NPCCMobileApplications.Droid
{
    public class text_recognition : Android.Support.V4.App.Fragment, ISurfaceHolderCallback, IProcessor
    {
        LayoutInflater InflaterMain;
        SurfaceView cameraView;
        TextView textView;
        CameraSource cameraSource;
        const int RequestCameraPermessionID = 1001;


        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            InflaterMain = inflater;
            View view = inflater.Inflate(Resource.Layout.text_recognition, container, false);

            cameraView = view.FindViewById<SurfaceView>(Resource.Id.surface_view);
            textView = view.FindViewById<TextView>(Resource.Id.text_view_dcr);

            TextRecognizer textRecognizer = new TextRecognizer.Builder(this.Activity.ApplicationContext).Build();
            if (!textRecognizer.IsOperational)
                Log.Error("text_recognition", "Detector depenencies are not avaliable yet!");
            else 
            {
                cameraSource = new CameraSource.Builder(Activity.ApplicationContext, textRecognizer)
                                               .SetFacing(CameraFacing.Back)
                                               .SetRequestedPreviewSize(1280, 1024)
                                               .SetRequestedFps(2.0f)
                                               .SetAutoFocusEnabled(true).Build();
            }

            cameraView.Holder.AddCallback(this);
            textRecognizer.SetProcessor(this);
            return view;
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
            switch(requestCode)
            {
                case RequestCameraPermessionID:
                    {
                        if(grantResults[0] == Permission.Granted)
                            cameraSource.Start(cameraView.Holder);
                    }
                    break;
            }
        }

        public void SurfaceChanged(ISurfaceHolder holder, [GeneratedEnum] Format format, int width, int height)
        {

        }

        public void SurfaceCreated(ISurfaceHolder holder)
        {
            if(Android.Support.V4.Content.ContextCompat.CheckSelfPermission(Activity.ApplicationContext,Manifest.Permission.Camera) != Android.Content.PM.Permission.Granted)
            {
                ActivityCompat.RequestPermissions(this.Activity, new string[]{
                    Android.Manifest.Permission.Camera
                }, RequestCameraPermessionID);
                return;
            }
            cameraSource.Start(cameraView.Holder);
        }

        public void SurfaceDestroyed(ISurfaceHolder holder)
        {
            cameraSource.Stop();
        }

        public void ReceiveDetections(Detections detections)
        {
            SparseArray items = detections.DetectedItems;
            const string Pattern = "#.*?#";
            Regex regex = new Regex(Pattern);


            string found="";
            if(items.Size() != 0){
                textView.Post(() => {
                    StringBuilder strBuilder = new StringBuilder();
                for (int i = 0; i < items.Size();i++)
                    {
                        //Match match = regex.Match(((TextBlock)items.ValueAt(i)).Value);
                        //if (match.Success)
                        //{
                        //    found = match.Value;
                        //    cameraSource.Stop();
                        //} 
                        ////else
                        ////{
                        ////    strBuilder.Append(((TextBlock)items.ValueAt(i)).Value);
                        ////    strBuilder.Append("\n");
                        ////}
                         strBuilder.Append(((TextBlock)items.ValueAt(i)).Value);
                         strBuilder.Append("\n");
                    }
                    //if (found != "")
                        //textView.Text = found;

                    textView.Text = strBuilder.ToString();
                    //else
                });
            }
        }

        public void Release()
        {

        }
    }
}
