using System;
using MvvmCross.Binding.BindingContext;
using MvvmCross.iOS.Views;
using UIKit;
using Tollminder.Core.ViewModels.Vehicles;

namespace Tollminder.Touch.Views.DriverViews
{
    public partial class VehicleViewController : MvxViewController
    {
        public VehicleViewController() : base("VehicleViewController", null)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            NavigationController.NavigationBar.Translucent = false;
            //NavigationController.NavigationBar.Hidden = true;

            //var sideMenuButtonView = SideMenuButton.Create();
            //var sideMenuButton = new UIButton(sideMenuButtonView.Frame);
            //sideMenuButton.AddSubview(sideMenuButtonView);
            //sideMenuButton.AddTarget((s, a) => openDrawer(), UIControlEvent.TouchUpInside);
            //GoNavigationItem.LeftBarButtonItem = new UIBarButtonItem(sideMenuButton);

            InitializeBindings();
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }

        private void InitializeBindings()
        {
            var bindingSet = this.CreateBindingSet<VehicleViewController, VehicleViewModel>();
            bindingSet.Bind(GoBackToVehiclesList).To(viewModel => viewModel.GoBakToVehicleListCommnad);

            bindingSet.Bind(PlateNumberTextField).To(viewModel => viewModel.Vehicle.PlateNumber);
            bindingSet.Bind(StateTextField).To(viewModel => viewModel.Vehicle.State);
            bindingSet.Bind(MakeAndModeltextField).To(viewModel => viewModel.Vehicle.Model);
            bindingSet.Bind(YearTextField).To(viewModel => viewModel.Vehicle.Year);
            bindingSet.Bind(ColorTextField).To(viewModel => viewModel.Vehicle.Color);

            bindingSet.Bind(AddVehicleButton).To(viewModel => viewModel.AddVehicleCommnad);
            bindingSet.Bind(CancelButton).To(viewModel => viewModel.CancelAddingCommnad);

            bindingSet.Apply();
        }
    }
}

