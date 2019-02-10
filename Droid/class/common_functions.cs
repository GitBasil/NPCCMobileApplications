
using System;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using FFImageLoading;
using SupportFragment = Android.Support.V4.App.Fragment;

namespace NPCCMobileApplications.Droid
{
    public static class common_functions
    {
        public static void npcc_apply_font(ViewGroup vg)
        {
            Typeface tf = Typeface.CreateFromAsset(Application.Context.Assets, "fonts/npcc_font.ttf");

            for (int i = 0; i < vg.ChildCount; i++)
            {
                View v = vg.GetChildAt(i);
                if ((v is TextView))
                {
                    ((TextView)(v)).SetTypeface(tf, TypefaceStyle.Normal);
                }
                else if ((v is ViewGroup))
                {
                    npcc_apply_font(((ViewGroup)(v)));
                }

            }

        }


        public static void npcc_show_fragment(AppCompatActivity act, FrameLayout mFragmentContainer, SupportFragment fragment, SupportFragment oldFragment)
        {
            var trans = act.SupportFragmentManager.BeginTransaction();
            trans.SetCustomAnimations(Resource.Animation.abc_fade_in, Resource.Animation.abc_fade_out);
            //trans.Replace(mFragmentContainer.Id, fragment).Commit();
            trans.Add(mFragmentContainer.Id, fragment,"ShowData");
            //fragment.View.BringToFront();
            trans.Hide(oldFragment);
            trans.Show(fragment);
            trans.AddToBackStack(null);
            trans.Commit();
        }

        public static async void npcc_setScaleImageView(AppCompatActivity act,View view, string url, ScaleImageView imageView)
        {
            var image = await ImageService.Instance
                        .LoadUrl(url)
                        .AsBitmapDrawableAsync();

            act.RunOnUiThread(() => {
                imageView.SetImageBitmap(image.Bitmap);
            });
        }

        public static void DisplayToast(String message, Context context)
        {
            // Construct the toast, set the view and display
            Toast toast;
            LayoutInflater inflater = (LayoutInflater)context.GetSystemService(Context.LayoutInflaterService);
            View view = inflater.Inflate(Resource.Layout.toast_layout, null);

            // Fill in the message into the textview
            TextView text = (TextView)view.FindViewById(Resource.Id.text);
            text.Text = message;
            toast = new Toast(context)
            {
                View = view,
                Duration = ToastLength.Long
            };
            //toast.SetGravity(GravityFlags.Bottom,0,20);
            toast.Show();
        }
    }
}
