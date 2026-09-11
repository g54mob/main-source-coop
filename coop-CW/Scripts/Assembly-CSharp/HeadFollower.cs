using UnityEngine;

public class HeadFollower : MonoBehaviour
{
	private Player player;

	private Vector3 offset;

	private Vector3 forward;

	private Vector3 up;

	private Bodypart head;

	private void Start()
	{
		player = GetComponentInParent<Player>();
		offset = player.refs.ragdoll.GetBodypart(BodypartType.Head).transform.InverseTransformPoint(base.transform.position);
		forward = player.refs.ragdoll.GetBodypart(BodypartType.Head).transform.InverseTransformDirection(base.transform.forward);
		up = player.refs.ragdoll.GetBodypart(BodypartType.Head).transform.InverseTransformDirection(base.transform.up);
		head = player.refs.ragdoll.GetBodypart(BodypartType.Head);
	}

	private void LateUpdate()
	{
		base.transform.SetPositionAndRotation(head.transform.TransformPoint(offset), Quaternion.LookRotation(head.transform.TransformDirection(forward), head.transform.TransformDirection(up)));
	}
}
