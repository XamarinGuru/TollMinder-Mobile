﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using Tollminder.Core.Models.PaymentData;
using Tollminder.Core.Services;

namespace Tollminder.Core.ViewModels.Payments
{
    public class PayHistoryViewModel : BaseViewModel
    {
        readonly IServerApiService serverApiService;
        readonly IStoredSettingsService storedSettingsService;
        bool isPayHistoryAwailableForUser;

        public PayHistoryViewModel()
        {
            serverApiService = Mvx.Resolve<IServerApiService>();
            storedSettingsService = Mvx.Resolve<IStoredSettingsService>();

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
                Mvx.Resolve<IProgressDialogManager>().ShowProgressDialog("Please wait!", "Pay history is loading...");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            History = await serverApiService.GetPayHistoryAsync(GetPayDateFrom, GetPayDateTo);
            try
            {
                if (History.Count != 0)
                {
                    Mvx.Resolve<IProgressDialogManager>().CloseProgressDialog();
                    isPayHistoryAwailableForUser = true;
                }
                else
                    Mvx.Resolve<IProgressDialogManager>().CloseAndShowMessage("Error", "Sorry, there is no pay history for now.");
            }
            catch (NullReferenceException ex)
            {
                Debug.WriteLine(ex.Message);
                Mvx.Resolve<IProgressDialogManager>().CloseAndShowMessage("Error", "Sorry, there is no pay history for now.");
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
                string result = await serverApiService.DownloadPayHistoryAsync(getPayDateFrom, getPayDateTo);
                if (result != null)
                    ShowViewModel<PayHistoryPdfViewModel>(new { pdfUrlFromServer = result, pdfNameFromDateRange = GetPdfName() });
            }
            else
                Mvx.Resolve<IProgressDialogManager>().CloseAndShowMessage("Error", "Sorry, there is no pay history for now.");
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
