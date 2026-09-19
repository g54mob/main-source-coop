namespace GameplayEvents
{
	public class OnSessionQuotaChangedEvent : GameplayEvent
	{
		public readonly float CurrentValue;

		public readonly float TargetValue;

		public OnSessionQuotaChangedEvent(float currentValue, float targetValue)
		{
			CurrentValue = currentValue;
			TargetValue = targetValue;
		}
	}
}
