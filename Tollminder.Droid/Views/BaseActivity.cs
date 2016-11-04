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
        }
    }
}
