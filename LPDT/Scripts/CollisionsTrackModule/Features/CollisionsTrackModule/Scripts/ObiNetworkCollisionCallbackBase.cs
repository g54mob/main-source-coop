using Fusion;
using Obi;

namespace Features.CollisionsTrackModule.Scripts
{
	[NetworkBehaviourWeaved(0)]
	public abstract class ObiNetworkCollisionCallbackBase : NetworkBehaviour, IObiCollisionCallBackBase
	{
		public void InvokeOnCollisionDetected(ObiColliderBase other)
		{
			OnCollisionDetected(other);
		}

		public virtual void OnCollisionDetected(ObiColliderBase other)
		{
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
