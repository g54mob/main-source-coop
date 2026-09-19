using System;

namespace Obi
{
	[AttributeUsage(AttributeTargets.Field)]
	public class MultiRange : MultiPropertyAttribute
	{
		private float min;

		private float max;

		public MultiRange(float min, float max)
		{
			this.min = min;
			this.max = max;
		}
	}
}
