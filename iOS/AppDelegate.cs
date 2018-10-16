using System;

using Foundation;
using UIKit;
using NPCCMobileApplications;
using Xamarin.Essentials;
using System.Threading.Tasks;
using NPCCMobileApplications.Library;

namespace NPCCMobileApplications.iOS
{
    [Register("AppDelegate")]
    public class AppDelegate : UIApplicationDelegate
    {
        public npcc_authentication oauth;

        public override UIWindow Window
        {
            get;
            set;
        }

        //Public property to access our MainStoryboard.storyboard file
        public UIStoryboard MainStoryboard
        {
            get { return UIStoryboard.FromName("Main", NSBundle.MainBundle); }
        }

        //Creates an instance of viewControllerName from storyboard
        public UIViewController GetViewController(UIStoryboard storyboard, string viewControllerName)
        {
            return storyboard.InstantiateViewController(viewControllerName);
        }

        //Sets the RootViewController of the Apps main window with an option for animation.
        public void SetRootViewController(UIViewController rootViewController, bool animate)
        {
            if (animate)
            {
                var transitionType = UIViewAnimationOptions.TransitionFlipFromRight;

                Window.RootViewController = rootViewController;
                UIView.Transition(Window, 0.5, transitionType,
                                  () => Window.RootViewController = rootViewController,
                                  null);
            }
            else
            {
                Window.RootViewController = rootViewController;
            }
        }

        //Override FinishedLaunching. This executes after the app has started.
        public override  bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
        {
            //isAuthenticated can be used for an auto-login feature, you'll have to implement this
            //as you see fit or get rid of the if statement if you want.
            try
            {
                setMainControllerAsync();
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return false;
            }

        }

        public async void setMainControllerAsync(){
            oauth = new npcc_authentication();

            bool isAuth = await oauth.IsAuthenticatedAsync();
            if (isAuth)
            {
                //We are already authenticated, so go to the main tab bar controller;
                var tabBarController = GetViewController(MainStoryboard, "InitialController");
                SetRootViewController(tabBarController, false);
            }
            else
            {
                //User needs to log in, so show the Login View Controlller
                var loginController = GetViewController(MainStoryboard, "LoginViewController") as LoginViewController;
                loginController.OnLoginSuccess += LoginController_OnLoginSuccess;
                SetRootViewController(loginController, false);
            }
        }

        public void LoginController_OnLoginSuccess(object sender, EventArgs e)
        {
            //We have successfully Logged In
            var tabBarController = GetViewController(MainStoryboard, "InitialController");
            SetRootViewController(tabBarController, true);
        }


    }
}

