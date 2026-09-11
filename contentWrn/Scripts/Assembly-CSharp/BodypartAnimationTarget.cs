using UnityEngine;

public class BodypartAnimationTarget : MonoBehaviour
{
	public BodypartType bodypartType;

	internal void Config(BodypartType partType)
	{
		bodypartType = partType;
	}

	private void Start()
	{
		Player componentInParent = GetComponentInParent<Player>();
		componentInParent.data.physicsAreReady = true;
		componentInParent.refs.ragdoll.GetBodypart(bodypartType).SetTarget(this);
	}
}
