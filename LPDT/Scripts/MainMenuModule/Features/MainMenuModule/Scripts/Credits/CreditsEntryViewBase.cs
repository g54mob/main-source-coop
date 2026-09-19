using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;

namespace Features.MainMenuModule.Scripts.Credits
{
	public abstract class CreditsEntryViewBase : ViewBehaviour
	{
		public abstract void SetProfession(string profession);

		public abstract void SetName(string entryName);
	}
}
