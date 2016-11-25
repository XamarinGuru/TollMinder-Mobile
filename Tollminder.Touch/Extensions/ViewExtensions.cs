using System;
using UIKit;
using Cirrious.FluentLayouts.Touch;

namespace Tollminder.Touch.Extensions
{
    public static class ViewExtensions
    {
        /// <summary>
        /// Find the first responder in the <paramref name="view"/>'s subview hierarchy
        /// </summary>
        /// <param name="view">
        /// A <see cref="UIView"/>
        /// </param>
        /// <returns>
        /// A <see cref="UIView"/> that is the first responder or null if there is no first responder
        /// </returns>
        public static UIView FindFirstResponder(this UIView view)
        {
            if (view.IsFirstResponder)
            {
                return view;
            }
            foreach (UIView subView in view.Subviews)
            {
                var firstResponder = subView.FindFirstResponder();
                if (firstResponder != null)
                    return firstResponder;
            }
            return null;
        }

        /// <summary>
        /// Find the first Superview of the specified type (or descendant of)
        /// </summary>
        /// <param name="view">
        /// A <see cref="UIView"/>
        /// </param>
        /// <param name="stopAt">
        /// A <see cref="UIView"/> that indicates where to stop looking up the superview hierarchy
        /// </param>
        /// <param name="type">
        /// A <see cref="Type"/> to look for, this should be a UIView or descendant type
        /// </param>
        /// <returns>
        /// A <see cref="UIView"/> if it is found, otherwise null
        /// </returns>
        public static UIView FindSuperviewOfType(this UIView view, UIView stopAt, Type type)
        {
            if (view.Superview != null)
            {
                if (type.IsInstanceOfType(view.Superview))
                {
                    return view.Superview;
                }

                if (view.Superview != stopAt)
                    return view.Superview.FindSuperviewOfType(stopAt, type);
            }

            return null;
        }

        public static UIView AddTopBottomLine(this UIView view, UIColor color, int height)
        {
            UIView _top = new UIView();
            _top.BackgroundColor = color;

            UIView _bottom = new UIView();
            _bottom.BackgroundColor = color;

            view.AddIfNotNull(_top, _bottom);
            view.AddConstraints(
                _top.AtTopOf(view),
                _top.AtLeftOf(view),
                _top.AtRightOf(view),
                _top.Height().EqualTo(height),

                _bottom.AtBottomOf(view),
                _bottom.AtLeftOf(view),
                _bottom.AtRightOf(view),
                _bottom.Height().EqualTo(height)
            );

            return view;
        }

        public static UILabel ChangeLabelStyle(this UILabel label, UIFont font, nfloat minimumSize, UIColor textColor, bool translateResize, UITextAlignment aligment)
        {
            label.TextAlignment = aligment;
            label.Font = font;
            label.MinimumFontSize = minimumSize;
            label.TextColor = textColor;
            label.TranslatesAutoresizingMaskIntoConstraints = translateResize;
            return label;
        }

        public static UILabel ChangeLabelStyle(this UILabel label, UIFont font, nfloat minimumSize, UIColor textColor, bool translateResize, UITextAlignment aligment, bool shrinkText)
        {
            label.AdjustsFontSizeToFitWidth = shrinkText;
            label.TextAlignment = aligment;
            label.Font = font;
            label.MinimumFontSize = minimumSize;
            label.TextColor = textColor;
            label.TranslatesAutoresizingMaskIntoConstraints = translateResize;
            return label;
        }

        public static UITextField ChangeTextFieldStyle(this UITextField textField, UIFont font, nfloat minimumSize, UIColor textColor, bool translateResize, UITextAlignment aligment)
        {
            textField.TextAlignment = aligment;
            textField.Font = font;
            textField.MinimumFontSize = minimumSize;
            textField.TextColor = textColor;
            textField.TranslatesAutoresizingMaskIntoConstraints = translateResize;
            return textField;
        }

        public static UITextView ChangeTextViewStyle(this UITextView textView, UIFont font, nfloat minimumSize, UIColor textColor, bool translateResize, UITextAlignment aligment)
        {
            textView.TextAlignment = aligment;
            textView.Font = font;
            textView.TextColor = textColor;
            textView.TranslatesAutoresizingMaskIntoConstraints = translateResize;
            return textView;
        }
    }
}
