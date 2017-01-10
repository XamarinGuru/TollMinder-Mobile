using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using Tollminder.Core.Models;
using Tollminder.Core.Services;

namespace Tollminder.Core.ViewModels
{
    public class PayHistoryViewModel : BaseViewModel
    {
        readonly IServerApiService serverApiService;
        private DateTime dateFromCalendarView;

        public PayHistoryViewModel()
        {
            serverApiService = Mvx.Resolve<IServerApiService>();

            backHomeCommand = new MvxCommand(() => { ShowViewModel<HomeViewModel>(); });
            backToPayHistoryCommand = new MvxCommand(() => { ShowViewModel<PayHistoryViewModel>(); });

            openCalendarFromCommand = new MvxCommand(async () => {
                GetPayDateFrom = await Mvx.Resolve<ICalendarDialog>().ShowDialog();
            });
            openCalendarToCommand = new MvxCommand(async () =>
            {
                GetPayDateTo = await Mvx.Resolve<ICalendarDialog>().ShowDialog();
            });

            downloadHistoryCommand = new MvxCommand(async () => { 
                History = await serverApiService.GetPayHistory("58625cb651d7202e7dfaa69c", getPayDateFrom, getPayDateTo);
            });
            //States = dataBaseService.GetStates();
        }

        async Task DownloadHistory()
        {
            await serverApiService.GetPayHistory("58625cb651d7202e7dfaa69c", getPayDateFrom, getPayDateTo);
        }

        public override void Start()
        {
            //DownloadHistory();
            base.Start();

        }
        private DateTime getPayDateFrom;
        public DateTime GetPayDateFrom { 
            get { return getPayDateFrom; } 
            set 
            {
                SetProperty(ref getPayDateFrom, value);
                RaisePropertyChanged(() => GetPayDateFrom);
            }
        }

        private DateTime getPayDateTo;
        public DateTime GetPayDateTo { 
            get { return getPayDateTo; }
            set
            {
                SetProperty(ref getPayDateTo, value);
                RaisePropertyChanged(() => GetPayDateTo);
            }
        }

        private MvxCommand backToPayHistoryCommand;
        public ICommand BackToPayHistoryCommand { get { return backToPayHistoryCommand; } }
       
        private MvxCommand backHomeCommand;
        public ICommand BackHomeCommand { get { return backHomeCommand; } }

        private MvxCommand downloadHistoryCommand;
        public ICommand DownloadHistoryCommand { get { return downloadHistoryCommand; } }

        private MvxCommand openCalendarFromCommand;
        public ICommand OpenCalendarFromCommand { get { return openCalendarFromCommand; } }

        private MvxCommand openCalendarToCommand;
        public ICommand OpenCalendarToCommand { get { return openCalendarToCommand; } }

        private PayHistoryTrips history;
        public PayHistoryTrips History
        {
            get { return history; }
            set
            {
                SetProperty(ref history, value);
                RaisePropertyChanged(() => History);
            }
        }

        private List<StatesData> states;
        public List<StatesData> States
        {
            get { return states; }
            set
            {
                SetProperty(ref states, value);
                RaisePropertyChanged(() => States);
            }
        }
    }
}
