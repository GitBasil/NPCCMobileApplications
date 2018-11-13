
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
using SupportFragment = Android.Support.V4.App.Fragment;

namespace NPCCMobileApplications.Droid
{
    public class CustomListView : Android.Support.V4.App.Fragment
    {
        LayoutInflater InflaterMain;
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
            InflaterMain = inflater;
            View view = inflater.Inflate(Resource.Layout.CustomListView, container, false);

            mFragmentContainer = this.Activity.FindViewById<FrameLayout>(Resource.Id.fragmentContainer);
            act = (AppCompatActivity)this.Activity;

            _swipeRefresh = view.FindViewById<SwipeRefreshLayout>(Resource.Id.swiperefresh);
            _lvw = view.FindViewById<ListView>(Resource.Id.customListView);
            _swipeRefresh.Refresh += _swipeRefresh_Refresh;

            _lvw.ItemClick += _lvw_ItemClick;

            first_fill();

            return view;
        }

        void _swipeRefresh_Refresh(object sender, EventArgs e)
        {
            fill_list();
        }

        void _lvw_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            var SelObjId = e.Position;

            showData mshowData = new showData();
            common_functions.npcc_show_fragment(act, mFragmentContainer, mshowData);
        }

        void first_fill(){
            List<lsCustoms> lstObjs = new List<lsCustoms>
            {
                new lsCustoms{propVal1="Nepal", propVal2="Nepal",propVal3=Resource.Drawable.icon },
                new lsCustoms{propVal1="Solomon Sea", propVal2="May 7, 2015",propVal3=Resource.Drawable.icon }
            };



            _lvw.Adapter = new CustomViewAdapter(this.Activity, lstObjs);
        }

        void fill_list()
        {
            List<lsCustoms> lstObjs = new List<lsCustoms>
            {
                new lsCustoms{propVal1="Nepal", propVal2="Nepal",propVal3=Resource.Drawable.icon },
                new lsCustoms{propVal1="Solomon Sea", propVal2="May 7, 2015",propVal3=Resource.Drawable.icon },
                new lsCustoms{propVal1="Papua New Guinea", propVal2="May 5, 2015",propVal3=Resource.Drawable.icon },
                new lsCustoms{propVal1="Nepal", propVal2="April 25, 2015",propVal3=Resource.Drawable.icon },
                new lsCustoms{propVal1="Taiwan", propVal2="April 20, 2015",propVal3=Resource.Drawable.icon },
                new lsCustoms{propVal1="Papua New Guinea", propVal2="March 29, 2015",propVal3=Resource.Drawable.icon },
                new lsCustoms{propVal1="Flores Sea", propVal2="Febdruary 27, 2015",propVal3=Resource.Drawable.icon },
                new lsCustoms{propVal1="Mid-Atlantic range", propVal2="Febdruary 13, 2015",propVal3=Resource.Drawable.icon }
            };



            _lvw.Adapter = new CustomViewAdapter(this.Activity, lstObjs);
            _swipeRefresh.Refreshing = false;
        }
    }

}
