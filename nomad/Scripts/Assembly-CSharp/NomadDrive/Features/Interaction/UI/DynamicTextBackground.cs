using TMPro;
using UnityEngine;

namespace NomadDrive.Features.Interaction.UI
{
	public class DynamicTextBackground : MonoBehaviour
	{
		[SerializeField]
		private TextMeshProUGUI _contextText;

		private RectTransform _backgroundRectTransform;

		[SerializeField]
		private TextGrowthDirection _growthDirection = TextGrowthDirection.Right;

		[SerializeField]
		private float _baseWidth = 100f;

		[SerializeField]
		private float _widthPerCharacter = 15f;

		private bool _needsUpdate;

		private string _lastText = "";

		private void Awake()
		{
			if (_contextText == null)
			{
				_contextText = GetComponentInChildren<TextMeshProUGUI>();
			}
			_backgroundRectTransform = GetComponent<RectTransform>();
			if (_contextText != null)
			{
				_lastText = _contextText.text;
			}
			UpdateBackgroundSize();
		}

		private void OnEnable()
		{
			UpdateBackgroundSize();
		}

		private void LateUpdate()
		{
			if (_contextText != null && _lastText != _contextText.text)
			{
				_lastText = _contextText.text;
				UpdateBackgroundSize();
			}
			if (_needsUpdate)
			{
				_needsUpdate = false;
				UpdateBackgroundSize();
			}
		}

		private void OnTextChanged(Object obj)
		{
			if (obj == _contextText)
			{
				_needsUpdate = true;
			}
		}

		private void UpdateBackgroundSize()
		{
			if (!(_contextText == null) && !(_backgroundRectTransform == null))
			{
				int length = _contextText.text.Length;
				float x = _baseWidth + (float)length * _widthPerCharacter;
				Vector2 pivot = _backgroundRectTransform.pivot;
				Vector2 vector = _backgroundRectTransform.localPosition;
				switch (_growthDirection)
				{
				case TextGrowthDirection.Left:
					_backgroundRectTransform.pivot = new Vector2(1f, pivot.y);
					break;
				case TextGrowthDirection.Right:
					_backgroundRectTransform.pivot = new Vector2(0f, pivot.y);
					break;
				case TextGrowthDirection.Center:
					_backgroundRectTransform.pivot = new Vector2(0.5f, pivot.y);
					break;
				}
				_backgroundRectTransform.sizeDelta = new Vector2(x, _backgroundRectTransform.sizeDelta.y);
				if (pivot != _backgroundRectTransform.pivot)
				{
					_backgroundRectTransform.localPosition = vector;
				}
			}
		}

		private void OnDisable()
		{
			_contextText = null;
			_backgroundRectTransform = null;
		}
	}
}
