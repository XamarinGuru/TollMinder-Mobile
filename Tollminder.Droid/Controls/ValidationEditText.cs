using System;
using Android.Content;
using Android.Runtime;
using Android.Util;
using Android.Widget;

namespace Tollminder.Droid.Controls
{
    public class ValidationEditText : FrameLayout
    {
        private bool valid = true;
        public ValidationEditText(IntPtr javaRef, JniHandleOwnership transfer) :
             base(javaRef, transfer)
        {
        }

        public ValidationEditText(Context context) :
            base(context)
        {
            Initialize();
        }

        public ValidationEditText(Context context, IAttributeSet attrs) :
            base(context, attrs)
        {
            Initialize();
        }

        public ValidationEditText(Context context, IAttributeSet attrs, int defStyle) :
            base(context, attrs, defStyle)
        {
            Initialize();
        }

        public EditText EditText { get; protected set; }
        public TextView ErrorTextView { get; protected set; }

        void Initialize()
        {
            Inflate(Context, Resource.Layout.validation_edit_text, this);

            ErrorTextView = this.FindViewById<TextView>(Resource.Id.validation_error_text);
            EditText = this.FindViewById<EditText>(Resource.Id.validation_edit_text);
        }

        public void SetError(string errorText)
        {
            if (valid == string.IsNullOrWhiteSpace(errorText))
                return;

            if (string.IsNullOrWhiteSpace(errorText))
            {
                EditText.SetBackgroundResource(Resource.Drawable.edit_text_rounded_background);
                ErrorTextView.Visibility = Android.Views.ViewStates.Invisible;
                valid = true;
            }
            else
            {
                EditText.SetBackgroundResource(Resource.Drawable.edit_text_rounded_error_background);
                ErrorTextView.Visibility = Android.Views.ViewStates.Visible;
                ErrorTextView.Text = errorText;
                valid = false;
            }
        }
    }
}
