using System;
using Android.OS;
using Android.Views;
using Android.Support.V7.App;
using SupportToolbar = Android.Support.V7.Widget.Toolbar;
using SupportFragment = Android.Support.V4.App.Fragment;
using Android.Widget;
using Android.Graphics;
using NPCCMobileApplications.Library;
using Com.Bumptech.Glide;
using Com.Bumptech.Glide.Request;

namespace NPCCMobileApplications.Droid
{
    public class showData : SupportFragment
    {
        LayoutInflater InflaterMain;
        SupportToolbar mToolbar;
        AppCompatActivity act;
        FrameLayout mFragmentContainer;
        Spools _spl;
        ListView SpoolItemListView;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            HasOptionsMenu = true;
        }

        public showData(Spools spl)
        {
            _spl = spl;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            InflaterMain = inflater;
            View view = inflater.Inflate(Resource.Layout.showData, container, false);

            mFragmentContainer = this.Activity.FindViewById<FrameLayout>(Resource.Id.fragmentContainer);

            act = (AppCompatActivity)this.Activity;

            mToolbar = act.FindViewById<SupportToolbar>(Resource.Id.toolbar);
            act.SetSupportActionBar(mToolbar);

            act.SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            mToolbar.NavigationIcon.SetColorFilter(Color.ParseColor("#FFFFFF"), PorterDuff.Mode.SrcAtop);

            SpoolItemListView = view.FindViewById<ListView>(Resource.Id.SpoolItemListView);
            SpoolItemListView.Adapter = new SpoolItemAdapter(this.Activity, _spl.SpoolItem);

            view.FindViewById<TextView>(Resource.Id.lblcSpoolNo).Text = _spl.cSpoolNo;
            view.FindViewById<TextView>(Resource.Id.lbliProjNo).Text = _spl.iProjNo.ToString();
            view.FindViewById<TextView>(Resource.Id.lblcEngrDrwgCode).Text = _spl.cEngrDrwgCode;
            view.FindViewById<TextView>(Resource.Id.lblcNpccDrwgCode).Text = _spl.cNpccDrwgCode;

            ScaleImageView imageView = view.FindViewById<ScaleImageView>(Resource.Id.imgView);
            Glide
             .With(this)
             .Load(_spl.icon)
             .Into(imageView);
            return view;
        }

        public override void OnDestroyView()
        {
            base.OnDestroyView();
            act.SupportActionBar.SetDisplayHomeAsUpEnabled(false);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            act.SupportFragmentManager.PopBackStack();
            return base.OnOptionsItemSelected(item);
        }


    }
}
