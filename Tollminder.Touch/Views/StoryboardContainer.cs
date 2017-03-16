using System;
using MvvmCross.iOS.Views;
using UIKit;

namespace Tollminder.Touch.Views
{
    public class StoryboardContainer : MvxIosViewsContainer
    {
        protected override IMvxIosView CreateViewOfType(Type viewType, MvvmCross.Core.ViewModels.MvxViewModelRequest request)
        {
            return (IMvxIosView)UIStoryboard.FromName("Pay", null)
                .InstantiateViewController(viewType.Name);
        }
    }
}
