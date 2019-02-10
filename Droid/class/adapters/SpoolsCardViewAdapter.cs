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

namespace NPCCMobileApplications.Droid
{
    public class SpoolsCardViewAdapter : RecyclerView.Adapter
    {
        public List<Spools> _lsObjs { get; set; }
        private AppCompatActivity _currentContext;
        private Spools _spl;
        private SupportFragment _fragment;
        FrameLayout mFragmentContainer;
        SpoolsCardViewAdapter _ins;
        npcc_types.inf_assignment_type _assignment_Type;

        public SpoolsCardViewAdapter(AppCompatActivity currentContext, SupportFragment fragment, List<Spools> lsObjs, npcc_types.inf_assignment_type assignment_Type)
        {
            _ins = this;
            this._lsObjs = lsObjs;
            _currentContext = currentContext;
            _fragment = fragment;
            _assignment_Type = assignment_Type;
            mFragmentContainer = currentContext.FindViewById<FrameLayout>(Resource.Id.fragmentContainer);
        }

        //BIND DATA TO VIEWS
        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            _spl = _lsObjs[position];
            MyViewHolder h = holder as MyViewHolder;
            h.lblcSpoolNo.Text ="Spool: " + _spl.cSpoolNo;
            h.lbliProjNo.Text = "Project: " + _spl.iProjNo.ToString();
            h.lblcISO.Text = "ISO: " + _spl.cISO;
            h.btnDetails.Tag = position;
            h.btnDetails.Click += BtnDetails_Click;
            h.btnAssign.Tag = position;
            ImageService.Instance
                            .LoadUrl(_spl.icon)
                            .LoadingPlaceholder("loadingimg", FFImageLoading.Work.ImageSource.CompiledResource)
                            .ErrorPlaceholder("notfound", FFImageLoading.Work.ImageSource.CompiledResource)
                            .Transform(new CircleTransformation())
                            .IntoAsync(h.imageView);
            h.textViewOptions.SetOnClickListener(new ExtraMenuActions(_currentContext, _fragment, mFragmentContainer, _spl.iProjectId, _spl.cTransmittal, _spl.iDrwgSrl));

            switch (_assignment_Type)
            {
                case inf_assignment_type.Pending:
                    h.btnAssign.Click += BtnAssign_Click;
                    break;
                case inf_assignment_type.UnderFabrication:
                    h.btnAssign.Visibility = ViewStates.Gone;
                    break;
                case inf_assignment_type.UnderWelding:
                    h.btnAssign.Visibility = ViewStates.Gone;
                    break;
                case inf_assignment_type.Completed:
                    h.btnAssign.Visibility = ViewStates.Gone;
                    break;
            }



        }

        void BtnAssign_Click(object sender, EventArgs e)
        {
            _spl = _lsObjs[(int)((Button)sender).Tag];
            var dialog = new Assign(_spl, _ins, (int)((Button)sender).Tag);
            dialog.Show(_currentContext.FragmentManager, "Assign");
        }


        void BtnDetails_Click(object sender, EventArgs e)
        {
            _spl = _lsObjs[(int)((Button)sender).Tag];
            showData mshowData = new showData(_spl);
            common_functions.npcc_show_fragment(_currentContext, mFragmentContainer, mshowData, _fragment);
        }



        //INITIALIZE VH
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            //INFLATE LAYOUT TO VIEW
            View v = LayoutInflater.From(_currentContext).Inflate(Resource.Layout.spoolcardView, parent, false);
            MyViewHolder holder = new MyViewHolder(v);

            return holder;
        }

        public override int ItemCount
        {

            get {
                int c = 0;
                if (_lsObjs != null) c = _lsObjs.Count;
                return c; }
        }

        internal class MyViewHolder : RecyclerView.ViewHolder
        {
            public TextView lblcSpoolNo;
            public TextView lbliProjNo;
            public TextView lblcISO;
            public ImageViewAsync imageView;
            public Button btnDetails;
            public Button btnAssign;
            public Button textViewOptions;

            public MyViewHolder(View itemView)
                : base(itemView)
            {
                lblcSpoolNo = itemView.FindViewById<TextView>(Resource.Id.lblcSpoolNo);
                lbliProjNo=itemView.FindViewById<TextView>(Resource.Id.lbliProjNo);
                lblcISO = itemView.FindViewById<TextView>(Resource.Id.lblcISO);
                imageView = itemView.FindViewById<ImageViewAsync>(Resource.Id.imgView);
                btnDetails = itemView.FindViewById<Button>(Resource.Id.btnDetails);
                btnAssign = itemView.FindViewById<Button>(Resource.Id.btnAssign);
                textViewOptions = itemView.FindViewById<Button>(Resource.Id.textViewOptions);
            }

        }
    }
}
