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
    public class SpoolsCardViewAdapter : RecyclerView.Adapter
    {
        public List<Spools> _lsObjs { get; set; }
        private AppCompatActivity _currentContext;
        private Spools _spl;
        private SupportFragment _fragment;
        FrameLayout mFragmentContainer;
        SpoolsCardViewAdapter _ins;
        npcc_types.inf_assignment_type _assignment_Type;
        int assignRes;

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
            h.lblcSpoolNo.Text = "Spool: " + _spl.cSpoolNo;
            h.lblcSpoolSize.Text = "Size: " + _spl.cSpoolSize;
            h.lblcSpoolMaterial.Text = "Material: " + _spl.cSpoolMaterial;
            h.lbliProjNo.Text = "Project: " + _spl.iProjNo.ToString();
            h.lblcISO.Text = "ISO: " + _spl.cISO;
            h.btnDetails.Tag = position;
            h.btnDetails.Click += BtnDetails_Click;
            h.btnAssignment.Tag = position;

            ImageService.Instance
                            .LoadUrl(_spl.icon)
                            .LoadingPlaceholder("loadingimg", FFImageLoading.Work.ImageSource.CompiledResource)
                            .ErrorPlaceholder("notfound", FFImageLoading.Work.ImageSource.CompiledResource)
                            .Transform(new CircleTransformation())
                            .IntoAsync(h.imageView);
            h.textViewOptions.SetOnClickListener(new ExtraMenuActions(_currentContext, _fragment, mFragmentContainer, _spl.iProjectId, _spl.cTransmittal, _spl.iDrwgSrl));

            DBRepository dBRepository = new DBRepository();
            UserInfo user = dBRepository.GetUserInfo();
            switch(user.group)
            {
                case "Foreman":
                    switch (_assignment_Type)
                    {
                        case inf_assignment_type.Pending:
                            h.btnAssignment.Text = "Assign";
                            h.btnAssignment.Click += BtnAssign_Click;
                            break;
                        case inf_assignment_type.UnderFabrication:
                            h.btnAssignment.Text = "Re-Assign";
                            h.btnAssignment.Click += BtnAssign_Click;
                            break;
                        case inf_assignment_type.UnderWelding:
                            h.btnAssignment.Visibility = ViewStates.Gone;
                            break;
                        case inf_assignment_type.Completed:
                            h.btnAssignment.Visibility = ViewStates.Gone;
                            break;
                    }
                    break;
                case "Fabricator":
                    h.btnAssignment.Text = "Complete";
                    h.btnAssignment.Click += BtnAssign_Click;
                    break;
                case "Welder":
                    h.btnAssignment.Text = "Fill Weld Log";
                    h.btnAssignment.Click += BtnAssign_Click;
                    break;
            }
        }

        void BtnAssign_Click(object sender, EventArgs e)
        {
            DBRepository dBRepository = new DBRepository();
            UserInfo user = dBRepository.GetUserInfo();
            switch (user.group)
            {
                case "Foreman":
                    _spl = _lsObjs[(int)((Button)sender).Tag];
                    var dialog = new Assign(_spl, _ins, (int)((Button)sender).Tag);
                    dialog.Show(_currentContext.FragmentManager, "Assign");

                    break;
                case "Fabricator":
                    _spl = _lsObjs[(int)((Button)sender).Tag];
                    //set alert for executing the task
                    Android.App.AlertDialog.Builder alert = new Android.App.AlertDialog.Builder(_currentContext);
                    alert.SetTitle("Confirm Complete");
                    alert.SetMessage("Are you sure you want to complete this task.");
                    alert.SetPositiveButton("Yes", (senderAlert, args) => {
                        string url = "https://webapps.npcc.ae/ApplicationWebServices/api/paperless/FabricatorComplete";

                        inf_assignment objAssignment = new inf_assignment();
                        objAssignment.iAssignmentId = _spl.iAssignmentId;

                        Task.Run(async () => {
                            assignRes = await npcc_services.inf_CallWebServiceAsync<int, inf_assignment>(inf_method.Post, url, objAssignment);
                        }).ContinueWith(fn => {
                            if (assignRes == 1)
                            {
                                _spl.cStatus = "W";
                                dBRepository.UpdateSpool(_spl);
                                _currentContext.RunOnUiThread(() =>
                                {
                                    _ins._lsObjs.RemoveAt((int)((Button)sender).Tag);
                                    _ins.NotifyItemRemoved((int)((Button)sender).Tag);
                                    _ins.NotifyItemRangeChanged((int)((Button)sender).Tag, _ins._lsObjs.Count);
                                });
                                common_functions.DisplayToast("Task assigned successfully!!", _currentContext);
                            }
                            else
                            {
                                common_functions.DisplayToast("Error occurred while assigning the task, contact system admin!!", _currentContext);
                            }
                        });


                    });

                    alert.SetNegativeButton("No", (senderAlert, args) => {
                    });

                    Dialog d = alert.Create();
                    d.Show();

                    break;
            }
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
            public TextView lblcSpoolSize;
            public TextView lblcSpoolMaterial;
            public TextView lbliProjNo;
            public TextView lblcISO;
            public ImageViewAsync imageView;
            public Button btnDetails;
            public Button btnAssignment;
            public Button textViewOptions;

            public MyViewHolder(View itemView)
                : base(itemView)
            {
                lblcSpoolNo = itemView.FindViewById<TextView>(Resource.Id.lblcSpoolNo);
                lblcSpoolSize = itemView.FindViewById<TextView>(Resource.Id.lblcSpoolSize);
                lblcSpoolMaterial = itemView.FindViewById<TextView>(Resource.Id.lblcSpoolMaterial);
                lbliProjNo=itemView.FindViewById<TextView>(Resource.Id.lbliProjNo);
                lblcISO = itemView.FindViewById<TextView>(Resource.Id.lblcISO);
                imageView = itemView.FindViewById<ImageViewAsync>(Resource.Id.imgView);
                btnDetails = itemView.FindViewById<Button>(Resource.Id.btnDetails);
                btnAssignment = itemView.FindViewById<Button>(Resource.Id.btnAssignment);
                textViewOptions = itemView.FindViewById<Button>(Resource.Id.textViewOptions);
            }

        }
    }
}
