using System;

namespace Obi
{
	[AttributeUsage(AttributeTargets.Field)]
	public class MinMaxAttribute : MultiPropertyAttribute
	{
		private float min;

		private float max;

		public MinMaxAttribute(float min, float max)
		{
			this.min = min;
			this.max = max;
		}
	}
}
