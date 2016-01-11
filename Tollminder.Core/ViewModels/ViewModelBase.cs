using System;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.CrossCore;
using System.Collections.Generic;
using Tollminder.Core.Models;

namespace Tollminder.Core.ViewModels
{
	// view model with general functionality for any view models
	public abstract class ViewModelBase : MvxViewModel 
	{
		#region Localization


		#endregion

		// track allocations
		protected ViewModelBase()
		{
			AllocationTracker.Track(this);
		}

		// track allocations
		~ViewModelBase()
		{
			AllocationTracker.Untrack(this);

		}

		protected virtual void OnDestroy ()
		{
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

	}
}

