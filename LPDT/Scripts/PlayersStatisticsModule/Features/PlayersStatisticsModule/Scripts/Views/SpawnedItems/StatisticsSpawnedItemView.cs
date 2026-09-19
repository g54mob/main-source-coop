using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Features.PlayersStatisticsModule.Scripts.Views.SpawnedItems
{
	public class StatisticsSpawnedItemView : StatisticsSpawnedItemViewBase
	{
		[SerializeField]
		private Image _iconImage;

		[SerializeField]
		private TMP_Text _countText;

		[SerializeField]
		private GameObject _countContainer;

		[SerializeField]
		private float _minRotationZ = -12f;

		[SerializeField]
		private float _maxRotationZ = 12f;

		private void Start()
		{
			float minInclusive = Mathf.Min(_minRotationZ, _maxRotationZ);
			float maxInclusive = Mathf.Max(_minRotationZ, _maxRotationZ);
			float z = Random.Range(minInclusive, maxInclusive);
			Vector3 localEulerAngles = base.transform.localEulerAngles;
			localEulerAngles.z = z;
			base.transform.localEulerAngles = localEulerAngles;
		}

		public override void SetIcon(Sprite icon)
		{
			if (!(_iconImage == null))
			{
				_iconImage.sprite = icon;
				_iconImage.enabled = icon != null;
			}
		}

		public override void SetCount(int count)
		{
			if (!(_countText == null))
			{
				bool flag = count > 1;
				if (_countContainer != null)
				{
					_countContainer.SetActive(flag);
				}
				if (flag)
				{
					_countText.SetText(count.ToString());
				}
			}
		}
	}
}
