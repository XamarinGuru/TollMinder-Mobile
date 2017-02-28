using Foundation;
using System;
using UIKit;
using ObjCRuntime;

namespace Tollminder.Touch
{
    public partial class PayHistoryHeader : UIView
    {
        public PayHistoryHeader(IntPtr handle) : base(handle)
        {
        }
        public static UIView Create()
        {
            var arr = NSBundle.MainBundle.LoadNib("PayHistoryHeader", null, null);
            var v = Runtime.GetNSObject<PayHistoryHeader>(arr.ValueAt(0));

            return v;
        }

    }
}