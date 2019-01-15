using System;
using System.Collections.Generic;
using Android.App;
using Android.Views;
using Android.Widget;
using FFImageLoading;
using FFImageLoading.Views;
using NPCCMobileApplications.Library;

namespace NPCCMobileApplications.Droid
{
    public class SpoolItemAdapter : BaseAdapter<SpoolItem>
    {
        List<SpoolItem> _lstObjs;
        Activity _context;

        public SpoolItemAdapter(Activity currentContext, List<SpoolItem> lstObj) : base()
        {
            this._lstObjs = lstObj;
            this._context = currentContext;
        }

        public override SpoolItem this[int position] => _lstObjs[position];

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
                view = _context.LayoutInflater.Inflate(Resource.Layout.SpoolItemView, null);
                view.FindViewById<TextView>(Resource.Id.cMatType).Text = "cMatType: " + _lstObjs.ToArray()[position].cMatType;
                view.FindViewById<TextView>(Resource.Id.cVocab).Text = "cVocab: " + _lstObjs.ToArray()[position].cVocab;
                view.FindViewById<TextView>(Resource.Id.cClassCode).Text = "cClassCode: " + _lstObjs.ToArray()[position].cClassCode;
                  
            }



            return view;
        }


    }
}
