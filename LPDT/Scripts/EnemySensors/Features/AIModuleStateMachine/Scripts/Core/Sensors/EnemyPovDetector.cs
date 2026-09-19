using System;
using System.Collections.Generic;
using Features.Movement.Scripts;
using UnityEngine;

namespace Features.AIModuleStateMachine.Scripts.Core.Sensors
{
	public class EnemyPovDetector : MonoBehaviour
	{
		[Serializable]
		private struct MovementStateVisibilityAngle
		{
			public MovementState MovementState;

			public float VisibilityAngle;
		}

		[SerializeField]
		private Transform _eyesSight;

		[SerializeField]
		private float _hardDetectDistance;

		[SerializeField]
		private List<MovementStateVisibilityAngle> _playerVisibilityAngles;

		public bool IsObjectInPov(PlayerCharacterMovableBase characterMovable, Vector3 objectPosition, float distanceToTarget, out DetectionType detectionType)
		{
			detectionType = DetectionType.None;
			if (characterMovable == null || _eyesSight == null)
			{
				return false;
			}
			Vector3 position = _eyesSight.position;
			Vector3 vector = objectPosition - position;
			if (vector.magnitude <= 0.001f)
			{
				return false;
			}
			if (distanceToTarget < _hardDetectDistance)
			{
				detectionType = DetectionType.HardDetection;
				return true;
			}
			TryGetVisibilityAngle(characterMovable.MovementState, out var visibilityAngle);
			if (!(Vector3.Dot(_eyesSight.forward, vector.normalized) >= Mathf.Cos(visibilityAngle * 0.5f * (MathF.PI / 180f))))
			{
				return false;
			}
			detectionType = DetectionType.Default;
			return true;
		}

		private void TryGetVisibilityAngle(MovementState movementState, out float visibilityAngle)
		{
			if (_playerVisibilityAngles != null)
			{
				foreach (MovementStateVisibilityAngle playerVisibilityAngle in _playerVisibilityAngles)
				{
					if (playerVisibilityAngle.MovementState == movementState)
					{
						visibilityAngle = playerVisibilityAngle.VisibilityAngle;
						return;
					}
				}
			}
			visibilityAngle = 0f;
		}

		private void DrawFov(float visibilityAngle, Color color)
		{
			Vector3 position = _eyesSight.position;
			float num = visibilityAngle * 0.5f;
			int num2 = Mathf.Max(1, (int)num / 3);
			Vector3 vector = position;
			for (int i = 0; i <= num2; i++)
			{
				float y = 0f - num + visibilityAngle / (float)num2 * (float)i;
				Vector3 vector2 = Quaternion.Euler(0f, y, 0f) * _eyesSight.forward;
				Vector3 vector3 = position + vector2 * Mathf.Max(_hardDetectDistance, 1f);
				Gizmos.color = color;
				Gizmos.DrawLine(position, vector3);
				if (i > 0)
				{
					Gizmos.DrawLine(vector, vector3);
				}
				vector = vector3;
			}
		}

		private void OnDrawGizmosSelected()
		{
			if (_eyesSight == null)
			{
				return;
			}
			Vector3 position = _eyesSight.position;
			Vector3 to = position + _eyesSight.forward * 2f;
			Gizmos.color = Color.red;
			Gizmos.DrawLine(position, to);
			Gizmos.DrawWireSphere(base.transform.position, _hardDetectDistance);
			if (_playerVisibilityAngles == null)
			{
				return;
			}
			foreach (MovementStateVisibilityAngle playerVisibilityAngle in _playerVisibilityAngles)
			{
				DrawFov(playerVisibilityAngle.VisibilityAngle, GetColorFromState(playerVisibilityAngle.MovementState));
			}
		}

		private Color GetColorFromState(MovementState state)
		{
			float h = (float)state * 0.15f % 1f;
			float s = 0.8f;
			float v = 1f;
			return Color.HSVToRGB(h, s, v);
		}
	}
}
