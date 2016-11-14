using System;
using Cirrious.FluentLayouts.Touch;
using CoreGraphics;
using Foundation;
using Tollminder.Touch.Extensions;
using UIKit;

namespace Tollminder.Touch.Controls
{
    [Register("TextFieldValidationWithImage")]
    public class TextFieldValidationWithImage : UIView
    {
        UITapGestureRecognizer _clickAction;

        public TextFieldValidationWithImage() : base()
        {
            InitObjects();
        }

        public TextFieldValidationWithImage(IntPtr handle) : base(handle)
        {
            InitObjects();
        }

        public TextFieldValidationWithImage(NSCoder coder) : base(coder)
        {
            InitObjects();
        }

        public TextFieldValidationWithImage(CGRect rect) : base(rect)
        {
            InitObjects();
        }

        public UIImageView LeftImageView { get; private set; }
        public TextFieldWithValidator TextFieldWithValidator { get; private set; }

        public void SetClickAction(UITapGestureRecognizer action)
        {
            _clickAction = action;
            TextFieldWithValidator.AddGestureRecognizer(_clickAction);
        }

        void InitObjects()
        {
            LeftImageView = new UIImageView() { ContentMode = UIViewContentMode.ScaleAspectFit };
            TextFieldWithValidator = new TextFieldWithValidator();

            this.AddIfNotNull(LeftImageView);
            this.AddIfNotNull(TextFieldWithValidator);
            this.AddConstraints(
                LeftImageView.AtLeftOf(this, 8),
                LeftImageView.Height().EqualTo(20),
                LeftImageView.Width().EqualTo(20),
                LeftImageView.WithSameCenterY(this).Plus(6),

                TextFieldWithValidator.ToRightOf(LeftImageView, 8),
                TextFieldWithValidator.AtRightOf(this),
                TextFieldWithValidator.AtTopOf(this, 8),
                TextFieldWithValidator.AtBottomOf(this, 8)
            );
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                LeftImageView?.Dispose();
                LeftImageView = null;

                TextFieldWithValidator?.Dispose();
                TextFieldWithValidator = null;

                TextFieldWithValidator?.RemoveGestureRecognizer(_clickAction);
            }

            base.Dispose(disposing);
        }
    }
}