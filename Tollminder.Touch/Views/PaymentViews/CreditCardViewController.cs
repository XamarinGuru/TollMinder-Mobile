using System;
using Tollminder.Touch.Controllers;
using Tollminder.Core.ViewModels.Payments;

namespace Tollminder.Touch.Views
{
    public partial class CreditCardViewController : BaseViewController<AddCreditCardViewModel>
    {
        public CreditCardViewController() : base("CreditCardViewController", null)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            // Perform any additional setup after loading the view, typically from a nib.
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }
    }
}

