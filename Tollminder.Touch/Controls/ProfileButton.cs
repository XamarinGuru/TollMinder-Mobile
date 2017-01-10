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
    [Register("ProfileButton")]
    public class ProfileButton : UIButton
    {
        MvxImageView iconView;
        MvxImageView arrowView;
        UILabel buttonText;
        UIColor buttonBackGroundColor;
        UIColor buttonTextColor;
        nfloat distanceBetweenTextAndIcon;

        public UIImage ButtonIcon
        {
            get { return iconView.Image; }
            set { iconView.Image = value; }
        }

        public UIImage ButtonArrow
        {
            get { return arrowView.Image; }
            set { arrowView.Image = value; }
        }

        public UILabel ButtonText
        {
            get { return buttonText; }
            set { buttonText = value; }
        }

        public UIColor ButtonBackgroundColor
        {
            get { return buttonBackGroundColor; }
            set { 
                buttonBackGroundColor = value;
                this.BackgroundColor = buttonBackGroundColor;
            }
        }

        public UIColor ButtonTextColor
        {
            get { return buttonTextColor; }
            set { 
                buttonTextColor = value;
                buttonText.TextColor = buttonTextColor;
            }
        }

        public ProfileButton() : base()
        {
            distanceBetweenTextAndIcon = EnvironmentInfo.GetProfileButtonDistanceBetweenTextAndIcon;
            InitObjects();
        }

        public ProfileButton(nfloat distanceBetweenTextAndImage) : base()
        {
            distanceBetweenTextAndIcon = distanceBetweenTextAndImage == 0 ? EnvironmentInfo.GetProfileButtonDistanceBetweenTextAndIcon
                                                                            : distanceBetweenTextAndImage;
            InitObjects();
        }

        public ProfileButton(IntPtr handle) : base(handle)
        {
            InitObjects();
        }

        public ProfileButton(NSCoder coder) : base(coder)
        {
            InitObjects();
        }

        public ProfileButton(CGRect rect) : base(rect)
        {
            InitObjects();
        }

        void InitObjects()
        {
            buttonText = new UILabel();
            iconView = new MvxImageView();
            arrowView = new MvxImageView();

            buttonText.Font = UIFont.FromName("Helvetica", 12f);
            this.AddIfNotNull(buttonText, iconView, arrowView);
            this.Layer.CornerRadius = 10;
            this.UserInteractionEnabled = true;

            this.AddConstraints(
                iconView.WithSameCenterY(this),
                iconView.AtLeftOf(this, 20),
                iconView.WithRelativeWidth(this, 0.1f),
                iconView.WithRelativeHeight(this, 0.3f),

                buttonText.WithSameCenterX(this),
                buttonText.WithSameCenterY(this),
                buttonText.AtLeftOf(iconView, distanceBetweenTextAndIcon),
                buttonText.WithRelativeWidth(this, 0.8f),
                buttonText.WithRelativeHeight(this, 0.4f),

                arrowView.WithSameCenterY(this),
                arrowView.AtRightOf(this, 20),
                arrowView.WithRelativeWidth(this, 0.1f),
                arrowView.WithRelativeHeight(this, 0.3f)
            );
        }
    }
}
