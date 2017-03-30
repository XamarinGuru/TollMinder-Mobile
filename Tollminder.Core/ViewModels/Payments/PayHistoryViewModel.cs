using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using MvvmCross.Core.ViewModels;
using Tollminder.Core.Models.PaymentData;
using Tollminder.Core.Services.Api;
using Tollminder.Core.Services.Notifications;
using Tollminder.Core.Services.Settings;

namespace Tollminder.Core.ViewModels.Payments
{
    public class PayHistoryViewModel : BaseViewModel
    {
        readonly IPaymentProcessing paymentProcessing;
        readonly IStoredSettingsService storedSettingsService;
        readonly ICalendarDialog calendarDialog;
        readonly IProgressDialogManager progressDialogManager;

        private bool isPayHistoryAwailableForUser;

        public PayHistoryViewModel(IPaymentProcessing paymentProcessing, IStoredSettingsService storedSettingsService, ICalendarDialog calendarDialog, IProgressDialogManager progressDialogManager)
        {
            this.paymentProcessing = paymentProcessing;
            this.storedSettingsService = storedSettingsService;
            this.calendarDialog = calendarDialog;
            this.progressDialogManager = progressDialogManager;

            GetPayDateFrom = new DateTime(2016, 10, 5);
            GetPayDateTo = DateTime.Now;

            backHomeCommand = new MvxCommand(() => { ShowViewModel<HomeViewModel>(); });
            backToPayHistoryCommand = new MvxCommand(() => { ShowViewModel<PayHistoryViewModel>(); });

            openCalendarFromCommand = new MvxCommand(async () =>
            {
                GetPayDateFrom = await calendarDialog.ShowDialogAsync(GetPayDateFrom);
                await LoadHistoryAsync();
            });
            openCalendarToCommand = new MvxCommand(async () =>
            {
                GetPayDateTo = await calendarDialog.ShowDialogAsync(GetPayDateTo);
                await LoadHistoryAsync();
            });

            downloadHistoryCommand = new MvxCommand(() => ServerCommandWrapperAsync(() => DownloadPdfAsync()));
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
                progressDialogManager.ShowProgressDialog("Please wait!", "Pay history is loading...");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            History = await paymentProcessing.GetPayHistoryAsync(GetPayDateFrom, GetPayDateTo);
            try
            {
                if (History.Count != 0)
                {
                    progressDialogManager.CloseProgressDialog();
                    isPayHistoryAwailableForUser = true;
                }
                else
                    progressDialogManager.CloseAndShowMessage("Error", "Sorry, there is no pay history for now.");
            }
            catch (NullReferenceException ex)
            {
                Debug.WriteLine(ex.Message);
                progressDialogManager.CloseAndShowMessage("Error", "Sorry, there is no pay history for now.");
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
                progressDialogManager.CloseAndShowMessage("Error", "Sorry, there is no pay history for now.");
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
