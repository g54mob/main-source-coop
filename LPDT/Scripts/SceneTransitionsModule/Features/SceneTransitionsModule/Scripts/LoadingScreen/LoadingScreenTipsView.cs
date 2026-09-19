using TMPro;
using UnityEngine;

namespace Features.SceneTransitionsModule.Scripts.LoadingScreen
{
	public class LoadingScreenTipsView : LoadingScreenTipsViewBase
	{
		[SerializeField]
		private GameObject _tipsContent;

		[SerializeField]
		private TMP_Text _tipText;

		[Header("Animated loading tip")]
		[SerializeField]
		private float _animatedTipDotsInterval = 0.4f;

		[SerializeField]
		private int _animatedTipMaxDots = 3;

		private string _prefabDefaultText;

		public override float AnimatedTipDotsInterval => _animatedTipDotsInterval;

		public override int AnimatedTipMaxDots => _animatedTipMaxDots;

		public override string DefaultText => _prefabDefaultText;

		private void Awake()
		{
			_prefabDefaultText = _tipText.text;
		}

		public override void SetVisible(bool visible)
		{
			if (_tipsContent != null)
			{
				_tipsContent.SetActive(visible);
			}
		}

		public override void SetText(string tip)
		{
			if ((Object)(object)_tipText != null)
			{
				_tipText.text = tip ?? string.Empty;
			}
		}

		public override void Reset()
		{
			SetText(string.Empty);
			SetVisible(visible: false);
		}
	}
}
