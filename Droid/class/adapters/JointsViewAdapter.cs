using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using NPCCMobileApplications.Library;
using SupportFragment = Android.Support.V4.App.Fragment;
using SearchableSpinner.Droid.Controls;
using System.Linq;
using Android.Graphics;
using System.Threading.Tasks;
using static NPCCMobileApplications.Library.npcc_types;

namespace NPCCMobileApplications.Droid
{
    public class JointsViewAdapter : RecyclerView.Adapter
    {
        public List<SpoolJoints> _lsObjs { get; set; }
        private AppCompatActivity _currentContext;
        private SpoolJoints _splJ;
        private string _dWeld;
        private SupportFragment _fragment;
        FrameLayout mFragmentContainer;
        public JointsViewAdapter _ins;
        List<inf_JointWPS> lstJointWPS;
        List<inf_JointWelder> lstJointWelders;

        public JointsViewAdapter(AppCompatActivity currentContext, SupportFragment fragment, List<SpoolJoints> lsObjs, string dWeld)
        {
            _ins = this;
            this._lsObjs = lsObjs;
            _currentContext = currentContext;
            _fragment = fragment;
            mFragmentContainer = currentContext.FindViewById<FrameLayout>(Resource.Id.fragmentContainer);
            _dWeld = dWeld;
        }



        //BIND DATA TO VIEWS
        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            _splJ = _lsObjs[position];
            MyViewHolder h = holder as MyViewHolder;
            h.lblJointNo.Text = _splJ.iJointNo.ToString() + _splJ.cJointSuffix.Trim() + _splJ.cCreatedFor.Trim();
            h.lblClass.Text = _splJ.cClass;
            h.lblDia.Text = _splJ.rDia.ToString();
            h.lblJointThk.Text = _splJ.rJointThk.ToString();
            h.btnDeSelect.Visibility = ViewStates.Invisible;
            h.btnDeSelect.Tag = position;

            h.btnDeSelect.Click += (sender, e) => {
                h.SpnWPS.SetItems(h.SpnWPS.Items.Select(c => { c.IsSelected = false; return c; }).ToList(), -1, null);
                _lsObjs[(int)((ImageButton)sender).Tag].cWPSCode = null;
                 _lsObjs[(int)((ImageButton)sender).Tag].cFCWelders = null;
                 _lsObjs[(int)((ImageButton)sender).Tag].cRHWelders = null;
            };

            bindWPSData(h, position);
        }

        void bindWPSData(MyViewHolder h, int position)
        {
            h.SpnWPS.SpinnerTitle = "Select WPS";
            h.SpnWPS.DefaultText = "Select WPS";

            Task.Run(async () => {
                h.SpnWPS.Enabled = false;
                string url = "https://webapps.npcc.ae/ApplicationWebServices/api/paperless/GetWPSList?iProjectId=" + _splJ.Spool.iProjectId + "&cJointType=" + _splJ.cJointType.Trim() + "&cClasses=" + _splJ.cClass.Trim() + "&rJointThk=" + _splJ.rJointThk + "&rDia=" + _splJ.rDia;
                lstJointWPS = await npcc_services.inf_CallWebServiceAsync<List<inf_JointWPS>, string>(inf_method.Get, url);
            }).ContinueWith(fn => {
                if (lstJointWPS != null)
                {
                    _currentContext.RunOnUiThread(() => {
                        var items = new List<SpinnerItem>();
                        foreach (inf_JointWPS objWPS in lstJointWPS)
                        {
                            items.Add(new SpinnerItem { Name = "WPS:" + objWPS.cWPSCode + ", Weld-Type:" + objWPS.cWeldType + ", Qual.:" + objWPS.cQualRefDesc.Trim(), Item = objWPS });
                        }

                        h.SpnWPS.SetItems(items, -1, null);
                    });
                    h.SpnWPS.Enabled = true;
                    h.SpnWPS.Visibility = ViewStates.Visible;
                }
            });

            h.SpnWPS.Tag = position;
            h.SpnWPS.ItemSelected += (object sender, AdapterView.ItemSelectedEventArgs e) =>
            {
                if (((SpinnerSearch)sender).GetSelectedItem() != null)
                {
                    _lsObjs[(int)((SpinnerSearch)sender).Tag].cWPSCode = ((inf_JointWPS)((SpinnerSearch)sender).GetSelectedItem().Item).cWPSCode;

                    bindWeldersData(h, position, (inf_JointWPS)((SpinnerSearch)sender).GetSelectedItem().Item);

                    h.SpnMultiFC.Visibility = ViewStates.Visible;
                    h.SpnMultiRH.Visibility = ViewStates.Visible;
                    h.btnDeSelect.Visibility = ViewStates.Visible;
                }
                else
                {
                    h.SpnMultiFC.Visibility = ViewStates.Invisible;
                    h.SpnMultiRH.Visibility = ViewStates.Invisible;
                    h.btnDeSelect.Visibility = ViewStates.Invisible;
                }
                if(h.SpnMultiFC.Items != null)
                    h.SpnMultiFC.SetItems(h.SpnMultiFC.Items.Select(c => { c.IsSelected = false; return c; }).ToList(), null);

                if (h.SpnMultiRH.Items != null)
                    h.SpnMultiRH.SetItems(h.SpnMultiRH.Items.Select(c => { c.IsSelected = false; return c; }).ToList(), null);
            };
        }

        void bindWeldersData(MyViewHolder h, int position, inf_JointWPS objWPS)
        {
            Task.Run(async () => {
                h.SpnMultiFC.Enabled = false;
                h.SpnMultiRH.Enabled = false;
                string url = "https://webapps.npcc.ae/ApplicationWebServices/api/paperless/GetWeldersList?iProjectId=" + _splJ.Spool.iProjectId + "&cJointType=" + _splJ.cJointType.Trim() + "&rDia=" + _splJ.rDia + "&cQualRefKey=" + objWPS.cQualRefKey.Trim() + "&dWeld=" + _dWeld;
                lstJointWelders = await npcc_services.inf_CallWebServiceAsync<List<inf_JointWelder>, string>(inf_method.Get, url);
            }).ContinueWith(fn => {
                if (lstJointWelders != null)
                {
                    _currentContext.RunOnUiThread(() => {
                        var RHitems = new List<SpinnerItem>();
                        var FCitems = new List<SpinnerItem>();
                        foreach (inf_JointWelder objWelder in lstJointWelders)
                        {
                            RHitems.Add(new SpinnerItem { Name = "(#" + objWelder.cBadgeNo.Trim() + ")" + objWelder.cName.Trim(), Item = objWelder });
                            FCitems.Add(new SpinnerItem { Name = "(#" + objWelder.cBadgeNo.Trim() + ")" + objWelder.cName.Trim(), Item = objWelder });
                        }

                        h.SpnMultiRH.SetItems(RHitems, null);
                        h.SpnMultiFC.SetItems(FCitems, null);
                    });

                    h.SpnMultiFC.Enabled = true;
                    h.SpnMultiRH.Enabled = true;
                }
            });

            h.SpnMultiRH.SpinnerTitle = "Select RH Welders";
            h.SpnMultiRH.DefaultText = "Select RH Welders";
            h.SpnMultiRH.Tag = position;
            h.SpnMultiRH.Visibility = ViewStates.Invisible;
            h.SpnMultiRH.ItemSelected += (object sender, AdapterView.ItemSelectedEventArgs e) =>
            {
                if (((MultiSpinnerSearch)sender).GetSelectedItems().Any())
                {
                    _lsObjs[(int)((MultiSpinnerSearch)sender).Tag].cRHWelders = ((MultiSpinnerSearch)sender).GetSelectedItems().Select(r => ((inf_JointWelder)r.Item).cBadgeNo).ToList();
                }
            };

            h.SpnMultiFC.SpinnerTitle = "Select FC Welders";
            h.SpnMultiFC.DefaultText = "Select FC Welders";
            h.SpnMultiFC.Tag = position;
            h.SpnMultiFC.Visibility = ViewStates.Invisible;
            h.SpnMultiFC.ItemSelected += (object sender, AdapterView.ItemSelectedEventArgs e) =>
            {
                if (((MultiSpinnerSearch)sender).GetSelectedItems().Any())
                {
                    _lsObjs[(int)((MultiSpinnerSearch)sender).Tag].cFCWelders = ((MultiSpinnerSearch)sender).GetSelectedItems().Select(r => ((inf_JointWelder)r.Item).cBadgeNo).ToList();
                }
            };
        }

        //INITIALIZE VH
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            //INFLATE LAYOUT TO VIEW
            View v = LayoutInflater.From(_currentContext).Inflate(Resource.Layout.JointCardView, parent, false);
            MyViewHolder holder = new MyViewHolder(v);

            return holder;
        }

        public override int ItemCount
        {

            get
            {
                int c = 0;
                if (_lsObjs != null) c = _lsObjs.Count;
                return c;
            }
        }

        internal class MyViewHolder : RecyclerView.ViewHolder
        {
            public TextView lblJointNo;
            public TextView lblClass;
            public TextView lblDia;
            public TextView lblJointThk;
            public SpinnerSearch SpnWPS;
            public MultiSpinnerSearch SpnMultiRH;
            public MultiSpinnerSearch SpnMultiFC;
            public ImageButton btnDeSelect;

            public MyViewHolder(View itemView)
                : base(itemView)
            {
                lblJointNo = itemView.FindViewById<TextView>(Resource.Id.lblJoint);
                lblClass = itemView.FindViewById<TextView>(Resource.Id.lblClass);
                lblDia = itemView.FindViewById<TextView>(Resource.Id.lblDia);
                lblJointThk = itemView.FindViewById<TextView>(Resource.Id.lblJointThk);
                SpnWPS = itemView.FindViewById<SpinnerSearch>(Resource.Id.spnWPS);
                SpnMultiRH = itemView.FindViewById<MultiSpinnerSearch>(Resource.Id.spnMultRH);
                SpnMultiFC = itemView.FindViewById<MultiSpinnerSearch>(Resource.Id.spnMultFC);
                btnDeSelect = itemView.FindViewById<ImageButton>(Resource.Id.btnDeSelect);
            }

        }
    }
}
