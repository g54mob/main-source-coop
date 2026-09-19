using Fusion;
using UnityEngine;

namespace Features.ChunkSystem
{
	public class PlayerActiveChunksPoint : ActiveChunksPoint
	{
		[SerializeField]
		private NetworkObject _networkObject;

		[SerializeField]
		[Min(0.01f)]
		private float _updateInterval = 0.1f;

		private Vector3 _lastPosition;

		private float _timer;

		protected override Vector3Int AxisMask => new Vector3Int(1, 0, 1);

		private void Update()
		{
			if (_networkObject == null || !_networkObject.HasInputAuthority)
			{
				return;
			}
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
