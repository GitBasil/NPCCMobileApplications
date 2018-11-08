using System;
using System.Collections.Generic;
using Android.App;
using Android.Views;
using Android.Widget;

namespace NPCCMobileApplications.Droid
{
    public class CustomViewAdapter : BaseAdapter<lsCustoms>
    {
        List<lsCustoms> _lstObjs;
        Activity _context;

        public CustomViewAdapter(Activity currentContext, List<lsCustoms> lstObj) : base()
        {
            this._lstObjs = lstObj;
            this._context = currentContext;
        }

        public override lsCustoms this[int position] => _lstObjs[position];

        public override int Count => _lstObjs.Count;

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View view = convertView;
            if (view == null)
            {
                view = _context.LayoutInflater.Inflate(Resource.Layout.CustomListItem, null);
                view.FindViewById<TextView>(Resource.Id.lblVal1).Text = _lstObjs.ToArray()[position].propVal1;
                view.FindViewById<TextView>(Resource.Id.lblVal2).Text = _lstObjs.ToArray()[position].propVal2;
                view.FindViewById<ImageView>(Resource.Id.imgView).SetImageResource(_lstObjs.ToArray()[position].propVal3);
            }
            return view;
        }

    }
}
