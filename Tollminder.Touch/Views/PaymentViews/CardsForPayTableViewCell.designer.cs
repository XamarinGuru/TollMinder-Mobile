// WARNING
//
// This file has been generated automatically by Xamarin Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace Tollminder.Touch.Views.PaymentViews
{
    [Register ("CardsForPayTableViewCell")]
    partial class CardsForPayTableViewCell
    {
        [Outlet]
        UIKit.UINavigationBar CardsForPayNavigationBar { get; set; }


        [Outlet]
        UIKit.UIButton CardsForPayNavigationButtonClose { get; set; }


        [Outlet]
        UIKit.UITableView CardsForPayTableVIew { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (CardsForPayNavigationButtonClose != null) {
                CardsForPayNavigationButtonClose.Dispose ();
                CardsForPayNavigationButtonClose = null;
            }

            if (CardsForPayTableVIew != null) {
                CardsForPayTableVIew.Dispose ();
                CardsForPayTableVIew = null;
            }
        }
    }
}