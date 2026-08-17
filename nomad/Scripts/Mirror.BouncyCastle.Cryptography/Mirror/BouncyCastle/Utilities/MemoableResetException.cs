using System;
using System.Runtime.Serialization;

namespace Mirror.BouncyCastle.Utilities
{
	[Serializable]
	public class MemoableResetException : InvalidCastException
	{
		public MemoableResetException()
		{
		}

		public MemoableResetException(string message)
			: base(message)
		{
		}

		protected MemoableResetException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
