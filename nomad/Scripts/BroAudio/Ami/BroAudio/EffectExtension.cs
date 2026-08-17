namespace Ami.BroAudio
{
	public static class EffectExtension
	{
		public static bool IsMoreIntenseThan(this Effect x, Effect y)
		{
			return x.CompareTo(y) > 0;
		}
	}
}
