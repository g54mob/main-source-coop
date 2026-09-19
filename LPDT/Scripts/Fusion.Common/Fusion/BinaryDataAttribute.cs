using System;

namespace Fusion
{
	[AttributeUsage(AttributeTargets.Field)]
	public class BinaryDataAttribute : DrawerPropertyAttribute
	{
		public BinaryDataAttribute()
			: base(applyToCollection: true)
		{
		}
	}
}
