
using Android.App;
using Android.Widget;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V4.View;
using Android.Support.V4.Widget;
using Android.Support.V7.App;
using Android.Views;
using Xamarin.Essentials;
using SupportFragment = Android.Support.V4.App.Fragment;
using System.Collections.Generic;

namespace NPCCMobileApplications.Droid
{
    [Activity(Theme = "@style/AppTheme.NoActionBar")]
    public class HomeActivity : AppCompatActivity, NavigationView.IOnNavigationItemSelectedListener
    {
        private FrameLayout mFragmentContainer;
        private SupportFragment mCurrentFragment;
        private Stack<SupportFragment> mStackFragments;
        private DrawerLayout drawer;
        private Fragment1 mFragment1;
        private Fragment2 mFragment2;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Home);

            drawer = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            var menuLeft = FindViewById<Button>(Resource.Id.menuLeft);

            menuLeft.Click += (sender, args) =>
            {
                if (drawer.IsDrawerOpen(GravityCompat.Start))
                {
                    drawer.CloseDrawer(GravityCompat.Start);
                }
                else
                {
                    drawer.OpenDrawer(GravityCompat.Start);
                }
            };


            var navigationView = FindViewById<NavigationView>(Resource.Id.nav_view);

            navigationView.SetNavigationItemSelectedListener(this);


            //Fragment
            mFragmentContainer = FindViewById<FrameLayout>(Resource.Id.fragmentContainer);
            setFragments();
        }

        void setFragments()
        {
            mFragment1 = new Fragment1();
            mFragment2 = new Fragment2();

            mStackFragments = new Stack<SupportFragment>();

            var trans = SupportFragmentManager.BeginTransaction();

            trans.Add(Resource.Id.fragmentContainer, mFragment2, "Fragment2");
            trans.Hide(mFragment2);

            trans.Add(Resource.Id.fragmentContainer, mFragment1, "Fragment1");
            trans.Commit();

            mCurrentFragment = mFragment1;
        }

        private void ShowFragment(SupportFragment fragment)
        {
            if (fragment.IsVisible)
            {
                return;
            }

            var trans = SupportFragmentManager.BeginTransaction();

            trans.SetCustomAnimations(Resource.Animation.slide_in, Resource.Animation.slide_out, Resource.Animation.slide_in, Resource.Animation.slide_out);

            fragment.View.BringToFront();
            mCurrentFragment.View.BringToFront();

            trans.Hide(mCurrentFragment);
            trans.Show(fragment);

            trans.AddToBackStack(null);
            mStackFragments.Push(mCurrentFragment);
            trans.Commit();

            mCurrentFragment = fragment;
        }

        public override void OnBackPressed()
        {
            drawer = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);

            if (drawer.IsDrawerOpen(GravityCompat.Start))
            {
                drawer.CloseDrawer(GravityCompat.Start);
            }
            else
            {

                if (SupportFragmentManager.BackStackEntryCount > 0)
                {
                    SupportFragmentManager.PopBackStack();
                    mCurrentFragment = mStackFragments.Pop();
                }
                else {
                    base.OnBackPressed();
                }
            }
        }


        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            // Handle action bar item clicks here. The action bar will
            // automatically handle clicks on the Home/Up button, so long
            // as you specify a parent activity in AndroidManifest.xml.
            int id = item.ItemId;

            return base.OnOptionsItemSelected(item);
        }

        public bool OnNavigationItemSelected(IMenuItem item)
        {
            // Handle navigation view item clicks here.
            int id = item.ItemId;
            switch(id){
                case Resource.Id.logout:
                    SecureStorage.Remove("oauth_token");
                    StartActivity(typeof(LoginActivity));
                    Finish();
                    return true;
                case Resource.Id.challengeZone:
                    ShowFragment(mFragment1);
                    drawer.CloseDrawer(GravityCompat.Start);
                    return true;
                case Resource.Id.interviewSchedule:
                    ShowFragment(mFragment2);
                    drawer.CloseDrawer(GravityCompat.Start);
                    return true;
            }

            Toast.MakeText(this, "You have chosen ", ToastLength.Long).Show();


            drawer.CloseDrawer(GravityCompat.Start);
            return true;
        }

    }
}
