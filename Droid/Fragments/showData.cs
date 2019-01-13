using System;
using Android.OS;
using Android.Views;
using Android.Support.V7.App;
using SupportToolbar = Android.Support.V7.Widget.Toolbar;
using static Android.Views.View;
using Android.Runtime;
using SupportFragment = Android.Support.V4.App.Fragment;
using Android.Widget;
using Android.Graphics;
using NPCCMobileApplications.Library;
using FFImageLoading.Views;
using FFImageLoading;
using FFImageLoading.Transformations;

namespace NPCCMobileApplications.Droid
{
    public class showData : Android.Support.V4.App.Fragment
    {
        LayoutInflater InflaterMain;
        SupportToolbar mToolbar;
        AppCompatActivity act;
        FrameLayout mFragmentContainer;
        Spools _spl;
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


            view.FindViewById<TextView>(Resource.Id.lblcSpoolNo).Text = "Spool: " + _spl.cSpoolNo;
            view.FindViewById<TextView>(Resource.Id.lbliProjNo).Text = "Project: " + _spl.iProjNo.ToString();
            view.FindViewById<TextView>(Resource.Id.lblcEngrDrwgCode).Text = "ISO: " + _spl.cEngrDrwgCode;
            view.FindViewById<TextView>(Resource.Id.lblcNpccDrwgCode).Text = "ISO: " + _spl.cNpccDrwgCode;
            ImageViewAsync imageView = view.FindViewById<ImageViewAsync>(Resource.Id.imgView);

            ImageService.Instance
                        .LoadUrl(_spl.icon)
                        .LoadingPlaceholder("loadingimg", FFImageLoading.Work.ImageSource.CompiledResource)
                        .ErrorPlaceholder("notfound", FFImageLoading.Work.ImageSource.CompiledResource)
                        //.Transform(new CircleTransformation())
                        //.Transform(new GrayscaleTransformation())
                        //.Retry(3, 200)
                        //.DownSample(300, 300)
                        .IntoAsync(imageView);


            return view;
        }


        public override void OnDestroy()
        {
            base.OnDestroy();
            act.SupportActionBar.SetDisplayHomeAsUpEnabled(false);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            act.SupportFragmentManager.PopBackStack();
            return base.OnOptionsItemSelected(item);
        }


    }
}
