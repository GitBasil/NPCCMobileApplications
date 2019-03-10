
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.Widget;
using Android.Support.V7.App;
using Android.Support.V7.Widget;
using Android.Util;
using Android.Views;
using Android.Widget;
using FFImageLoading;
using Java.Interop;
using NPCCMobileApplications.Library;
using SupportToolbar = Android.Support.V7.Widget.Toolbar;
using SupportFragment = Android.Support.V4.App.Fragment;

namespace NPCCMobileApplications.Droid
{
    public class FillWeldLog : SupportFragment
    {
        FrameLayout mFragmentContainer;
        AppCompatActivity act;
        List<Spools> lstObjs;
        private RecyclerView rv;
        private JointsViewAdapter adapter;
        Spools _spl;
        SupportToolbar mToolbar;

        public FillWeldLog(Spools spl)
        {
            _spl = spl;
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            HasOptionsMenu = true;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.FillWeldLog, container, false);

            mFragmentContainer = this.Activity.FindViewById<FrameLayout>(Resource.Id.fragmentContainer);
            act = (AppCompatActivity)this.Activity;

            mToolbar = act.FindViewById<SupportToolbar>(Resource.Id.toolbar);
            act.SetSupportActionBar(mToolbar);
            act.SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            mToolbar.NavigationIcon.SetColorFilter(Color.ParseColor("#FFFFFF"), PorterDuff.Mode.SrcAtop);

            rv = view.FindViewById<RecyclerView>(Resource.Id.mRecylcerID);
            rv.SetLayoutManager(new GridLayoutManager(act, 1));
            rv.SetItemAnimator(new DefaultItemAnimator());
            rv.AddOnScrollListener(new CustomScrollListener());

            fill_list();

            return view;
        }

        public void fill_list()
        {
            adapter = new JointsViewAdapter(act, this, _spl.SpoolJoints);
            rv.SetAdapter(adapter);
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

        public override void OnHiddenChanged(bool hidden)
        {
            base.OnHiddenChanged(hidden);
            if (!hidden)
            {
                HasOptionsMenu = true;
                act.SupportActionBar.SetDisplayHomeAsUpEnabled(true);
                act.SupportActionBar.SetDisplayShowHomeEnabled(true);
                mToolbar.NavigationIcon.SetColorFilter(Color.ParseColor("#FFFFFF"), PorterDuff.Mode.SrcAtop);
            }
        }
    }
}
