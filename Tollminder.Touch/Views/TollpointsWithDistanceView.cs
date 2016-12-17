using System;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.iOS.Views;
using MvvmCross.iOS.Views;
using Tollminder.Core.ViewModels;

namespace Tollminder.Touch.Views
{
    //public class TollPointsWithDistanceView : MvxTableView
    //{
    //    public override void ViewDidLoad()
    //    {
    //        base.ViewDidLoad();
    //        var source = new MvxStandardTableViewSource(TableView, "TitleText NearestTollpointsString;Distance Distance");
    //        TableView.Source = source;

    //        var set = this.CreateBindingSet<TollPointsWithDistanceView, HomeTestViewModel>();
    //        set.Bind(source).To(vm => vm.Distance);
    //        set.Apply();

    //        TableView.ReloadData();
    //    }
    //}
}
