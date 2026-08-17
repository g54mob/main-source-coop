using FishNet.Documenting;

namespace FishNet.Object
{
	[APIExclude]
	internal static class ChangedTransformPropertiesEnum
	{
		public static bool Contains(ChangedTransformProperties whole, ChangedTransformProperties part)
		{
			return (whole & part) == part;
		}
	}
}
