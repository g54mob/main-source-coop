using Features.GrabModule.Scripts;
using UnityEngine;

namespace Features.CompositeItemModule.Scripts
{
	public class PosableHingeJoint : MonoBehaviour
	{
		[SerializeField]
		private HingeJoint _joint;

		[SerializeField]
		private SimplePointGrabable _grabable;

		[SerializeField]
		private float _lockSlackDegrees = 1f;

		private JointLimits _authoredLimits;

		private bool _isLocked;

		private void Awake()
		{
			_authoredLimits = _joint.limits;
			LockAtCurrentAngle();
		}

		private void FixedUpdate()
		{
			if (_joint == null)
			{
				base.enabled = false;
				return;
			}
			bool flag = _grabable.GrabbedByPlayers.Count > 0 || _grabable.GrabbedByExternalsCount > 0 || _grabable.GrabbedBySomethingCount > 0;
			if (flag && _isLocked)
			{
				Unlock();
			}
			else if (!flag && !_isLocked)
			{
				LockAtCurrentAngle();
			}
		}

		private void LockAtCurrentAngle()
		{
			float num = Mathf.Clamp(_joint.angle, _authoredLimits.min, _authoredLimits.max);
			JointLimits limits = _joint.limits;
			limits.min = num - _lockSlackDegrees;
			limits.max = num + _lockSlackDegrees;
			_joint.limits = limits;
			_isLocked = true;
		}

		private void Unlock()
		{
			_joint.limits = _authoredLimits;
			_isLocked = false;
		}
	}
}
