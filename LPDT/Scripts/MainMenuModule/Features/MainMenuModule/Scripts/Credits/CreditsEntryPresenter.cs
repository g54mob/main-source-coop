using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;
using UnityEngine;

namespace Features.MainMenuModule.Scripts.Credits
{
	public class CreditsEntryPresenter : PresenterBehaviour<CreditsEntryViewBase>
	{
		public void Setup(CreditsEntryData entry)
		{
			base.View.SetProfession(entry.Profession);
			base.View.SetName(entry.Name);
		}

		public void SetParent(Transform parent)
		{
			base.View.transform.SetParent(parent, worldPositionStays: false);
		}

		public void DestroyView()
		{
			Object.Destroy(base.View.gameObject);
		}
	}
}
