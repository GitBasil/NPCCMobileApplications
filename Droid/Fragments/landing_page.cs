
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using SearchableSpinner.Droid.Controls;

namespace NPCCMobileApplications.Droid
{
    public class landing_page : Android.Support.V4.App.Fragment
    {
        LayoutInflater InflaterMain;
        SpinnerSearch SpnTest;
        MultiSpinnerSearch SpnMultiTest;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            InflaterMain = inflater;
            View view = inflater.Inflate(Resource.Layout.landing_page, container, false);


            var items = new List<SpinnerItem>();
            for (int i = 0; i < 30; i++)
            {
                items.Add(new SpinnerItem { Id = i, Name = "Item " + i });
            }
            SpnTest = view.FindViewById<SpinnerSearch>(Resource.Id.spnTest);
            SpnTest.SpinnerTitle = "Selecione Um Item";
            SpnTest.SetItems(items, -1, null);

            var items2 = new List<SpinnerItem>();
            for (int i = 0; i < 30; i++)
            {
                items2.Add(new SpinnerItem { Id = i, Name = "Item " + i });
            }
            SpnMultiTest = view.FindViewById<MultiSpinnerSearch>(Resource.Id.spnMultTest);
            SpnMultiTest.SpinnerTitle = "Selecione";
            SpnMultiTest.SetItems(items2, null);

            return view;
        }
    }
}
