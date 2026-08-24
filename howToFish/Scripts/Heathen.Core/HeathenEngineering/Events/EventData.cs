using System;

namespace HeathenEngineering.Events
{
	[Serializable]
	public struct EventData
	{
		public object sender;

		public EventData(object sender)
		{
			this.sender = sender;
		}
	}
	[Serializable]
	public struct EventData<T>
	{
		public object sender;

		public T value;

		public EventData(object sender, T value)
		{
			this.sender = sender;
			this.value = value;
		}
	}
}
