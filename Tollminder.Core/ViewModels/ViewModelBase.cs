using System;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.CrossCore;
using System.Collections.Generic;
using MessengerHub;
using Tollminder.Core.Models;

namespace Tollminder.Core.ViewModels
{
	// view model with general functionality for any view models
	public abstract class ViewModelBase : MvxViewModel 
	{
		#region Localization
//
//		public string OkText { get { return GetLocalizedString ("Common.OK"); } }
//		public string CancelText { get { return GetLocalizedString ("Common.Cancel"); } }

		#endregion


		// publish a message to all registered view models (avoid locking and threads)
		public static void Publish(Type type, object message)
		{
			foreach (var viewModel in AllocationTracker.GetTrackedObjectsAssignableTo<ViewModelBase>())
			{
				foreach (var subscription in viewModel._Subscriptions)
				{
					try
					{
						if (subscription.Type == type && subscription.Filter(message))
						{
							subscription.Process(message);
						}
					}
					catch (Exception ex)
					{
						Mvx.Resolve<IMessengerHub>().LogException(ex);
					}
				}
			}
		}

		// track allocations
		public ViewModelBase()
		{
			AllocationTracker.Track(this);
		}

		// track allocations
		~ViewModelBase()
		{
			AllocationTracker.Untrack(this);

		}

		// subscriptions to events
		private IList<ViewModelSubscription> _Subscriptions = new List<ViewModelSubscription>();

		// subscriptions to events
		protected void WeakSubscribe<T>(Action<T> process, Func<T, bool> filter) where T : MessageBase
		{
			_Subscriptions.Add(new ViewModelSubscription()
				{
					Type = typeof(T),
					Process = ((object x) => process((T)x)),
					Filter = ((object x) => filter((T)x))
				});
		}

		// subscribe with tiny filter
		protected void WeakSubscribe<T>(Action<T> process) where T : MessageBase
		{
			WeakSubscribe<T>(process, x => true);
		}

		public virtual string Title
		{
			get { return ""; }
		}

		// busy indicator
		private bool _IsBusy;
		public virtual bool IsBusy
		{
			get { return _IsBusy; }
			set { _IsBusy = value; RaisePropertyChanged(() => IsBusy); }
		}

		// retrieve a localized string
//		public string GetLocalizedString(string key, params object[] parameters)
//		{
//			return Resources.Translator.GetText(key, parameters);
//		}

		// retrieve time ago (helpful function)
		protected string GetFriendlyTimeDuration(DateTime time)
		{
			if (time == DateTime.MinValue)
				return "";

			// abs TimeSpan
			var elapsedTime = (DateTime.Now - time).Duration();
			if (elapsedTime.TotalMinutes < 1) 
				return string.Format("{0}m", 1);
			else if (elapsedTime.TotalHours < 1) 
				return string.Format("{0}m", (int)Math.Round(elapsedTime.TotalMinutes));
			else if (elapsedTime.TotalDays < 1)
				return string.Format("{0}h", (int)Math.Round(elapsedTime.TotalHours));
			else 
				return string.Format("{0}d", (int)Math.Round(elapsedTime.TotalDays));
		}



		#region Exceptions processing

//		ReportableException _CurrentError;
//		public ReportableException CurrentError {
//			get { return _CurrentError; }
//			set { _CurrentError = value; RaisePropertyChanged(() => CurrentError);}
//		}

		// to be overridden when wanted
		protected void ReportError(Exception exception)
		{
//			if (exception is AggregateException)
//			{
//				var exceptions = ((AggregateException)exception).Flatten().InnerExceptions;
//				var reportableException = exceptions.OfType<ReportableException>().FirstOrDefault();
//				if (reportableException != null)
//					ReportError(reportableException);
//				else
//					ReportError(exceptions.First());
//			}
//			else
//			{
//
//				if (exception is ReportableException) {
//					var reportableExceptionq = (ReportableException)exception;
//					if (!reportableExceptionq.IsSilent) {
//						ShowViewModel<AlertViewModel> (new {
//							title = reportableExceptionq.PublicTitle,
//							message = reportableExceptionq.PublicDetail,
//							messageType = UserMessageType.Error.ToString ()
//						});
//					}
//				} else {
//					ShowViewModel<AlertViewModel> (new {
//						title = GetLocalizedString("Common.Error"),
//						message = GetLocalizedString("Error.ServerConnection"),
//						confirmTitle = GetLocalizedString("Common.OK")
//					});	
//				}
//
//				// log the exception come what may
//				var messengerHub = null as IMessengerHub;
//				if (Mvx.TryResolve<IMessengerHub>(out messengerHub))
//				{
//					messengerHub.LogException(exception);
//				}
//			}
		}

		protected void ReportMessage(string title, string message, UserMessageType messageType)
		{
//			ShowViewModel<AlertViewModel>(new {
//				title = title,
//				message = message,
//				messageType = messageType.ToString(),
//			});
		}

		#endregion

		// encapsulates a subscription
		private class ViewModelSubscription
		{
			public Type Type { get; set; }
			public Action<object> Process { get; set; }
			public Func<object, bool> Filter { get; set; }
		}
	}
}

