using System;
using UnityEngine;

[Serializable]
public class JointConfig
{
	public bool hasJoint;

	public float springMultiplier = 1f;

	public float dragMultiplier = 1f;

	[Range(-180f, 180f)]
	public float minX;

	[Range(-180f, 180f)]
	public float maxX;

	public float yAngle;

	public float zAngle;

	internal void CopyJointSettings(ConfigurableJoint configurableJoint)
	{
		minX = configurableJoint.lowAngularXLimit.limit;
		maxX = configurableJoint.highAngularXLimit.limit;
		yAngle = configurableJoint.angularYLimit.limit;
		zAngle = configurableJoint.angularZLimit.limit;
	}

	internal ConfigurableJoint SpawnJoint(Rigidbody ownRig, Rigidbody otherRig, float spring, float drag)
	{
		ConfigurableJoint configurableJoint = ownRig.gameObject.AddComponent<ConfigurableJoint>();
		SoftJointLimit lowAngularXLimit = configurableJoint.lowAngularXLimit;
		lowAngularXLimit.limit = minX;
		configurableJoint.lowAngularXLimit = lowAngularXLimit;
		lowAngularXLimit = configurableJoint.highAngularXLimit;
		lowAngularXLimit.limit = maxX;
		configurableJoint.highAngularXLimit = lowAngularXLimit;
		lowAngularXLimit = configurableJoint.angularYLimit;
		lowAngularXLimit.limit = yAngle;
		configurableJoint.angularYLimit = lowAngularXLimit;
		lowAngularXLimit = configurableJoint.angularZLimit;
		lowAngularXLimit.limit = zAngle;
		configurableJoint.angularZLimit = lowAngularXLimit;
		JointDrive angularXDrive = configurableJoint.angularXDrive;
		angularXDrive.positionSpring = springMultiplier * ownRig.mass * spring;
		angularXDrive.positionDamper = dragMultiplier * ownRig.mass * drag;
		configurableJoint.angularXDrive = angularXDrive;
		configurableJoint.angularYZDrive = angularXDrive;
		configurableJoint.angularXMotion = ConfigurableJointMotion.Limited;
		configurableJoint.angularYMotion = ConfigurableJointMotion.Limited;
		configurableJoint.angularZMotion = ConfigurableJointMotion.Limited;
		configurableJoint.xMotion = ConfigurableJointMotion.Locked;
		configurableJoint.yMotion = ConfigurableJointMotion.Locked;
		configurableJoint.zMotion = ConfigurableJointMotion.Locked;
		configurableJoint.projectionMode = JointProjectionMode.PositionAndRotation;
		configurableJoint.connectedBody = otherRig;
		return configurableJoint;
	}
}
