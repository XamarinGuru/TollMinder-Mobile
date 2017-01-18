using System;
using MvvmCross.Droid.Views;
using Tollminder.Core.ViewModels;

namespace Tollminder.Droid.Views
{
    public abstract class LifeCycleActivity<TViewModel> : MvxActivity<TViewModel>
             where TViewModel : BaseViewModel
    {
        protected override void OnResume()
        {
            base.OnResume();
            ViewModel.OnResume();
        }

        protected override void OnPause()
        {
            base.OnPause();
            ViewModel.OnPause();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            ViewModel.OnDestroy();
        }
    }
}
