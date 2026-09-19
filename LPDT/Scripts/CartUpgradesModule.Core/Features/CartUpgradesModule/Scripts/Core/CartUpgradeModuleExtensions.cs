namespace Features.CartUpgradesModule.Scripts.Core
{
	public static class CartUpgradeModuleExtensions
	{
		public static int ToMask(this CartUpgradeModule module)
		{
			if (module != CartUpgradeModule.None)
			{
				return 1 << (int)(module - 1);
			}
			return 0;
		}

		public static bool IsInMask(this CartUpgradeModule module, int modulesMask)
		{
			if (module != CartUpgradeModule.None)
			{
				return (modulesMask & module.ToMask()) != 0;
			}
			return false;
		}
	}
}
