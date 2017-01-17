using System;
using System.Collections.Generic;
using System.Text;
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
        readonly IStoredSettingsService storedSettingsService;

        public PayHistoryViewModel()
        {
            serverApiService = Mvx.Resolve<IServerApiService>();
            storedSettingsService = Mvx.Resolve<IStoredSettingsService>();

            GetPayDateFrom = new DateTime(2016, 10, 5);
            GetPayDateTo = DateTime.Now;

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
                GetPdfUrl = await serverApiService.DownloadPayHistory(storedSettingsService.ProfileId, getPayDateFrom, getPayDateTo);
            });
        }

        async Task DownloadHistory()
        {
            History = await serverApiService.GetPayHistory(storedSettingsService.ProfileId, getPayDateFrom, getPayDateTo);
        }

        public override void Start()
        {
            DownloadHistory();
            base.Start();
        }
        private string getPdfUrl;
        public string GetPdfUrl { get { return getPdfUrl;} 
            set{
                SetProperty(ref getPdfUrl, value);
                RaisePropertyChanged(() => GetPdfUrl);
                ShowViewModel<PayHistoryPdfViewModel>(new { pdfUrlFromServer = GetPdfUrl,  pdfNameFromDateRange = GetPdfName() });
            }
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

        private List<PayHistory> history;
        public List<PayHistory> History
        {
            get { return history; }
            set
            {
                SetProperty(ref history, value);
                RaisePropertyChanged(() => History);
            }
        }

        private PayHistory selectedTransaction;
        public PayHistory SelectedTransaction
        {
            get { return selectedTransaction; }
            set
            {
                SetProperty(ref selectedTransaction, value);
                RaisePropertyChanged(() => SelectedTransaction);
            }
        }

        public string Transaction
        {
            get { return SelectedTransaction.ToString(); }
        }

        private string GetPdfName()
        {
            StringBuilder pdfName = new StringBuilder();
            pdfName.Append("payhistory_");
            pdfName.Append(GetPayDateFrom.ToString("u"));
            pdfName.Append("_");
            pdfName.Append(GetPayDateTo.ToString("u"));

            return pdfName.ToString();
        }
    }
}
