using System;
using System.Drawing;
using Foundation;
using UIKit;

namespace Tollminder.Touch.Helpers
{
    public class KeyboardHandler
    {
        private UIView _activeview; // Controller that activated the
                                    // keyboard
        private float _scrollamount = 0; // amount to scroll 
        private float _scrolledamount = 0; // how much we've
                                           // scrolled already
        private float _bottom = 0.0f; // bottom point
        private const float Offset = 10.0f; // extra offset 
        public UIView View { get; set; } // The UIView for the
                                         // keyboard handler
        public void KeyboardUpNotification(NSNotification notification)
        {
            // get the keyboard size
            //var val = new NSValue(notification.UserInfo.ValueForKey(UIKeyboard.FrameBeginUserInfoKey).Handle);
            //RectangleF keyboardFrame = val.RectangleFValue;
            //// Find what opened the keyboard
            //foreach (UIView view in this.View.Subviews)
            //{
            //    if (view.IsFirstResponder)
            //        _activeview = view;
            //}
            //// Determine if we need to scroll up or down.
            //// Bottom of the controller = initial position + height
            //// + offset 
            //_bottom = ((float)(_activeview.Frame.Y + _activeview.Frame.Height + Offset));
            //// Calculate how far we need to scroll
            //_scrollamount = ((float)(keyboardFrame.Height - (View.Frame.Size.Height - _bottom)));
            ////Move view up
            //if (_scrollamount > 0)
            //{
            //    //Subtract the scrolledamount. We can’t do this
            //    //subtraction
            //    //above because the calculations won’t work
            //    //correctly.
            //    _bottom -= _scrolledamount;
            //    _scrollamount = ((float)(keyboardFrame.Height - (View.Frame.Size.Height - _bottom)));
            //    _scrolledamount += _scrollamount;
            //    ScrollTheView(false);
            //}
            ////Reset the view.
            //else
            //{
            //    ScrollTheView(true);
            //}
        }
        public void KeyboardDownNotification(NSNotification notification)
        {
            ScrollTheView(true);
        }
        private void ScrollTheView(bool reset)
        {
            // scroll the view up or down
            UIView.BeginAnimations(string.Empty,
                System.IntPtr.Zero);
            UIView.SetAnimationDuration(0.3);
            RectangleF frame = (System.Drawing.RectangleF)View.Frame;
            if (reset)
            {
                frame.Y = frame.Y + _scrolledamount;
                _scrollamount = 0;
                _scrolledamount = 0;
            }
            else
            {
                frame.Y -= _scrollamount;
            }
            View.Frame = frame;
            UIView.CommitAnimations();
        }
    }
}
