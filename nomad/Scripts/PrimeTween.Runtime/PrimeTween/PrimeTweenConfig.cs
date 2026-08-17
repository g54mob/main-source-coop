using JetBrains.Annotations;

namespace PrimeTween
{
	[PublicAPI]
	public static class PrimeTweenConfig
	{
		internal static PrimeTweenManager Instance => PrimeTweenManager.Instance;

		public static Ease defaultEase => Instance.defaultEase;

		public static bool warnZeroDuration
		{
			set
			{
				Instance.warnZeroDuration = value;
			}
		}

		public static void SetTweensCapacity(int capacity)
		{
			if (PrimeTweenManager.HasInstance && !PrimeTweenManager.Instance.isDestroyed)
			{
				PrimeTweenManager.Instance.SetTweensCapacity(capacity);
			}
			else
			{
				PrimeTweenManager.customInitialCapacity = capacity;
			}
		}
	}
}
