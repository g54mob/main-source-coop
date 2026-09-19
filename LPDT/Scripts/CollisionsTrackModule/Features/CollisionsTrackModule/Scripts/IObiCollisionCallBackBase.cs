using Obi;

namespace Features.CollisionsTrackModule.Scripts
{
	internal interface IObiCollisionCallBackBase
	{
		void InvokeOnCollisionDetected(ObiColliderBase other);

		void OnCollisionDetected(ObiColliderBase other);
	}
}
