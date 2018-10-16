using Foundation;
using System;
using UIKit;

namespace NPCCMobileApplications.iOS
{
    public partial class SplashScreenController : UIViewController
    {
        public SplashScreenController(IntPtr handle) : base(handle)
        {
        }

        UIActivityIndicatorView Indtr = new UIActivityIndicatorView();
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            Indtr.Center = this.View.Center;
            Indtr.HidesWhenStopped = true;
            Indtr.ActivityIndicatorViewStyle = UIActivityIndicatorViewStyle.Gray;
            View.AddSubview(Indtr);
            Indtr.StartAnimating();
        }
    }
}