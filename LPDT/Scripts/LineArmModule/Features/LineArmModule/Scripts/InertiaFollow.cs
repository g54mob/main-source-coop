using Fusion;
using UnityEngine;

namespace Features.LineArmModule.Scripts
{
	[NetworkBehaviourWeaved(0)]
	public class InertiaFollow : NetworkBehaviour
	{
		[SerializeField]
		private Transform _target;

		[SerializeField]
		private float _followSpeed = 5f;

		[SerializeField]
		private float _maxDistance = 10f;

		private bool _isInitialized;

		public float FollowSpeed
		{
			get
			{
				return _followSpeed;
			}
			set
			{
				_followSpeed = value;
			}
		}

		public override void Spawned()
		{
			_isInitialized = true;
		}

		public void SetTarget(Transform target)
		{
			_target = target;
		}

		private void LateUpdate()
		{
			if (_isInitialized && (bool)_target)
			{
				Vector3 position = base.transform.position;
				Vector3 position2 = _target.position;
				if (Vector3.Distance(position, position2) > _maxDistance)
				{
					base.transform.position = position2;
					return;
				}
				float maxDistanceDelta = _followSpeed * Time.deltaTime;
				base.transform.position = Vector3.MoveTowards(position, position2, maxDistanceDelta);
			}
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
