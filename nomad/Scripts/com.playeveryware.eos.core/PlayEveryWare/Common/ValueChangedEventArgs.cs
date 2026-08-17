using System;

namespace PlayEveryWare.Common
{
	public class ValueChangedEventArgs<TValueType> : EventArgs
	{
		public readonly TValueType OldValue;

		public readonly TValueType NewValue;

		public ValueChangedEventArgs(TValueType oldValue, TValueType newValue)
		{
			OldValue = oldValue;
			NewValue = newValue;
		}
	}
}
