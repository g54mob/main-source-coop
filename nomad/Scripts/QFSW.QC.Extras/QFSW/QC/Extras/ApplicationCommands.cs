using UnityEngine;

namespace QFSW.QC.Extras
{
	public static class ApplicationCommands
	{
		[Command("quit", "Quits the player application", Platform.AllPlatforms, MonoTargetType.Single)]
		[CommandPlatform(~(Platform.EditorPlatforms | Platform.WebGLPlayer))]
		private static void Quit()
		{
			Application.Quit();
		}
	}
}
