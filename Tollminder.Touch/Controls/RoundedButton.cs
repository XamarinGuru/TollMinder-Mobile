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
    public class RoundedButton : UIView
    {
        MvxImageView _imageView;
        UILabel _buttonText;
        UIView _separatorView;
        string _labelText;
        string _imageUrl;

        public UIView SeparatorView
        {
            get { return _separatorView; }
            set { _separatorView = value; }
        }

        public string ButtonText
        {
            get { return _labelText; }
            set { 
                _labelText = value;
                
            }
        }

        public string ImageUrl
        {
            get { return _imageUrl; }
            set { _imageUrl = value; }
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
            _buttonText = new UILabel().ChangeLabelStyle(UIFont.FromName("Helvetica", Theme.LargeTextSize), Theme.LargeTextSize, Theme.BlueDark.ToUIColor(), false, UITextAlignment.Center);
            _separatorView = new UIView() { BackgroundColor = Theme.BlueGray50.ToUIColor() };
            _buttonText.Text = _labelText;
            _imageView.ImageUrl = _imageUrl;

            this.AddIfNotNull(_buttonText);
            this.AddIfNotNull(_separatorView);
            this.AddIfNotNull(_imageView);

            this.AddConstraints(

                _buttonText.Below(_imageView),
                _buttonText.AtLeftOf(this),
                _buttonText.AtRightOf(this),
                _buttonText.Height().EqualTo(17),

                _imageView.AtTopOf(this),
                _imageView.AtLeftOf(this),
                _imageView.AtRightOf(this),
                _imageView.Height().EqualTo(20),

                _separatorView.Below(_imageView, 2),
                _separatorView.WithSameWidth(_imageView),
                _separatorView.WithSameLeft(_imageView),
                _separatorView.WithSameRight(_imageView),
                _separatorView.Height().EqualTo(1)
            );
        }
    }
}
