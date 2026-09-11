using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class RigCreatorBodypart
{
	public enum ColliderMaterial
	{
		Default = 0,
		Slippery = 1
	}

	[Serializable]
	public class RigCreatorColliderData
	{
		public ColliderType colliderType;

		public Vector3 colliderPosition;

		public Vector3 colliderRotation;

		public Vector3 colliderScale = Vector3.one;

		public ColliderMaterial physicsMaterial;

		public int overrideLayer;
	}

	public BodypartType partType;

	public float mass = 10f;

	public bool useMovementForceMultiplier;

	public float movementForceMultiplier = 1f;

	public Vector3 rotation;

	public List<RigCreatorColliderData> colliders = new List<RigCreatorColliderData>();

	[HideInInspector]
	public ColliderType colliderType;

	[HideInInspector]
	public Vector3 colliderPosition;

	[HideInInspector]
	public Vector3 colliderRotation;

	[HideInInspector]
	public Vector3 colliderScale = Vector3.one;

	[HideInInspector]
	public ColliderMaterial physicsMaterial;

	[HideInInspector]
	public int overrideLayer;

	public JointConfig joint;

	public GameObject rigObject;

	public Rigidbody rig;

	public Transform rigObjectParent;
}
