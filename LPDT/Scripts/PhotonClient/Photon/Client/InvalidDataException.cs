using System;

namespace Photon.Client
{
	public class InvalidDataException : Exception
	{
		public InvalidDataException(string message)
			: base(message)
		{
		}
	}
}
