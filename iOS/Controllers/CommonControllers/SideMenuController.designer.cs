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
    [Register ("SideMenuController")]
    partial class SideMenuController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnSideButton { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (btnSideButton != null) {
                btnSideButton.Dispose ();
                btnSideButton = null;
            }
        }
    }
}