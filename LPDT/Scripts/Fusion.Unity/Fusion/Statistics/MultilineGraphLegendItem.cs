using System;
using UnityEngine;
using UnityEngine.UI;

namespace Fusion.Statistics
{
	public class MultilineGraphLegendItem : MonoBehaviour
	{
		[SerializeField]
		private Image _swatch;

		[SerializeField]
		private Image[] _dashedSwatch;

		[SerializeField]
		private Text _label;

		private Color _color;

		private bool _visible = true;

		public Action<bool> OnToggled;

		public void UI_Clicked()
		{
			_visible = !_visible;
			OnToggled?.Invoke(_visible);
			UpdateColors();
		}

		public void Set(string label, Color color, bool usesDashedSwatch)
		{
			_color = color;
			GetColor();
			_label.text = label;
			_swatch.gameObject.SetActive(!usesDashedSwatch);
			Image[] dashedSwatch = _dashedSwatch;
			for (int i = 0; i < dashedSwatch.Length; i++)
			{
				dashedSwatch[i].gameObject.SetActive(usesDashedSwatch);
			}
			UpdateColors();
		}

		private void UpdateColors()
		{
			Color color = GetColor();
			_label.color = color;
			_swatch.color = color;
			Image[] dashedSwatch = _dashedSwatch;
			for (int i = 0; i < dashedSwatch.Length; i++)
			{
				dashedSwatch[i].color = color;
			}
		}

		private Color GetColor()
		{
			if (!_visible)
			{
				return Color.grey;
			}
			return _color;
		}
	}
}
