using Epic.OnlineServices.IntegratedPlatform;
using Newtonsoft.Json.Linq;
using PlayEveryWare.EpicOnlineServices.Extensions;

namespace PlayEveryWare.EpicOnlineServices
{
	internal class ListOfStringsToIntegratedPlatformManagementFlags : ListOfStringsToEnumConverter<IntegratedPlatformManagementFlags>
	{
		protected override IntegratedPlatformManagementFlags FromStringArray(JArray array)
		{
			return FromStringArrayWithCustomMapping(array, IntegratedPlatformManagementFlagsExtensions.CustomMappings);
		}
	}
}
