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
        readonly IDataBaseService dataBaseService;
        readonly IServerApiService serverApiService;

        public PayHistoryViewModel()
        {
            dataBaseService = Mvx.Resolve<IDataBaseService>();
            serverApiService = Mvx.Resolve<IServerApiService>();

            backHomeCommand = new MvxCommand(() => { ShowViewModel<HomeViewModel>(); });
            openCalendarCommand = new MvxCommand(() => { ShowViewModel<CalendarViewModel>(); });
            downloadHistoryCommand = new MvxCommand(async () => { 
                history = await serverApiService.GetPayHistory("58625cb651d7202e7dfaa69c", "10/1/2016", "1/5/2017"); 
            });
            //states = dataBaseService.GetStates();
        }

        async Task DownloadHistory()
        {
        }

        public async override void Start()
        {
            //history = await serverApiService.GetPayHistory("58625cb651d7202e7dfaa69c", "10/1/2016", "1/5/2017");
            //Task.Run(DownloadHistory);
            base.Start();
        }

        private MvxCommand backHomeCommand;
        public ICommand BackHomeCommand { get { return backHomeCommand; } }

        private MvxCommand downloadHistoryCommand;
        public ICommand DownloadHistoryCommand { get { return downloadHistoryCommand; } }

        private MvxCommand openCalendarCommand;
        public ICommand OpenCalendarCommand { get { return openCalendarCommand; } }

        private IList<PayHistory> history;
        public IList<PayHistory> History
        {
            get { return history; }
            set
            {
                history = value;
                RaisePropertyChanged(() => History);
            }
        }

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
