using Obi;
using UnityEngine;

namespace Features.CollisionsTrackModule.Scripts
{
	public abstract class ObiCollisionCallbackBase : MonoBehaviour, IObiCollisionCallBackBase
	{
		public void InvokeOnCollisionDetected(ObiColliderBase other)
		{
			OnCollisionDetected(other);
		}

		public virtual void OnCollisionDetected(ObiColliderBase other)
		{
		}
	}
}
