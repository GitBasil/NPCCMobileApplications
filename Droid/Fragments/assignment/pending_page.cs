
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.Widget;
using Android.Support.V7.App;
using Android.Support.V7.Widget;
using Android.Util;
using Android.Views;
using Android.Widget;
using Java.Interop;
using NPCCMobileApplications.Library;
using SupportFragment = Android.Support.V4.App.Fragment;

namespace NPCCMobileApplications.Droid
{
    public class pending_page : SupportFragment
    {
        SwipeRefreshLayout _swipeRefresh;
        FrameLayout mFragmentContainer;
        AppCompatActivity act;
        List<Spools> lstObjs;
        private RecyclerView rv;
        private SpoolsCardViewAdapter adapter;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.assignment, container, false);

            mFragmentContainer = this.Activity.FindViewById<FrameLayout>(Resource.Id.fragmentContainer);
            act = (AppCompatActivity)this.Activity;

            _swipeRefresh = view.FindViewById<SwipeRefreshLayout>(Resource.Id.swiperefresh);
            rv = view.FindViewById<RecyclerView>(Resource.Id.mRecylcerID);
            rv.SetLayoutManager(new GridLayoutManager(act, 2));
            rv.SetItemAnimator(new DefaultItemAnimator());

            _swipeRefresh.Refresh += _swipeRefresh_Refresh;

            fill_listAsync();

            return view;
        }
        void _swipeRefresh_Refresh(object sender, EventArgs e)
        {
            refresh_listAsync();
        }

        async void refresh_listAsync()
        {
            DBRepository dBRepository = new DBRepository();
            dBRepository.CreateTable();
            await dBRepository.RefreshSpoolAsync();
            lstObjs = dBRepository.GetSpools();

            adapter = new SpoolsCardViewAdapter(act, this, lstObjs);
            rv.SetAdapter(adapter);

            _swipeRefresh.Refreshing = false;
        }

        void fill_listAsync()
        {
            DBRepository dBRepository = new DBRepository();
            dBRepository.CreateTable();
            lstObjs = dBRepository.GetSpools();
            if (lstObjs.Count == 0) refresh_listAsync();

            adapter = new SpoolsCardViewAdapter(act, this, lstObjs);
            rv.SetAdapter(adapter);

            _swipeRefresh.Refreshing = false;
        }

        public override void OnCreateContextMenu(IContextMenu menu, View v, IContextMenuContextMenuInfo menuInfo)
        {
            if (v.Id == Resource.Id.customListView)
            {
                var info = (AdapterView.AdapterContextMenuInfo)menuInfo;
                var menuItems = Resources.GetStringArray(Resource.Array.menu);
                for (var i = 0; i < menuItems.Length; i++)
                    menu.Add(Menu.None, i, i, menuItems[i]);
            }
        }

        public override bool OnContextItemSelected(IMenuItem item)
        {
            var info = (AdapterView.AdapterContextMenuInfo)item.MenuInfo;
            var menuItemIndex = item.ItemId;
            var menuItems = Resources.GetStringArray(Resource.Array.menu);
            var menuItemName = menuItems[menuItemIndex];
            var listItemName = info.Position.ToString();

            Toast.MakeText(Context, string.Format("Selected {0} for item {1}", menuItemName, listItemName), ToastLength.Short).Show();
            return true;
        }
    }

}
