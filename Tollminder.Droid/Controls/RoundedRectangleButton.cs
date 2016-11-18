using System;
using Android.Content;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using MvvmCross.Binding.Droid.Views;

namespace Tollminder.Droid.Controls
{
    [Register("tollminder.droid.controls.RoundedRectangleButton")]
    public class RoundedRectangleButton : LinearLayout
    {
        MvxImageView imageView;
        TextView textView;

        int _image = 0;
        public int Image
        {
            get { return _image; }
            set
            {
                if(_image != value)
                {
                    _image = value;
                    imageView.SetImageResource(_image);
                }
            }
        }

        public string ImageUrl
        {
            get { return imageView.ImageUrl; }
            set { imageView.ImageUrl = value; }
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

        public RoundedRectangleButton(Android.Content.Context context, IAttributeSet attrs) : base(context, attrs)
        {
            Init(context, attrs);
        }

        public RoundedRectangleButton(Android.Content.Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
        {
            Init(context, attrs);
        }

        public RoundedRectangleButton(Android.Content.Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes) : base(context, attrs, defStyleAttr, defStyleRes)
        { 
            Init(context, attrs);
        }

        void Init(Context context, IAttributeSet attrs)
        {
            var inflater = LayoutInflater.FromContext(context);
            inflater.Inflate(Resource.Layout.home_view_button, this);

            imageView = FindViewById<MvxImageView>(Resource.Id.image_view);
            textView = FindViewById<TextView>(Resource.Id.text_view);

            var attr = context.ObtainStyledAttributes(attrs, Resource.Styleable.RoundedRectangleButton);

            var drawable = attr.GetDrawable(Resource.Styleable.RoundedRectangleButton_image);

            if(drawable != null)
                imageView?.SetImageDrawable(drawable);

            Text = attr.GetString(Resource.Styleable.RoundedRectangleButton_text);

            attr.Recycle();
        }
    }
}
