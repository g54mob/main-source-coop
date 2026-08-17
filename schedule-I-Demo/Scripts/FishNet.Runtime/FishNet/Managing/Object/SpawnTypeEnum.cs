using FishNet.Documenting;

namespace FishNet.Managing.Object
{
	[APIExclude]
	internal static class SpawnTypeEnum
	{
		public static bool Contains(SpawnType whole, SpawnType part)
		{
			return (whole & part) == part;
		}
	}
}
