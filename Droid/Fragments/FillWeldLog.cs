
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.Widget;
using Android.Support.V7.App;
using Android.Support.V7.Widget;
using Android.Util;
using Android.Views;
using Android.Widget;
using FFImageLoading;
using Java.Interop;
using NPCCMobileApplications.Library;
using SupportToolbar = Android.Support.V7.Widget.Toolbar;
using SupportFragment = Android.Support.V4.App.Fragment;
using System.Globalization;

namespace NPCCMobileApplications.Droid
{
    public class FillWeldLog : SupportFragment, DatePickerDialog.IOnDateSetListener
    {
        FrameLayout mFragmentContainer;
        AppCompatActivity act;
        private RecyclerView rv;
        private JointsViewAdapter adapter;
        Spools _spl;
        DateTime _dWeld;
        SupportToolbar mToolbar;
        Button btnSubmitWeldLog;
        EditText txtWeldDate;

        public FillWeldLog(Spools spl)
        {
            _spl = spl;
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            HasOptionsMenu = true;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.FillWeldLog, container, false);

            mFragmentContainer = this.Activity.FindViewById<FrameLayout>(Resource.Id.fragmentContainer);
            act = (AppCompatActivity)this.Activity;

            mToolbar = act.FindViewById<SupportToolbar>(Resource.Id.toolbar);
            act.SetSupportActionBar(mToolbar);
            act.SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            mToolbar.NavigationIcon.SetColorFilter(Color.ParseColor("#FFFFFF"), PorterDuff.Mode.SrcAtop);

            view.FindViewById<TextView>(Resource.Id.lbliProjNo).Text = _spl.iProjNo.ToString();
            view.FindViewById<TextView>(Resource.Id.lblcISO).Text = _spl.cISO.Trim();
            view.FindViewById<TextView>(Resource.Id.lblcSpoolNo).Text = _spl.cSpoolNo.Trim();

            txtWeldDate = view.FindViewById<EditText>(Resource.Id.txtWeldDate);
            _dWeld = DateTime.Now;
            txtWeldDate.Text = _dWeld.ToString("dd/MM/yyyy");
            txtWeldDate.Click += (sender, e) =>
            {
                CultureInfo provider= CultureInfo.InvariantCulture;
                DateTime dateTime = DateTime.ParseExact(txtWeldDate.Text, "dd/MM/yyyy", provider);

                DatePickerDialog datePicker = new DatePickerDialog(this.Context, this, dateTime.Year, dateTime.Month -1, dateTime.Day);
                DateTime baseDate = new DateTime(1970, 1, 1);
                TimeSpan diff = DateTime.Now - baseDate;

                datePicker.DatePicker.MaxDate =(long)diff.TotalMilliseconds;
                datePicker.Show();
            };

            txtWeldDate.AfterTextChanged += (sender, e) => {
                fill_list();
            };

            btnSubmitWeldLog = view.FindViewById<Button>(Resource.Id.btnSubmitWeldLog);
            btnSubmitWeldLog.Click += BtnSubmitWeldLog_Click;

            rv = view.FindViewById<RecyclerView>(Resource.Id.mRecylcerID);
            rv.SetLayoutManager(new GridLayoutManager(act, 1));
            rv.SetItemAnimator(new DefaultItemAnimator());
            rv.AddOnScrollListener(new CustomScrollListener());

            fill_list();

            return view;
        }


        public void OnDateSet(DatePicker view, int year, int month, int dayOfMonth)
        {
            month += 1;
            _dWeld = new DateTime(year, month, dayOfMonth);
            txtWeldDate.Text = _dWeld.ToString("dd/MM/yyyy");
        }

        void BtnSubmitWeldLog_Click(object sender, EventArgs e)
        {
            var dialog = new SubmitWeldLog(adapter._lsObjs, adapter._ins);
            dialog.Show(act.FragmentManager, "SubmitWeldLog");
        }


        public void fill_list()
        {
            adapter = new JointsViewAdapter(act, this, _spl.SpoolJoints, _dWeld);
            rv.SetAdapter(adapter);
        }

        public override void OnDestroyView()
        {
            base.OnDestroyView();
            act.SupportActionBar.SetDisplayHomeAsUpEnabled(false);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            act.SupportFragmentManager.PopBackStack();
            return base.OnOptionsItemSelected(item);
        }

        public override void OnHiddenChanged(bool hidden)
        {
            base.OnHiddenChanged(hidden);
            if (!hidden)
            {
                HasOptionsMenu = true;
                act.SupportActionBar.SetDisplayHomeAsUpEnabled(true);
                act.SupportActionBar.SetDisplayShowHomeEnabled(true);
                mToolbar.NavigationIcon.SetColorFilter(Color.ParseColor("#FFFFFF"), PorterDuff.Mode.SrcAtop);
            }
        }
    }
}
