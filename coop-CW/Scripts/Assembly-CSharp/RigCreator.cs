using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RigCreator : MonoBehaviour
{
	public List<RigCreatorBodypart> bodyparts;

	public GameObject source;

	public GameObject sphereCol;

	public GameObject capsuleCol;

	public GameObject boxColldier;

	public PhysicsMaterial slipperyMat;

	public int setDefaultLayer;

	public int smartFillLegs;

	public bool useGravity;

	public float massMultiplier = 1f;

	public bool useTargetRotation = true;

	public float targetRotationSpring = 500f;

	public float targetRotationDragFactor = 0.1f;

	public UnityEvent createRigEvent;

	private void Start()
	{
		Object.Destroy(this);
	}

	public void CreateRig()
	{
		SmartFillLegs();
		Transform transform = base.transform.Find("Rig");
		if ((bool)transform)
		{
			Object.DestroyImmediate(transform.gameObject);
		}
		GameObject gameObject = Object.Instantiate(source, base.transform.position, base.transform.rotation, base.transform);
		gameObject.SetActive(value: true);
		gameObject.name = "Rig";
		ClearMesh(gameObject);
		RegisterParts();
		ConfigRotations();
		AddRigs();
		AddJoints();
		AddCollision();
		AddScripts();
		createRigEvent.Invoke();
	}

	public void ResetColliders()
	{
		for (int i = 0; i < bodyparts.Count; i++)
		{
			bodyparts[i].colliderPosition = Vector3.zero;
			bodyparts[i].colliderRotation = Vector3.zero;
			bodyparts[i].colliderScale = Vector3.one * 0.2f;
		}
		CreateRig();
	}

	private void SmartFillLegs()
	{
		RigCreatorBodypart bodypart = GetBodypart(BodypartType.Leg_1_L);
		if (bodypart != null)
		{
			for (int i = 0; i < smartFillLegs; i++)
			{
				RigCreatorBodypart orAddBodypart = GetOrAddBodypart((BodypartType)(59 + i * 6));
				CopyDataTo(bodypart, orAddBodypart);
				RigCreatorBodypart orAddBodypart2 = GetOrAddBodypart((BodypartType)(62 + i * 6));
				CopyDataTo(bodypart, orAddBodypart2);
			}
		}
		RigCreatorBodypart bodypart2 = GetBodypart(BodypartType.Knee_1_L);
		if (bodypart2 != null)
		{
			for (int j = 0; j < smartFillLegs; j++)
			{
				RigCreatorBodypart orAddBodypart3 = GetOrAddBodypart((BodypartType)(60 + j * 6));
				CopyDataTo(bodypart2, orAddBodypart3);
				RigCreatorBodypart orAddBodypart4 = GetOrAddBodypart((BodypartType)(63 + j * 6));
				CopyDataTo(bodypart2, orAddBodypart4);
			}
		}
		RigCreatorBodypart bodypart3 = GetBodypart(BodypartType.Foot_1_L);
		if (bodypart3 != null)
		{
			for (int k = 0; k < smartFillLegs; k++)
			{
				RigCreatorBodypart orAddBodypart5 = GetOrAddBodypart((BodypartType)(61 + k * 6));
				CopyDataTo(bodypart3, orAddBodypart5);
				RigCreatorBodypart orAddBodypart6 = GetOrAddBodypart((BodypartType)(64 + k * 6));
				CopyDataTo(bodypart3, orAddBodypart6);
			}
		}
	}

	private void CopyDataTo(RigCreatorBodypart from, RigCreatorBodypart to)
	{
		to.mass = from.mass;
		to.rotation = from.rotation;
		to.colliderType = from.colliderType;
		to.colliderPosition = from.colliderPosition;
		to.colliderRotation = from.colliderRotation;
		to.colliderScale = from.colliderScale;
		to.joint.hasJoint = from.joint.hasJoint;
		to.joint.springMultiplier = from.joint.springMultiplier;
		to.joint.dragMultiplier = from.joint.dragMultiplier;
		to.joint.minX = from.joint.minX;
		to.joint.maxX = from.joint.maxX;
		to.joint.yAngle = from.joint.yAngle;
		to.joint.zAngle = from.joint.zAngle;
	}

	private RigCreatorBodypart GetOrAddBodypart(BodypartType bodypartType)
	{
		RigCreatorBodypart bodypart = GetBodypart(bodypartType);
		if (bodypart != null)
		{
			return bodypart;
		}
		RigCreatorBodypart rigCreatorBodypart = new RigCreatorBodypart();
		rigCreatorBodypart.partType = bodypartType;
		rigCreatorBodypart.joint = new JointConfig();
		bodyparts.Add(rigCreatorBodypart);
		return rigCreatorBodypart;
	}

	private RigCreatorBodypart GetBodypart(BodypartType targetBodypart)
	{
		for (int i = 0; i < bodyparts.Count; i++)
		{
			if (bodyparts[i].partType == targetBodypart)
			{
				return bodyparts[i];
			}
		}
		return null;
	}

	private void AddScripts()
	{
		for (int i = 0; i < bodyparts.Count; i++)
		{
			if (bodyparts[i].rigObject == null)
			{
				Debug.LogError(bodyparts[i].partType);
			}
			Bodypart bodypart = bodyparts[i].rigObject.AddComponent<Bodypart>();
			bodypart.Config(bodyparts[i].partType);
			if (bodyparts[i].useMovementForceMultiplier)
			{
				bodypart.movementForceMultiplier = bodyparts[i].movementForceMultiplier;
			}
		}
	}

	private void AddRigs()
	{
		for (int i = 0; i < bodyparts.Count; i++)
		{
			if ((bool)bodyparts[i].rigObject)
			{
				Rigidbody rigidbody = bodyparts[i].rigObject.AddComponent<Rigidbody>();
				rigidbody.mass = bodyparts[i].mass * massMultiplier;
				rigidbody.useGravity = useGravity;
				rigidbody.interpolation = RigidbodyInterpolation.Interpolate;
				bodyparts[i].rig = rigidbody;
			}
		}
	}

	private void AddJoints()
	{
		for (int i = 0; i < bodyparts.Count; i++)
		{
			if ((bool)bodyparts[i].rigObject && bodyparts[i].joint.hasJoint)
			{
				bodyparts[i].rigObject.AddComponent<RigCreatorJoint>().Init(this, bodyparts[i]);
				bodyparts[i].joint.SpawnJoint(bodyparts[i].rig, bodyparts[i].rigObject.transform.parent.GetComponentInParent<Rigidbody>(), useTargetRotation ? targetRotationSpring : 0f, useTargetRotation ? (targetRotationSpring * targetRotationDragFactor) : 0f);
			}
		}
	}

	private void ConfigRotations()
	{
		for (int i = 0; i < bodyparts.Count; i++)
		{
			if ((bool)bodyparts[i].rigObject)
			{
				bodyparts[i].rigObjectParent = bodyparts[i].rigObject.transform.parent;
			}
		}
		for (int j = 0; j < bodyparts.Count; j++)
		{
			if ((bool)bodyparts[j].rigObject)
			{
				bodyparts[j].rigObject.transform.SetParent(null, worldPositionStays: true);
			}
		}
		for (int k = 0; k < bodyparts.Count; k++)
		{
			if ((bool)bodyparts[k].rigObject)
			{
				bodyparts[k].rigObject.transform.Rotate(bodyparts[k].rotation, Space.World);
			}
		}
		for (int l = 0; l < bodyparts.Count; l++)
		{
			if ((bool)bodyparts[l].rigObject)
			{
				bodyparts[l].rigObject.transform.SetParent(bodyparts[l].rigObjectParent);
			}
		}
	}

	private void RegisterParts()
	{
		for (int i = 0; i < bodyparts.Count; i++)
		{
			Transform transform = HelperFunctions.FindChildRecursive(bodyparts[i].partType.ToString(), base.transform);
			if ((bool)transform)
			{
				bodyparts[i].rigObject = transform.gameObject;
			}
		}
	}

	public void MigrateColliders()
	{
		for (int i = 0; i < bodyparts.Count; i++)
		{
			bodyparts[i].colliders = new List<RigCreatorBodypart.RigCreatorColliderData>();
			RigCreatorBodypart.RigCreatorColliderData rigCreatorColliderData = new RigCreatorBodypart.RigCreatorColliderData();
			rigCreatorColliderData.colliderPosition = bodyparts[i].colliderPosition;
			rigCreatorColliderData.colliderRotation = bodyparts[i].colliderRotation;
			rigCreatorColliderData.colliderScale = bodyparts[i].colliderScale;
			rigCreatorColliderData.colliderType = bodyparts[i].colliderType;
			rigCreatorColliderData.overrideLayer = bodyparts[i].overrideLayer;
			rigCreatorColliderData.physicsMaterial = bodyparts[i].physicsMaterial;
			bodyparts[i].colliders.Add(rigCreatorColliderData);
		}
	}

	private void AddCollision()
	{
		for (int i = 0; i < bodyparts.Count; i++)
		{
			if ((bool)bodyparts[i].rigObject)
			{
				AddColliders(bodyparts[i]);
			}
		}
	}

	private void AddColliders(RigCreatorBodypart bodypart)
	{
		for (int i = 0; i < bodypart.colliders.Count; i++)
		{
			GameObject original = null;
			if (bodypart.colliders[i].colliderType == ColliderType.Box)
			{
				original = boxColldier;
			}
			else if (bodypart.colliders[i].colliderType == ColliderType.Capsule)
			{
				original = capsuleCol;
			}
			else if (bodypart.colliders[i].colliderType == ColliderType.Sphere)
			{
				original = sphereCol;
			}
			GameObject gameObject = Object.Instantiate(original, bodypart.rigObject.transform.position, bodypart.rigObject.transform.rotation, bodypart.rigObject.transform);
			gameObject.name = bodypart.partType.ToString() + "_Collider";
			gameObject.transform.localPosition = bodypart.colliders[i].colliderPosition;
			gameObject.transform.localEulerAngles = bodypart.colliders[i].colliderRotation;
			gameObject.transform.localScale = bodypart.colliders[i].colliderScale;
			if (bodypart.colliders[i].physicsMaterial == RigCreatorBodypart.ColliderMaterial.Slippery)
			{
				gameObject.GetComponent<Collider>().sharedMaterial = slipperyMat;
			}
			if (bodypart.colliders[i].overrideLayer != 0 && bodypart.colliders[i].overrideLayer != -1)
			{
				gameObject.layer = bodypart.colliders[i].overrideLayer;
			}
			else
			{
				gameObject.layer = setDefaultLayer;
			}
			gameObject.GetComponent<MeshRenderer>().sharedMaterial = (Material)Resources.Load("M_Debug");
			gameObject.AddComponent<RigCreatorCollider>();
		}
	}

	private void ClearMesh(GameObject spawned)
	{
		if (source.activeSelf)
		{
			SkinnedMeshRenderer[] componentsInChildren = spawned.GetComponentsInChildren<SkinnedMeshRenderer>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].enabled = false;
			}
		}
		Animator[] componentsInChildren2 = spawned.GetComponentsInChildren<Animator>();
		for (int j = 0; j < componentsInChildren2.Length; j++)
		{
			Object.DestroyImmediate(componentsInChildren2[j].gameObject);
		}
		Object.DestroyImmediate(spawned.GetComponent<PlayerVisual>());
		MeshRenderer[] componentsInChildren3 = spawned.GetComponentsInChildren<MeshRenderer>();
		for (int k = 0; k < componentsInChildren3.Length; k++)
		{
			componentsInChildren3[k].enabled = false;
		}
	}

	internal void ClearColliderData()
	{
		for (int i = 0; i < bodyparts.Count; i++)
		{
			bodyparts[i].colliders.Clear();
		}
	}

	internal void SaveBodypartCollider(string colliderObjectName, Transform colTrans)
	{
		try
		{
			RigCreatorBodypart rigCreatorBodypart = null;
			for (int i = 0; i < bodyparts.Count; i++)
			{
				if (colliderObjectName == bodyparts[i].partType.ToString())
				{
					rigCreatorBodypart = bodyparts[i];
					break;
				}
			}
			RigCreatorBodypart.RigCreatorColliderData rigCreatorColliderData = new RigCreatorBodypart.RigCreatorColliderData();
			rigCreatorColliderData.colliderPosition = colTrans.localPosition;
			rigCreatorColliderData.colliderRotation = colTrans.localEulerAngles;
			rigCreatorColliderData.colliderScale = colTrans.localScale;
			if ((bool)colTrans.GetComponent<BoxCollider>())
			{
				rigCreatorColliderData.colliderType = ColliderType.Box;
			}
			else if ((bool)colTrans.GetComponent<SphereCollider>())
			{
				rigCreatorColliderData.colliderType = ColliderType.Sphere;
			}
			else if ((bool)colTrans.GetComponent<CapsuleCollider>())
			{
				rigCreatorColliderData.colliderType = ColliderType.Capsule;
			}
			if (colTrans.GetComponent<Collider>().sharedMaterial == null)
			{
				rigCreatorColliderData.physicsMaterial = RigCreatorBodypart.ColliderMaterial.Default;
			}
			else if (colTrans.GetComponent<Collider>().sharedMaterial == slipperyMat)
			{
				rigCreatorColliderData.physicsMaterial = RigCreatorBodypart.ColliderMaterial.Slippery;
			}
			if (colTrans.gameObject.layer != setDefaultLayer)
			{
				rigCreatorColliderData.overrideLayer = colTrans.gameObject.layer;
			}
			rigCreatorBodypart.colliders.Add(rigCreatorColliderData);
		}
		catch
		{
			Debug.Log("SOMETHING WENT WRONG");
		}
	}

	internal void SaveJoint(RigCreatorBodypart targetPart, ConfigurableJoint configurableJoint)
	{
		for (int i = 0; i < bodyparts.Count; i++)
		{
			if (bodyparts[i].partType == targetPart.partType)
			{
				bodyparts[i].joint.CopyJointSettings(configurableJoint);
				break;
			}
		}
	}

	private void OnDrawGizmosSelected()
	{
		for (int i = 0; i < bodyparts.Count; i++)
		{
			if ((bool)bodyparts[i].rigObject)
			{
				Debug.DrawLine(bodyparts[i].rigObject.transform.position, bodyparts[i].rigObject.transform.position + bodyparts[i].rigObject.transform.forward * 0.15f, Color.blue);
				Debug.DrawLine(bodyparts[i].rigObject.transform.position, bodyparts[i].rigObject.transform.position + bodyparts[i].rigObject.transform.right * 0.15f, Color.red);
				Debug.DrawLine(bodyparts[i].rigObject.transform.position, bodyparts[i].rigObject.transform.position + bodyparts[i].rigObject.transform.up * 0.15f, Color.green);
			}
		}
	}

	internal Transform GetBodypartTransform(BodypartType target)
	{
		for (int i = 0; i < bodyparts.Count; i++)
		{
			if (target == bodyparts[i].partType)
			{
				return bodyparts[i].rigObject.transform;
			}
		}
		return null;
	}
}
