// WARNING
//
// This file has been generated automatically by Xamarin Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace Tollminder.Touch.Views
{
    [Register ("HomeView")]
    partial class HomeDebugView
    {
        [Outlet]
        UIKit.UILabel ActivityLabel { get; set; }


        [Outlet]
        UIKit.UILabel GeoLabel { get; set; }


        [Outlet]
        UIKit.UILabel GeoLabelData { get; set; }


        [Outlet]
        UIKit.UITextView LogArea { get; set; }


        [Outlet]
        UIKit.UIButton LogOut { get; set; }


        [Outlet]
        UIKit.UILabel NextWaypointString { get; set; }


        [Outlet]
        UIKit.UIButton StartButton { get; set; }


        [Outlet]
        UIKit.UILabel StatusLabel { get; set; }


        [Outlet]
        UIKit.UIButton StopButton { get; set; }


        [Outlet]
        UIKit.UILabel TollRoadString { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (ActivityLabel != null) {
                ActivityLabel.Dispose ();
                ActivityLabel = null;
            }

            if (GeoLabel != null) {
                GeoLabel.Dispose ();
                GeoLabel = null;
            }

            if (GeoLabelData != null) {
                GeoLabelData.Dispose ();
                GeoLabelData = null;
            }

            if (LogArea != null) {
                LogArea.Dispose ();
                LogArea = null;
            }

            if (LogOut != null) {
                LogOut.Dispose ();
                LogOut = null;
            }

            if (NextWaypointString != null) {
                NextWaypointString.Dispose ();
                NextWaypointString = null;
            }

            if (StartButton != null) {
                StartButton.Dispose ();
                StartButton = null;
            }

            if (StatusLabel != null) {
                StatusLabel.Dispose ();
                StatusLabel = null;
            }

            if (StopButton != null) {
                StopButton.Dispose ();
                StopButton = null;
            }

            if (TollRoadString != null) {
                TollRoadString.Dispose ();
                TollRoadString = null;
            }
        }
    }
}