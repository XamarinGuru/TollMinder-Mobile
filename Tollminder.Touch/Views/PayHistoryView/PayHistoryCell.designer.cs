// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
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
