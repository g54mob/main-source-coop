using UnityEngine;

namespace Features.GrabModule.Scripts.PhysGrab
{
	public sealed class EnemyCarryController
	{
		private SimplePointGrabable _grabable;

		private PhysGrabber _physGrabber;

		private EnemyCarryHandle _enemyCarryHandle;

		private Transform _currentCarryHandle;

		private bool _isLockedCarryActive;

		public Transform CurrentHandle => _currentCarryHandle;

		public bool IsLockedCarryActive => _isLockedCarryActive;

		public Transform Setup(SimplePointGrabable grabable, Transform carryAnchor, PhysGrabber physGrabber)
		{
			Release();
			if (grabable == null || grabable.GrabObject == null)
			{
				return null;
			}
			_grabable = grabable;
			_physGrabber = physGrabber;
			_enemyCarryHandle = ResolveEnemyCarryHandle(grabable);
			_currentCarryHandle = ResolveCarryHandle(grabable, carryAnchor, _enemyCarryHandle);
			if (_enemyCarryHandle != null && _enemyCarryHandle.LockToCarryAnchor)
			{
				RemovePhysGrabLink();
				_enemyCarryHandle.ApplyCarryPhysicsLock();
				_isLockedCarryActive = true;
				ApplyPose(carryAnchor);
				return _currentCarryHandle;
			}
			if (_physGrabber != null && !grabable.GrabObject.Grabbers.Contains(_physGrabber))
			{
				grabable.GrabObject.Grabbers.Add(_physGrabber);
			}
			if (_physGrabber != null)
			{
				_physGrabber.physGrabPoints[grabable.GrabObject] = _currentCarryHandle;
			}
			return _currentCarryHandle;
		}

		public void ApplyPose(Transform carryAnchor)
		{
			if (_isLockedCarryActive && !(_grabable == null) && !(_currentCarryHandle == null) && !(carryAnchor == null))
			{
				Transform transform = _grabable.transform;
				Quaternion rotation = Quaternion.Inverse(transform.rotation) * _currentCarryHandle.rotation;
				Vector3 vector = Quaternion.Inverse(transform.rotation) * (_currentCarryHandle.position - transform.position);
				Quaternion quaternion = carryAnchor.rotation * Quaternion.Inverse(rotation);
				Vector3 position = carryAnchor.position - quaternion * vector;
				transform.SetPositionAndRotation(position, quaternion);
				if (_grabable.Rigidbody != null)
				{
					_grabable.Rigidbody.linearVelocity = Vector3.zero;
					_grabable.Rigidbody.angularVelocity = Vector3.zero;
				}
			}
		}

		public void Release()
		{
			if (_isLockedCarryActive && _enemyCarryHandle != null)
			{
				_enemyCarryHandle.ReleaseCarryPhysicsLock();
			}
			RemovePhysGrabLink();
			_grabable = null;
			_physGrabber = null;
			_enemyCarryHandle = null;
			_currentCarryHandle = null;
			_isLockedCarryActive = false;
		}

		private EnemyCarryHandle ResolveEnemyCarryHandle(SimplePointGrabable grabable)
		{
			EnemyCarryHandle component = grabable.GetComponent<EnemyCarryHandle>();
			if (component != null && component.Handle != null)
			{
				return component;
			}
			return grabable.GetComponentInChildren<EnemyCarryHandle>(includeInactive: true);
		}

		private Transform ResolveCarryHandle(SimplePointGrabable grabable, Transform carryAnchor, EnemyCarryHandle enemyCarryHandle)
		{
			if (enemyCarryHandle != null && enemyCarryHandle.Handle != null)
			{
				return enemyCarryHandle.Handle;
			}
			return grabable.GetNearestHandle(carryAnchor.position);
		}

		private void RemovePhysGrabLink()
		{
			if (!(_grabable == null) && !(_grabable.GrabObject == null) && !(_physGrabber == null))
			{
				_grabable.GrabObject.Grabbers.Remove(_physGrabber);
				_physGrabber.physGrabPoints.Remove(_grabable.GrabObject);
				if (_grabable.Rigidbody != null)
				{
					_grabable.Rigidbody.linearVelocity = Vector3.zero;
				}
			}
		}
	}
}
