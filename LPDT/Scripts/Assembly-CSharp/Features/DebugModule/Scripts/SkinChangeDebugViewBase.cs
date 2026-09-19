using System;
using Features.SkinChangeModule.Scripts;
using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;

namespace Features.DebugModule.Scripts
{
	public abstract class SkinChangeDebugViewBase : ViewBehaviour
	{
		public Action<SkinPartType, int> OnSkinChangeRequested;
	}
}
