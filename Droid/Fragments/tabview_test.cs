
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V4.View;
using Android.Util;
using Android.Views;
using Android.Widget;
using SupportFragment = Android.Support.V4.App.Fragment;

namespace NPCCMobileApplications.Droid
{
    public class tabview_test : Android.Support.V4.App.Fragment
    {
        LayoutInflater InflaterMain;
        View view;

        private TabLayout tabLayout;
        private ViewPager viewPager;


        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            InflaterMain = inflater;
            view = inflater.Inflate(Resource.Layout.tabview_test, container,false);

            tabLayout = view.FindViewById<TabLayout>(Resource.Id.tabLayout_id);
            viewPager = view.FindViewById<ViewPager>(Resource.Id.viewPager_id);

            ViewPagerAdapter adapter = new ViewPagerAdapter(this.Activity.SupportFragmentManager);
            adapter.AddFragment(new QrCode_test(), new Java.Lang.String("QrCode_test"));
            adapter.AddFragment(new contact(), new Java.Lang.String("contact"));
            adapter.AddFragment(new Webview_test(), new Java.Lang.String("Webview_test"));

            viewPager.Adapter = adapter;
            tabLayout.SetupWithViewPager(viewPager);
            return view;
        }
    }
}
