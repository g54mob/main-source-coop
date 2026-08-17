using EvilCore.Extensions;
using EvilCore.Localization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace EvilCore.Inputs.UI
{
	public class InputActionGlyphBinder : MonoBehaviour
	{
		[SerializeField]
		[RewiredAction]
		private string rewiredActionName;

		[SerializeField]
		private Image targetIcon;

		[Header("Optional Label")]
		[SerializeField]
		private TextMeshProUGUI actionLabel;

		[SerializeField]
		[LocalizationKey(null)]
		private string actionLabelKey;

		[Inject]
		private IInputGlyphService _glyphService;

		[Inject]
		private ILocalizationService _localizationService;

		private void Awake()
		{
			base.gameObject.InjectGameObject();
			if (targetIcon == null)
			{
				targetIcon = GetComponent<Image>();
			}
		}

		private void OnEnable()
		{
			if (_glyphService != null)
			{
				_glyphService.OnGlyphsChanged += Refresh;
			}
			if (_localizationService != null)
			{
				_localizationService.OnLocaleChanged += Refresh;
			}
			Refresh();
		}

		private void OnDisable()
		{
			if (_glyphService != null)
			{
				_glyphService.OnGlyphsChanged -= Refresh;
			}
			if (_localizationService != null)
			{
				_localizationService.OnLocaleChanged -= Refresh;
			}
		}

		public void Refresh()
		{
			if (_glyphService != null && !string.IsNullOrEmpty(rewiredActionName))
			{
				if (targetIcon != null)
				{
					targetIcon.sprite = _glyphService.GetSpriteForAction(rewiredActionName);
				}
				if (actionLabel != null && !string.IsNullOrEmpty(actionLabelKey))
				{
					actionLabel.text = ((_localizationService != null) ? _localizationService.Localize(actionLabelKey) : actionLabelKey);
				}
			}
		}

		public void SetAction(string actionName)
		{
			rewiredActionName = actionName;
			Refresh();
		}

		private void OnValidate()
		{
			if (targetIcon == null)
			{
				targetIcon = GetComponent<Image>();
			}
		}
	}
}
