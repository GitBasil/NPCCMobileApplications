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
using Android.Content;
using Calligraphy;

namespace NPCCMobileApplications.Droid
{
    [Activity(Theme = "@style/AppTheme.NoActionBar")]
    public class HomeActivity : AppCompatActivity, NavigationView.IOnNavigationItemSelectedListener
    {
        private FrameLayout mFragmentContainer;
        private DrawerLayout drawer;

        private landing_page mlanding_page;
        private contact mcontact;
        private Webview_test mWebview_test;
        private Pdfview_test mPdfview_test;
        private tabview_test mtabview_test;
        private QrCode_test mQrCode_test;
        private QrCodeScan_test mQrCodeScan_test;
        private CustomListView mCustomListView;
        private text_recognition mtext_recognition;


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


            NavigationView navigationView = FindViewById<NavigationView>(Resource.Id.nav_view);
            navigationView.SetCheckedItem(Resource.Id.landing_page);

            navigationView.SetNavigationItemSelectedListener(this);


            //Fragment
            mFragmentContainer = FindViewById<FrameLayout>(Resource.Id.fragmentContainer);

            mlanding_page = new landing_page();
            ShowFragment(mlanding_page);
        }

        private void ShowFragment(SupportFragment fragment)
        {
            var trans = SupportFragmentManager.BeginTransaction();
            trans.SetCustomAnimations(Resource.Animation.abc_fade_in,Resource.Animation.abc_fade_out);
            trans.Replace(mFragmentContainer.Id, fragment).Commit();
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
                case Resource.Id.landing_page:
                    mlanding_page = new landing_page();
                    ShowFragment(mlanding_page);
                    drawer.CloseDrawer(GravityCompat.Start);
                    return true;
                case Resource.Id.helpDesk:
                    mcontact = new contact();
                    ShowFragment(mcontact);
                    drawer.CloseDrawer(GravityCompat.Start);
                    return true;
                case Resource.Id.Webview_test:
                    mWebview_test = new Webview_test();
                    ShowFragment(mWebview_test);
                    drawer.CloseDrawer(GravityCompat.Start);
                    return true;
                case Resource.Id.Pdfview_test:
                    mPdfview_test = new Pdfview_test();
                    ShowFragment(mPdfview_test);
                    drawer.CloseDrawer(GravityCompat.Start);
                    return true;
                case Resource.Id.tabview_test:
                    mtabview_test = new tabview_test();
                    ShowFragment(mtabview_test);
                    drawer.CloseDrawer(GravityCompat.Start);
                    return true;
                case Resource.Id.QrCode_test:
                    mQrCode_test = new QrCode_test();
                    ShowFragment(mQrCode_test);
                    drawer.CloseDrawer(GravityCompat.Start);
                    return true;
                case Resource.Id.QrCodeScan_test:
                    mQrCodeScan_test = new QrCodeScan_test();
                    ShowFragment(mQrCodeScan_test);
                    drawer.CloseDrawer(GravityCompat.Start);
                    return true;
                case Resource.Id.customListView_test:
                    mCustomListView = new CustomListView();
                    ShowFragment(mCustomListView);
                    drawer.CloseDrawer(GravityCompat.Start);
                    return true;
                case Resource.Id.text_recognition:
                    mtext_recognition = new text_recognition();
                    ShowFragment(mtext_recognition);
                    drawer.CloseDrawer(GravityCompat.Start);
                    return true;

            }

            Toast.MakeText(this, "You have chosen ", ToastLength.Long).Show();


            drawer.CloseDrawer(GravityCompat.Start);
            return true;
        }

        protected override void AttachBaseContext(Context @base)
        {
            base.AttachBaseContext(CalligraphyContextWrapper.Wrap(@base));
        }
    }
}
