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
using NPCCMobileApplications.Library;
using FFImageLoading;
using FFImageLoading.Views;
using FFImageLoading.Transformations;
using static NPCCMobileApplications.Library.npcc_types;
using System.Threading.Tasks;
using System.IO;
using System.Threading;
using Android.Content.PM;
using System;

namespace NPCCMobileApplications.Droid
{
    [Activity(Theme = "@style/AppTheme", ScreenOrientation = ScreenOrientation.Portrait)]
    public class HomeActivity : AppCompatActivity, NavigationView.IOnNavigationItemSelectedListener
    {
        private FrameLayout mFragmentContainer;
        private DrawerLayout drawer;

        private assignment_page mAssignment_page;
        private landing_page mlanding_page;
        private contact mcontact;
        private Webview_test mWebview_test;
        private tabview_test mtabview_test;
        private QrCode_test mQrCode_test;
        private QrCodeScan_test mQrCodeScan_test;
        private CustomListView mCustomListView;
        private text_recognition mtext_recognition;
        private UserInfo lstObjs;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Window.AddFlags(WindowManagerFlags.DrawsSystemBarBackgrounds);

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
            navigationView.SetCheckedItem(Resource.Id.assignment_page);

            navigationView.SetNavigationItemSelectedListener(this);


            DBRepository dBRepository = new DBRepository();
            lstObjs = dBRepository.GetUserInfo();

            navigationView.GetHeaderView(0).FindViewById<TextView>(Resource.Id.lblUsername).Text += lstObjs.fullname;
            ImageViewAsync imageView = navigationView.GetHeaderView(0).FindViewById<ImageViewAsync>(Resource.Id.imgUser);
            ImageService.Instance
                .LoadStream((token) => { return npcc_services.GetStreamFromImageByte(lstObjs.img); })
                .LoadingPlaceholder("loadingimg", FFImageLoading.Work.ImageSource.CompiledResource)
                .ErrorPlaceholder("notfound", FFImageLoading.Work.ImageSource.CompiledResource)
                .Transform(new CircleTransformation())
                .IntoAsync(imageView); 

            //Fragment
            mFragmentContainer = FindViewById<FrameLayout>(Resource.Id.fragmentContainer);

            mAssignment_page = new assignment_page();
            ShowFragment(mAssignment_page);

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

                case Resource.Id.assignment_page:
                    mAssignment_page = new assignment_page();
                    ShowFragment(mAssignment_page);
                    drawer.CloseDrawer(GravityCompat.Start);
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
