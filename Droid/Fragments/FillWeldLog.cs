
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
        SwipeRefreshLayout _swipeRefresh;
        FrameLayout mFragmentContainer;
        AppCompatActivity act;
        List<Spools> lstObjs;
        private RecyclerView rv;
        private SpoolsCardViewAdapter adapter;
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

            _swipeRefresh = view.FindViewById<SwipeRefreshLayout>(Resource.Id.swiperefresh);
            rv = view.FindViewById<RecyclerView>(Resource.Id.mRecylcerID);
            rv.SetLayoutManager(new GridLayoutManager(act, 1));
            rv.SetItemAnimator(new DefaultItemAnimator());
            rv.AddOnScrollListener(new CustomScrollListener());
            _swipeRefresh.Refresh += _swipeRefresh_Refresh;

            fill_list();

            return view;
        }

        void _swipeRefresh_Refresh(object sender, EventArgs e)
        {
            refresh_listAsync();
        }

        void refresh_listAsync()
        {
            DBRepository dBRepository = new DBRepository();
            dBRepository.CreateTable();
            Task.Run(async () => {
                await dBRepository.RefreshSpoolAsync(npcc_types.inf_assignment_type.UnderWelding);
            }).ContinueWith(fn => {
                act.RunOnUiThread(() => {
                    lstObjs = dBRepository.GetSpools(npcc_types.inf_assignment_type.UnderWelding);
                    adapter = new SpoolsCardViewAdapter(act, this, lstObjs, npcc_types.inf_assignment_type.UnderWelding);
                    rv.SetAdapter(adapter);
                    _swipeRefresh.Refreshing = false;
                });
            });
        }

        public void fill_list()
        {
            DBRepository dBRepository = new DBRepository();
            dBRepository.CreateTable();
            lstObjs = dBRepository.GetSpools(npcc_types.inf_assignment_type.UnderWelding);
            if (lstObjs != null && lstObjs.Count == 0)
            {
                _swipeRefresh.Refreshing = true;
                refresh_listAsync();
            }
            else
            {
                adapter = new SpoolsCardViewAdapter(act, this, lstObjs, npcc_types.inf_assignment_type.UnderWelding);
                rv.SetAdapter(adapter);

                _swipeRefresh.Refreshing = false;
            }

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
