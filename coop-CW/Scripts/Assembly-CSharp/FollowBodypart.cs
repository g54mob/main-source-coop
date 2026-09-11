using UnityEngine;

public class FollowBodypart : MonoBehaviour
{
	public BodypartType target;

	private void Start()
	{
		base.transform.SetParent(GetComponentInParent<Player>().refs.ragdoll.GetBodypart(target).rig.transform, worldPositionStays: true);
	}
}
