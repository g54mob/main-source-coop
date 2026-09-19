using TMPro;
using UnityEngine;

namespace Features.MainMenuModule.Scripts.Credits
{
	public class CreditsEntryView : CreditsEntryViewBase
	{
		[SerializeField]
		private TMP_Text _professionText;

		[SerializeField]
		private TMP_Text _nameText;

		public override void SetProfession(string profession)
		{
			_professionText.SetText(profession ?? string.Empty);
		}

		public override void SetName(string entryName)
		{
			_nameText.SetText(entryName ?? string.Empty);
		}
	}
}
