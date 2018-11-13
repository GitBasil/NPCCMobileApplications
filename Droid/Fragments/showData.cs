using System;
using Android.OS;
using Android.Views;
using Android.Support.V7.App;
using SupportToolbar = Android.Support.V7.Widget.Toolbar;
using static Android.Views.View;
using Android.Runtime;
using SupportFragment = Android.Support.V4.App.Fragment;
using Android.Widget;
using Android.Graphics;

namespace NPCCMobileApplications.Droid
{
    public class showData : Android.Support.V4.App.Fragment
    {
        LayoutInflater InflaterMain;
        SupportToolbar mToolbar;
        AppCompatActivity act;
        FrameLayout mFragmentContainer;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            HasOptionsMenu = true;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            InflaterMain = inflater;
            View view = inflater.Inflate(Resource.Layout.showData, container, false);

            mFragmentContainer = this.Activity.FindViewById<FrameLayout>(Resource.Id.fragmentContainer);

            act = (AppCompatActivity)this.Activity;

            mToolbar = act.FindViewById<SupportToolbar>(Resource.Id.toolbar);
            act.SetSupportActionBar(mToolbar);

            act.SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            mToolbar.NavigationIcon.SetColorFilter(Color.ParseColor("#FFFFFF"), PorterDuff.Mode.SrcAtop);


            return base.OnCreateView(inflater, container, savedInstanceState);
        }


        public override void OnDestroy()
        {
            base.OnDestroy();
            act.SupportActionBar.SetDisplayHomeAsUpEnabled(false);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            CustomListView mCustomListView = new CustomListView();
            common_functions.npcc_show_fragment(act,mFragmentContainer,mCustomListView);
            return base.OnOptionsItemSelected(item);
        }


    }
}
