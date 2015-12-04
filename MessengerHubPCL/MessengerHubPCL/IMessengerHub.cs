using System;

namespace MessengerHub
{
	public interface IMessengerHub
	{
		void InterceptAll(Action<Type, object> process);

		// subscriptions and unsubscriptions
		SubscriptionToken Subscribe<T>(Action<T> process, Func<T, bool> filter) where T : MessageBase;
		SubscriptionToken Subscribe<T>(Action<T> process) where T : MessageBase;
		void Unsubscribe(SubscriptionToken token);
		void Publish(MessageBase message);

		// report an error
		void LogException(Exception ex);
	}
}

