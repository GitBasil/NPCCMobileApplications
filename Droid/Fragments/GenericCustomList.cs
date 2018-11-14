
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
using Fragment = Android.Support.V4.App.Fragment;

namespace NPCCMobileApplications.Droid
{
    public class GenericCustomList : Fragment
    {
        private static string ARG_LAYOUT_RES_ID = "layoutResId";
        private int layoutResId;
        private LayoutInflater InflaterMain;
        private View view;

        public static GenericCustomList NewInstance(int layoutResId)
        {
            GenericCustomList genericCustomList = new GenericCustomList();
            Bundle args = new Bundle();
            args.PutInt(ARG_LAYOUT_RES_ID, layoutResId);
            genericCustomList.Arguments = args;
            return genericCustomList;
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            if (Arguments != null && Arguments.ContainsKey(ARG_LAYOUT_RES_ID))
            {
                layoutResId = Arguments.GetInt(ARG_LAYOUT_RES_ID);
            }
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            InflaterMain = inflater;
            view = inflater.Inflate(layoutResId, container, false);

            return view;
        }
    }
}
