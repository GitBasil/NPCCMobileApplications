using System;
using Android.Views;
using Android.Support.V7.App;
using SupportFragment = Android.Support.V4.App.Fragment;
using Android.Widget;
using static Android.Views.View;
using static NPCCMobileApplications.Library.npcc_types;

namespace NPCCMobileApplications.Droid
{
    public class ExtraMenuActions : Java.Lang.Object, IOnClickListener
    {
        AppCompatActivity _currentContext;
        SupportFragment _fragment;
        FrameLayout _mFragmentContainer;
        int _iProjectId;
        string _cTransmittal;
        int _iDrwgSrl;
        public ExtraMenuActions(AppCompatActivity currentContext, SupportFragment fragment, FrameLayout mFragmentContainer, int iProjectId, string cTransmittal, int iDrwgSrl)
        {
            _currentContext = currentContext;
            _fragment = fragment;
            _mFragmentContainer = mFragmentContainer;
            _iDrwgSrl = iDrwgSrl;
            _iProjectId = iProjectId;
            _cTransmittal = cTransmittal;
        }
        void IOnClickListener.OnClick(View v)
        {
            Android.Widget.PopupMenu menu = new Android.Widget.PopupMenu(v.Context, v);
            menu.Menu.Add(Menu.None, 0, 0, "ISO & Control Sheet");

            menu.MenuItemClick += Menu_MenuItemClick;

            menu.Show();
        }


        void Menu_MenuItemClick(object sender, Android.Widget.PopupMenu.MenuItemClickEventArgs e)
        {
            string menuItemName = e.Item.TitleFormatted.ToString();
            switch (menuItemName)
            {
                case "ISO & Control Sheet":
                    Pdfview_test mPDF = new Pdfview_test("http://webapps.npcc.ae/ApplicationWebServices/api/paperless/GetPDF?Type=" + inf_pdf_type.ISO + "&iProjectId=" + _iProjectId + "&cTransmittal=" + _cTransmittal + "&iDrwgSrl=" + _iDrwgSrl);
                    Console.WriteLine("#############################");
                    Console.WriteLine("http://webapps.npcc.ae/ApplicationWebServices/api/paperless/GetPDF?Type=" + inf_pdf_type.ISO + "&iProjectId=" + _iProjectId + "&cTransmittal=" + _cTransmittal + "&iDrwgSrl=" + _iDrwgSrl);
                    common_functions.npcc_show_fragment(_currentContext, _mFragmentContainer, mPDF, _fragment);
                    break;
            }

        }

    }
}
