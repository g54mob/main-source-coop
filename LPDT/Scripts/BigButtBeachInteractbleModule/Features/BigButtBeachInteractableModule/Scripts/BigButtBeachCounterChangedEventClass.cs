using System;

namespace Features.BigButtBeachInteractableModule.Scripts
{
	public class BigButtBeachCounterChangedEventClass
	{
		public event Action<BigButtBeachInteractable, long, long> OnCounterIncreased;

		internal void InvokeCounterIncreased(BigButtBeachInteractable source, long previousValue, long currentValue)
		{
			this.OnCounterIncreased?.Invoke(source, previousValue, currentValue);
		}
	}
}
