using UnityEngine;

namespace Features.RumModule.Scripts.Views
{
	public class RumBuffsView : RumBuffsViewBase
	{
		[SerializeField]
		private Transform _itemsContainer;

		[SerializeField]
		private TemporalRumBuffItemViewBase _temporalItemPrefab;

		[SerializeField]
		private PermanentRumBuffItemViewBase _permanentItemPrefab;

		public override Transform ItemsContainer => _itemsContainer;

		public override TemporalRumBuffItemViewBase TemporalItemPrefab => _temporalItemPrefab;

		public override PermanentRumBuffItemViewBase PermanentItemPrefab => _permanentItemPrefab;

		public override void SetVisible(bool visible)
		{
			_itemsContainer.gameObject.SetActive(visible);
		}
	}
}
