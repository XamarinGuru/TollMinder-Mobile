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
        nfloat _distanceBetweenTextAndImage;

        public UIImage ButtonImage
        {
            get { return _imageView.Image; }
            set { _imageView.Image = value; }
        }

        public UILabel ButtonText
        {
            get { return _buttonText; }
            set { _buttonText = value; }
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
            _distanceBetweenTextAndImage = EnvironmentInfo.GetRoundedButtonDistanceBetweenTextAndImage;
            InitObjects();
        }

        public RoundedButton(nfloat distanceBetweenTextAndImage) : base()
        {
            _distanceBetweenTextAndImage = distanceBetweenTextAndImage == 0 ? EnvironmentInfo.GetRoundedButtonDistanceBetweenTextAndImage
                                                                            : distanceBetweenTextAndImage;
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

            _buttonText.Font = UIFont.FromName("Helvetica", 16f);
            this.AddIfNotNull(_buttonText, _imageView);
            this.Layer.CornerRadius = 30;
            this.UserInteractionEnabled = true;


            this.AddConstraints(
                _imageView.AtTopOf(this, 10),
                _imageView.WithSameCenterX(this),
                _imageView.WithRelativeWidth(this, 0.6f),
                _imageView.WithRelativeHeight(this, 0.6f),

                _buttonText.Below(_imageView),
                _buttonText.WithSameCenterX(this),
                _buttonText.Height().EqualTo(20),
                _buttonText.AtBottomOf(this, _distanceBetweenTextAndImage)
            );
        }
    }
}
