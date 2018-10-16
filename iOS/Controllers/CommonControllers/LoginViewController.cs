using Foundation;
using NPCCMobileApplications.Library;
using SVProgressHUDBinding;
using System;
using UIKit;
using Xamarin.Essentials;
using static NPCCMobileApplications.Library.npcc_types;

namespace NPCCMobileApplications.iOS
{
    public partial class LoginViewController : UIViewController
    {
        //Create an event when a authentication is successful
        public event EventHandler OnLoginSuccess;

        public LoginViewController(IntPtr handle) : base(handle) { }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            try
            {
                NavigationItem.Title = "Login to NPCC";

                txtPassword.AttributedPlaceholder = new NSAttributedString("Enter Your Password", null, UIColor.White);
                txtUsername.AttributedPlaceholder = new NSAttributedString("Enter Your Username", null, UIColor.White);



                btnLogin.TouchUpInside += BtnLogin_TouchUpInsideAsync;
                View.EndEditing(true);

                txtPassword.ShouldReturn += TxtPassword_ShouldReturn;
            }
            catch (Exception ex)
            {
                npcc_services.inf_mobile_exception_managerAsync(ex.Message);
            }
        }

        async void BtnLogin_TouchUpInsideAsync(object sender, EventArgs e)
        {
            try
            {
                txtPassword.ResignFirstResponder();
                //Validate our Username & Password.
                if (IsUserNameValid() && IsPasswordValid())
                {
                    SVProgressHUD.SetDefaultMaskType(SVProgressHUDMaskType.Black);
                    SVProgressHUD.ShowWithStatus("Checking Your Details...");

                    npcc_authentication oauth = new npcc_authentication();
                    inf_login_info lg = await oauth.Login(txtUsername.Text, txtPassword.Text);

                    SVProgressHUD.Dismiss();
                    SVProgressHUD.SetDefaultMaskType(SVProgressHUDMaskType.None);
                    //We have successfully authenticated a the user,
                    //Now fire our OnLoginSuccess Event.
                    if (lg.Authenticated == inf_login_result.SuccessfullyAuthenticated)
                    {
                        await SecureStorage.SetAsync("oauth_token", lg.Token);
                        //We have successfully authenticated a the user,
                        //Now fire our OnLoginSuccess Event.
                        if (OnLoginSuccess != null)
                        {
                            OnLoginSuccess(sender, new EventArgs());
                        }
                    }
                    else
                    {
                        SVProgressHUD.ShowErrorWithStatus(lg.Authenticated.ToString());
                        SVProgressHUD.DismissWithDelay(10);

                    }
                }
                else
                {
                    SVProgressHUD.ShowErrorWithStatus("Empty Username/ Password!");
                    SVProgressHUD.DismissWithDelay(10);
                }
            }
            catch (Exception ex)
            {
                npcc_services.inf_mobile_exception_managerAsync(ex.Message);
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

        public override void TouchesBegan(NSSet touches, UIEvent evt)
        {
            View.EndEditing(true);
        }

        public bool TxtPassword_ShouldReturn(UITextField textField)
        {
            textField.ResignFirstResponder();
            btnLogin.SendActionForControlEvents(UIControlEvent.TouchUpInside);
            return true;
        }
    }
}