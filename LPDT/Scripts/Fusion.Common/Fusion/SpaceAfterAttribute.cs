using System;

namespace Fusion
{
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
	public class SpaceAfterAttribute : DecoratingPropertyAttribute
	{
		public new const int DefaultOrder = -5000;

		public readonly float Height;

		public SpaceAfterAttribute(float height = 8f)
		{
			Height = height;
			base.order = -5000;
		}
	}
}
