using System;
using System.ComponentModel;
using Cirrious.FluentLayouts.Touch;
using CoreGraphics;
using Foundation;
using MvvmCross.Binding.iOS.Views;
using Tollminder.Core;
using Tollminder.Touch.Extensions;
using UIKit;

namespace Tollminder.Touch.Controls
{
    [Register("BoardField"), DesignTimeVisible(true)]
    public class BoardField : UIView
    {
        MvxImageView iconView;
        UILabel labelText;
        UIColor labelTextColor;
        nfloat distanceBetweenIconAndLabel;
        UILabel valueText;
        UIColor valueTextColor;
        nfloat distanceBetweenLabelAndValue;

        public UIImage FieldIcon
        {
            get { return iconView.Image; }
            set { iconView.Image = value; }
        }

        public UILabel LabelText
        {
            get { return labelText; }
            set { labelText = value; }
        }

        public UIColor LabelTextColor
        {
            get { return labelTextColor; }
            set
            {
                labelTextColor = value;
                labelText.TextColor = labelTextColor;
            }
        }

        public UILabel ValueText
        {
            get { return valueText; }
            set
            {
                valueText = value;
                valueText.Lines = 2;
            }
        }

        public UIColor ValueTextColor
        {
            get { return valueTextColor; }
            set
            {
                valueTextColor = value;
                valueText.TextColor = valueTextColor;
            }
        }

        public BoardField() : base()
        {
            distanceBetweenIconAndLabel = EnvironmentInfo.GetProfileButtonDistanceBetweenTextAndIcon;
            InitObjects();
        }

        public BoardField(nfloat distanceBetweenImageAndLabel, nfloat distanceBetweenLabelAndValue) : base()
        {
            this.distanceBetweenIconAndLabel = distanceBetweenImageAndLabel == 0 ? EnvironmentInfo.GetProfileButtonDistanceBetweenTextAndIcon
                                                                            : distanceBetweenImageAndLabel;
            this.distanceBetweenLabelAndValue = distanceBetweenLabelAndValue;
            InitObjects();
        }

        public BoardField(IntPtr handle) : base(handle)
        {
            InitObjects();
        }

        public BoardField(NSCoder coder) : base(coder)
        {
            InitObjects();
        }

        public BoardField(CGRect rect) : base(rect)
        {
            InitObjects();
        }

        void InitObjects()
        {
            iconView = new MvxImageView();
            labelText = new UILabel();
            valueText = new UILabel();
            labelText.Font = UIFont.BoldSystemFontOfSize(14f);
            this.AddIfNotNull(iconView, labelText, valueText);

            this.AddConstraints(
                iconView.WithSameCenterY(this),
                iconView.AtLeftOf(this, 20),
                iconView.WithRelativeWidth(this, 0.06f),
                iconView.WithRelativeHeight(this, 0.5f),

                labelText.WithSameCenterX(this),
                labelText.WithSameCenterY(this),
                labelText.AtLeftOf(iconView, distanceBetweenIconAndLabel),
                labelText.WithRelativeWidth(this, 0.6f),
                labelText.WithRelativeHeight(this, 0.4f),

                valueText.WithSameCenterY(this),
                valueText.AtLeftOf(labelText, distanceBetweenLabelAndValue),
                //valueText.WithRelativeWidth(this, 0.1f),
                valueText.WithRelativeHeight(this, 0.5f)
            );
        }
    }
}
