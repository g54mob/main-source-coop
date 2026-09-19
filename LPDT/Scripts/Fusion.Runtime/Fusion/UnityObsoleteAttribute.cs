using System;

namespace Fusion
{
	internal class UnityObsoleteAttribute : Attribute
	{
		public string Message => _003Cmessage_003EP;

		public bool IsError => _003Cerror_003EP;

		public UnityObsoleteAttribute(string message, bool error = false)
		{
			_003Cmessage_003EP = message;
			_003Cerror_003EP = error;
			base._002Ector();
		}
	}
}
