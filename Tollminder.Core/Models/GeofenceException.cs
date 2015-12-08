using System;

namespace Tollminder.Core.Models
{
	public class GeofenceException : Exception
	{
		public GeofenceStatus Status { get; private set; }

		public GeofenceException () : base ()
		{			
		}

		public GeofenceException (string message) : base (message)
		{			
		}

		public GeofenceException (string message, GeofenceStatus status) : base (message)
		{
			Status = status;
		}

		public GeofenceException (string message, Exception innerException) : base (message, innerException)
		{			
		}

		public GeofenceException (string message, Exception innerException, GeofenceStatus status) : base (message, innerException)
		{	
			Status = status;
		}

	}
}

