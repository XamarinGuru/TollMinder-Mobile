using System;
using Android.Content;
using Android.Runtime;
using Android.Support.Percent;
using Android.Util;
using Android.Views;
using Android.Widget;
using MvvmCross.Binding.Droid.Views;

namespace Tollminder.Droid.Controls
{
    [Register("tollminder.droid.controls.ProfileButton")]
    public class ProfileButton : PercentRelativeLayout
    {
        MvxImageView iconView;
        MvxImageView arrowView;
        TextView textView;

        int icon = 0;
        public int Icon
        {
            get { return icon; }
            set
            {
                if (icon != value)
                {
                    icon = value;
                    iconView.SetImageResource(icon);
                }
            }
        }

        public string IconUrl
        {
            get { return iconView.ImageUrl; }
            set { iconView.ImageUrl = value; }
        }

        int arrow = 0;
        public int Arrow
        {
            get { return arrow; }
            set
            {
                if (arrow != value)
                {
                    arrow = value;
                    iconView.SetImageResource(arrow);
                }
            }
        }

        public string ArrowUrl
        {
            get { return arrowView.ImageUrl; }
            set { arrowView.ImageUrl = value; }
        }

        public string Text
        {
            get { return textView?.Text; }
            set
            {
                if (textView != null && textView.Text != value)
                    textView.Text = value;
            }
        }

        public ProfileButton(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            Init(context, attrs);
        }

        public ProfileButton(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
        {
            Init(context, attrs);
        }

        void Init(Context context, IAttributeSet attrs)
        {
            var inflater = LayoutInflater.FromContext(context);
            inflater.Inflate(Resource.Layout.profile_view_button, this);

            iconView = FindViewById<MvxImageView>(Resource.Id.icon_view);
            textView = FindViewById<TextView>(Resource.Id.text_view);
            //arrowView = FindViewById<MvxImageView>(Resource.Id.arrow_view);

            var attr = context.ObtainStyledAttributes(attrs, Resource.Styleable.ProfileButton);

            iconView?.SetImageDrawable(attr.GetDrawable(Resource.Styleable.ProfileButton_icon_profile));
            //arrowView?.SetImageDrawable(attr.GetDrawable(Resource.Styleable.ProfileButton_arrow_profile));

            Text = attr.GetString(Resource.Styleable.ProfileButton_text_profile);

            attr.Recycle();
        }
    }
}
