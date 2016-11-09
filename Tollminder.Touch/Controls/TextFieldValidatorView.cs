using System;
using Cirrious.FluentLayouts.Touch;
using CoreGraphics;
using Foundation;
using Tollminder.Touch.Extensions;
using UIKit;

namespace Tollminder.Touch.Controls
{
    [Register("TextFieldValidatorView")]
    public class TextFieldValidatorView : UIView
    {
        public TextFieldValidatorView() : base()
        {
            InitObjects();
        }

        public TextFieldValidatorView(IntPtr handle) : base(handle)
        {
            InitObjects();
        }

        public TextFieldValidatorView(NSCoder coder) : base(coder)
        {
            InitObjects();
        }

        public TextFieldValidatorView(CGRect rect) : base(rect)
        {
            InitObjects();
        }

        TextFieldHandleText _textField;
        public TextFieldHandleText TextField
        {
            get { return _textField; }
            set
            {
                _textField = value;
            }
        }

        UILabel _errorMessageLabel;
        public UILabel ErrorMessageLabel { get { return _errorMessageLabel; } set { _errorMessageLabel = value; } }

        public string ErrorMessageString
        {
            get { return ErrorMessageLabel.Text; }
            set
            {
                ErrorMessageLabel.Text = value;
            }
        }

        public UIColor ErrorColor { get; set; } = UIColor.Red;

        void InitObjects()
        {
            TextField = new TextFieldHandleText();
            (TextField as UITextField).ChangeTextFieldStyle(UIFont.FromName("Helvetica", Theme.MediumTextSize), Theme.SmallTextSize, UIColor.Black, false, UITextAlignment.Left);
            TextField.ClearButtonMode = UITextFieldViewMode.WhileEditing;

            _errorMessageLabel = new UILabel().ChangeLabelStyle(UIFont.FromName("Helvetica", Theme.SmallestTextSize), Theme.SmallestTextSize, ErrorColor, false, UITextAlignment.Left);
            _errorMessageLabel.Lines = 0;

            TextField.ResignFirstResponder();

            this.AddIfNotNull(_errorMessageLabel);
            this.AddIfNotNull(_textField);

            this.AddConstraints(
                _textField.AtTopOf(this),
                _textField.AtLeftOf(this, 4),
                _textField.AtRightOf(this, 4),

                _errorMessageLabel.Below(_textField, 4),
                _errorMessageLabel.AtLeftOf(this),
                _errorMessageLabel.AtRightOf(this),
                _errorMessageLabel.Height().EqualTo(15),
                _errorMessageLabel.AtBottomOf(this)
            );
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (TextField != null)
            {
                TextField.Dispose();
                TextField = null;
            }

            ErrorMessageLabel?.Dispose();
            ErrorMessageLabel = null;
        }
    }
}