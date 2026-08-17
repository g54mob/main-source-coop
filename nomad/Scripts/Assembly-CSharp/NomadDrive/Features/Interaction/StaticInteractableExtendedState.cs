using UnityEngine;

namespace NomadDrive.Features.Interaction
{
	public struct StaticInteractableExtendedState
	{
		public int id;

		public byte stateData;

		public byte normalizedValue;

		public float timestamp;

		public StaticInteractableExtendedState(int id, byte stateData, byte normalizedValue, float timestamp)
		{
			this.id = id;
			this.stateData = stateData;
			this.normalizedValue = normalizedValue;
			this.timestamp = timestamp;
		}

		public float GetNormalizedFloat()
		{
			return (float)(int)normalizedValue / 255f;
		}

		public void SetNormalizedFloat(float value)
		{
			normalizedValue = (byte)(Mathf.Clamp01(value) * 255f);
		}
	}
}
