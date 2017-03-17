// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace Tollminder.Touch.Views.DriverViews
{
	[Register ("VehicleViewController")]
	partial class VehicleViewController
	{
		[Outlet]
		UIKit.UIButton AddVehicleButton { get; set; }

		[Outlet]
		UIKit.UIButton CancelButton { get; set; }

		[Outlet]
		UIKit.UIImageView ColorImage { get; set; }

		[Outlet]
		UIKit.UITextField ColorTextField { get; set; }

		[Outlet]
		UIKit.UIButton GoBackToVehiclesList { get; set; }

		[Outlet]
		UIKit.UIImageView MakeAndModelImage { get; set; }

		[Outlet]
		UIKit.UITextField MakeAndModeltextField { get; set; }

		[Outlet]
		UIKit.UIImageView PlateNumberImage { get; set; }

		[Outlet]
		UIKit.UITextField PlateNumberTextField { get; set; }

		[Outlet]
		UIKit.UIImageView StateImage { get; set; }

		[Outlet]
		UIKit.UITextField StateTextField { get; set; }

		[Outlet]
		UIKit.UIImageView YearImage { get; set; }

		[Outlet]
		UIKit.UITextField YearTextField { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (GoBackToVehiclesList != null) {
				GoBackToVehiclesList.Dispose ();
				GoBackToVehiclesList = null;
			}

			if (PlateNumberImage != null) {
				PlateNumberImage.Dispose ();
				PlateNumberImage = null;
			}

			if (PlateNumberTextField != null) {
				PlateNumberTextField.Dispose ();
				PlateNumberTextField = null;
			}

			if (StateImage != null) {
				StateImage.Dispose ();
				StateImage = null;
			}

			if (StateTextField != null) {
				StateTextField.Dispose ();
				StateTextField = null;
			}

			if (MakeAndModelImage != null) {
				MakeAndModelImage.Dispose ();
				MakeAndModelImage = null;
			}

			if (MakeAndModeltextField != null) {
				MakeAndModeltextField.Dispose ();
				MakeAndModeltextField = null;
			}

			if (YearImage != null) {
				YearImage.Dispose ();
				YearImage = null;
			}

			if (YearTextField != null) {
				YearTextField.Dispose ();
				YearTextField = null;
			}

			if (ColorImage != null) {
				ColorImage.Dispose ();
				ColorImage = null;
			}

			if (ColorTextField != null) {
				ColorTextField.Dispose ();
				ColorTextField = null;
			}

			if (AddVehicleButton != null) {
				AddVehicleButton.Dispose ();
				AddVehicleButton = null;
			}

			if (CancelButton != null) {
				CancelButton.Dispose ();
				CancelButton = null;
			}
		}
	}
}
