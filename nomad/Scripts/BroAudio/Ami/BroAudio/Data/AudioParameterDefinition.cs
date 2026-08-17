using System;

namespace Ami.BroAudio.Data
{
	[Serializable]
	public struct AudioParameterDefinition
	{
		public string Id;

		public string Name;

		public AudioParameterType Type;

		public bool DefaultBool;

		public int DefaultInt;

		public float DefaultFloat;

		public string Description;

		public float MinValue;

		public float MaxValue;

		public bool UseRangeSlider;

		public object GetDefault()
		{
			return Type switch
			{
				AudioParameterType.Bool => DefaultBool, 
				AudioParameterType.Int => DefaultInt, 
				AudioParameterType.Float => DefaultFloat, 
				_ => null, 
			};
		}

		public bool HasMeaningfulRange()
		{
			return MinValue < MaxValue;
		}

		public void EnsureId()
		{
			if (string.IsNullOrEmpty(Id))
			{
				Id = Guid.NewGuid().ToString("N");
			}
		}
	}
}
