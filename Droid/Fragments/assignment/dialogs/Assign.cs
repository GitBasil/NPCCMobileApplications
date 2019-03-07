using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using NPCCMobileApplications.Library;
using SearchableSpinner.Droid.Controls;
using static NPCCMobileApplications.Library.npcc_types;

namespace NPCCMobileApplications.Droid
{
    class Assign : DialogFragment
    {
        SpinnerSearch SpnTest;
        Spools _spl;
        View view;
        AppCompatActivity act;
        List<inf_userinfo> lstFabUsers;
        SpoolsCardViewAdapter _ins;
        int _position;
        public Assign(Spools spl, SpoolsCardViewAdapter ins, int position)
        {
            _spl = spl;
            _ins = ins;
            _position = position;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            view = inflater.Inflate(Resource.Layout.Assign, container, false);

            //SET TITLE FOR DIALOG
            this.Dialog.SetTitle("Assign");
            Button btnSubmit = view.FindViewById<Button>(Resource.Id.btnSubmit);
            SpnTest = view.FindViewById<SpinnerSearch>(Resource.Id.spnTest);

            btnSubmit.Click+= BtnSubmit_ClickAsync;
            act = (AppCompatActivity)this.Activity;
            fillFabList();

            return view;
        }

        void fillFabList()
        {
            Task.Run(async () => {
                string url = "https://webapps.npcc.ae/ApplicationWebServices/api/paperless/GetFabricatorsList?iStationId=" + _spl.iStationId;
                lstFabUsers = await npcc_services.inf_CallWebServiceAsync<List<inf_userinfo>, string>(inf_method.Get, url);
            }).ContinueWith(fn => {
                if(lstFabUsers != null)
                act.RunOnUiThread(() => {
                    var items = new List<SpinnerItem>();
                    foreach(inf_userinfo fabUser in lstFabUsers)
                    {
                        items.Add(new SpinnerItem { Name = fabUser.cFullName, Item = fabUser });
                    }

                    SpnTest.SpinnerTitle = "Select user from the list";
                    SpnTest.SetItems(items, -1, null);
                });
            });
        }


        async void BtnSubmit_ClickAsync(object sender, EventArgs e)
        {
            if (SpnTest.GetSelectedItem() != null)
            {
                inf_userinfo fabUser = (inf_userinfo)SpnTest.GetSelectedItem().Item;


                string url = "https://webapps.npcc.ae/ApplicationWebServices/api/paperless/assignfabricator";

                inf_assignment objAssignment = new inf_assignment();
                objAssignment.iAssignmentId = _spl.iAssignmentId;
                objAssignment.cFabricatorUser = fabUser.cUsername;

                int assignRes = await npcc_services.inf_CallWebServiceAsync<int, inf_assignment>(inf_method.Post, url, objAssignment);
                if(assignRes == 1)
                {
                    DBRepository dBRepository = new DBRepository();
                    _spl.cStatus = "F";
                    dBRepository.UpdateSpool(_spl);

                    common_functions.DisplayToast("Task assigned successfully!!", Context);
                } else if (assignRes == 2)
                {
                    DBRepository dBRepository = new DBRepository();
                    _spl.cStatus = "F";
                    dBRepository.UpdateSpool(_spl);

                    _ins._lsObjs.RemoveAt(_position);
                    _ins.NotifyItemRemoved(_position);
                    _ins.NotifyItemRangeChanged(_position, _ins._lsObjs.Count);

                    common_functions.DisplayToast("Task assigned successfully!!", Context);
                }
                else
                {
                    common_functions.DisplayToast("Error occurred while assigning the task, contact system admin!!", Context);
                }


                Assign _exportFragment = (Assign)FragmentManager.FindFragmentByTag("Assign");
                if (_exportFragment != null)
                {
                    _exportFragment.Dismiss();
                }
            }
            else
            {
                common_functions.DisplayToast("You have to select a user!!", Context);
            }
        }

    }
}