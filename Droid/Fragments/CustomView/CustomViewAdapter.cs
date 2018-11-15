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
    public class CustomViewAdapter : BaseAdapter<Spools>
    {
        List<Spools> _lstObjs;
        Activity _context;

        public CustomViewAdapter(Activity currentContext, List<Spools> lstObj) : base()
        {
            this._lstObjs = lstObj;
            this._context = currentContext;
        }

        public override Spools this[int position] => _lstObjs[position];

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
                view.FindViewById<TextView>(Resource.Id.lblVal1).Text = _lstObjs.ToArray()[position].Ename;
                view.FindViewById<TextView>(Resource.Id.lblVal2).Text = _lstObjs.ToArray()[position].Job;
                ImageViewAsync imageView = view.FindViewById<ImageViewAsync>(Resource.Id.imgView);
                ImageService.Instance
                            .LoadUrl(_lstObjs.ToArray()[position].icon)
                            .LoadingPlaceholder("loadingimg", FFImageLoading.Work.ImageSource.CompiledResource)
                            .ErrorPlaceholder("notfound", FFImageLoading.Work.ImageSource.CompiledResource)
                            .Into(imageView);
            }
            return view;
        }

    }
}
