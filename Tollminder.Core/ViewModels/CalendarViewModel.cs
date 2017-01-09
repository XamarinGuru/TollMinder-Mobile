using System;
using System.Windows.Input;
using MvvmCross.Core.ViewModels;

namespace Tollminder.Core.ViewModels
{
    public class CalendarViewModel : BaseViewModel
    {
        public CalendarViewModel()
        {
            backToPayHistoryCommand = new MvxCommand(() => { ShowViewModel<PayHistoryViewModel>(); });
        }

        public override void Start()
        {
            base.Start();
        }

        private MvxCommand backToPayHistoryCommand;
        public ICommand BackToPayHistoryCommand { get { return backToPayHistoryCommand; }}
    } 
}
