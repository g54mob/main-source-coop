using UnityEngine;
using UnityEngine.UI;

namespace Features.PlayersStatisticsModule.Scripts.Views.SpawnedNewItem
{
	public class SpawnedNewItemView : SpawnedNewItemViewBase
	{
		[SerializeField]
		private Image _image;

		public override void SetImage(Sprite sprite)
		{
			_image.sprite = sprite;
			_image.enabled = sprite != null;
		}

		public override void SetVisible(bool visible)
		{
			base.gameObject.SetActive(visible);
		}
	}
}
