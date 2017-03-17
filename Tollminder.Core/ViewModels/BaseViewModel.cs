using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Chance.MvvmCross.Plugins.UserInteraction;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using MvvmCross.Plugins.Messenger;
using MvvmValidation;
using Tollminder.Core.Extensions.FieldValidator;
using Tollminder.Core.Models;
using Tollminder.Core.Services;

namespace Tollminder.Core.ViewModels
{
    public class BaseViewModel : MvxViewModel
    {
        readonly IInsightsService _insightsService;

        protected List<MvxSubscriptionToken> _subscriptions = new List<MvxSubscriptionToken>();

        //Dictionary<string, string> _errors = new Dictionary<string, string>();
        ValidationHelper validator;
        public ValidationHelper Validator
        {
            get { return validator; }
            set
            {
                validator = value;
                RaisePropertyChanged(() => Validator);
            }
        }

        ObservableDictionary<string, string> errors;
        public ObservableDictionary<string, string> Errors
        {
            get { return errors; }
            set { errors = value; RaisePropertyChanged(() => Errors); }
        }
        //public Dictionary<string, string> Errors
        //{
        //    get { return _errors; }
        //    set { _errors = value; RaisePropertyChanged(() => Errors); }
        //}

        List<Validator> _validators = new List<Validator>();
        public List<Validator> Validators
        {
            get { return _validators; }
            set { _validators = value; RaisePropertyChanged(() => Validators); }
        }

        public virtual string Title
        {
            get { return ""; }
        }

        bool _IsBusy;
        public virtual bool IsBusy
        {
            get
            {
                return _IsBusy;
            }
            set
            {
                _IsBusy = value;
                RaisePropertyChanged(() => IsBusy);
                RaisePropertyChanged(() => NotBusy);
            }
        }

        public virtual bool NotBusy
        {
            get
            {
                return !_IsBusy;
            }
        }

        public BaseViewModel()
        {
            validator = new ValidationHelper();
            _insightsService = Mvx.Resolve<IInsightsService>();
        }

        public virtual void Init()
        {
            SetValidators();
        }

        protected virtual void SetValidators()
        {
        }

        protected bool Validate()
        {
            var result = Validator.ValidateAll();

            Errors = result.AsObservableDictionary();

            return result.IsValid;
            //foreach (var item in Validators)
            //    UpdateError(item.Validate());

            //return Errors.Count == 0;
        }

        protected virtual async Task ServerCommandWrapperParrallelAsync(Func<Task> action)
        {
            try
            {
                IsBusy = true;
                await action();
            }
            catch (Exceptions.UiApiException ex)
            {
                Mvx.Trace(ex.Message, ex.StackTrace);
                InvokeOnMainThread(async () => await Mvx.Resolve<IUserInteraction>().AlertAsync(ex.Message, ex.Title));
            }
            catch (Exception ex)
            {
                Mvx.Trace(ex.Message + "PARALEL", ex.StackTrace);
                _insightsService.LogError(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        protected virtual async Task ServerCommandWrapperAsync(Func<Task> action)
        {
            if (IsBusy)
            {
                return;
            }

            try
            {
                IsBusy = true;
                await action();
            }
            catch (Exceptions.UiApiException ex)
            {
                Mvx.Trace(ex.Message, ex.StackTrace);
                InvokeOnMainThread(async () => await Mvx.Resolve<IUserInteraction>().AlertAsync(ex.Message, ex.Title));
            }
            catch (Exception ex)
            {
                Mvx.Trace(ex.Message, ex.StackTrace);
                Mvx.Resolve<IInsightsService>().LogError(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        protected virtual async Task ServerCommandWrapperAsync<T>(Func<T, Task> action, T arg)
        {
            if (IsBusy)
            {
                return;
            }
            try
            {
                IsBusy = true;
                await action(arg);
            }
            catch (Exceptions.UiApiException ex)
            {
                Mvx.Trace(ex.Message, ex.StackTrace);
                InvokeOnMainThread(async () => await Mvx.Resolve<IUserInteraction>().AlertAsync(ex.Message, ex.Title));
            }
            catch (Exception ex)
            {
                Mvx.Trace(ex.Message, ex.StackTrace);
                _insightsService.LogError(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        protected void UpdateError(ValidatorDataItem item)
        {
            if (!string.IsNullOrEmpty(item.ValidationText))
            {
                if (!Errors.ContainsKey(item.Name))
                    Errors[item.Name] = item.ValidationText;
            }
            else
            {
                if (Errors.ContainsKey(item.Name))
                    Errors.Remove(item.Name);
            }

            RaisePropertyChanged(() => Errors);
        }

        void DebugMethod([CallerMemberName] string memberName = "")
        {
            Debug.WriteLine(string.Format("{1} of {0}", memberName, this.GetType().FullName));
        }

        public virtual void OnCreateFinish()
        {
            DebugMethod();
        }

        public virtual void OnResume()
        {
            DebugMethod();
        }

        public virtual void OnPause()
        {
            DebugMethod();
        }

        public virtual void OnDestroy()
        {
            DebugMethod();
            foreach (var item in _subscriptions)
                item.Dispose();
        }
    }
}
