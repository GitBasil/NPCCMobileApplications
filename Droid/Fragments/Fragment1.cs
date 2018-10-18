
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace NPCCMobileApplications.Droid
{
    public class Fragment1 : Android.Support.V4.App.Fragment
    {
        TextView stationName;
        LayoutInflater InflaterMain;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            InflaterMain = inflater;
            View view = inflater.Inflate(Resource.Layout.Fragment1, container, false);

            stationName = view.FindViewById<TextView>(Resource.Id.txtFragment1);
            setMsg();
            return view;
        }

        public void setMsg()
        {
            stationName.Text = $"Fragment 1 ";
        }
    }
}
