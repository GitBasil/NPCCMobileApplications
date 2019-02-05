using System;
using System.Collections.Generic;
using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using SearchableSpinner.Droid.Controls;

namespace NPCCMobileApplications.Droid
{
    class Assign : DialogFragment
    {
        SpinnerSearch SpnTest;
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.Assign, container, false);

            //SET TITLE FOR DIALOG
            this.Dialog.SetTitle("Assign");
            Button btnSubmit = view.FindViewById<Button>(Resource.Id.btnSubmit);
            btnSubmit.Click+= BtnSubmit_Click;

            var items = new List<SpinnerItem>();
            for (int i = 0; i < 30; i++)
            {
                items.Add(new SpinnerItem { Id = i, Name = "Item " + i });
            }
            SpnTest = view.FindViewById<SpinnerSearch>(Resource.Id.spnTest);
            SpnTest.SpinnerTitle = "Selecione Um Item";
            SpnTest.SetItems(items, -1, null);

            return view;
        }

        void BtnSubmit_Click(object sender, EventArgs e)
        {
            Assign _exportFragment = (Assign)FragmentManager.FindFragmentByTag("Assign");
            if (_exportFragment != null)
            {
                _exportFragment.Dismiss();
            }
        }

    }
}