using UnityEngine;

namespace Obi.Samples
{
	public class HighlightCollidingRopes : MonoBehaviour
	{
		public void Highlight(ActorActorCollisionDetector.ActorPair pair)
		{
			if (pair.actorA.TryGetComponent<ActorBlinker>(out var component))
			{
				component.Blink(pair.particleA);
			}
			if (pair.actorB.TryGetComponent<ActorBlinker>(out var component2))
			{
				component2.Blink(pair.particleB);
			}
		}
	}
}
