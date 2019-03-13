
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Dmax.Dialog;
using NPCCMobileApplications.Library;
using Xamarin.Essentials;
using static NPCCMobileApplications.Library.npcc_types;

namespace NPCCMobileApplications.Droid
{
    [Activity(Theme = "@style/ThemeLogin", ScreenOrientation = ScreenOrientation.Portrait)]
    public class LoginActivity : Activity
    {
        Button btnLogin;
        EditText txtUsername, txtPassword;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.LoginLayout);

            txtUsername = FindViewById<EditText>(Resource.Id.txtUsername);
            txtPassword = FindViewById<EditText>(Resource.Id.txtPassword);
            btnLogin = FindViewById<Button>(Resource.Id.btnLogin);
            btnLogin.Click += BtnLogin_ClickAsync;
        }

        async void BtnLogin_ClickAsync(object sender, EventArgs e)
        {
            if (IsUserNameValid() && IsPasswordValid())
            {
                AlertDialog dialog = new SpotsDialog(this, Resource.Style.CustomDialog);
                dialog.SetMessage("Checking Your Details...");
                dialog.SetCancelable(false);
                dialog.Show();

                npcc_authentication oauth = new npcc_authentication();
                inf_login_info lg = await oauth.Login(txtUsername.Text, txtPassword.Text);

                dialog.Dismiss();

                //We have successfully authenticated a the user,
                //Now fire our OnLoginSuccess Event.
                if (lg.Authenticated == inf_login_result.SuccessfullyAuthenticated)
                {
                    await SecureStorage.SetAsync("oauth_token", lg.Token);
                    //We have successfully authenticated a the user,
                    //Now fire our OnLoginSuccess Event.
                    DBRepository dBRepository = new DBRepository();
                    dBRepository.DropTable();
                    bool b = await dBRepository.RefreshUserInfoAsync();

                    StartActivity(typeof(HomeActivity));
                    Finish();
                }
                else
                {
                    Toast.MakeText(this, lg.Authenticated.ToString(), ToastLength.Long).Show();
                }
            }
            else
            {
                Toast.MakeText(this, "Empty Username/ Password!", ToastLength.Long).Show();
            }
        }

        private bool IsUserNameValid()
        {
            return !String.IsNullOrEmpty(txtUsername.Text.Trim());
        }

        private bool IsPasswordValid()
        {
            return !String.IsNullOrEmpty(txtPassword.Text.Trim());
        }


    }
}
