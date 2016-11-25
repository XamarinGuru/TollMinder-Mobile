using System;
using Cirrious.FluentLayouts.Touch;
using CoreGraphics;
using Foundation;
using MvvmCross.Binding.iOS.Views;
using Tollminder.Core;
using Tollminder.Touch.Extensions;
using UIKit;

namespace Tollminder.Touch.Controls
{
    [Register("RoundedButton")]
    public class RoundedButton : UIButton
    {
        MvxImageView _imageView;
        UILabel _buttonText;
        UIColor _buttonBackGroundColor;
        UIColor _buttonTextColor;
        //string _labelText;
        //UIImage _imageUrl;

        public string ButtonText
        {
            get { return _buttonText.Text; }
            set { _buttonText.Text = value; }
        }

        public UIImage ButtonImage
        {
            get { return _imageView.Image; }
            set { _imageView.Image = value; }
        }

        public UIColor ButtonBackgroundColor
        {
            get { return _buttonBackGroundColor; }
            set { 
                _buttonBackGroundColor = value;
                this.BackgroundColor = _buttonBackGroundColor;
            }
        }

        public UIColor ButtonTextColor
        {
            get { return _buttonTextColor; }
            set { 
                _buttonTextColor = value;
                _buttonText.TextColor = _buttonTextColor;
            }
        }

        public RoundedButton() : base()
        {
            InitObjects();
        }

        public RoundedButton(IntPtr handle) : base(handle)
        {
            InitObjects();
        }

        public RoundedButton(NSCoder coder) : base(coder)
        {
            InitObjects();
        }

        public RoundedButton(CGRect rect) : base(rect)
        {
            InitObjects();
        }

        void InitObjects()
        {
            _buttonText = new UILabel();
            _imageView = new MvxImageView();

            _buttonText.Font = UIFont.FromName("Helvetica", 13f);
            this.AddIfNotNull(_buttonText, _imageView);
            this.Layer.CornerRadius = 20;

            this.AddConstraints(
                _imageView.AtTopOf(this, 10),
                _imageView.WithSameCenterX(this),
                _imageView.Height().EqualTo(80),
                _imageView.Width().EqualTo(80),

                _buttonText.Below(_imageView, 10),
                _buttonText.WithSameCenterX(this),
                _buttonText.Height().EqualTo(15),
                _buttonText.AtBottomOf(this, 10)
            );
        }
    }
}
