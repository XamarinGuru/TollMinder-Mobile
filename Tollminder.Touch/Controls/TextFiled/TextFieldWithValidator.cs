using System;
using Cirrious.FluentLayouts.Touch;
using CoreGraphics;
using Foundation;
using Tollminder.Core;
using Tollminder.Touch.Extensions;
using UIKit;

namespace Tollminder.Touch.Controls
{
    [Register("TextFieldWithValidator")]
    public class TextFieldWithValidator : UIView
    {
        UILabel _topLabel;
        UIColor topLabelColor;
        public UIColor ErrorColor { get; set; } = UIColor.Red;

        TextFieldHandleText _textField;
        public TextFieldHandleText TextField
        {
            get { return _textField; }
            set
            {
                _textField = value;
                //HandleTextField(this, EventArgs.Empty);
            }
        }

        UILabel _errorMessageLabel;
        public UILabel ErrorMessageLabel { get { return _errorMessageLabel; } set { _errorMessageLabel = value; } }

        public UILabel TopLabel { get { return _topLabel; } set { _topLabel = value; } }

        public UIColor TopLabelColor{ 
            get { 
                return topLabelColor == null ? Theme.MainBlue.ToUIColor() : topLabelColor;
            } set { topLabelColor = value; } 
        }

        UIView _separatorView;
        public UIView SeparatorView { get { return _separatorView; } set { _separatorView = value; } }

        public string ErrorMessageString
        {
            get { return ErrorMessageLabel.Text; }
            set
            {
                ErrorMessageLabel.Text = value;
                SeparatorView.BackgroundColor = string.IsNullOrEmpty(value) ? Theme.BlueGray50.ToUIColor() : ErrorColor;
            }
        }

        public TextFieldWithValidator() : base()
        {
            InitObjects();
        }

        public TextFieldWithValidator(IntPtr handle) : base(handle)
        {
            InitObjects();
        }

        public TextFieldWithValidator(NSCoder coder) : base(coder)
        {
            InitObjects();
        }

        public TextFieldWithValidator(CGRect rect) : base(rect)
        {
            InitObjects();
        }

        void InitObjects()
        {
            _topLabel = new UILabel().ChangeLabelStyle(UIFont.FromName("Helvetica", Theme.SmallTextSize), Theme.SmallestTextSize, topLabelColor, false, UITextAlignment.Left);

            TextField = new TextFieldHandleText();
            (TextField as UITextField).ChangeTextFieldStyle(UIFont.FromName("Helvetica", Theme.MediumTextSize), Theme.SmallTextSize, UIColor.Black, false, UITextAlignment.Left);
            TextField.ClearButtonMode = UITextFieldViewMode.WhileEditing;
            TextField.InitTextBindHandle = (() =>
                _topLabel.Text = string.IsNullOrEmpty(TextField?.Text) ? string.Empty : TextField?.Placeholder
            );

            _topLabel.Text = TextField.Placeholder;

            _errorMessageLabel = new UILabel().ChangeLabelStyle(UIFont.FromName("Helvetica", Theme.SmallestTextSize), Theme.SmallestTextSize, ErrorColor, false, UITextAlignment.Left);
            _errorMessageLabel.Lines = 0;

            _separatorView = new UIView() { BackgroundColor = Theme.BlueGray50.ToUIColor() };
            TextField.ResignFirstResponder();

            TextField.AddTarget(HandleTextField, UIControlEvent.EditingChanged);

            this.AddIfNotNull(_topLabel);
            this.AddIfNotNull(_separatorView);
            this.AddIfNotNull(_errorMessageLabel);
            this.AddIfNotNull(_textField);

            this.AddConstraints(

                _topLabel.AtTopOf(this),
                _topLabel.AtLeftOf(this),
                _topLabel.AtRightOf(this),
                _topLabel.WithRelativeHeight(this, 0.3f),

                _textField.Below(_topLabel, 2),
                _textField.AtLeftOf(this),
                _textField.AtRightOf(this),
                _textField.WithRelativeHeight(this, 0.5f),

                _separatorView.Below(_textField, 2),
                _separatorView.WithSameWidth(_textField),
                _separatorView.WithSameLeft(_textField),
                _separatorView.WithSameRight(_textField),
                _separatorView.Height().EqualTo(1),

                _errorMessageLabel.Below(_separatorView, 4),
                _errorMessageLabel.AtLeftOf(this),
                _errorMessageLabel.AtRightOf(this),
                _errorMessageLabel.AtBottomOf(this)
            );
        }

        void HandleTextField(object sender, EventArgs args)
        {
            _topLabel.Text = string.IsNullOrEmpty(TextField.Text) ? string.Empty : TextField.Placeholder;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (TextField != null)
                {
                    //TextField.RemoveTarget(HandleTextField, UIControlEvent.EditingChanged);
                    TextField.InitTextBindHandle = null;
                    TextField.Dispose();
                    TextField = null;
                }

                ErrorMessageLabel?.Dispose();
                ErrorMessageLabel = null;
                _topLabel?.Dispose();
                _topLabel = null;
                SeparatorView?.Dispose();
                SeparatorView = null;
            }
            base.Dispose(disposing);
        }
    }
}
