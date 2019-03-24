
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.Widget;
using Android.Support.V7.App;
using Android.Util;
using Android.Views;
using Android.Widget;
using NPCCMobileApplications.Library;
using SupportFragment = Android.Support.V4.App.Fragment;

namespace NPCCMobileApplications.Droid
{
    public class CustomListView : SupportFragment
    {
        SwipeRefreshLayout _swipeRefresh;
        ListView _lvw;
        FrameLayout mFragmentContainer;
        AppCompatActivity act;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.CustomListView, container, false);

            mFragmentContainer = this.Activity.FindViewById<FrameLayout>(Resource.Id.fragmentContainer);
            act = (AppCompatActivity)this.Activity;

            _swipeRefresh = view.FindViewById<SwipeRefreshLayout>(Resource.Id.swiperefresh);
            _lvw = view.FindViewById<ListView>(Resource.Id.customListView);
            _swipeRefresh.Refresh += _swipeRefresh_Refresh;

            //_lvw.ItemClick += _lvw_ItemClick;

            first_fill();

            return view;
        }

        void _swipeRefresh_Refresh(object sender, EventArgs e)
        {
            fill_list();
        }

        //void _lvw_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        //{
        //    var SelObjId = e.Position;

        //    Spools selectedSp = (Spools)_lvw.GetItemAtPosition(e.Position);
        //    showData mshowData = new showData(selectedSp);
        //    common_functions.npcc_show_fragment(act, mFragmentContainer, mshowData, this);
        //}

        void first_fill(){
            DBRepository dBRepository = new DBRepository();
            dBRepository.RefreshSpoolAsync(npcc_types.inf_assignment_type.Pending);
            List<Spools> lstObjs = dBRepository.GetSpools(npcc_types.inf_assignment_type.Pending);
            _lvw.Adapter = new CustomViewAdapter(this.Activity, lstObjs);
        }

        void fill_list()
        {
            DBRepository dBRepository = new DBRepository();
            List<Spools> lstObjs = dBRepository.GetSpools(npcc_types.inf_assignment_type.Pending);
            _lvw.Adapter = new CustomViewAdapter(this.Activity, lstObjs);
            _swipeRefresh.Refreshing = false;
        }
    }

}
