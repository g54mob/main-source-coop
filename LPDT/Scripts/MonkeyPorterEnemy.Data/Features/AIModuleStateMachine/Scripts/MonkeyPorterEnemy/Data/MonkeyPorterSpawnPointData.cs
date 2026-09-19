using UnityEngine;

namespace Features.AIModuleStateMachine.Scripts.MonkeyPorterEnemy.Data
{
	public class MonkeyPorterSpawnPointData
	{
		private readonly Transform _point;

		private readonly Vector3 _fallbackPosition;

		private readonly Quaternion _fallbackRotation;

		public Vector3 Position
		{
			get
			{
				if (!(_point != null))
				{
					return _fallbackPosition;
				}
				return _point.position;
			}
		}

		public Quaternion Rotation
		{
			get
			{
				if (!(_point != null))
				{
					return _fallbackRotation;
				}
				return _point.rotation;
			}
		}

		public MonkeyPorterSpawnPointData(Transform point)
		{
			_point = point;
			_fallbackPosition = point.position;
			_fallbackRotation = point.rotation;
		}
	}
}
