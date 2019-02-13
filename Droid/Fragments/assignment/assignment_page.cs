
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
using FFImageLoading;
using NPCCMobileApplications.Library;
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
        assignment_lists _UnderFabricationLists;
        assignment_lists _UnderWeldingLists;
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

            DBRepository dBRepository = new DBRepository();
            UserInfo user = dBRepository.GetUserInfo();
            switch (user.group)
            {
                case "Foreman":
                    _PendingLists = new assignment_lists(Library.npcc_types.inf_assignment_type.Pending);
                    _UnderFabricationLists = new assignment_lists(Library.npcc_types.inf_assignment_type.UnderFabrication);
                    _UnderWeldingLists = new assignment_lists(Library.npcc_types.inf_assignment_type.UnderWelding);
                    _CompletedLists = new assignment_lists(Library.npcc_types.inf_assignment_type.Completed);

                    adapter.AddFragment(_PendingLists, new Java.Lang.String("pending"));
                    adapter.AddFragment(_UnderFabricationLists, new Java.Lang.String("Under Fabrication"));
                    adapter.AddFragment(_UnderWeldingLists, new Java.Lang.String("Under Welding"));
                    adapter.AddFragment(_CompletedLists, new Java.Lang.String("Completed"));

                    viewPager.OffscreenPageLimit = 4;
                    break;
                case "Fabricator":
                case "Welder":
                    _PendingLists = new assignment_lists(Library.npcc_types.inf_assignment_type.Pending);
                    adapter.AddFragment(_PendingLists, new Java.Lang.String("pending"));

                    viewPager.OffscreenPageLimit = 1;

                    break;
            }
            viewPager.Adapter = adapter;
            tabLayout.SetupWithViewPager(viewPager);
            tabLayout.Post(_PendingLists.fill_list);
            tabLayout.TabSelected += TabLayout_TabSelected;

            common_functions.npcc_apply_font(view.FindViewById<TabLayout>(Resource.Id.tabLayout_id));

            ImageService.Instance.InvalidateCacheAsync(FFImageLoading.Cache.CacheType.All);

            return view;
        }

        void TabLayout_TabSelected(object sender, TabLayout.TabSelectedEventArgs e)
        {
            ImageService.Instance.SetExitTasksEarly(true);
            switch (e.Tab.Text)
            {
                case "Under Fabrication":
                    _UnderFabricationLists.ins.fill_list();
                    break;
                case "Under Welding":
                    _UnderWeldingLists.ins.fill_list();
                    break;
                case "Completed":
                    _CompletedLists.ins.fill_list();
                    break;
            }
            ImageService.Instance.SetExitTasksEarly(false);
        }

    }
}
