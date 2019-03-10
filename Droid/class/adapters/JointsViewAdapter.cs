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
using FFImageLoading;
using FFImageLoading.Transformations;
using FFImageLoading.Views;
using NPCCMobileApplications.Library;
using static Android.Views.View;
using static NPCCMobileApplications.Library.npcc_types;
using SupportFragment = Android.Support.V4.App.Fragment;
using LayoutParams = Android.Views.ViewGroup.LayoutParams;
using System.Threading.Tasks;

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
            _splJ = _lsObjs[position];
            MyViewHolder h = holder as MyViewHolder;
            h.lblcSpoolNo.Text = "iJointNo: " + _splJ.iJointNo.ToString();
            h.lblcSpoolSize.Text = "cCreatedFor: " + _splJ.cCreatedFor;
            h.lblcSpoolMaterial.Text = "cJointSuffix: " + _splJ.cJointSuffix;
            h.lbliProjNo.Text = "cWPSCode: " + _splJ.cWPSCode;
            h.lblcISO.Text = "rDia: " + _splJ.rDia.ToString();
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
            public TextView lblcSpoolNo;
            public TextView lblcSpoolSize;
            public TextView lblcSpoolMaterial;
            public TextView lbliProjNo;
            public TextView lblcISO;

            public MyViewHolder(View itemView)
                : base(itemView)
            {
                lblcSpoolNo = itemView.FindViewById<TextView>(Resource.Id.lblcSpoolNo);
                lblcSpoolSize = itemView.FindViewById<TextView>(Resource.Id.lblcSpoolSize);
                lblcSpoolMaterial = itemView.FindViewById<TextView>(Resource.Id.lblcSpoolMaterial);
                lbliProjNo = itemView.FindViewById<TextView>(Resource.Id.lbliProjNo);
                lblcISO = itemView.FindViewById<TextView>(Resource.Id.lblcISO);
            }

        }
    }
}
