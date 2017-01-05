using System;
using System.Collections.Generic;
using System.Windows.Input;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using Tollminder.Core.Models;
using Tollminder.Core.Services;

namespace Tollminder.Core.ViewModels
{
    public class PayHistoryViewModel : BaseViewModel
    {
        readonly IDataBaseService dataBaseService;

        public PayHistoryViewModel()
        {
            dataBaseService = Mvx.Resolve<IDataBaseService>();
            backHomeCommand = new MvxCommand(() => { ShowViewModel<HomeViewModel>(); });
            states = dataBaseService.GetStates();
        }

        public override void Start()
        {
            base.Start();
        }

        private MvxCommand backHomeCommand;
        public ICommand BackHomeCommand { get { return backHomeCommand; } }

        private List<StatesData> states;
        public List<StatesData> States
        {
            get { return states; }
            set
            {
                states = value;
                RaisePropertyChanged(() => States);
            }
        }
    }
}
