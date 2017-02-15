using System;
using UIKit;

namespace Tollminder.Touch.Helpers.MvxAlertActionHelpers
{
    public class MvxAlertAction: UIAlertAction
    {
        public UIAlertAction AlertAction;

        public MvxAlertAction(string title, UIAlertActionStyle style)
        {
            AlertAction = UIAlertAction.Create(title, style, action =>
                {
                    if (Clicked != null)
                    {
                        Clicked(this, null);
                    }
                });
        }

        public event EventHandler Clicked;
    }
}
