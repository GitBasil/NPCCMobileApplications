using System;
using Android.Support.V7.Widget;
using FFImageLoading;

namespace NPCCMobileApplications.Droid
{
    class CustomScrollListener : RecyclerView.OnScrollListener
    {
        public override void OnScrollStateChanged(RecyclerView recyclerView, int newState)
        {
            base.OnScrollStateChanged(recyclerView, newState);

            switch (newState)
            {
                case RecyclerView.ScrollStateDragging:
                    ImageService.Instance.SetPauseWork(true);
                    break;

                case RecyclerView.ScrollStateIdle:
                    ImageService.Instance.SetPauseWork(false);
                    break;
            }
        }
    }
}
