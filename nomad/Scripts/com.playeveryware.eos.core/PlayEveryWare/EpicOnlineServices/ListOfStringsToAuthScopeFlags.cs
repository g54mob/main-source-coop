using Epic.OnlineServices.Auth;
using Newtonsoft.Json.Linq;
using PlayEveryWare.EpicOnlineServices.Extensions;

namespace PlayEveryWare.EpicOnlineServices
{
	internal class ListOfStringsToAuthScopeFlags : ListOfStringsToEnumConverter<AuthScopeFlags>
	{
		protected override AuthScopeFlags FromStringArray(JArray array)
		{
			return FromStringArrayWithCustomMapping(array, AuthScopeFlagsExtensions.CustomMappings);
		}
	}
}
