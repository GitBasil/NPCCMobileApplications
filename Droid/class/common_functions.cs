
using System;
using Android.App;
using Android.Graphics;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
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


        public static void npcc_show_fragment(AppCompatActivity act, FrameLayout mFragmentContainer, SupportFragment fragment)
        {
            var trans = act.SupportFragmentManager.BeginTransaction();
            trans.SetCustomAnimations(Resource.Animation.abc_fade_in, Resource.Animation.abc_fade_out);
            trans.Replace(mFragmentContainer.Id, fragment).Commit();
        }
    }
}
