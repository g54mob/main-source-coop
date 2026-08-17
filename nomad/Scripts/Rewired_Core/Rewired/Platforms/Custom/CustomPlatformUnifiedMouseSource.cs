using UnityEngine;

namespace Rewired.Platforms.Custom
{
	public abstract class CustomPlatformUnifiedMouseSource : CustomPlatformUnifiedControllerSource
	{
		public abstract Vector2 mousePosition { get; }

		public CustomPlatformUnifiedMouseSource()
			: base(4, 7)
		{
		}
	}
}
