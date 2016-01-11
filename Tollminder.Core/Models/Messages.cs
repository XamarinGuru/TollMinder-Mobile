using System;
using MvvmCross.Plugins.Messenger;

namespace Tollminder.Core.Models
{
	public class GenericMessage<T> : MvxMessage 
	{
		public T Data { get; private set; }

		public GenericMessage (object sender, T obj)
			: base (sender)
		{		
			Data = obj;	
		}
	}
}