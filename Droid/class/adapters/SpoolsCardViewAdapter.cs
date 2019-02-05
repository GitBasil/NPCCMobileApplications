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
        private readonly List<Spools> _lsObjs;
        private AppCompatActivity _currentContext;
        private Spools _spl;
        private SupportFragment _fragment;
        FrameLayout mFragmentContainer;


        public SpoolsCardViewAdapter(AppCompatActivity currentContext, SupportFragment fragment, List<Spools> lsObjs)
        {
            this._lsObjs = lsObjs;
            _currentContext = currentContext;
            _fragment = fragment;
            mFragmentContainer = currentContext.FindViewById<FrameLayout>(Resource.Id.fragmentContainer);
        }

        //BIND DATA TO VIEWS
        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            _spl = _lsObjs[position];

            MyViewHolder h = holder as MyViewHolder;
            h.lblcSpoolNo.Text ="Spool: " + _lsObjs[position].cSpoolNo;
            h.lbliProjNo.Text = "Project: " + _lsObjs[position].iProjNo.ToString();
            h.lblcISO.Text = "ISO: " + _lsObjs[position].cISO;
            h.btnDetails.Click += BtnDetails_Click;
            h.btnAssign.Click += BtnAssign_Click;
            h.textViewOptions.SetOnClickListener(new ExtraMenuActions(_currentContext, _fragment, mFragmentContainer, _spl.iProjectId, _spl.cTransmittal, _spl.iDrwgSrl));

            ImageService.Instance
                            .LoadUrl(_lsObjs.ToArray()[position].icon)
                            .LoadingPlaceholder("loadingimg", FFImageLoading.Work.ImageSource.CompiledResource)
                            .ErrorPlaceholder("notfound", FFImageLoading.Work.ImageSource.CompiledResource)
                            .Transform(new CircleTransformation())
                            .IntoAsync(h.imageView);
        }

        void BtnAssign_Click(object sender, EventArgs e)
        {
            var dialog = new Assign();
            dialog.Show(_currentContext.FragmentManager, "Assign");
        }


        void BtnDetails_Click(object sender, EventArgs e)
        {
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
