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

namespace NPCCMobileApplications.Droid
{
    public class JointsViewAdapter : RecyclerView.Adapter
    {
        public List<SpoolJoints> _lsObjs { get; set; }
        private AppCompatActivity _currentContext;
        private SpoolJoints _splJ;
        private SupportFragment _fragment;
        FrameLayout mFragmentContainer;
        JointsViewAdapter _ins;

        public JointsViewAdapter(AppCompatActivity currentContext, SupportFragment fragment, List<SpoolJoints> lsObjs)
        {
            _ins = this;
            this._lsObjs = lsObjs;
            _currentContext = currentContext;
            _fragment = fragment;
            mFragmentContainer = currentContext.FindViewById<FrameLayout>(Resource.Id.fragmentContainer);
        }

        //BIND DATA TO VIEWS
        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            var lstWPS = new List<SpinnerItem>();
            var lstRH = new List<SpinnerItem>();
            var lstFC = new List<SpinnerItem>();
            for (int i = 0; i < 30; i++)
            {
                lstWPS.Add(new SpinnerItem { Id = i, Name = "WPS " + i });
                lstRH.Add(new SpinnerItem { Id = i, Name = "RH " + i });
                lstFC.Add(new SpinnerItem { Id = i, Name = "FC " + i });
            }

            _splJ = _lsObjs[position];
            MyViewHolder h = holder as MyViewHolder;
            h.lblJointNo.Text = _splJ.iJointNo.ToString() + "/" + _splJ.cJointSuffix.Trim() + "/" + _splJ.cCreatedFor.Trim();
            h.lblClass.Text = _splJ.cClass;
            h.lblDia.Text = _splJ.rDia.ToString();
            h.lblJiontThk.Text = _splJ.rJiontThk.ToString();
            h.btnDeSelect.Visibility = ViewStates.Invisible;

            h.btnDeSelect.Click += (sender, e) => {
                h.SpnWPS.SetItems(h.SpnWPS.Items.Select(c => { c.IsSelected = false; return c; }).ToList(), -1, null);
            };
           //_splJ.rJiontThk

            //_splJ.cClass



            h.SpnWPS.SpinnerTitle = "Select WPS";
            h.SpnWPS.DefaultText = "Select WPS";
            h.SpnWPS.SetItems(lstWPS, -1, null);
            h.SpnWPS.Tag = position;
            h.SpnWPS.ItemSelected += (object sender, AdapterView.ItemSelectedEventArgs e) =>
            {
                if(((SpinnerSearch)sender).GetSelectedItem() != null)
                {
                    _lsObjs[(int)((SpinnerSearch)sender).Tag].cWPSCode = ((SpinnerSearch)sender).GetSelectedItem().Name;
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

                h.SpnMultiFC.SetItems(h.SpnMultiFC.Items.Select(c => { c.IsSelected = false; return c; }).ToList(), null);
                h.SpnMultiRH.SetItems(h.SpnMultiRH.Items.Select(c => { c.IsSelected = false; return c; }).ToList(), null);
            };

            h.SpnMultiRH.SpinnerTitle = "Select RH Welders";
            h.SpnMultiRH.DefaultText = "Select RH Welders";
            h.SpnMultiRH.SetItems(lstRH, null);
            h.SpnMultiRH.Tag = position;
            h.SpnMultiRH.Visibility = ViewStates.Invisible;
            h.SpnMultiRH.ItemSelected += (object sender, AdapterView.ItemSelectedEventArgs e) =>
            {
                if (((MultiSpinnerSearch)sender).GetSelectedItems().Any())
                {
                    _lsObjs[(int)((MultiSpinnerSearch)sender).Tag].cRHWelders = ((MultiSpinnerSearch)sender).GetSelectedItems().Select(r => r.Name).ToList();
                }
            };

            h.SpnMultiFC.SpinnerTitle = "Select FC Welders";
            h.SpnMultiFC.DefaultText = "Select FC Welders";
            h.SpnMultiFC.SetItems(lstFC, null);
            h.SpnMultiFC.Tag = position;
            h.SpnMultiFC.Visibility = ViewStates.Invisible;
            h.SpnMultiFC.ItemSelected += (object sender, AdapterView.ItemSelectedEventArgs e) =>
            {
                if (((MultiSpinnerSearch)sender).GetSelectedItems().Any())
                {
                    _lsObjs[(int)((MultiSpinnerSearch)sender).Tag].cFCWelders = ((MultiSpinnerSearch)sender).GetSelectedItems().Select(r => r.Name).ToList();
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
            public TextView lblJiontThk;
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
                lblJiontThk = itemView.FindViewById<TextView>(Resource.Id.lblJiontThk);
                SpnWPS = itemView.FindViewById<SpinnerSearch>(Resource.Id.spnWPS);
                SpnMultiRH = itemView.FindViewById<MultiSpinnerSearch>(Resource.Id.spnMultRH);
                SpnMultiFC = itemView.FindViewById<MultiSpinnerSearch>(Resource.Id.spnMultFC);
                btnDeSelect = itemView.FindViewById<ImageButton>(Resource.Id.btnDeSelect);
            }

        }
    }
}
