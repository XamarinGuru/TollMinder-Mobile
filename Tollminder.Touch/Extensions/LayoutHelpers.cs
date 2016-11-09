using System;
using Cirrious.FluentLayouts.Touch;
using CoreGraphics;
using UIKit;

namespace Tollminder.Touch.Extensions
{
    public static class LayoutHelpers
    {
        public static T CreateAndAdd<T>(this UIView container)
            where T : UIView, new()
        {
            return CreateAndAdd(container, new T());
        }

        public static T CreateAndAdd<T>(this UIView container, T view)
            where T : UIView
        {
            container.AddIfNotNull(view);
            return view.Normalize();
        }

        public static T Normalize<T>(this T view)
            where T : UIView
        {
            view.TranslatesAutoresizingMaskIntoConstraints = false;
            return view;
        }

        public static T AddIfNotNull<T>(this T container, UIView view)
            where T : UIView
        {
            if (view != null)
            {
                view.Normalize();
                container.Add(view);
            }
            return container;
        }

        public static T AddIfNotNull<T>(this T container, params UIView[] views)
            where T : UIView
        {
            foreach (var view in views)
            {
                container.AddIfNotNull(view);
            }
            return container;
        }

        public static void SetButton(this UIButton button, string title, UIColor normal, UIColor selected)
        {
            button.SetTitle(title, UIControlState.Normal);
            button.SetTitleColor(normal, UIControlState.Normal);
            button.SetTitleColor(selected, UIControlState.Selected);
        }

        public static void AddAndFillSubview(this UIView childView, UIView parentView)
        {
            childView.AddConstraints(
                parentView.WithSameTop(childView),
                parentView.WithSameLeft(childView),
                parentView.WithSameWidth(childView),
                parentView.WithSameHeight(childView)
            );
        }

        public static void AddBottomLine(this UIView view, UIColor lineColor, int lineHeight = 1)
        {
            var lineView = new UIView(new CGRect(0, view.Frame.Height - lineHeight, view.Frame.Width, lineHeight));
            lineView.BackgroundColor = lineColor;
            view.AddSubview(lineView);
        }

        public static void MakeViewRound(this UIView view)
        {
            view.ClipsToBounds = true;
            view.Layer.CornerRadius = view.Frame.Height / 2;
        }
    }
}