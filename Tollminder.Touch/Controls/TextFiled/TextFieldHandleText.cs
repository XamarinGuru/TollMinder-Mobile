using System;
using CoreGraphics;
using Foundation;
using UIKit;

namespace Tollminder.Touch.Controls
{
   public class TextFieldHandleText : UITextField
    {
        public Action InitTextBindHandle;

        public TextFieldHandleText() : base()
        {

        }

        public TextFieldHandleText(CGRect rect) : base(rect)
        {

        }

        public TextFieldHandleText(IntPtr handle) : base(handle)
        {

        }

        public TextFieldHandleText(NSCoder coder) : base(coder)
        {

        }

        public override string Text
        {
            get
            {
                return base.Text;
            }
            set
            {
                base.Text = value;
                InitTextBindHandle?.Invoke();
            }
        }
    }
}