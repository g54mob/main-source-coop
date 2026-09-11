namespace Zorro.Core
{
	public static class SpringClamp
	{
		public static T BounceClamp<T>(T value, float min, float max, float energyConservation = 0.6f) where T : IOneDimension
		{
			float num = value.Current;
			float num2 = value.Velocity;
			if (num < min)
			{
				num = min;
				num2 = (0f - num2) * energyConservation;
			}
			else if (num > max)
			{
				num = max;
				num2 = (0f - num2) * energyConservation;
			}
			value.SetCurrent(num);
			value.SetVelocity(num2);
			return value;
		}
	}
}
