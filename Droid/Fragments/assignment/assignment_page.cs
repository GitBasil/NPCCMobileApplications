
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
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
    public class assignment_page : SupportFragment
    {
        LayoutInflater InflaterMain;
        View view;

        private TabLayout tabLayout;
        private ViewPager viewPager;
        private ViewPagerAdapter adapter;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            InflaterMain = inflater;
            view = inflater.Inflate(Resource.Layout.tabview_test, container, false);

            tabLayout = view.FindViewById<TabLayout>(Resource.Id.tabLayout_id);
            viewPager = view.FindViewById<ViewPager>(Resource.Id.viewPager_id);

            adapter = new ViewPagerAdapter(this.Activity.SupportFragmentManager);
            adapter.AddFragment(new pending_page(), new Java.Lang.String("pending"));
            adapter.AddFragment(new pending_page(), new Java.Lang.String("partial completed"));
            adapter.AddFragment(new pending_page(), new Java.Lang.String("completed"));

            viewPager.Adapter = adapter;
            viewPager.OffscreenPageLimit = 3;
            tabLayout.SetupWithViewPager(viewPager);
            common_functions.npcc_apply_font(view.FindViewById<TabLayout>(Resource.Id.tabLayout_id));
            return view;
        }
    }
}
