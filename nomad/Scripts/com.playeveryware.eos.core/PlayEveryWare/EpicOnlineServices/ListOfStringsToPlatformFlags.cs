using System.Collections.Generic;
using Epic.OnlineServices.Platform;
using Newtonsoft.Json.Linq;
using PlayEveryWare.EpicOnlineServices.Extensions;

namespace PlayEveryWare.EpicOnlineServices
{
	internal class ListOfStringsToPlatformFlags : ListOfStringsToEnumConverter<WrappedPlatformFlags>
	{
		protected override WrappedPlatformFlags FromStringArray(JArray array)
		{
			Dictionary<string, WrappedPlatformFlags> dictionary = new Dictionary<string, WrappedPlatformFlags>();
			foreach (KeyValuePair<string, PlatformFlags> customMapping in PlatformFlagsExtensions.CustomMappings)
			{
				string key = customMapping.Key;
				PlatformFlags value = customMapping.Value;
				dictionary[key] = value.Wrap();
			}
			return FromStringArrayWithCustomMapping(array, dictionary);
		}
	}
}
