using Android.App;
using Android.Content.PM;
using Android.OS;
using NPCCMobileApplications.Library;

namespace NPCCMobileApplications.Droid
{
    [Activity(MainLauncher = true, Theme = "@style/SplashTheme", ScreenOrientation = ScreenOrientation.Portrait)]
    public class InitialActivity : Activity
    {
        public npcc_authentication oauth;
        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            oauth = new npcc_authentication();
            bool isAuth = await oauth.IsAuthenticatedAsync();

            if (isAuth)
            {
                StartActivity(typeof(HomeActivity));
                Finish();
            }
            else
            {
                StartActivity(typeof(LoginActivity));
                Finish();
            }
        }




    }
}
