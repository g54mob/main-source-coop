using UnityEngine;

namespace Zorro.Core
{
	public static class Timez
	{
		public static float CappedDeltaTime => Mathf.Clamp(Time.deltaTime, 0f, 0.02f);

		public static float CappedUnscaledDeltaTime => Mathf.Clamp(Time.unscaledDeltaTime, 0f, 0.02f);

		public static float CappedFixedDeltaTime => Mathf.Clamp(Time.fixedDeltaTime, 0f, 0.08f);
	}
}
