using CoreGraphics;
using Foundation;
using System;
using UIKit;
using ZXing.Mobile;

namespace NPCCMobileApplications.iOS
{
    public partial class GenerateBarcodeController : UIViewController
    {
        public GenerateBarcodeController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            NavigationItem.Title = "Generate Barcode";

            var barcodeWriter = new BarcodeWriter
            {
                Format = ZXing.BarcodeFormat.QR_CODE,
                Options = new ZXing.Common.EncodingOptions
                {
                    Width = 280,
                    Height = 280
                }
            };

            var barcode = barcodeWriter.Write("Basel AbuBaker");

            imageBarcode.Image = barcode;

            btnBack.TouchUpInside += BtnBack_TouchUpInside;
        }

        void BtnBack_TouchUpInside(object sender, EventArgs e)
        {
            DismissViewController(true, null);
        }

    }
}