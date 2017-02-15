using System;
using System.Windows.Input;
using MvvmCross.Binding;
using MvvmCross.Binding.Bindings.Target;
using MvvmCross.Core.ViewModels;

namespace Tollminder.Touch.Helpers.MvxAlertActionHelpers
{
    public class MvxAlertActionBinding: MvxTargetBinding
    {
        private readonly MvxAlertAction _view;
        private ICommand _command;

        public MvxAlertActionBinding(MvxAlertAction view, ICommand command) : base(view)
        {
            _view = view;
            _view.Clicked += OnClicked;
            _command = command;
        }

        void OnClicked(object sender, EventArgs e)
        {
            if (_command != null)
            {
                _command.Execute(sender);
            }
        }

        public override void SetValue(object value)
        {
            //_command = (IMvxCommand)value;
        }

        protected override void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                _view.Clicked -= OnClicked;
            }
            base.Dispose(isDisposing);
        }

        public override Type TargetType
        {
            get
            {
                return typeof(IMvxCommand);
            }
        }

        public override MvxBindingMode DefaultMode
        {
            get
            {
                return MvxBindingMode.OneWay;
            }
        }
    }
}
