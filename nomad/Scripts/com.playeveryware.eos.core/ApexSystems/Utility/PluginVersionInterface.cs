using Epic.OnlineServices;

namespace ApexSystems.Utility
{
	public class PluginVersionInterface
	{
		public const int MAJOR = 1;

		public const int MINOR = 18;

		public const int PATCH = 1;

		public const int HOTFIX = 2;

		public static readonly Utf8String PRODUCT_IDENTIFIER = "Unity Plugin for Epic Online Services";

		public static readonly Utf8String PRODUCT_NAME = "Unity Plugin for Epic Online Services";

		public static Utf8String GetVersion()
		{
			return 1 + "." + 18 + "." + 1 + "." + 2;
		}
	}
}
