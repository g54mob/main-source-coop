using TMPro;
using UnityEngine;

namespace Features.MainMenuModule.Scripts.Credits
{
	public class CreditsSectionView : CreditsSectionViewBase
	{
		[SerializeField]
		private TMP_Text _titleText;

		[SerializeField]
		private GameObject _titleContainer;

		public override void SetTitle(string title)
		{
			if (!(_titleText == null))
			{
				_titleContainer.SetActive(title != string.Empty);
				_titleText.SetText(title ?? string.Empty);
			}
		}
	}
}
