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
    [Register("tollminder.droid.controls.CalendarRangeButton")]
    public class CalendarRangeButton : PercentRelativeLayout
    {
        MvxImageView iconView;
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

        public string Text
        {
            get { return textView?.Text; }
            set
            {
                if (textView != null && textView.Text != value)
                    textView.Text = value;
            }
        }

        public CalendarRangeButton(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            Init(context, attrs);
        }

        public CalendarRangeButton(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
        {
            Init(context, attrs);
        }

        void Init(Context context, IAttributeSet attrs)
        {
            var inflater = LayoutInflater.FromContext(context); 
            inflater.Inflate(Resource.Layout.calendar_view_button, this);

            iconView = FindViewById<MvxImageView>(Resource.Id.icon_range_view);
            textView = FindViewById<TextView>(Resource.Id.text_range_view);

            var attr = context.ObtainStyledAttributes(attrs, Resource.Styleable.CalendarRangeButton);

            iconView?.SetImageDrawable(attr.GetDrawable(Resource.Styleable.CalendarRangeButton_icon_calendar));

            Text = attr.GetString(Resource.Styleable.CalendarRangeButton_text_calendar_field);

            attr.Recycle();
        }
    }

}
