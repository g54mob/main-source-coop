using System;

namespace Features.BeachPresetModule.Scripts.Core
{
	[Serializable]
	public struct OptionalValue<T>
	{
		public bool Enabled { get; private set; }

		public T Value { get; private set; }

		public OptionalValue(T value, bool enabled = true)
		{
			Value = value;
			Enabled = enabled;
		}
	}
}
