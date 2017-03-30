using Tollminder.Core.ViewModels.Payments;
using UIKit;
using MvvmCross.Binding.BindingContext;
using MvvmCross.iOS.Views;
using Tollminder.Touch.Controls;

namespace Tollminder.Touch.Views
{
    public partial class AddCreditCardViewController : MvxViewController<AddCreditCardViewModel>
    {
        public AddCreditCardViewController() : base("AddCreditCardViewController", null)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            View.BackgroundColor = UIColor.FromPatternImage(UIImage.FromFile(@"Images/tab_background.png").Scale(View.Frame.Size));
            AcceptedCardsImageView.Image = UIImage.FromFile(@"Images/CreditCardView/creditCards.png");
            AddCreditCardNavigationItem.Title = "Add Your Credit Card";
            AddCreditCardNavigationBar.TitleTextAttributes = new UIStringAttributes() { ForegroundColor = UIColor.White };
            AddCreditCardNavigationItem.LeftBarButtonItem = new UIBarButtonItem(UIImage.FromFile("Images/ic_back.png"), UIBarButtonItemStyle.Plain, null);

            InitializeBindings();
            GestureRegnizer();
            AddDoneButtonOnKeyBoard();
        }

        private void GestureRegnizer()
        {
            var gestureRecognizer = new UITapGestureRecognizer(() =>
            {
                NameOnCardTextField.ResignFirstResponder();
                CardNumberTextField.ResignFirstResponder();
                ExpirationMonthTextField.ResignFirstResponder();
                ExpirationYearTextField.ResignFirstResponder();
                CvvTextField.ResignFirstResponder();
                ZipCodeTextField.ResignFirstResponder();
            });
            View.AddGestureRecognizer(gestureRecognizer);
        }

        private void AddDoneButtonOnKeyBoard()
        {
            NameOnCardTextField.InputAccessoryView = new EnhancedToolbar(NameOnCardTextField, null, CardNumberTextField);
            CardNumberTextField.InputAccessoryView = new EnhancedToolbar(CardNumberTextField, NameOnCardTextField, ExpirationMonthTextField);
            ExpirationMonthTextField.InputAccessoryView = new EnhancedToolbar(ExpirationMonthTextField, CardNumberTextField, ExpirationYearTextField);
            ExpirationYearTextField.InputAccessoryView = new EnhancedToolbar(ExpirationYearTextField, ExpirationMonthTextField, CvvTextField);
            CvvTextField.InputAccessoryView = new EnhancedToolbar(CvvTextField, ExpirationYearTextField, ZipCodeTextField);
            ZipCodeTextField.InputAccessoryView = new EnhancedToolbar(ZipCodeTextField, CvvTextField, null);
        }

        private void InitializeBindings()
        {
            var bindingSet = this.CreateBindingSet<AddCreditCardViewController, AddCreditCardViewModel>();
            bindingSet.Bind(NameOnCardTextField).To(vm => vm.CardHolder);
            bindingSet.Bind(CardNumberTextField).To(vm => vm.CreditCardNumber);
            bindingSet.Bind(ExpirationMonthTextField).To(vm => vm.ExpirationMonth);
            bindingSet.Bind(ExpirationYearTextField).To(vm => vm.ExpirationYear);
            bindingSet.Bind(CvvTextField).To(vm => vm.Cvv);
            bindingSet.Bind(ZipCodeTextField).To(vm => vm.ZipCode);
            bindingSet.Bind(AddCreditCardNavigationItem.LeftBarButtonItem).To(vm => vm.CloseAddCreditCardCommand);
            bindingSet.Bind(SaveCreditCardButton).To(vm => vm.SaveCreditCardCommand);
            bindingSet.Apply();
        }
    }
}

