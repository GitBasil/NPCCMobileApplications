using Foundation;
using System;
using UIKit;
using Xamarin.SideMenu;

namespace NPCCMobileApplications.iOS
{
    public partial class SideMenuController : UIViewController
    {
        public SideMenuController(IntPtr handle) : base(handle)
        {
        }

        SideMenuManager _sideMenuManager;

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            btnSideButton.TouchUpInside += BtnSideButton_TouchUpInside;

            this.NavigationItem.SetLeftBarButtonItem(
                new UIBarButtonItem("Left Menu", UIBarButtonItemStyle.Plain, (sender, e) => {
                    PresentViewController(_sideMenuManager.LeftNavigationController, true, null);
                }),
                false);

            this.NavigationItem.SetRightBarButtonItem(
                new UIBarButtonItem("Right Menu", UIBarButtonItemStyle.Plain, (sender, e) => {
                    PresentViewController(_sideMenuManager.RightNavigationController, true, null);
                }),
                false);

            _sideMenuManager = new SideMenuManager();

            Title = "Side Menu";

            SetupSideMenu();

            SetDefaults();
        }

        void SetupSideMenu()
        {
            _sideMenuManager.LeftNavigationController = new UISideMenuNavigationController(_sideMenuManager, new SampleTableView(), leftSide: true);
            _sideMenuManager.RightNavigationController = new UISideMenuNavigationController(_sideMenuManager, new SampleTableView(), leftSide: false);
            //_sideMenuManager.AddScreenEdgePanGesturesToPresent(toView: this.NavigationController?.View);

            // Set up a cool background image for demo purposes
            _sideMenuManager.AnimationBackgroundColor = UIColor.FromPatternImage(UIImage.FromFile("stars.png"));
        }

        void SetDefaults()
        {
            _sideMenuManager.BlurEffectStyle = UIBlurEffectStyle.ExtraLight;
            _sideMenuManager.AnimationFadeStrength = .6f;
            _sideMenuManager.ShadowOpacity = .6f;
            _sideMenuManager.AnimationTransformScaleFactor = .6f;
            _sideMenuManager.FadeStatusBar = true;
            _sideMenuManager.PresentMode = SideMenuManager.MenuPresentMode.MenuDissolveIn;
            _sideMenuManager.FadeStatusBar = true;
            _sideMenuManager.BlurEffectStyle = default(UIBlurEffectStyle);
            _sideMenuManager.AnimationFadeStrength = .6f;
            _sideMenuManager.ShadowOpacity = .6f;
            _sideMenuManager.MenuWidth = this.View.Frame.Width * .6f;
            _sideMenuManager.AnimationTransformScaleFactor = 1f;
            _sideMenuManager.FadeStatusBar = true;
        }

        void BtnSideButton_TouchUpInside(object sender, EventArgs e)
        {
            PresentViewController(_sideMenuManager.LeftNavigationController, true, null);
        }

    }
}