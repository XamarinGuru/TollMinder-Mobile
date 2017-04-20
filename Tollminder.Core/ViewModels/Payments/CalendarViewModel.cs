using System;
using System.Windows.Input;
using MvvmCross.Core.ViewModels;

namespace Tollminder.Core.ViewModels.Payments
{
    public class CalendarViewModel : BaseViewModel
    {
        public CalendarViewModel()
        {
            backToPayHistoryCommand = new MvxCommand(() =>
            {
                Close(this);
                ShowViewModel<PayHistoryViewModel>();
            });
        }

        public override void Start()
        {
            base.Start();
        }

        private MvxCommand backToPayHistoryCommand;
        public ICommand BackToPayHistoryCommand
        {
            get
            {
                DialogVisible = false;
                return backToPayHistoryCommand;
            }
        }

        private bool dialogVisible;
        public bool DialogVisible
        {
            get { return dialogVisible; }
            set
            {
                dialogVisible = value;
                RaisePropertyChanged(() => DialogVisible);
            }
        }
    }
}
