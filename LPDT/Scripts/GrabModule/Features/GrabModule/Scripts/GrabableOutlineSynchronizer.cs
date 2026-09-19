using UnityEngine;

namespace Features.GrabModule.Scripts
{
	public class GrabableOutlineSynchronizer : OutlineSynchronizer
	{
		[SerializeField]
		private SimplePointGrabable _grabable;

		protected override void Awake()
		{
			base.Awake();
			_grabable.OnOutlineUpdated += ProcessOutlineUpdated;
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();
			_grabable.OnOutlineUpdated -= ProcessOutlineUpdated;
		}

		private void ProcessOutlineUpdated()
		{
			Initialize(_grabable.Outline);
		}
	}
}
