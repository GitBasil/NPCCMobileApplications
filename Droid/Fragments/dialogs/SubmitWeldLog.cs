using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;
using NPCCMobileApplications.Library;
using SearchableSpinner.Droid.Controls;
using static NPCCMobileApplications.Library.npcc_types;

namespace NPCCMobileApplications.Droid
{
    class SubmitWeldLog : DialogFragment
    {
        List<SpoolJoints> _splJoint;
        View view;
        AppCompatActivity act;
        SubmitWeldLog _exportFragment;
        Button btnSubmit;
        EditText txtWeldLogNo;
        inf_ReturnStatus status;
        JointsViewAdapter _jointsView;
        public SubmitWeldLog(List<SpoolJoints> splJoint, JointsViewAdapter jointsView)
        {
            _splJoint = splJoint;
            _jointsView = jointsView;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            view = inflater.Inflate(Resource.Layout.SubmitWeldLog, container, false);

            //SET TITLE FOR DIALOG
            this.Dialog.SetTitle("SubmitWeldLog");
            btnSubmit = view.FindViewById<Button>(Resource.Id.btnSubmit);
            txtWeldLogNo = view.FindViewById<EditText>(Resource.Id.txtWeldLogNo);
            _exportFragment = (SubmitWeldLog)FragmentManager.FindFragmentByTag("SubmitWeldLog");

            btnSubmit.Click += BtnSubmit_Click;
            act = (AppCompatActivity)this.Activity;

            return view;
        }

        void BtnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                var lstFJ = _splJoint.Where((arg) => arg.cWPSCode != null).ToList();

                if (lstFJ.Any())
                {
                    int WldrFilled = lstFJ.Where(c => c.cRHWelders == null && c.cFCWelders == null).Count();
                    if (WldrFilled == 0)
                    {
                        if (txtWeldLogNo.Text.Trim() != "")
                        {
                            decimal iWeldLogNo = Convert.ToDecimal(txtWeldLogNo.Text.Trim());
                            Task.Run(async () => {
                                string url = "https://webapps.npcc.ae/ApplicationWebServices/api/paperless/FillWeldLog";
                                List<inf_SpoolJoints> lsFinal = lstFJ.Select(c => new inf_SpoolJoints { cProjType = c.cProjType, iProjYear = c.iProjYear, iProjNo = c.iProjNo, cProjSuffix = c.cProjSuffix, iDrwgSrl = c.iDrwgSrl, iSubDrwgSrl = c.iSubDrwgSrl, iJointNo = c.iJointNo, iJointSerial = c.iJointSerial, cJointSuffix = c.cJointSuffix, cCreatedFor = c.cCreatedFor, cJointType = c.cJointType, cClass = c.cClass, rDia = c.rDia, rLength = c.rLength, rJointThk = c.rJointThk, cWPSCode = c.cWPSCode, iWeldLogNo = iWeldLogNo, dWeld = c.dWeld, cRHWelders = c.cRHWelders, cFCWelders = c.cFCWelders, cJointAreaCode = c.cJointAreaCode, cMatType=c.cMatType }).ToList();
                                Console.WriteLine(JsonConvert.SerializeObject(lsFinal));
                                 status = await npcc_services.inf_CallWebServiceAsync<inf_ReturnStatus, List< inf_SpoolJoints>>(inf_method.Post, url, lsFinal);
                            }).ContinueWith(fn => {
                                act.RunOnUiThread(() => {
                                    if (status != null && status.status)
                                    {
                                        DBRepository dBRepository = new DBRepository();
                                        dBRepository.DeleteSpoolJoints(lstFJ);

                                        _jointsView._lsObjs.RemoveAll(c => lstFJ.Contains(c));
                                        _jointsView.NotifyDataSetChanged();

                                        if (_exportFragment != null)
                                        {
                                            _exportFragment.Dismiss();
                                        }
                                        common_functions.DisplayToast("Weld log filled successfully!!", Context);
                                    }
                                    else
                                    {
                                        if (_exportFragment != null)
                                        {
                                            _exportFragment.Dismiss();
                                        }
                                        string msg;
                                        if (status == null)
                                            msg = "API request error, Please contact the administrator!!";
                                        else
                                            msg = status.msg;

                                        common_functions.DisplayToast(msg, Context);
                                    }
                                });
                            });
                        }
                        else
                        {
                            common_functions.DisplayToast("Please fill the weld log number!!", Context);
                        }
                    }
                    else
                    {
                        if (_exportFragment != null)
                        {
                            _exportFragment.Dismiss();
                        }
                        common_functions.DisplayToast("You have to select at least one welder for each joint!!", Context);
                    }

                }
                else
                {
                    if (_exportFragment != null)
                    {
                        _exportFragment.Dismiss();
                    }
                    common_functions.DisplayToast("You have to fill at least one joint!!", Context);
                }
            }
            catch (Exception ex)
            {
                npcc_services.inf_mobile_exception_managerAsync(ex.Message);
            }
        }

    }
}