using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Features.CustomUiElementsModule.Scripts
{
	public class GroupedTmpAutoSize : MonoBehaviour
	{
		private const float DefaultFontSizeMultiplier = 1f;

		[SerializeField]
		private List<TextAutoSizeEntry> _textList = new List<TextAutoSizeEntry>();

		private Coroutine _refreshCoroutine;

		private bool _isRefreshing;

		private void OnEnable()
		{
			TMPro_EventManager.TEXT_CHANGED_EVENT.Add(OnTextChanged);
			ScheduleRefresh();
		}

		private void OnDisable()
		{
			TMPro_EventManager.TEXT_CHANGED_EVENT.Remove(OnTextChanged);
			if (_refreshCoroutine != null)
			{
				StopCoroutine(_refreshCoroutine);
				_refreshCoroutine = null;
			}
		}

		private void OnRectTransformDimensionsChange()
		{
			if (base.isActiveAndEnabled)
			{
				ScheduleRefresh();
			}
		}

		public void Refresh()
		{
			Canvas.ForceUpdateCanvases();
			RefreshTexts();
		}

		private void OnTextChanged(Object changedObject)
		{
			if (base.isActiveAndEnabled && changedObject is TMP_Text text && ContainsText(text))
			{
				ScheduleRefresh();
			}
		}

		private void RefreshTexts()
		{
			if (_isRefreshing)
			{
				return;
			}
			_isRefreshing = true;
			float num = float.MaxValue;
			bool flag = false;
			for (int i = 0; i < _textList.Count; i++)
			{
				TextAutoSizeEntry textAutoSizeEntry = _textList[i];
				if (textAutoSizeEntry.Mode == TextAutoSizeMode.MeasureAndApply)
				{
					TMP_Text text = textAutoSizeEntry.Text;
					if (IsValidForRefresh(text))
					{
						text.enableAutoSizing = true;
						text.fontSize = text.fontSizeMax;
						text.ForceMeshUpdate(ignoreActiveState: false, forceTextReparsing: true);
						num = Mathf.Min(num, text.fontSize);
						flag = true;
					}
				}
			}
			if (flag)
			{
				for (int j = 0; j < _textList.Count; j++)
				{
					TextAutoSizeEntry entry = _textList[j];
					TMP_Text text2 = entry.Text;
					if (IsValidForRefresh(text2))
					{
						text2.enableAutoSizing = false;
						text2.fontSize = num * GetFontSizeMultiplier(entry);
						text2.ForceMeshUpdate(ignoreActiveState: false, forceTextReparsing: true);
					}
				}
			}
			_isRefreshing = false;
		}

		private bool ContainsText(TMP_Text text)
		{
			for (int i = 0; i < _textList.Count; i++)
			{
				if (_textList[i].Text == text)
				{
					return true;
				}
			}
			return false;
		}

		private static float GetFontSizeMultiplier(TextAutoSizeEntry entry)
		{
			if (!(entry.FontSizeMultiplier > 0f))
			{
				return 1f;
			}
			return entry.FontSizeMultiplier;
		}

		private void ScheduleRefresh()
		{
			if (_refreshCoroutine != null)
			{
				StopCoroutine(_refreshCoroutine);
			}
			_refreshCoroutine = StartCoroutine(RefreshAtEndOfFrame());
		}

		private IEnumerator RefreshAtEndOfFrame()
		{
			yield return new WaitForEndOfFrame();
			_refreshCoroutine = null;
			Refresh();
		}

		private static bool IsValidForRefresh(TMP_Text text)
		{
			if (text != null && text.isActiveAndEnabled)
			{
				return text.rectTransform.rect.size.sqrMagnitude > 0f;
			}
			return false;
		}
	}
}
