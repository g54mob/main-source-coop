using System;
using UnityEngine;

namespace Features.SnakeModule.Scripts
{
	public class SnakeSegmentHitSurface : MonoBehaviour
	{
		[SerializeField]
		private int _segmentIndex;

		public int SegmentIndex => _segmentIndex;

		public event Action<Vector3> OnHit;

		public void SetSegmentIndex(int segmentIndex)
		{
			_segmentIndex = segmentIndex;
		}

		public void NotifyHit(Vector3 hitPoint)
		{
			this.OnHit?.Invoke(hitPoint);
		}
	}
}
