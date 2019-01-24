
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
        assignment_lists _PendingLists;
        assignment_lists _UnderProgressLists;
        assignment_lists _CompletedLists;

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

            _PendingLists = new assignment_lists(Library.npcc_types.inf_assignment_type.Pending);
            _UnderProgressLists = new assignment_lists(Library.npcc_types.inf_assignment_type.UnderProgress);
            _CompletedLists = new assignment_lists(Library.npcc_types.inf_assignment_type.Completed);

            adapter.AddFragment(_PendingLists, new Java.Lang.String("pending"));
            adapter.AddFragment(_UnderProgressLists, new Java.Lang.String("under progress"));
            adapter.AddFragment(_CompletedLists, new Java.Lang.String("completed"));

            viewPager.Adapter = adapter;
            viewPager.OffscreenPageLimit = 3;
            tabLayout.SetupWithViewPager(viewPager);
            tabLayout.Post(_PendingLists.fill_listAsync);
            tabLayout.TabSelected += TabLayout_TabSelected;

            common_functions.npcc_apply_font(view.FindViewById<TabLayout>(Resource.Id.tabLayout_id));
            return view;
        }

        void TabLayout_TabSelected(object sender, TabLayout.TabSelectedEventArgs e)
        {
            switch (e.Tab.Text)
            {
                case "under progress":
                    _UnderProgressLists.ins.fill_listAsync();
                    break;
                case "completed":
                    _CompletedLists.ins.fill_listAsync();
                    break;
            }

        }

    }
}
