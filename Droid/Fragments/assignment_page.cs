using Android.OS;
using Android.Views;
using Android.Support.Design.Widget;
using Android.Support.V4.View;
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
            adapter.AddFragment(new QrCode_test(), new Java.Lang.String("partial completed"));
            adapter.AddFragment(new Webview_test(), new Java.Lang.String("completed"));

            viewPager.Adapter = adapter;
            tabLayout.SetupWithViewPager(viewPager);
            common_functions.npcc_apply_font(view.FindViewById<TabLayout>(Resource.Id.tabLayout_id));
            return view;
        }
    }
}
