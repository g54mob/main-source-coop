using UnityEngine;

public class DebugConsoleKiller : MonoBehaviour
{
	public static bool killDebugConsole;

	private void LateUpdate()
	{
		if (Input.GetKeyDown(KeyCode.Delete))
		{
			killDebugConsole = !killDebugConsole;
		}
		if (killDebugConsole)
		{
			Debug.ClearDeveloperConsole();
		}
	}
}
