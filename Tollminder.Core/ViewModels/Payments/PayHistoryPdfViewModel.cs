﻿using System.Windows.Input;
using MvvmCross.Core.ViewModels;
using Tollminder.Core.Services.ProfileData;

namespace Tollminder.Core.ViewModels.Payments
{
    public class PayHistoryPdfViewModel : BaseViewModel
    {
        IFileManager downloadManager;

        public PayHistoryPdfViewModel(IFileManager downloadManager)
        {
            this.downloadManager = downloadManager;
            backToPayHistoryCommand = new MvxCommand(() => { ShowViewModel<PayHistoryViewModel>(); });
            fileOpenInCommand = new MvxCommand(() => { downloadManager.OpenIn(PdfUrl, PdfName); });
        }

        public void Init(string pdfUrlFromServer, string pdfNameFromDateRange)
        {
            PdfUrl = pdfUrlFromServer;
            PdfName = pdfNameFromDateRange;
        }

        public override void Start()
        {
            base.Start();
            downloadManager.Download(PdfUrl, PdfName);
        }

        private string pdfUrl;
        public string PdfUrl
        {
            get { return pdfUrl; }
            set
            {
                SetProperty(ref pdfUrl, value);
                RaisePropertyChanged(() => PdfUrl);
            }
        }

        private string pdfName;
        public string PdfName
        {
            get { return pdfName; }
            set
            {
                SetProperty(ref pdfName, value);
                RaisePropertyChanged(() => PdfName);
            }
        }

        private MvxCommand backToPayHistoryCommand;
        public ICommand BackToPayHistoryCommand { get { return backToPayHistoryCommand; } }

        private MvxCommand fileOpenInCommand;
        public ICommand FileOpenInCommand { get { return fileOpenInCommand; } }
    }
}
