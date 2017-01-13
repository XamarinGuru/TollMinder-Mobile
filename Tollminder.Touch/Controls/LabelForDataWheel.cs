using System;
using Cirrious.FluentLayouts.Touch;
using CoreGraphics;
using Foundation;
using Tollminder.Core;
using Tollminder.Touch.Extensions;
using UIKit;

namespace Tollminder.Touch.Controls
{
    public class LabelForDataWheel : UIButton
    {
        UILabel placeHolder;
        string placeHolderText;
        UILabel wheelText;
        UIColor controlBackGroundColor;
        UIColor labelTextColor;
        UIColor wheelTextColor;
        nfloat distanceBetweenLabelAndWheel;

        public string PlaceHolderText
        {
            get { return placeHolderText; }
            set {
                placeHolderText = value;
                placeHolder.Text = placeHolderText; 
            }
        }

        public UILabel WheelText
        {
            get { return wheelText; }
            set { wheelText = value; }
        }

        public UIColor ControlBackGroundColor
        {
            get { return controlBackGroundColor; }
            set
            {
                controlBackGroundColor = value;
                this.BackgroundColor = controlBackGroundColor;
            }
        }

        public UIColor LabelTextColor
        {
            get { return labelTextColor; }
            set
            {
                labelTextColor = value;
                placeHolder.TextColor = labelTextColor;
            }
        }

        public UIColor WheelTextColor
        {
            get { return wheelTextColor; }
            set
            {
                wheelTextColor = value;
                wheelText.TextColor = wheelTextColor;
            }
        }

        public LabelForDataWheel() : base()
        {
            distanceBetweenLabelAndWheel = EnvironmentInfo.GetLabelDataWheelDistanceBetweenPlaceholderAndWheelText;
            InitObjects();
        }

        public LabelForDataWheel(nfloat distanceBetweenLabelAndWheel) : base()
        {
            distanceBetweenLabelAndWheel = distanceBetweenLabelAndWheel == 0 ? EnvironmentInfo.GetLabelDataWheelDistanceBetweenPlaceholderAndWheelText
                                                                            : distanceBetweenLabelAndWheel;
            InitObjects();
        }

        public LabelForDataWheel(IntPtr handle) : base(handle)
        {
            InitObjects();
        }

        public LabelForDataWheel(NSCoder coder) : base(coder)
        {
            InitObjects();
        }

        public LabelForDataWheel(CGRect rect) : base(rect)
        {
            InitObjects();
        }

        void InitObjects()
        {
            placeHolder = new UILabel();
            wheelText = new UILabel();

            placeHolder.Font = UIFont.FromName("Helvetica", Theme.SmallTextSize);
            wheelText.Font = UIFont.FromName("Helvetica", Theme.MediumTextSize);
            this.AddIfNotNull(placeHolder, wheelText);
            this.UserInteractionEnabled = true;

            this.AddConstraints(
                placeHolder.AtTopOf(this, 7),
                placeHolder.AtLeftOf(this, 20),
                placeHolder.WithRelativeHeight(this, 0.3f),

                wheelText.Below(placeHolder, distanceBetweenLabelAndWheel),
                wheelText.WithRelativeHeight(this, 0.4f),
                wheelText.AtLeftOf(this, 20)
            );
        }
    }
}
