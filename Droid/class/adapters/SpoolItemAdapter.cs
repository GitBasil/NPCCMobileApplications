using System;
using System.Collections.Generic;
using System.Drawing;
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

                View rectangle_at_the_top = view.FindViewById<View>(Resource.Id.rectangle_at_the_top);
                rectangle_at_the_top.SetBackgroundColor(Android.Graphics.Color.ParseColor("#" + _lstObjs.ToArray()[position].cColorCode.Split("~")[0]));

                TextView cMatType = view.FindViewById<TextView>(Resource.Id.cMatType);
                cMatType.Text = "Material: " + _lstObjs.ToArray()[position].cMatType;

                TextView cVocab = view.FindViewById<TextView>(Resource.Id.cVocab);
                cVocab.Text = "Vocab: " + _lstObjs.ToArray()[position].cVocab;

                TextView cClassCode = view.FindViewById<TextView>(Resource.Id.cClassCode);
                cClassCode.Text = "Class: " + _lstObjs.ToArray()[position].cClassCode;
            }



            return view;
        }


    }
}
