using System;
using System.Collections.Generic;
using System.Drawing;
using Android.App;
using Android.Graphics.Drawables;
using Android.Support.V7.Widget;
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
            MyViewHolder h;

            if (view == null)
            {
                view = _context.LayoutInflater.Inflate(Resource.Layout.SpoolItemView, parent, false);
                MyViewHolder holder = new MyViewHolder(view);
                view.Tag = holder;
            }

            h = (MyViewHolder)view.Tag;
            h.drawable.SetStroke(3, Android.Graphics.Color.ParseColor("#" + _lstObjs[position].cColorCode.Split("~")[0]));
            h.rectangle_at_the_top.SetBackgroundColor(Android.Graphics.Color.ParseColor("#" + _lstObjs[position].cColorCode.Split("~")[0]));
            h.cMatType.Text = _lstObjs[position].cMatType;
            h.cVocab.Text = _lstObjs[position].cVocab;
            h.cClassCode.Text = _lstObjs[position].cClassCode;


            return view;
        }

        internal class MyViewHolder : RecyclerView.ViewHolder
        {
            public GradientDrawable drawable;
            public View rectangle_at_the_top;
            public TextView cMatType;
            public TextView cVocab;
            public TextView cClassCode;

            public MyViewHolder(View itemView)
                : base(itemView)
            {
                drawable = (GradientDrawable)itemView.Background;
                rectangle_at_the_top = itemView.FindViewById<View>(Resource.Id.rectangle_at_the_top);
                cMatType = itemView.FindViewById<TextView>(Resource.Id.cMatType);
                cVocab = itemView.FindViewById<TextView>(Resource.Id.cVocab);
                cClassCode = itemView.FindViewById<TextView>(Resource.Id.cClassCode);
            }
        }
    }
}