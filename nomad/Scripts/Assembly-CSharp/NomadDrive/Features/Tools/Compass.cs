using NomadDrive.Features.Interaction;
using PrimeTween;
using UnityEngine;

namespace NomadDrive.Features.Tools
{
	public class Compass : HeldItem
	{
		[SerializeField]
		private Transform _compassCap;

		[SerializeField]
		private Transform _compassNeedle;

		[Header("Compass Settings")]
		[SerializeField]
		private float _rotationSpeed = 2f;

		[SerializeField]
		private float _dampingFactor = 0.1f;

		private bool _isOpen;

		private float _currentAngle;

		private float _targetAngle;

		private float _velocity;

		private Sequence _openSequence;

		private Sequence _closeSequence;

		protected override void Awake()
		{
			base.Awake();
		}

		protected override void OnEnable()
		{
			base.OnEnable();
			base.OnEquipped.AddListener(OnEquippedActions);
			base.OnDropped.AddListener(OnDroppedActions);
		}

		protected override void OnDisable()
		{
			base.OnDisable();
			base.OnEquipped.RemoveListener(OnEquippedActions);
			base.OnDropped.RemoveListener(OnDroppedActions);
		}

		private void Update()
		{
			if (_isOpen)
			{
				float y = base.transform.eulerAngles.y;
				_targetAngle = 0f - y;
				UpdateCompassNeedle();
			}
		}

		private void UpdateCompassNeedle()
		{
			_currentAngle = Mathf.SmoothDampAngle(_currentAngle, _targetAngle, ref _velocity, _dampingFactor, _rotationSpeed * 360f);
			_compassNeedle.localRotation = Quaternion.Euler(0f, _currentAngle, 0f);
		}

		public void SetCompassDirection(float direction)
		{
			_compassNeedle.localRotation = Quaternion.Euler(0f, 0f, direction);
		}

		private void OnEquippedActions()
		{
			SetCapState(open: true, instant: false);
		}

		private void OnDroppedActions()
		{
			SetCapState(open: false, instant: false);
		}

		private void SetCapState(bool open, bool instant)
		{
			if (open == _isOpen)
			{
				if (instant && open)
				{
					_compassCap.localRotation = Quaternion.Euler(-110f, 0f, 0f);
				}
				return;
			}
			_isOpen = open;
			if (open)
			{
				_closeSequence.Stop();
				_currentAngle = _compassNeedle.localRotation.eulerAngles.z;
				if (instant)
				{
					_compassCap.localRotation = Quaternion.Euler(-110f, 0f, 0f);
				}
				else
				{
					_openSequence = Sequence.Create().Chain(Tween.LocalRotation(_compassCap, Quaternion.Euler(-110f, 0f, 0f), 0.5f, Ease.OutBounce));
				}
			}
			else
			{
				_openSequence.Stop();
				if (instant)
				{
					_compassCap.localRotation = Quaternion.Euler(0f, 0f, 0f);
				}
				else
				{
					_closeSequence = Sequence.Create().Chain(Tween.LocalRotation(_compassCap, Quaternion.Euler(0f, 0f, 0f), 0.5f, Ease.OutBounce));
				}
			}
		}

		public override void UpdateState()
		{
			base.UpdateState();
			SetCapState(base.IsEquipped, instant: false);
		}

		protected override void SetForLateJoiner()
		{
			base.SetForLateJoiner();
			SetCapState(base.IsEquipped, instant: true);
		}

		public override bool Weaved()
		{
			return true;
		}
	}
}
