using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PlayerRagdoll : MonoBehaviour
{
	public float force;

	public float torque;

	public float drag;

	public float angularDrag;

	public bool addOpposingForce = true;

	internal Player player;

	internal List<Rigidbody> rigList = new List<Rigidbody>();

	internal Dictionary<BodypartType, Bodypart> bodypartDict = new Dictionary<BodypartType, Bodypart>();

	internal List<Bodypart> bodypartList = new List<Bodypart>();

	private List<BodyPartCollision> collisionsToHandle = new List<BodyPartCollision>();

	internal Collider[] colliders;

	private ProfilerMarker followAnimMarker = new ProfilerMarker("FollowAnim");

	private ProfilerMarker simplifiedFollowAnimMarker = new ProfilerMarker("SimplifiedFollowAnim");

	private ProfilerMarker dragMarker = new ProfilerMarker("Drag");

	private ProfilerMarker groundRaycastMarker = new ProfilerMarker("GroundRaycast");

	private ProfilerMarker handleCollisionsMarker = new ProfilerMarker("HandleCollisions");

	private ProfilerMarker clearSavedCollisionsMarker = new ProfilerMarker("ClearSavedCollisions");

	internal Action<Collision, Bodypart> collisionAction;

	public SFX_Instance[] ragdollSound;

	private float ragdollSoundTime;

	public static float RagdollIfFellForLongerThan = 1.5f;

	private bool isSplit;

	private void OnEnable()
	{
		RagdollHandler.RegisterRagdoll(this);
	}

	private void OnDisable()
	{
		RagdollHandler.UnregisterRagdoll(this);
	}

	internal void DoInit()
	{
		float num = Time.fixedDeltaTime / 0.016f;
		angularDrag = 1f - (1f - angularDrag) * num;
		drag = 1f - (1f - drag) * num;
		player = GetComponent<Player>();
		Bodypart[] componentsInChildren = GetComponentsInChildren<Bodypart>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].InitPart();
			rigList.Add(componentsInChildren[i].rig);
			bodypartDict.Add(componentsInChildren[i].bodypartType, componentsInChildren[i]);
			bodypartList.Add(componentsInChildren[i]);
		}
		StripRagdoll();
		BodyChanged();
	}

	private void StripRagdoll()
	{
		Transform transform = player.refs.rigRoot.transform.Find("IK");
		if ((bool)transform)
		{
			UnityEngine.Object.DestroyImmediate(transform.gameObject);
			UnityEngine.Object.DestroyImmediate(player.refs.rigRoot.GetComponent<RigBuilder>());
		}
		UnityEngine.Object.DestroyImmediate(player.refs.rigRoot.GetComponent<Animator>());
	}

	private void BodyChanged()
	{
		float num = 0f;
		for (int i = 0; i < rigList.Count; i++)
		{
			num += rigList[i].mass;
		}
		player.data.totalMass = num;
		colliders = player.refs.rigRoot.GetComponentsInChildren<Collider>();
	}

	internal Bodypart GetRandomBodypart()
	{
		return bodypartList[UnityEngine.Random.Range(0, bodypartList.Count)];
	}

	public void ToggleSimplifiedRagdoll(bool setSimple)
	{
		if (setSimple != player.data.simplifiedRagdoll)
		{
			VerboseDebug.Log("Toggled simplified ragdoll: " + setSimple + " for " + player.name);
			player.data.simplifiedRagdoll = setSimple;
			for (int i = 0; i < rigList.Count; i++)
			{
				rigList[i].isKinematic = player.data.simplifiedRagdoll;
				rigList[i].interpolation = ((!setSimple) ? RigidbodyInterpolation.Interpolate : RigidbodyInterpolation.None);
			}
			player.data.lastSimplifiedPosition = player.refs.ragdoll.GetBodypart(BodypartType.Hip).rig.position;
			if (setSimple && player.refs.simpleCollider == null)
			{
				player.refs.simpleCollider = new GameObject("SimpleCollider").AddComponent<SphereCollider>();
				player.refs.simpleCollider.radius = player.data.simplifiedColliderRadius;
				player.refs.simpleCollider.transform.SetParent(base.transform.root);
				player.refs.simpleCollider.enabled = false;
			}
		}
	}

	public void FixedUpdateHandled()
	{
		if (player.Ragdoll())
		{
			if (!rigList[0].useGravity)
			{
				ToggleGravity(setGrav: true);
			}
			return;
		}
		if (rigList[0].useGravity)
		{
			ToggleGravity(setGrav: false);
		}
		if (player.data.simplifiedRagdoll)
		{
			using (simplifiedFollowAnimMarker.Auto())
			{
				FollowAnimSimplified();
			}
		}
		if (player.data.simplifiedRagdoll)
		{
			return;
		}
		player.data.groundBelow = player.data.isGrounded;
		using (groundRaycastMarker.Auto())
		{
			if (player.refs.controller.alwaysGroundRaycast || (player.data.sinceGrounded < 0.1f && player.data.sinceJump > 0.25f && player.refs.controller.groundRaycast && player.data.movementVector.magnitude > 0.1f))
			{
				GroundRaycast();
			}
			else
			{
				GroundRaycast(justChecking: true);
			}
		}
		using (handleCollisionsMarker.Auto())
		{
			HandleCollisions();
		}
		using (clearSavedCollisionsMarker.Auto())
		{
			ClearSavedCollisions();
		}
		using (followAnimMarker.Auto())
		{
			FollowAnim();
		}
		using (dragMarker.Auto())
		{
			Drag();
		}
	}

	private void ToggleGravity(bool setGrav)
	{
		for (int i = 0; i < rigList.Count; i++)
		{
			rigList[i].useGravity = setGrav;
		}
	}

	internal Bodypart GetBodypartFromID(int bodyPartID)
	{
		if (bodyPartID >= 0 && bodyPartID < bodypartList.Count)
		{
			return bodypartList[bodyPartID];
		}
		return null;
	}

	internal int GetBodypartIDFromCollider(Collider col)
	{
		Bodypart componentInParent = col.GetComponentInParent<Bodypart>();
		if ((bool)componentInParent)
		{
			return GetBodyPartID(componentInParent);
		}
		return -1;
	}

	internal int GetBodyPartID(Bodypart part)
	{
		for (int i = 0; i < bodypartList.Count; i++)
		{
			if (bodypartList[i] == part)
			{
				return i;
			}
		}
		return -1;
	}

	internal int GetBodyPartID(BodypartType part)
	{
		for (int i = 0; i < bodypartList.Count; i++)
		{
			if (bodypartList[i].bodypartType == part)
			{
				return i;
			}
		}
		return -1;
	}

	private void Drag()
	{
		for (int i = 0; i < rigList.Count; i++)
		{
			Rigidbody rigidbody = rigList[i];
			rigidbody.linearVelocity *= drag;
			rigidbody.angularVelocity *= angularDrag;
		}
	}

	private void FollowAnimSimplified()
	{
		Bodypart bodypart = bodypartDict[BodypartType.Hip];
		if ((bool)bodypart.animationTarget)
		{
			for (int i = 0; i < bodypartList.Count; i++)
			{
				bodypartList[i].FollowAnimSimple(bodypart);
			}
		}
	}

	private void FollowAnim()
	{
		Bodypart bodypart = bodypartDict[BodypartType.Hip];
		if ((bool)bodypart.animationTarget)
		{
			for (int i = 0; i < bodypartList.Count; i++)
			{
				bodypartList[i].FollowAnimJointDriveVelocity(bodypart.transform, bodypart.animationTarget.transform, force, addOpposingForce, torque);
			}
		}
	}

	internal Bodypart GetBodypart(BodypartType bodypartType)
	{
		if (bodypartDict.ContainsKey(bodypartType))
		{
			return bodypartDict[bodypartType];
		}
		return null;
	}

	public RaycastHit GroundRaycast(bool justChecking = false, bool justTryingToGetTheRaycastHit = false, bool ignoreAngle = false)
	{
		RaycastHit groundPosRaycast = HelperFunctions.GetGroundPosRaycast(bodypartDict[BodypartType.Head].transform.position, HelperFunctions.LayerType.TerrainProp, player.data.gravityDirection);
		if (!groundPosRaycast.transform)
		{
			return default(RaycastHit);
		}
		if ((bool)groundPosRaycast.rigidbody)
		{
			return default(RaycastHit);
		}
		float num = player.data.targetHeight + 0.25f;
		if (justChecking)
		{
			num = 2f;
		}
		if (groundPosRaycast.distance > num)
		{
			return default(RaycastHit);
		}
		if (!ignoreAngle && Vector3.Angle(Vector3.up, groundPosRaycast.normal) > 60f)
		{
			return default(RaycastHit);
		}
		if (justTryingToGetTheRaycastHit)
		{
			return groundPosRaycast;
		}
		if (justChecking)
		{
			player.data.groundBelow = true;
		}
		else
		{
			RegisterGroundRaycast(groundPosRaycast, bodypartDict[BodypartType.Hip]);
		}
		return groundPosRaycast;
	}

	internal void RegisterGroundRaycast(RaycastHit hit, Bodypart bodypart)
	{
		RegCollision(RaycastHitToBodyPartCollision(hit, bodypart));
	}

	internal BodyPartCollision RaycastHitToBodyPartCollision(RaycastHit hit, Bodypart bodypart)
	{
		BodyPartCollision bodyPartCollision = new BodyPartCollision();
		bodyPartCollision.ConfigFromRaycastHit(hit);
		bodyPartCollision.bodyPart = bodypart;
		return bodyPartCollision;
	}

	internal void BodyPartCollision(Collision collision, Bodypart bodypart)
	{
		if (bodypart.bodypartType == BodypartType.Item)
		{
			return;
		}
		BodyPartCollision bodyPartCollision = new BodyPartCollision();
		bodyPartCollision.ConfigFromCollision(collision);
		bodyPartCollision.bodyPart = bodypart;
		RegCollision(bodyPartCollision);
		collisionAction?.Invoke(collision, bodypart);
		bool flag = true;
		SurfaceCollisionEffect component = collision.collider.GetComponent<SurfaceCollisionEffect>();
		if ((bool)component)
		{
			component.CollideWithSurface(collision, bodypart);
			flag = !component.stopRagdollSounds;
		}
		if (!player.Ragdoll())
		{
			return;
		}
		float magnitude = collision.relativeVelocity.magnitude;
		if (magnitude > 5f && Time.time > ragdollSoundTime + 0.3f && flag)
		{
			ragdollSoundTime = Time.time;
			float t = Mathf.InverseLerp(5f, 20f, magnitude);
			for (int i = 0; i < ragdollSound.Length; i++)
			{
				ragdollSound[i].Play(bodypart.rig.position, local: false, Mathf.Lerp(0.3f, 1f, t));
			}
		}
	}

	private void RegCollision(BodyPartCollision col)
	{
		if (!(player.data.sinceJump < 0.2f) && (player.refs.controller.wallClimb || !(Vector3.Angle(-player.data.gravityDirection, col.normal) > 60f)))
		{
			collisionsToHandle.Add(col);
		}
	}

	private void ClearSavedCollisions()
	{
		player.data.nrOfGroundCols = collisionsToHandle.Count;
		collisionsToHandle.Clear();
	}

	private void HandleCollisions()
	{
		BodyPartCollision bodyPartCollision = null;
		float num = 10000f;
		for (int i = 0; i < collisionsToHandle.Count; i++)
		{
			if ((!collisionsToHandle[i].rigidbody || (bool)collisionsToHandle[i].rigidbody.GetComponent<StandableRig>()) && collisionsToHandle[i].bodyPart.bodypartType != BodypartType.Hand_L && collisionsToHandle[i].bodyPart.bodypartType != BodypartType.Hand_R && collisionsToHandle[i].bodyPart.bodypartType != BodypartType.Elbow_L && collisionsToHandle[i].bodyPart.bodypartType != BodypartType.Elbow_R && collisionsToHandle[i].bodyPart.bodypartType != BodypartType.Arm_L && collisionsToHandle[i].bodyPart.bodypartType != BodypartType.Arm_R && !(Vector3.Angle(collisionsToHandle[i].normal, Vector3.up) > 60f))
			{
				if (player.refs.controller.wallClimb)
				{
					player.data.gravityDirection = Vector3.RotateTowards(player.data.gravityDirection, -collisionsToHandle[i].normal, player.refs.controller.wallClimbGravityAdjustSpeed * Time.fixedDeltaTime, 0f);
				}
				if (Mathf.Abs(bodypartDict[BodypartType.Head].rig.position.y - collisionsToHandle[i].point.y) < num)
				{
					bodyPartCollision = collisionsToHandle[i];
				}
			}
		}
		if (bodyPartCollision != null)
		{
			HandleGroundCollision(bodyPartCollision);
			if (player.data.sinceGrounded > RagdollIfFellForLongerThan && player.IsLocal)
			{
				player.CallTakeDamageAndAddForceAndFall(0f, Vector3.zero, 2f);
			}
			player.data.isGrounded = true;
		}
		else
		{
			player.data.isGrounded = false;
		}
	}

	public void HandleGroundCollision(BodyPartCollision bestGroundCollision)
	{
		if (!(bestGroundCollision.collider == null))
		{
			float distanceToGround = Mathf.Abs(bodypartDict[BodypartType.Head].rig.position.y - bestGroundCollision.point.y);
			float groundAngle = Vector3.Angle(bestGroundCollision.normal, Vector3.up);
			player.data.distanceToGround = distanceToGround;
			player.data.groundAngle = groundAngle;
			player.data.groundPos = bestGroundCollision.point;
			player.data.groundNormal = bestGroundCollision.normal;
			player.data.groundTag = bestGroundCollision.collider.transform.tag;
			Debug.DrawLine(player.data.groundPos, player.data.groundPos + Vector3.up, Color.blue);
		}
	}

	internal void AddForce(Vector3 force, ForceMode forceMode)
	{
		for (int i = 0; i < rigList.Count; i++)
		{
			rigList[i].AddForce(force * bodypartList[i].movementForceMultiplier, forceMode);
		}
	}

	internal void AddForce(Vector3 force, ForceMode forceMode, Vector3 position, float radius)
	{
		for (int i = 0; i < rigList.Count; i++)
		{
			float value = Vector3.Distance(position, rigList[i].position);
			float num = Mathf.InverseLerp(radius, radius * 0.25f, value);
			rigList[i].AddForce(force * num, forceMode);
		}
	}

	internal void RemoveItem(ItemInstance item)
	{
		Bodypart component = item.GetComponent<Bodypart>();
		rigList.Remove(item.rig);
		bodypartList.Remove(component);
		bodypartDict.Remove(BodypartType.Item);
		BodyChanged();
	}

	internal void AddItem(ItemInstance item)
	{
		Bodypart bodypart = item.gameObject.AddComponent<Bodypart>();
		bodypart.Config(BodypartType.Item);
		bodypart.InitPart();
		rigList.Add(item.rig);
		bodypartList.Add(bodypart);
		bodypartDict.Add(BodypartType.Item, bodypart);
		BodyChanged();
	}

	internal void MoveAllRigsInDirection(Vector3 delta)
	{
		if (!isSplit)
		{
			Transform parent = GetBodypart(BodypartType.Hip).rig.transform.parent;
			isSplit = true;
			for (int i = 0; i < rigList.Count; i++)
			{
				rigList[i].transform.SetParent(parent, worldPositionStays: true);
			}
		}
		for (int j = 0; j < rigList.Count; j++)
		{
			if (rigList[j].isKinematic)
			{
				rigList[j].transform.position = rigList[j].transform.position + delta;
			}
			else
			{
				rigList[j].MovePosition(rigList[j].position + delta);
			}
		}
	}

	internal void MoveAllRigsTo(Vector3 targetPos)
	{
	}

	internal void Fall(float fallTime)
	{
		if (player.data.fallTime < fallTime)
		{
			player.data.fallTime = fallTime;
		}
	}

	internal void ExtraDrag(float extraDrag)
	{
		foreach (Rigidbody rig in rigList)
		{
			if (!rig.isKinematic)
			{
				rig.linearVelocity *= extraDrag;
				rig.angularVelocity *= extraDrag;
			}
		}
	}

	internal void TaseShock(float shockSeconds)
	{
		Fall(shockSeconds);
		player.data.tazeTime = shockSeconds;
		StartCoroutine(IWobble(shockSeconds));
		IEnumerator IWobble(float num)
		{
			while (num > 0f)
			{
				num -= Time.fixedDeltaTime;
				for (int i = 0; i < rigList.Count; i++)
				{
					rigList[i].AddTorque(UnityEngine.Random.onUnitSphere * 100f, ForceMode.Acceleration);
					rigList[i].AddForce(UnityEngine.Random.onUnitSphere * 30f, ForceMode.Acceleration);
				}
				yield return new WaitForFixedUpdate();
			}
		}
	}

	internal void CallFall(float seconds)
	{
		if (player.refs.view.IsMine)
		{
			player.refs.view.RPC("RPCA_Fall", RpcTarget.All, seconds);
		}
	}

	[PunRPC]
	internal void RPCA_Fall(float seconds)
	{
		Fall(seconds);
	}

	internal Bodypart GetBodypartFromCollider(Collider col)
	{
		return col.GetComponentInParent<Bodypart>();
	}

	internal void SetColliderLayer(int setLayer)
	{
		for (int i = 0; i < colliders.Length; i++)
		{
			colliders[i].gameObject.layer = setLayer;
		}
	}

	internal object GetRandomBodypartID()
	{
		return UnityEngine.Random.Range(0, bodypartList.Count);
	}
}
