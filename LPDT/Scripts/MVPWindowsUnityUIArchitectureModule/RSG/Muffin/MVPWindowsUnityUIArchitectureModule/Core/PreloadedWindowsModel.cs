using System.Collections.Generic;

namespace RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core
{
	public class PreloadedWindowsModel
	{
		public Dictionary<string, MonoWindowInstance> PreloadedWindows { get; } = new Dictionary<string, MonoWindowInstance>();
	}
}
