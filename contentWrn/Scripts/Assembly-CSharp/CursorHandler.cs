using UnityEngine;
using Zorro.ControllerSupport;
using Zorro.Core;
using Zorro.Core.CLI;

public class CursorHandler : MonoBehaviour
{
	public bool defaultCursorVisible;

	private void Update()
	{
		bool num = Player.localPlayer != null;
		bool flag = false;
		if (!num)
		{
			flag = defaultCursorVisible;
		}
		if (!flag)
		{
			flag = Singleton<DebugUIHandler>.Instance.IsOpen || (Singleton<EscapeMenu>.Instance != null && Singleton<EscapeMenu>.Instance.Open) || (RetrievableResourceSingleton<Modal>.Instance != null && RetrievableResourceSingleton<Modal>.Instance.Open);
		}
		if (num && (Player.localPlayer.data.isInCostomizeTerminal || Player.localPlayer.data.isInTitleCardTerminal))
		{
			flag = true;
		}
		if (flag && InputHandler.GetCurrentUsedInputScheme() == InputScheme.Gamepad)
		{
			flag = false;
		}
		Cursor.visible = flag;
		Cursor.lockState = ((!flag) ? CursorLockMode.Locked : CursorLockMode.None);
	}
}
