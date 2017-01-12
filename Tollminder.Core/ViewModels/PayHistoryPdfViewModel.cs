using System.Windows.Input;
using MvvmCross.Core.ViewModels;
using Tollminder.Core.Services;

namespace Tollminder.Core.ViewModels
{
    public class PayHistoryPdfViewModel : BaseViewModel
    {
        IDownloadManager downloadManager;

        public PayHistoryPdfViewModel(IDownloadManager downloadManager)
        {
            this.downloadManager = downloadManager;
            backToPayHistoryCommand = new MvxCommand(() => { ShowViewModel<PayHistoryViewModel>(); });
            downloadPayHistoryPdfCommand = new MvxCommand(() => { downloadManager.Download(PdfUrl);});
        }

        public void Init(string pdfUrlFromServer)
        {
            PdfUrl = pdfUrlFromServer;
        }

        public override void Start()
        {
            base.Start();
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

        private MvxCommand backToPayHistoryCommand;
        public ICommand BackToPayHistoryCommand { get { return backToPayHistoryCommand; } }

        private MvxCommand downloadPayHistoryPdfCommand;
        public ICommand DownloadPayHistoryPdfCommand { get { return downloadPayHistoryPdfCommand; } }
    }
}
