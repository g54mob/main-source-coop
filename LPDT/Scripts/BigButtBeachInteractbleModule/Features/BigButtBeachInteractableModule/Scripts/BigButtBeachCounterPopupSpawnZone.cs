using System;
using UnityEngine;

namespace Features.BigButtBeachInteractableModule.Scripts
{
	public class BigButtBeachCounterPopupSpawnZone : MonoBehaviour
	{
		[SerializeField]
		[Min(0f)]
		private float _radius = 0.5f;

		[SerializeField]
		private Vector2 _movementDirection = Vector2.up;

		public BigButtBeachCounterPopupSpawnData GetSpawnData()
		{
			return new BigButtBeachCounterPopupSpawnData(GetRandomPosition(), GetMovementWorldDirection());
		}

		private Vector3 GetRandomPosition()
		{
			Vector2 vector = UnityEngine.Random.insideUnitCircle * _radius;
			return base.transform.position + base.transform.right * vector.x + base.transform.up * vector.y;
		}

		private Vector2 GetMovementDirection()
		{
			if (!(_movementDirection.sqrMagnitude > 0.0001f))
			{
				return Vector2.up;
			}
			return _movementDirection.normalized;
		}

		private Vector3 GetMovementWorldDirection()
		{
			Vector2 movementDirection = GetMovementDirection();
			return (base.transform.right * movementDirection.x + base.transform.up * movementDirection.y).normalized;
		}

		private void OnDrawGizmosSelected()
		{
			Gizmos.color = Color.cyan;
			Vector3 vector = GetCirclePoint(31, 32);
			for (int i = 0; i < 32; i++)
			{
				Vector3 circlePoint = GetCirclePoint(i, 32);
				Gizmos.DrawLine(vector, circlePoint);
				vector = circlePoint;
			}
			DrawMovementDirectionGizmo();
		}

		private void DrawMovementDirectionGizmo()
		{
			Gizmos.color = Color.yellow;
			Vector3 movementWorldDirection = GetMovementWorldDirection();
			float num = Mathf.Max(_radius, 0.25f);
			Vector3 position = base.transform.position;
			Vector3 vector = position + movementWorldDirection * num;
			Gizmos.DrawLine(position, vector);
			Vector3 vector2 = Quaternion.AngleAxis(150f, base.transform.forward) * movementWorldDirection;
			Vector3 vector3 = Quaternion.AngleAxis(-150f, base.transform.forward) * movementWorldDirection;
			float num2 = num * 0.25f;
			Gizmos.DrawLine(vector, vector + vector2 * num2);
			Gizmos.DrawLine(vector, vector + vector3 * num2);
		}

		private Vector3 GetCirclePoint(int index, int segmentCount)
		{
			float f = (float)index * MathF.PI * 2f / (float)segmentCount;
			return base.transform.position + base.transform.right * (Mathf.Cos(f) * _radius) + base.transform.up * (Mathf.Sin(f) * _radius);
		}
	}
}
