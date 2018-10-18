
using Android.App;
using Android.Widget;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V4.View;
using Android.Support.V4.Widget;
using Android.Support.V7.App;
using Android.Views;

namespace NPCCMobileApplications.Droid
{
    [Activity(Theme = "@style/AppTheme.NoActionBar")]
    public class HomeActivity : AppCompatActivity, NavigationView.IOnNavigationItemSelectedListener
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Home);

            var drawer = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
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
        }

        public override void OnBackPressed()
        {
            var drawer = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);

            if (drawer.IsDrawerOpen(GravityCompat.Start))
            {
                drawer.CloseDrawer(GravityCompat.Start);
            }
            else if (drawer.IsDrawerOpen(GravityCompat.End))
            {
                drawer.CloseDrawer(GravityCompat.End);
            }
            else
            {
                base.OnBackPressed();
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
            var text = "";
            if (id == Resource.Id.nav_camera)
            {
                text = "camera";
            }
            else if (id == Resource.Id.nav_gallery)
            {
                text = "gallery";
            }
            else if (id == Resource.Id.nav_slideshow)
            {
                text = "slideshow";
            }
            else if (id == Resource.Id.nav_manage)
            {
                text = "tools";
            }
            else if (id == Resource.Id.nav_share)
            {
                text = "share";
            }
            else if (id == Resource.Id.nav_send)
            {
                text = "send";
            }


            Toast.MakeText(this, "You have chosen " + text, ToastLength.Long).Show();

            var drawer = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            drawer.CloseDrawer(GravityCompat.Start);
            return true;
        }

    }
}
