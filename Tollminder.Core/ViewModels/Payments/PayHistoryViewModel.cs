using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using Tollminder.Core.Models.PaymentData;
using Tollminder.Core.Services.Api;
using Tollminder.Core.Services.Notifications;
using Chance.MvvmCross.Plugins.UserInteraction;

namespace Tollminder.Core.ViewModels.Payments
{
    public class PayHistoryViewModel : BaseViewModel
    {
        readonly IPaymentProcessing paymentProcessing;

        private bool isPayHistoryAwailableForUser;

        public MvxObservableCollection<PayHistory> History { get; set; }

        public PayHistoryViewModel(IPaymentProcessing paymentProcessing)
        {
            this.paymentProcessing = paymentProcessing;

            GetPayDateFrom = new DateTime(2016, 10, 5);
            GetPayDateTo = DateTime.Now;

            backHomeCommand = new MvxCommand(() => { ShowViewModel<HomeViewModel>(); });
            backToPayHistoryCommand = new MvxCommand(() => { ShowViewModel<PayHistoryViewModel>(); });

            openCalendarFromCommand = new MvxCommand(async () =>
            {
                GetPayDateFrom = await Mvx.Resolve<ICalendarDialog>().ShowDialogAsync(GetPayDateFrom);
                await LoadHistoryAsync();
            });
            openCalendarToCommand = new MvxCommand(async () =>
            {
                GetPayDateTo = await Mvx.Resolve<ICalendarDialog>().ShowDialogAsync(GetPayDateTo);
                await LoadHistoryAsync();
            });

            downloadHistoryCommand = new MvxCommand(() => ServerCommandWrapperAsync(() => DownloadPdfAsync()));

            History = new MvxObservableCollection<PayHistory>();
        }

        public async override void Start()
        {
            await LoadHistoryAsync();
            base.Start();
        }

        async Task LoadHistoryAsync()
        {
            try
            {
                await Mvx.Resolve<IUserInteraction>().AlertAsync("Your pay history is loading...", "Information");

                var result = await paymentProcessing.GetPayHistoryAsync(GetPayDateFrom, GetPayDateTo);

                if (result != null)
                {
                    isPayHistoryAwailableForUser = true;
                    History.AddRange(result);
                }
                else
                    await Mvx.Resolve<IUserInteraction>().AlertAsync("Sorry, there is no pay history for now.", "Error");
            }
            catch (NullReferenceException ex)
            {
                Debug.WriteLine(ex.Message);
                await Mvx.Resolve<IUserInteraction>().AlertAsync("Sorry, there is no pay history for now.", "Error");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        async Task DownloadPdfAsync()
        {
            if (isPayHistoryAwailableForUser)
            {
                string result = await paymentProcessing.DownloadPayHistoryAsync(getPayDateFrom, getPayDateTo);
                if (result != null)
                    ShowViewModel<PayHistoryPdfViewModel>(new { pdfUrlFromServer = result, pdfNameFromDateRange = GetPdfName() });
            }
            else
                await Mvx.Resolve<IUserInteraction>().AlertAsync("Sorry, there is no pay history for now.", "Error");
        }

        private string selectedItemFromCalendarRadioGroup;
        public string SelectedItemFromCalendarRadioGroup
        {
            get { return selectedItemFromCalendarRadioGroup; }
            set
            {
                SetProperty(ref selectedItemFromCalendarRadioGroup, value);
                RaisePropertyChanged(() => SelectedItemFromCalendarRadioGroup);
            }
        }

        private string getPdfUrl;
        public string GetPdfUrl
        {
            get { return getPdfUrl; }
            set
            {
                SetProperty(ref getPdfUrl, value);
                RaisePropertyChanged(() => GetPdfUrl);
            }
        }
        private DateTime getPayDateFrom;
        public DateTime GetPayDateFrom
        {
            get { return getPayDateFrom; }
            set
            {
                SetProperty(ref getPayDateFrom, value);
                RaisePropertyChanged(() => GetPayDateFrom);
            }
        }

        private DateTime getPayDateTo;
        public DateTime GetPayDateTo
        {
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
