using System;
using System.Collections.Generic;
using Photon.Pun;
using Sirenix.Utilities;
using UnityEngine;

public class HelperFunctions : MonoBehaviour
{
	public enum LayerType
	{
		Terrain = 0,
		TerrainProp = 1,
		Prop = 2,
		All = 3,
		Interactable = 4,
		Tangible = 5
	}

	public static LayerMask terrainMask = LayerMask.GetMask("Terrain");

	public static LayerMask terrainPropMask = LayerMask.GetMask("Terrain", "Prop");

	public static LayerMask AllMask = LayerMask.GetMask("Terrain", "Prop", "Player", "Interactable", "Interactible_NoCollide", "Default");

	public static LayerMask tangibleMask = LayerMask.GetMask("Terrain", "Prop", "Player", "Default");

	public static LayerMask PropMask = LayerMask.GetMask("Prop");

	public static LayerMask InteractableMask = LayerMask.GetMask("Interactable", "Interactible_NoCollide");

	private static Vector3 m_downVector = new Vector3(0f, -1000f, 0f);

	private static RaycastHit[] m_hit = new RaycastHit[1];

	internal static Terrain GetTerrain(Vector3 center)
	{
		RaycastHit raycastHit = LineCheck(center + Vector3.up * 1000f, center - Vector3.up * 1000f, LayerType.Terrain);
		if ((bool)raycastHit.transform)
		{
			return raycastHit.transform.GetComponent<Terrain>();
		}
		return null;
	}

	public static LayerMask GetMask(LayerType layerType)
	{
		return layerType switch
		{
			LayerType.Terrain => terrainMask, 
			LayerType.Prop => PropMask, 
			LayerType.All => AllMask, 
			LayerType.Interactable => InteractableMask, 
			LayerType.Tangible => tangibleMask, 
			_ => terrainPropMask, 
		};
	}

	public static int GetLayer(LayerType layerType)
	{
		return layerType switch
		{
			LayerType.Terrain => LayerMask.NameToLayer("Terrain"), 
			LayerType.Prop => LayerMask.NameToLayer("Prop"), 
			LayerType.Interactable => LayerMask.NameToLayer("Interactable"), 
			_ => throw new Exception($"No way to get layer from LayerType: {layerType}"), 
		};
	}

	public static Vector3 GetGroundPos(Vector3 from, LayerType layerType)
	{
		Vector3 result = from;
		RaycastHit hitInfo = default(RaycastHit);
		Physics.Raycast(new Ray(from, from + m_downVector), out hitInfo, Vector3.Distance(from, from + m_downVector), GetMask(layerType));
		if ((bool)hitInfo.transform)
		{
			result = hitInfo.point;
		}
		return result;
	}

	public static RaycastHit GetGroundPosRaycast(Vector3 from, LayerType layerType)
	{
		RaycastHit hitInfo = default(RaycastHit);
		Physics.Raycast(new Ray(from, from + m_downVector), out hitInfo, Vector3.Distance(from, from + m_downVector), GetMask(layerType));
		return hitInfo;
	}

	internal static GameObject InstantiatePrefab(GameObject sourceObj, Transform transform)
	{
		return UnityEngine.Object.Instantiate(sourceObj, transform);
	}

	public static RaycastHit GetGroundPosRaycast(Vector3 from, LayerType layerType, Vector3 gravityDir)
	{
		Vector3 vector = from + gravityDir * (0f - m_downVector.y);
		RaycastHit hitInfo = default(RaycastHit);
		Physics.Raycast(new Ray(from, vector - from), out hitInfo, Vector3.Distance(from, vector), GetMask(layerType));
		return hitInfo;
	}

	public static RaycastHit LineCheckNonAlloc(Vector3 from, Vector3 to, LayerType layerType)
	{
		if (Physics.RaycastNonAlloc(new Ray(from, to - from), m_hit, Vector3.Distance(from, to), GetMask(layerType)) <= 0)
		{
			return default(RaycastHit);
		}
		return m_hit[0];
	}

	public static RaycastHit LineCheck(Vector3 from, Vector3 to, LayerType layerType)
	{
		RaycastHit hitInfo = default(RaycastHit);
		Physics.Raycast(new Ray(from, to - from), out hitInfo, Vector3.Distance(from, to), GetMask(layerType));
		return hitInfo;
	}

	public static RaycastHit SphereLineCheck(Vector3 from, Vector3 to, LayerType layerType, float radius)
	{
		if (Physics.SphereCastNonAlloc(new Ray(from, to - from), radius, m_hit, Vector3.Distance(from, to), GetMask(layerType)) <= 0)
		{
			return default(RaycastHit);
		}
		return m_hit[0];
	}

	public static RaycastHit[] LineCheckAll(Vector3 from, Vector3 to, LayerType layerType)
	{
		return Physics.RaycastAll(from, to - from, Vector3.Distance(from, to), GetMask(layerType));
	}

	public static RaycastHit[] SphereLineCheckAll(Vector3 from, Vector3 to, LayerType layerType, float radius = 0f)
	{
		return Physics.SphereCastAll(from, radius, to - from, Vector3.Distance(from, to), GetMask(layerType));
	}

	internal static ConfigurableJoint AttachPositionJoint(Rigidbody rig1, Rigidbody rig2, bool useCustomConnection = false, Vector3 customConnectionPoint = default(Vector3))
	{
		ConfigurableJoint configurableJoint = rig1.gameObject.AddComponent<ConfigurableJoint>();
		configurableJoint.xMotion = ConfigurableJointMotion.Locked;
		configurableJoint.yMotion = ConfigurableJointMotion.Locked;
		configurableJoint.zMotion = ConfigurableJointMotion.Locked;
		configurableJoint.projectionMode = JointProjectionMode.PositionAndRotation;
		configurableJoint.anchor = ((!useCustomConnection) ? rig1.transform.InverseTransformPoint(rig2.position) : rig1.transform.InverseTransformPoint(customConnectionPoint));
		configurableJoint.enableCollision = false;
		configurableJoint.connectedBody = rig2;
		return configurableJoint;
	}

	internal static Joint AttachFixedJoint(Rigidbody rig1, Rigidbody rig2)
	{
		FixedJoint fixedJoint = rig1.gameObject.AddComponent<FixedJoint>();
		fixedJoint.enableCollision = false;
		fixedJoint.connectedBody = rig2;
		return fixedJoint;
	}

	internal static Vector3 RandomOnFlatCircle()
	{
		Vector2 insideUnitCircle = UnityEngine.Random.insideUnitCircle;
		return new Vector3(insideUnitCircle.x, 0f, insideUnitCircle.y);
	}

	internal static void DestroyAll(UnityEngine.Object[] objects)
	{
		for (int num = objects.Length - 1; num >= 0; num--)
		{
			UnityEngine.Object.Destroy(objects[num]);
		}
	}

	internal static Vector3 EulerToLook(Vector2 euler)
	{
		return new Vector3(euler.y, 0f - euler.x, 0f);
	}

	internal static Vector3 LookToEuler(Vector2 lookRotationValues)
	{
		return new Vector3(0f - lookRotationValues.y, lookRotationValues.x, 0f);
	}

	internal static Vector3 LookToDirection(Vector3 look, Vector3 targetDir)
	{
		return EulerToDirection(LookToEuler(look), targetDir);
	}

	internal static Vector3 EulerToDirection(Vector3 euler, Vector3 targetDir)
	{
		return Quaternion.Euler(euler) * targetDir;
	}

	internal static Vector3 DirectionToEuler(Vector3 dir)
	{
		return Quaternion.LookRotation(dir, Vector3.up).eulerAngles;
	}

	internal static Vector3 DirectionToLook(Vector3 dir)
	{
		Vector3 vector = DirectionToEuler(dir);
		while (vector.x > 180f)
		{
			vector.x -= 360f;
		}
		return EulerToLook(vector);
	}

	internal static Vector3 GroundDirection(Vector3 planeNormal, Vector3 sideDirection)
	{
		return -Vector3.Cross(sideDirection, planeNormal);
	}

	internal static Vector3 SeparateClamps(Vector3 rotationError, float clamp)
	{
		rotationError.x = Mathf.Clamp(rotationError.x, 0f - clamp, clamp);
		rotationError.y = Mathf.Clamp(rotationError.y, 0f - clamp, clamp);
		rotationError.z = Mathf.Clamp(rotationError.z, 0f - clamp, clamp);
		return rotationError;
	}

	internal static float FlatDistance(Vector3 from, Vector3 to)
	{
		return Vector2.Distance(from.XZ(), to.XZ());
	}

	internal static void IgnoreConnect(Rigidbody rig1, Rigidbody rig2)
	{
		rig1.gameObject.AddComponent<ConfigurableJoint>().connectedBody = rig2;
	}

	internal static RaycastHit[] SortRaycastResults(RaycastHit[] hitsToSort)
	{
		hitsToSort.Sort(RaycastHitComparer);
		return hitsToSort;
	}

	private static int RaycastHitComparer(RaycastHit x, RaycastHit y)
	{
		if (!(x.distance < y.distance))
		{
			return 1;
		}
		return -1;
	}

	internal static Quaternion GetRandomRotationWithUp(Vector3 normal)
	{
		Vector3 onUnitSphere = UnityEngine.Random.onUnitSphere;
		onUnitSphere.y = 0f;
		onUnitSphere = Vector3.Cross(normal, Vector3.Cross(normal, onUnitSphere));
		return Quaternion.LookRotation(onUnitSphere, normal);
	}

	internal static Bounds GetTotalBounds(GameObject gameObject)
	{
		MeshRenderer[] componentsInChildren = gameObject.GetComponentsInChildren<MeshRenderer>();
		Bounds result = default(Bounds);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			if (i == 0)
			{
				result = componentsInChildren[i].bounds;
			}
			else
			{
				result.Encapsulate(componentsInChildren[i].bounds);
			}
		}
		return result;
	}

	internal static float FlatAngle(Vector3 dir1, Vector3 dir2)
	{
		return Vector2.Angle(dir1.Flat(), dir2.Flat());
	}

	internal static void SetChildCollidersLayer(Transform root, int layerID)
	{
		Collider[] componentsInChildren = root.GetComponentsInChildren<Collider>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].gameObject.layer = layerID;
		}
	}

	internal static void SetChildRendererLayer(Transform root, int layerID)
	{
		Renderer[] componentsInChildren = root.GetComponentsInChildren<Renderer>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].gameObject.layer = layerID;
		}
	}

	internal static float GetCameraDistanceMultiplier(Vector3 position, float range)
	{
		if (range == 0f)
		{
			return 1f;
		}
		if (position == Vector3.zero)
		{
			return 1f;
		}
		float value = Vector3.Distance(MainCamera.instance.transform.position, position);
		return Mathf.InverseLerp(range, 0f, value);
	}

	internal static void SetJointDrive(ConfigurableJoint joint, float spring, float damper, Rigidbody rig)
	{
		JointDrive angularXDrive = joint.angularXDrive;
		angularXDrive.positionSpring = spring * rig.mass;
		angularXDrive.positionDamper = damper * rig.mass;
		joint.angularXDrive = angularXDrive;
		joint.angularYZDrive = angularXDrive;
	}

	internal static Transform FindChildRecursive(string targetName, Transform root)
	{
		if (root.gameObject.name.ToUpper() == targetName.ToUpper())
		{
			return root;
		}
		for (int i = 0; i < root.childCount; i++)
		{
			Transform transform = FindChildRecursive(targetName, root.GetChild(i));
			if (!(transform == null) && transform.gameObject.name.ToUpper() == targetName.ToUpper())
			{
				return transform;
			}
		}
		return null;
	}

	internal static void PhysicsRotateTowards(Rigidbody rig, Vector3 from, Vector3 to, float force)
	{
		Vector3 vector = Vector3.Cross(from, to).normalized * Vector3.Angle(from, to);
		rig.AddTorque(vector * force, ForceMode.Acceleration);
	}

	internal static Vector3 MultiplyVectors(Vector3 v1, Vector3 v2)
	{
		v1.x *= v2.x;
		v1.y *= v2.y;
		v1.z *= v2.z;
		return v1;
	}

	public static Vector3 CubicBezier(Vector3 Start, Vector3 _P1, Vector3 _P2, Vector3 end, float _t)
	{
		return (1f - _t) * QuadraticBezier(Start, _P1, _P2, _t) + _t * QuadraticBezier(_P1, _P2, end, _t);
	}

	public static Vector3 QuadraticBezier(Vector3 start, Vector3 _P1, Vector3 end, float _t)
	{
		return (1f - _t) * LinearBezier(start, _P1, _t) + _t * LinearBezier(_P1, end, _t);
	}

	public static Vector3 LinearBezier(Vector3 start, Vector3 end, float _t)
	{
		return (1f - _t) * start + _t * end;
	}

	internal static Vector3 GetRandomPositionInBounds(Bounds bounds)
	{
		return new Vector3(Mathf.Lerp(bounds.min.x, bounds.max.x, UnityEngine.Random.value), Mathf.Lerp(bounds.min.y, bounds.max.y, UnityEngine.Random.value), Mathf.Lerp(bounds.min.z, bounds.max.z, UnityEngine.Random.value));
	}

	internal static void SpawnPrefab(GameObject gameObject, Vector3 position, Quaternion rotation, Transform transform)
	{
		GameObject gameObject2 = null;
		if (!Application.isEditor)
		{
			gameObject2 = UnityEngine.Object.Instantiate(gameObject);
		}
		gameObject2.transform.SetParent(transform);
		gameObject2.transform.rotation = rotation;
		gameObject2.transform.position = position;
	}

	internal static Quaternion GetRotationWithUp(Vector3 forward, Vector3 up)
	{
		return Quaternion.LookRotation(Vector3.ProjectOnPlane(forward, up), up);
	}

	internal static float BoxDistance(Vector3 pos1, Vector3 pos2)
	{
		return Mathf.Max(Mathf.Max(Mathf.Max(0f, Mathf.Abs(pos1.x - pos2.x)), Mathf.Abs(pos1.y - pos2.y)), Mathf.Abs(pos1.z - pos2.z));
	}

	internal static bool IsMine(GameObject target)
	{
		PhotonView componentInParent = target.GetComponentInParent<PhotonView>();
		if (!componentInParent)
		{
			return false;
		}
		return componentInParent.IsMine;
	}

	internal static bool CanSee(Transform looker, Vector3 pos, float maxAngle = 70f)
	{
		if (Vector3.Angle(looker.forward, pos - looker.position) > maxAngle)
		{
			return false;
		}
		if ((bool)LineCheck(looker.transform.position, pos, LayerType.TerrainProp).transform)
		{
			return false;
		}
		return true;
	}

	internal static bool InBoxRange(Vector3 position1, Vector3 position2, int range)
	{
		if (Mathf.Abs(position1.x - position2.x) > (float)range)
		{
			return false;
		}
		if (Mathf.Abs(position1.y - position2.y) > (float)range)
		{
			return false;
		}
		if (Mathf.Abs(position1.z - position2.z) > (float)range)
		{
			return false;
		}
		return true;
	}

	internal static UnityEngine.Random.State SetRandomSeedFromWorldPos(Vector3 position, int seed)
	{
		position.x = Mathf.RoundToInt(position.x);
		position.y = Mathf.RoundToInt(position.y);
		position.z = Mathf.RoundToInt(position.z);
		UnityEngine.Random.State state = UnityEngine.Random.state;
		UnityEngine.Random.InitState(Mathf.RoundToInt((float)seed + position.x + position.y * 100f + position.z * 10000f));
		return state;
	}

	public static List<Transform> FindAllChildrenWithTag(string targetTag, Transform target)
	{
		List<Transform> list = new List<Transform>();
		for (int i = 0; i < target.childCount; i++)
		{
			Transform child = target.GetChild(i);
			if (child.name.Contains(targetTag))
			{
				list.Add(child);
			}
			list.AddRange(FindAllChildrenWithTag(targetTag, child));
		}
		return list;
	}
}
