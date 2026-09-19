using UnityEngine;

namespace Features.PhysicsVolumeModule.Scripts
{
	public class WeightGauge : MonoBehaviour
	{
		[SerializeField]
		private Lift _lift;

		[Header("Indicator")]
		[SerializeField]
		private Transform _marker;

		[SerializeField]
		private Vector3 _markerMinLocalPosition = new Vector3(-0.53f, 0f, 0f);

		[SerializeField]
		private Vector3 _markerMaxLocalPosition = new Vector3(0.53f, 0f, 0f);

		[SerializeField]
		private float _smoothing = 4f;

		[Header("Wobble")]
		[Tooltip("Spring stiffness of the sideways wobble — higher = faster oscillation.")]
		[SerializeField]
		private float _wobbleStiffness = 400f;

		[Tooltip("Spring damping — higher = the wobble settles sooner.")]
		[SerializeField]
		private float _wobbleDamping = 6f;

		[Tooltip("Local-X kick per unit of normalized load change — the wobble strength.")]
		[SerializeField]
		private float _wobbleGain = 2f;

		[Tooltip("Normalized distance from an end within which the wobble fades to nothing (0/1 = empty/full).")]
		[SerializeField]
		private float _wobbleEdgeFalloff = 0.12f;

		private const float WOBBLE_STEP_SAFETY = 0.5f;

		private const int MAX_WOBBLE_STEPS = 8;

		private float _normalized;

		private float _lastTarget;

		private float _wobble;

		private float _wobbleVelocity;

		public float NormalizedWeight => _normalized;

		private void Update()
		{
			float num = ((_lift != null && _lift.Volume != null) ? _lift.Volume.SettledWeight : 0f);
			float b = ((_lift != null) ? _lift.MaxWeight : 0f);
			float num2 = Mathf.Clamp01(num / Mathf.Max(0.0001f, b));
			float deltaTime = Time.deltaTime;
			_wobbleVelocity += (num2 - _lastTarget) * _wobbleGain;
			_lastTarget = num2;
			_normalized = Mathf.Lerp(_normalized, num2, 1f - Mathf.Exp((0f - _smoothing) * deltaTime));
			float num3 = 2f / Mathf.Sqrt(Mathf.Max(0.0001f, _wobbleStiffness));
			int num4 = Mathf.Clamp(Mathf.CeilToInt(deltaTime / (num3 * 0.5f)), 1, 8);
			float num5 = deltaTime / (float)num4;
			for (int i = 0; i < num4; i++)
			{
				float num6 = (0f - _wobbleStiffness) * _wobble - _wobbleDamping * _wobbleVelocity;
				_wobbleVelocity += num6 * num5;
				_wobble += _wobbleVelocity * num5;
			}
			if (float.IsNaN(_wobble) || float.IsInfinity(_wobble) || float.IsNaN(_wobbleVelocity) || float.IsInfinity(_wobbleVelocity))
			{
				_wobble = 0f;
				_wobbleVelocity = 0f;
			}
			if (!(_marker == null))
			{
				float num7 = Mathf.Clamp01(Mathf.Min(_normalized, 1f - _normalized) / Mathf.Max(0.0001f, _wobbleEdgeFalloff));
				Vector3 localPosition = Vector3.Lerp(_markerMinLocalPosition, _markerMaxLocalPosition, _normalized);
				localPosition.x += _wobble * num7;
				_marker.localPosition = localPosition;
			}
		}
	}
}
