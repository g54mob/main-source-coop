using System;
using UnityEngine;

namespace Coffee.UIEffects.Timeline
{
	[AttributeUsage(AttributeTargets.Class)]
	public class ColorClipUsageAttribute : PropertyAttribute
	{
		public readonly bool alpha;

		public ColorClipUsageAttribute(bool alpha)
		{
			this.alpha = alpha;
		}
	}
}
