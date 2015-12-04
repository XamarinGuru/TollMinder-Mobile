using System;

namespace MessengerHub
{
	public class SubscriptionToken
	{
		public SubscriptionToken()
		{
			this.Identifier = Guid.NewGuid();
		}

		// identifier for a subscription
		public Guid Identifier { get; private set; }
	}

	public abstract class MessageBase
	{
		protected MessageBase(object sender)
		{
			this.Sender = sender;
		}

		public object Sender { get; private set; }
	}

	// generic message
	public abstract class GenericMessageBase<T> : MessageBase
	{
		protected GenericMessageBase(object sender, T content) : base(sender)
		{
			this.Content = content;
		}

		public T Content { get; private set; }
	}

	public class AppInForegroundMessage : GenericMessageBase<bool>
	{
		public AppInForegroundMessage(object sender, bool foreground)
			: base(sender, foreground)
		{
		}
	}
}

