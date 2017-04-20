// WARNING
//
// This file has been generated automatically by Xamarin Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
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
            if (AddVehicleButton != null) {
                AddVehicleButton.Dispose ();
                AddVehicleButton = null;
            }

            if (CancelButton != null) {
                CancelButton.Dispose ();
                CancelButton = null;
            }

            if (ColorImage != null) {
                ColorImage.Dispose ();
                ColorImage = null;
            }

            if (ColorTextField != null) {
                ColorTextField.Dispose ();
                ColorTextField = null;
            }

            if (MakeAndModelImage != null) {
                MakeAndModelImage.Dispose ();
                MakeAndModelImage = null;
            }

            if (MakeAndModeltextField != null) {
                MakeAndModeltextField.Dispose ();
                MakeAndModeltextField = null;
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

            if (YearImage != null) {
                YearImage.Dispose ();
                YearImage = null;
            }

            if (YearTextField != null) {
                YearTextField.Dispose ();
                YearTextField = null;
            }
        }
    }
}