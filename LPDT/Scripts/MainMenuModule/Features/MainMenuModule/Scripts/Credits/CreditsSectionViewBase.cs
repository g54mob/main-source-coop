using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;
using UnityEngine;

namespace Features.MainMenuModule.Scripts.Credits
{
	public abstract class CreditsSectionViewBase : ViewBehaviour
	{
		[field: SerializeField]
		public CreditsEntryViewBase EntryPrefab { get; private set; }

		[field: SerializeField]
		public Transform EntriesContainer { get; private set; }

		public abstract void SetTitle(string title);
	}
}
