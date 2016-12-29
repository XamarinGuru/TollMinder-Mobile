using System;
using System.Windows.Input;
using MvvmCross.Core.ViewModels;
using Xamarin.Forms;

namespace Tollminder.Core.ViewModels
{
    public class LicenseViewModel : BaseViewModel
    {
        private MvxCommand backToProfileCommand;
        public ICommand BackToProfileCommand { get { return backToProfileCommand; } }

        public LicenseViewModel()
        {
            backToProfileCommand = new MvxCommand(() => { ShowViewModel<ProfileViewModel>(); });   
        }

        public override void Start()
        {
            base.Start();
        }
    }
}
