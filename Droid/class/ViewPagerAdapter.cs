using System;
using System.Collections.Generic;
using Android.Support.V4.App;
using Java.Lang;

namespace NPCCMobileApplications.Droid
{
    public class ViewPagerAdapter : FragmentPagerAdapter
    {
        private readonly List<Android.Support.V4.App.Fragment> FragmentsList = new List<Android.Support.V4.App.Fragment>();
        private readonly List<Java.Lang.ICharSequence> FragmentListTitles = new List<Java.Lang.ICharSequence>();

        public ViewPagerAdapter(Android.Support.V4.App.FragmentManager fm)
           : base(fm)
        {

        }

        public override int Count
        {
            get { return FragmentsList.Count; }
        }

        public override Android.Support.V4.App.Fragment GetItem(int position)
        {
            return FragmentsList[position];
        }

        public override ICharSequence GetPageTitleFormatted(int position)
        {
            return FragmentListTitles[position];
        }

        public void AddFragment(Fragment fragment, Java.Lang.ICharSequence Title){
            FragmentsList.Add(fragment);
            FragmentListTitles.Add(Title);
        }
    }
}
