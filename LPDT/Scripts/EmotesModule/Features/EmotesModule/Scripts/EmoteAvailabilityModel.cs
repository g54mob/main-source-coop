using System;

namespace Features.EmotesModule.Scripts
{
	public class EmoteAvailabilityModel
	{
		public bool IsEmoteActive { get; private set; } = true;

		public event Action<bool> OnEmoteActiveChanged;

		public void SetEmoteActive(bool value)
		{
			if (IsEmoteActive != value)
			{
				IsEmoteActive = value;
				this.OnEmoteActiveChanged?.Invoke(value);
			}
		}
	}
}
