using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Calligraphy;

namespace NPCCMobileApplications.Droid
{
    [Application]
    public class App : Application
    {
        public override void OnCreate()
        {
            base.OnCreate();
            CalligraphyConfig.InitDefault(new CalligraphyConfig.Builder()
                                          .SetDefaultFontPath("fonts/npcc_font.ttf")
                                          .SetFontAttrId(Resource.Attribute.fontPath)
                                          .Build());
        }

        public App(IntPtr intPtr, JniHandleOwnership jniHandleOwnership)
            : base(intPtr, jniHandleOwnership) { }
    }
}
