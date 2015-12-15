using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Text;

namespace MessengerHub
{
	public class MessengerHub : IMessengerHub
	{
		// subscriptions go here, all strong references
		private object _SyncObject = new object();
		private bool _NotifyIntercepts = true;
		private IDictionary<Guid, Subscription> _Subscriptions;
		private IList<Action<Type, object>> _Intercepts;

		// startup messenger hub
		public MessengerHub()
		{
			_Subscriptions = new Dictionary<Guid, Subscription>();
			_Intercepts = new List<Action<Type, object>>();

			// don't notify intercepts (view models) if the app goes into the background
//			this.Subscribe<AppInForegroundMessage>(message => {
//				_NotifyIntercepts = message.Content;
//			});
		}

		// subscribe to all messages
		public void InterceptAll(Action<Type, object> process)
		{
			lock (_SyncObject)
			{
				_Intercepts.Add(process);
			}
		}

		// subscribe to messages
		public SubscriptionToken Subscribe<T>(Action<T> process, Func<T, bool> filter) where T : MessageBase
		{
			lock (_SyncObject)
			{
				var token = new SubscriptionToken();
				var subscription = new Subscription()
				{
					MessageType = typeof(T),
					Process = (x => process((T)x)),
					Filter = (x => filter((T)x))
				};

				// add the subscription and give back the token
				_Subscriptions.Add(token.Identifier, subscription);
				return token;
			}
		}

		// subscribe asynchronously
		public SubscriptionToken Subscribe<T>(Action<T> process) where T : MessageBase
		{
			return Subscribe(process, x => true);
		}

		// unsubscribe
		public void Unsubscribe(SubscriptionToken token)
		{
			lock (_SyncObject)
			{
				_Subscriptions.Remove(token.Identifier);
			}
		}

		// satisfy interfaces
		public void Publish(MessageBase message)
		{
			PublishInternal(message);
		}

		// publish internally ...
		private void PublishInternal(MessageBase message)
		{
			Task.Factory.StartNew(() => {

				// get subscriptions and intercepts
				var messageType = message.GetType();
				var subscriptions = null as Subscription[];
				var intercepts = null as Action<Type, object>[];
				lock (_SyncObject)
				{
					subscriptions = _Subscriptions.Values.ToArray();
					intercepts = _Intercepts.ToArray();
				}

				// subscription
				foreach (var subscription in subscriptions)
				{
					try
					{
						if (subscription.MessageType == messageType && subscription.Filter(message))
						{
							subscription.Process(message);
						}
					}
					catch (Exception ex)
					{
						LogException(ex);
					}
				}

				// process all subscriptions
				if (_NotifyIntercepts)
				{
					foreach (var intercept in intercepts)
					{
						intercept(messageType, message);
					}
				}
			});
		}

		// log an exception
		public virtual void LogException(Exception exception)
		{
			//do whatever you want
		}

		// subscription is heavy!
		private class Subscription
		{
			public Type MessageType { get; set; }
			public Action<object> Process { get; set; }
			public Func<object, bool> Filter { get; set; }
		}
	}
}

