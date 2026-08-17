using System.ComponentModel;
using Rewired.InputManagers;

namespace Rewired.Utils.Platforms.Windows
{
	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public static class Main
	{
		public static object GetPlatformInitializer()
		{
			return Rewired.InputManagers.Initializer.GetPlatformInitializer();
		}
	}
}
