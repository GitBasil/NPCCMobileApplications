// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace NPCCMobileApplications.iOS
{
    [Register ("HomeController")]
    partial class HomeController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnAVScan { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnContScan { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnCustomView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnLogout { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnNormalScan { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView CutomView { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (btnAVScan != null) {
                btnAVScan.Dispose ();
                btnAVScan = null;
            }

            if (btnContScan != null) {
                btnContScan.Dispose ();
                btnContScan = null;
            }

            if (btnCustomView != null) {
                btnCustomView.Dispose ();
                btnCustomView = null;
            }

            if (btnLogout != null) {
                btnLogout.Dispose ();
                btnLogout = null;
            }

            if (btnNormalScan != null) {
                btnNormalScan.Dispose ();
                btnNormalScan = null;
            }

            if (CutomView != null) {
                CutomView.Dispose ();
                CutomView = null;
            }
        }
    }
}