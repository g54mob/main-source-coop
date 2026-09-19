using Obi;
using UnityEngine;

namespace Features.CollisionsTrackModule.Scripts
{
	public class ObiCollisionTracker : MonoBehaviour
	{
		[SerializeField]
		private ObiSolver _obiSolver;

		private IObiCollisionCallBackBase _callbackBase;

		private void Start()
		{
			_callbackBase = GetComponentInChildren<IObiCollisionCallBackBase>();
		}

		private void OnEnable()
		{
			_obiSolver.OnCollision += ResolveCollisions;
		}

		private void OnDisable()
		{
			_obiSolver.OnCollision -= ResolveCollisions;
		}

		private void ResolveCollisions(ObiSolver solver, ObiNativeContactList contacts)
		{
			foreach (Oni.Contact contact in contacts)
			{
				ObiColliderBase owner = ObiColliderWorld.GetInstance().colliderHandles[contact.bodyB].owner;
				_callbackBase?.InvokeOnCollisionDetected(owner);
			}
		}
	}
}
