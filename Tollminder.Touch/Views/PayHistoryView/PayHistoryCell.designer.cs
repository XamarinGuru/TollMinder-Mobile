// WARNING
//
// This file has been generated automatically by Xamarin Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace Tollminder.Touch.Views
{
    [Register ("PayHistoryCell")]
    partial class PayHistoryCell
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel amountUILabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel roadNameLabel { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (amountUILabel != null) {
                amountUILabel.Dispose ();
                amountUILabel = null;
            }

            if (roadNameLabel != null) {
                roadNameLabel.Dispose ();
                roadNameLabel = null;
            }
        }
    }
}