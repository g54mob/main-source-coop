using UnityEngine;

namespace Features.AIModuleStateMachine.Scripts.Core.Sensors
{
	public class EnemyLineOfSightDetector : MonoBehaviour
	{
		[SerializeField]
		private Transform _eyesSight;

		[SerializeField]
		private LayerMask _obstacleLayerMask;

		private Vector3 _lastTargetPosition;

		private bool _hasLastQuery;

		private bool _lastHadObstacle;

		private Vector3 _lastHitPoint;

		public bool HasLineOfSight(Vector3 targetPosition)
		{
			return !IsObstacleBetween(targetPosition);
		}

		public bool IsObstacleBetween(Vector3 targetPosition)
		{
			if (_eyesSight == null)
			{
				return false;
			}
			Vector3 position = _eyesSight.position;
			Vector3 vector = targetPosition - position;
			float magnitude = vector.magnitude;
			_lastTargetPosition = targetPosition;
			_hasLastQuery = true;
			_lastHitPoint = targetPosition;
			if (magnitude <= 0.001f)
			{
				_lastHadObstacle = false;
				return false;
			}
			RaycastHit hitInfo;
			bool flag = (_lastHadObstacle = Physics.Raycast(position, vector.normalized, out hitInfo, magnitude, _obstacleLayerMask));
			if (flag)
			{
				_lastHitPoint = hitInfo.point;
			}
			return flag;
		}

		private void OnDrawGizmos()
		{
			if (!(_eyesSight == null))
			{
				Vector3 position = _eyesSight.position;
				Gizmos.color = Color.yellow;
				Gizmos.DrawWireSphere(position, 0.08f);
				if (!_hasLastQuery)
				{
					Gizmos.color = new Color(1f, 0.85f, 0.2f, 0.75f);
					Gizmos.DrawLine(position, position + _eyesSight.forward * 2f);
				}
				else if (_lastHadObstacle)
				{
					Gizmos.color = Color.red;
					Gizmos.DrawLine(position, _lastHitPoint);
					Gizmos.DrawWireSphere(_lastHitPoint, 0.06f);
					Gizmos.color = new Color(1f, 0.3f, 0.3f, 0.35f);
					Gizmos.DrawLine(_lastHitPoint, _lastTargetPosition);
				}
				else
				{
					Gizmos.color = Color.green;
					Gizmos.DrawLine(position, _lastTargetPosition);
					Gizmos.DrawWireSphere(_lastTargetPosition, 0.06f);
				}
			}
		}
	}
}
