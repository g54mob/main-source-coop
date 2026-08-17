using Epic.OnlineServices.UI;
using Newtonsoft.Json.Linq;

namespace PlayEveryWare.EpicOnlineServices
{
	internal class ListOfStringsToInputStateButtonFlags : ListOfStringsToEnumConverter<InputStateButtonFlags>
	{
		protected override InputStateButtonFlags FromStringArray(JArray array)
		{
			return FromStringArrayWithCustomMapping(array, null);
		}
	}
}
