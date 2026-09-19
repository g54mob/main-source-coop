using Fusion;
using UnityEngine;
using UnityEngine.AI;

namespace Features.SnakeModule.Scripts
{
	[NetworkBehaviourWeaved(0)]
	public class SnakeVisualSlither : NetworkBehaviour
	{
		private const float MIN_SPEED_SQR = 0.0001f;

		private const float WEIGHT_EPSILON = 0.0001f;

		[Tooltip("What sways left/right — SnakeView (child of Visual), not Visual itself.")]
		[SerializeField]
		private Transform _target;

		[Tooltip("Provides right axis — Visual after it faces agent velocity.")]
		[SerializeField]
		private Transform _axes;

		[SerializeField]
		private NavMeshAgent _navMeshAgent;

		[SerializeField]
		private float _amplitude = 0.25f;

		[SerializeField]
		private float _frequency = 5f;

		[Tooltip("Smooth blend-in when slither becomes active.")]
		[SerializeField]
		private float _stateBlend = 5f;

		[SerializeField]
		private bool _onlyWhenMoving = true;

		[SerializeField]
		private float _minSpeed = 0.05f;

		[SerializeField]
		private float _speedBlend = 5f;

		private Vector3 _restLocalPosition;

		private bool _hasRestPosition;

		private bool _wantSlitherActive;

		private float _stateWeight;

		private float _motionWeight;

		private float _phase;

		private bool _hasFrequencyOverride;

		private float _frequencyOverride;

		public void SetSlitherActive(bool active)
		{
			if (active)
			{
				_wantSlitherActive = true;
			}
			else if (_wantSlitherActive || !(_stateWeight <= 0.0001f))
			{
				_wantSlitherActive = false;
				_stateWeight = 0f;
				_motionWeight = 0f;
				RestoreRestLocalPosition();
			}
		}

		public void SetFrequencyOverride(float frequency)
		{
			_hasFrequencyOverride = true;
			_frequencyOverride = Mathf.Max(0f, frequency);
		}

		public void ClearFrequencyOverride()
		{
			_hasFrequencyOverride = false;
			_frequencyOverride = 0f;
		}

		private void Awake()
		{
			CacheRestPosition();
		}

		private void OnDisable()
		{
			NetworkBehaviourUtils.InternalOnDisable(this);
			_wantSlitherActive = false;
			_stateWeight = 0f;
			_motionWeight = 0f;
		}

		public void TickLate()
		{
			if (!_wantSlitherActive || _target == null)
			{
				return;
			}
			if (!_hasRestPosition)
			{
				CacheRestPosition();
			}
			UpdateStateWeight();
			if (!(_stateWeight <= 0.0001f))
			{
				UpdateMotionWeight();
				Transform transform = ((_axes != null) ? _axes : _target.parent);
				if (transform == null)
				{
					transform = _target;
				}
				_phase += GetEffectiveFrequency() * Time.deltaTime;
				float num = Mathf.Sin(_phase) * _amplitude * _stateWeight * _motionWeight;
				Vector3 vector = ((_target.parent != null) ? _target.parent.InverseTransformDirection(transform.right) : transform.right);
				_target.localPosition = _restLocalPosition + vector * num;
			}
		}

		private void UpdateStateWeight()
		{
			float t = 1f - Mathf.Exp((0f - _stateBlend) * Time.deltaTime);
			_stateWeight = Mathf.Lerp(_stateWeight, 1f, t);
		}

		private void CacheRestPosition()
		{
			if (_target == null)
			{
				_hasRestPosition = false;
				return;
			}
			_restLocalPosition = _target.localPosition;
			_hasRestPosition = true;
		}

		private void RestoreRestLocalPosition()
		{
			if (_hasRestPosition && !(_target == null))
			{
				_target.localPosition = _restLocalPosition;
			}
		}

		private void UpdateMotionWeight()
		{
			if (!_onlyWhenMoving)
			{
				_motionWeight = 1f;
				return;
			}
			float b = (IsMoving() ? 1f : 0f);
			float t = 1f - Mathf.Exp((0f - _speedBlend) * Time.deltaTime);
			_motionWeight = Mathf.Lerp(_motionWeight, b, t);
		}

		private float GetEffectiveFrequency()
		{
			if (!_hasFrequencyOverride)
			{
				return _frequency;
			}
			return _frequencyOverride;
		}

		private bool IsMoving()
		{
			if (_navMeshAgent == null || !_navMeshAgent.isActiveAndEnabled)
			{
				return true;
			}
			Vector3 velocity = _navMeshAgent.velocity;
			velocity.y = 0f;
			return velocity.sqrMagnitude >= _minSpeed * _minSpeed;
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
		}
	}
}
