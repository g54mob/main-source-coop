using UnityEngine;
using UnityEngine.Animations.Rigging;

public class IKRigHandler : MonoBehaviour
{
	private Player player;

	public TwoBoneIKConstraint left;

	public TwoBoneIKConstraint right;

	private void Start()
	{
		player = GetComponentInParent<Player>();
	}

	public void SetRightHandPosition(Vector3 worldPos, Quaternion rotation)
	{
		Vector3 position = worldPos;
		position = player.refs.ragdoll.GetBodypart(BodypartType.Hip).rig.transform.InverseTransformPoint(position);
		position = player.refs.ragdoll.GetBodypart(BodypartType.Hip).animationTarget.transform.TransformPoint(position);
		player.refs.IK_Hand_R.transform.position = position;
		player.refs.IK_Hand_R.transform.rotation = rotation;
	}

	internal void SetLeftHandPosition(Vector3 worldPos, Quaternion rotation)
	{
		Vector3 position = worldPos;
		position = player.refs.ragdoll.GetBodypart(BodypartType.Hip).rig.transform.InverseTransformPoint(position);
		position = player.refs.ragdoll.GetBodypart(BodypartType.Hip).animationTarget.transform.TransformPoint(position);
		player.refs.IK_Hand_L.transform.position = position;
		player.refs.IK_Hand_L.transform.rotation = rotation;
	}

	public void RigCreated()
	{
		RigCreator component = GetComponent<RigCreator>();
		TwoBoneIKConstraintData data = right.data;
		data.root = component.GetBodypartTransform(BodypartType.Arm_R);
		data.mid = component.GetBodypartTransform(BodypartType.Elbow_R);
		data.tip = component.GetBodypartTransform(BodypartType.Hand_R);
		right.data = data;
		data = left.data;
		data.root = component.GetBodypartTransform(BodypartType.Arm_L);
		data.mid = component.GetBodypartTransform(BodypartType.Elbow_L);
		data.tip = component.GetBodypartTransform(BodypartType.Hand_L);
		left.data = data;
	}
}
