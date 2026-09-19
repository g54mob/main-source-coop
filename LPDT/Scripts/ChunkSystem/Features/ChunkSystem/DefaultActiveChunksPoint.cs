using UnityEngine;

namespace Features.ChunkSystem
{
	public class DefaultActiveChunksPoint : ActiveChunksPoint
	{
		[SerializeField]
		private Vector3Int _axisMask = Vector3Int.one;

		[SerializeField]
		[Min(0.01f)]
		private float _updateInterval = 0.1f;

		private Vector3 _lastPosition;

		private float _timer;

		protected override Vector3Int AxisMask => _axisMask;

		private void Update()
		{
			_timer += Time.deltaTime;
			if (!(_timer < _updateInterval))
			{
				_timer = 0f;
				if (_lastPosition != base.transform.position)
				{
					ApplyChangePosition(base.transform.position);
					_lastPosition = base.transform.position;
				}
			}
		}
	}
}
