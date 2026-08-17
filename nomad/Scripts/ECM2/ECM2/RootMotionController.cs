using UnityEngine;

namespace ECM2
{
	[RequireComponent(typeof(Animator))]
	public class RootMotionController : MonoBehaviour
	{
		protected Animator _animator;

		protected Vector3 _rootMotionDeltaPosition;

		protected Quaternion _rootMotionDeltaRotation;

		public virtual void FlushAccumulatedDeltas()
		{
			_rootMotionDeltaPosition = Vector3.zero;
			_rootMotionDeltaRotation = Quaternion.identity;
		}

		public virtual Quaternion ConsumeRootMotionRotation()
		{
			Quaternion rootMotionDeltaRotation = _rootMotionDeltaRotation;
			_rootMotionDeltaRotation = Quaternion.identity;
			return rootMotionDeltaRotation;
		}

		public virtual Vector3 GetRootMotionVelocity(float deltaTime)
		{
			if (deltaTime == 0f)
			{
				return Vector3.zero;
			}
			return _rootMotionDeltaPosition / deltaTime;
		}

		public virtual Vector3 ConsumeRootMotionVelocity(float deltaTime)
		{
			Vector3 rootMotionVelocity = GetRootMotionVelocity(deltaTime);
			_rootMotionDeltaPosition = Vector3.zero;
			return rootMotionVelocity;
		}

		public virtual void Awake()
		{
			_animator = GetComponent<Animator>();
			if (_animator == null)
			{
				Debug.LogError("RootMotionController: There is no 'Animator' attached to the '" + base.name + "' game object.\nPlease attach a 'Animator' to the '" + base.name + "' game object");
			}
		}

		public virtual void Start()
		{
			_rootMotionDeltaPosition = Vector3.zero;
			_rootMotionDeltaRotation = Quaternion.identity;
		}

		public virtual void OnAnimatorMove()
		{
			_rootMotionDeltaPosition += _animator.deltaPosition;
			_rootMotionDeltaRotation = _animator.deltaRotation * _rootMotionDeltaRotation;
		}
	}
}
