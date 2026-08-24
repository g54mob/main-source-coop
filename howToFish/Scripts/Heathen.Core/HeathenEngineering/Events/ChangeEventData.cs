using System;

namespace HeathenEngineering.Events
{
	[Serializable]
	public struct ChangeEventData<T>
	{
		public object sender;

		public T oldValue;

		public T newValue;

		public ChangeEventData(object sender, T oldValue, T newValue)
		{
			this.sender = sender;
			this.oldValue = oldValue;
			this.newValue = newValue;
		}
	}
}
