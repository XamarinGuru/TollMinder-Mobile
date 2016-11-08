using System;
using MvvmCross.Droid.Support.V7.AppCompat;
using Tollminder.Core.ViewModels;

namespace Tollminder.Droid.Views
{
    public abstract class BaseActivity<TViewModel> : MvxAppCompatActivity<TViewModel> where TViewModel : BaseViewModel
    {
        protected abstract int LayoutId { get; }

        protected override void OnCreate(Android.OS.Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(LayoutId);
            ViewModel.OnCreateFinish();
        }

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
