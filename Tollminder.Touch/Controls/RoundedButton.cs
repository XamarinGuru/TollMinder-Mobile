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
        MvxImageView _pointerView;
        UILabel _buttonText;
        UIColor _buttonBackGroundColor;
        UIColor _buttonTextColor;
        nfloat _distanceBetweenTextAndImage;
        nfloat _distanceBetweenTextAndPointer;

        public UIImage ButtonImage
        {
            get { return _imageView.Image; }
            set { _imageView.Image = value; }
        }

        public UIImage ButtonPointer
        {
            get { return _pointerView.Image; }
            set { _pointerView.Image = value; }
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
            _distanceBetweenTextAndPointer = EnvironmentInfo.GetRoundedButtonDistanceBetweenTextAndPointer;

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
            _pointerView = new MvxImageView();

            _buttonText.Font = UIFont.FromName("Helvetica-Bold", 16f);
            this.Layer.CornerRadius = 30;
            this.UserInteractionEnabled = true;

            this.AddIfNotNull(_buttonText, _imageView, _pointerView);
            this.AddConstraints(
                _imageView.AtTopOf(this, 10),
                _imageView.WithSameCenterX(this),
                _imageView.WithRelativeWidth(this, 0.6f),
                _imageView.WithRelativeHeight(this, 0.6f),

                _buttonText.Below(_imageView),
                _buttonText.WithSameCenterX(this),
                _buttonText.Height().EqualTo(20),

                _pointerView.Below(_buttonText),
                _pointerView.WithSameCenterX(this),
                _pointerView.Height().EqualTo(20),
                _pointerView.WithRelativeWidth(this, 0.3f),
                _pointerView.AtBottomOf(this)
            );
        }
    }
}
