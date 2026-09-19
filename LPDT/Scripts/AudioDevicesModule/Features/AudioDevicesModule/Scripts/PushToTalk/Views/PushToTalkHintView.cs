using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Features.AudioDevicesModule.Scripts.PushToTalk.Views
{
	public class PushToTalkHintView : PushToTalkHintViewBase
	{
		[SerializeField]
		private RectTransform _rectTransform;

		[SerializeField]
		private GameObject _root;

		[SerializeField]
		private Image _iconImage;

		[SerializeField]
		private Image _iconImageActive;

		[SerializeField]
		private TMP_Text _hintText;

		[SerializeField]
		private bool _isVisibleOnDeadState;

		public override bool IsVisibleOnDeadState => _isVisibleOnDeadState;

		public override void SetVisible(bool isVisible)
		{
			_root.SetActive(isVisible);
		}

		public override void SetPressedVisual(bool isPressed)
		{
			if (!(_iconImage == null))
			{
				if (isPressed)
				{
					_iconImage.enabled = false;
					_iconImageActive.enabled = true;
				}
				else
				{
					_iconImage.enabled = true;
					_iconImageActive.enabled = false;
				}
			}
		}

		public override void SetHintText(string text)
		{
			if (_hintText != null)
			{
				_hintText.SetText(text);
			}
		}
	}
}
