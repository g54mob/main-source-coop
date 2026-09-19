using Features.StatsUsageModule.Scripts.Entities.EntityStatTypeEntities;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Features.StoreModule.Scripts
{
	public class StoreStatsView : StoreStatsViewBase
	{
		[SerializeField]
		private EntityStatType _trackedStatType;

		[SerializeField]
		private TMP_Text _maxStatValueText;

		[SerializeField]
		private TMP_Text _currentStatValueText;

		[SerializeField]
		private RectTransform _rectTransformToUpdate;

		[SerializeField]
		private Slider _statSlider;

		[SerializeField]
		private Transform _holder;

		public override EntityStatType TrackedStatType => _trackedStatType;

		public override void SetMaxValue(float health)
		{
			_maxStatValueText.SetText("/" + Mathf.RoundToInt(health));
			LayoutRebuilder.ForceRebuildLayoutImmediate(_rectTransformToUpdate);
			if (_statSlider != null)
			{
				_statSlider.maxValue = health;
			}
		}

		public override void SetCurrentValue(float health)
		{
			_currentStatValueText.SetText(Mathf.RoundToInt(health).ToString());
			LayoutRebuilder.ForceRebuildLayoutImmediate(_rectTransformToUpdate);
			if (_statSlider != null)
			{
				_statSlider.value = health;
			}
		}
	}
}
