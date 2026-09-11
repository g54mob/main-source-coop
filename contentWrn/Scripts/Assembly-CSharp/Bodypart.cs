using System;
using System.Collections.Generic;
using UnityEngine;

public class Bodypart : MonoBehaviour
{
	public BodypartType bodypartType;

	internal Rigidbody rig;

	public float movementForceMultiplier = 1f;

	public BodypartAnimationTarget animationTarget;

	private Quaternion startLocal;

	private Player player;

	private ConfigurableJoint joint;

	internal void Config(BodypartType partType)
	{
		bodypartType = partType;
	}

	internal void InitPart()
	{
		startLocal = base.transform.localRotation;
		rig = GetComponent<Rigidbody>();
		joint = GetComponent<ConfigurableJoint>();
		player = GetComponentInParent<Player>();
	}

	internal void SetTarget(BodypartAnimationTarget bodypartAnimationTarget)
	{
		animationTarget = bodypartAnimationTarget;
	}

	internal void FollowAnimJointDrive()
	{
		if ((bool)joint && (bool)animationTarget)
		{
			joint.SetTargetRotationLocal(animationTarget.transform.localRotation, startLocal);
		}
	}

	internal void FollowAnimSimple(Bodypart hip)
	{
		Vector3 vector = animationTarget.transform.position - hip.animationTarget.transform.position;
		base.transform.SetPositionAndRotation(hip.rig.transform.position + vector, animationTarget.transform.rotation);
	}

	internal void FollowAnimJointDriveVelocity(Transform physicsRoot, Transform animationRoot, float force, bool addOpposingForce, float torque)
	{
		Transform transform = animationTarget.transform;
		if ((bool)joint && (bool)animationTarget)
		{
			joint.SetTargetRotationLocal(transform.localRotation, startLocal);
		}
		Vector3 worldCenterOfMass = rig.worldCenterOfMass;
		Vector3 vector = transform.TransformPoint(base.transform.InverseTransformPoint(worldCenterOfMass)) - animationRoot.position;
		Vector3 vector2 = (physicsRoot.position + vector - worldCenterOfMass) * force;
		rig.AddForce(vector2, ForceMode.Acceleration);
		if (addOpposingForce)
		{
			AddOpposingForce(vector2 * rig.mass);
		}
		if (!player.data.dead || bodypartType != BodypartType.Hip)
		{
			Vector3 forward = transform.forward;
			Vector3 up = transform.up;
			(Quaternion.LookRotation(forward, up) * Quaternion.Inverse(rig.transform.rotation)).ToAngleAxis(out var angle, out var axis);
			if (angle > 180f)
			{
				angle -= 360f;
			}
			Vector3 torque2 = axis * (MathF.PI / 180f * angle * torque);
			rig.AddTorque(torque2, ForceMode.Acceleration);
		}
	}

	internal void FollowAnimJointVel(Transform physicsRoot, Transform animationRoot, float force, bool addOpposingForce)
	{
		Vector3 vector = animationTarget.transform.TransformPoint(base.transform.InverseTransformPoint(rig.worldCenterOfMass)) - animationRoot.position;
		Vector3 vector2 = (physicsRoot.transform.position + vector - rig.worldCenterOfMass) * force;
		rig.AddForce(vector2, ForceMode.Acceleration);
		if (addOpposingForce)
		{
			Vector3 force2 = vector2 * rig.mass;
			AddOpposingForce(force2);
		}
	}

	private void AddOpposingForce(Vector3 force)
	{
		float totalMass = player.data.totalMass;
		List<Rigidbody> rigList = player.refs.ragdoll.rigList;
		for (int i = 0; i < rigList.Count; i++)
		{
			Rigidbody rigidbody = rigList[i];
			if (rigidbody.gameObject.activeInHierarchy)
			{
				float num = rigidbody.mass / totalMass;
				rigidbody.AddForce(-force * num, ForceMode.Force);
			}
		}
	}

	internal void FollowAnimJointAngularVel(Transform physicsRoot, Transform animationRoot, float torque)
	{
		if (!player.data.dead || bodypartType != BodypartType.Hip)
		{
			Vector3 forward = animationTarget.transform.forward;
			Vector3 up = animationTarget.transform.up;
			Vector3 vector = Vector3.Cross(rig.transform.forward, forward).normalized * Vector3.Angle(rig.transform.forward, forward);
			vector += Vector3.Cross(rig.transform.up, up).normalized * (Vector3.Angle(rig.transform.up, up) * 0.5f);
			rig.AddTorque(vector * torque, ForceMode.Acceleration);
		}
	}

	public void OnCollisionEnter(Collision collision)
	{
		Collide(collision);
	}

	private void OnCollisionStay(Collision collision)
	{
		Collide(collision);
	}

	private void Collide(Collision collision)
	{
		if ((bool)player && !(collision.transform.root == base.transform.root) && (bool)player.refs.ragdoll)
		{
			player.refs.ragdoll.BodyPartCollision(collision, this);
		}
	}
}
