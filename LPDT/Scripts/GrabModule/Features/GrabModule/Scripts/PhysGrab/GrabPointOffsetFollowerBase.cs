using UnityEngine;

namespace Features.GrabModule.Scripts.PhysGrab
{
	public abstract class GrabPointOffsetFollowerBase : MonoBehaviour
	{
		[SerializeField]
		private Transform _followPoint;

		[SerializeField]
		private Vector3 _offset;

		[SerializeField]
		private bool _useInterpolation;

		[SerializeField]
		private float _positionInterpolationSpeed = 15f;

		[SerializeField]
		private float _maxPositionError = 2f;

		private bool _hasTarget;

		private Vector3 _targetPosition;

		protected abstract bool IsGrabbing { get; }

		protected abstract Transform GrabbedTransform { get; }

		protected abstract Transform HolderTransform { get; }

		private void FixedUpdate()
		{
			if (!IsGrabbing)
			{
				_hasTarget = false;
				return;
			}
			_targetPosition = GrabbedTransform.position + HolderTransform.TransformDirection(_offset);
			if (!_useInterpolation)
			{
				_followPoint.position = _targetPosition;
				_hasTarget = false;
			}
			else if (!_hasTarget)
			{
				_followPoint.position = _targetPosition;
				_hasTarget = true;
			}
		}

		private void Update()
		{
			if (_useInterpolation && _hasTarget)
			{
				if (Vector3.Distance(_followPoint.position, _targetPosition) > _maxPositionError)
				{
					_followPoint.position = _targetPosition;
				}
				else
				{
					_followPoint.position = Vector3.Lerp(_followPoint.position, _targetPosition, Time.deltaTime * _positionInterpolationSpeed);
				}
			}
		}
	}
}
