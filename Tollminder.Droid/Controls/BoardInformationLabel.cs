using Android.Content;
using Android.Runtime;
using Android.Support.Percent;
using Android.Util;
using Android.Views;
using Android.Widget;
using MvvmCross.Binding.Droid.Views;

namespace Tollminder.Droid.Controls
{
    [Register("tollminder.droid.controls.BoardInformationLabel")]
    public class BoardInformationLabel : PercentRelativeLayout
    {
        MvxImageView iconView;
        TextView labelView;
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

        public string Label
        {
            get { return labelView?.Text; }
            set
            {
                if (labelView != null && labelView.Text != value)
                    labelView.Text = value;
            }
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

        public BoardInformationLabel(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            Init(context, attrs);
        }

        public BoardInformationLabel(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
        {
            Init(context, attrs);
        }

        void Init(Context context, IAttributeSet attrs)
        {
            var inflater = LayoutInflater.FromContext(context);
            inflater.Inflate(Resource.Layout.board_information_label_view, this);

            iconView = FindViewById<MvxImageView>(Resource.Id.icon_board_view);
            textView = FindViewById<TextView>(Resource.Id.text_board_view);
            labelView = FindViewById<TextView>(Resource.Id.label_board_view);

            var attr = context.ObtainStyledAttributes(attrs, Resource.Styleable.BoardInformationLabel);

            iconView?.SetImageDrawable(attr.GetDrawable(Resource.Styleable.BoardInformationLabel_iconBoard));

            Text = attr.GetString(Resource.Styleable.BoardInformationLabel_textBoard);
            Label = attr.GetString(Resource.Styleable.BoardInformationLabel_labelBoard);

            attr.Recycle();
        }
    }

}
