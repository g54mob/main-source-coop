using Zorro.Core;
using Zorro.Core.CLI;

public static class CanInput
{
	public static bool Can()
	{
		if (Singleton<DebugUIHandler>.Instance != null && Singleton<DebugUIHandler>.Instance.IsOpen)
		{
			return false;
		}
		return true;
	}
}
