using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using Zorro.Core;
using pworld.Scripts.Extensions;

namespace DefaultNamespace
{
	public class Bot_Slurper : MonoBehaviour, IThrowTarget
	{
		public float targetReelLevel = 0.5f;

		public float reelSpeed = 2f;

		public float currentReelLevel = 0.5f;

		public Vector2 minMaxIdleReelTarget = new Vector2(1f, 0.5f);

		public float cooldown = 1f;

		public Transform stickyBlob;

		public float sinceGrab;

		public float sinceGrabTimeMul = 0.5f;

		public float playerDrag = 0.95f;

		public float maxSinceGrounded = 2f;

		public float allRigMul = 0.5f;

		public float attachedLimbMul = 1f;

		public float pullForce = 80f;

		public Player playerAttached;

		public Rigidbody attachedLimb;

		public float maxLength = 12.5f;

		public float dealDamageInterval = 1f;

		public float damageAmount = 2f;

		public bool isSticky = true;

		private Bot bot_g;

		private Collider[] colliders;

		private Vector3 lineEndPoint;

		private LineRenderer lineRenderer_g;

		private float maxReelDistance;

		private float timeSinceDamage;

		private PhotonView view_g;

		public Material hiddenMaterial;

		public Material visibleMaterial;

		public MeshRenderer stickyBlobRenderer;

		public MeshRenderer handRenderer;

		public float itemChance = 1f;

		public SFX_Instance[] attatchSFX;

		public SFX_Instance[] detatchSFX;

		public GameObject sfxLoop;

		private bool t;

		public List<Item> excludeItemsFromSnatching = new List<Item>();

		private Vector3 targetBlobPos;

		private static float collideCount;

		public bool attackMonsters;

		private void Awake()
		{
			bot_g = GetComponent<Bot>();
			view_g = GetComponent<PhotonView>();
			lineRenderer_g = GetComponent<LineRenderer>();
			stickyBlobRenderer = stickyBlob.GetComponent<MeshRenderer>();
		}

		public void Start()
		{
			if (view_g.IsMine && (CanSpawn() || FindSpawnSpot()) && UnityEngine.Random.Range(0f, 1f) < itemChance && RoundArtifactSpawner.me != null)
			{
				Vector3 vector = base.transform.root.position + Vector3.down;
				if (Singleton<PickupHandler>.Instance == null || !PickupHandler.GetRandomPickup(out var pickup, excludeItemsFromSnatching))
				{
					pickup = PickupHandler.CreatePickup(RoundArtifactSpawner.GetRandomArtifactByRarity(RoundArtifactSpawner.me.possibleSpawns).id, new ItemInstanceData(Guid.NewGuid()), vector, UnityEngine.Random.rotation);
				}
				pickup.transform.position = vector;
			}
		}

		public void Update()
		{
			currentReelLevel = Mathf.Lerp(currentReelLevel, targetReelLevel, Time.deltaTime * reelSpeed);
			targetBlobPos = Vector3.Lerp(base.transform.position, lineEndPoint, currentReelLevel);
			if (!attachedLimb)
			{
				stickyBlob.transform.position = targetBlobPos;
				Hide();
			}
			else
			{
				Show();
			}
			sinceGrab += Time.deltaTime;
			if (!attachedLimb)
			{
				lineRenderer_g.SetPosition(0, base.transform.position);
				lineRenderer_g.SetPosition(1, targetBlobPos);
				if (isSticky && view_g.IsMine)
				{
					colliders = Physics.OverlapSphere(stickyBlob.position, stickyBlob.transform.lossyScale.x);
					Collider[] array = colliders;
					foreach (Collider collider in array)
					{
						if (!(collider == null))
						{
							Player componentInParent = collider.GetComponentInParent<Player>();
							if ((bool)componentInParent && !collider.GetComponentInParent<ItemInstance>())
							{
								int bodypartIDFromCollider = componentInParent.refs.ragdoll.GetBodypartIDFromCollider(collider);
								view_g.RPC("RPCA_AttachBlob", RpcTarget.All, componentInParent.refs.view.ViewID, bodypartIDFromCollider);
							}
						}
					}
				}
			}
			else
			{
				if (playerAttached.refs.view.IsMine)
				{
					timeSinceDamage += Time.deltaTime;
					if (timeSinceDamage > dealDamageInterval)
					{
						playerAttached.CallTakeDamage(damageAmount);
						timeSinceDamage = 0f;
					}
				}
				if (view_g.IsMine && (Vector3.Distance(attachedLimb.position, base.transform.position) > maxLength * 1.5f || playerAttached.data.sinceRescueDragged < 0.5f))
				{
					view_g.RPC("RPCA_ReleasePlayer", RpcTarget.All);
				}
				lineRenderer_g.SetPosition(0, base.transform.position);
				lineRenderer_g.SetPosition(1, attachedLimb.position);
			}
			if ((bool)playerAttached && !t)
			{
				sfxLoop.SetActive(value: true);
				if (!t)
				{
					for (int j = 0; j < attatchSFX.Length; j++)
					{
						attatchSFX[j].Play(stickyBlob.position);
					}
				}
				t = true;
			}
			if ((bool)playerAttached || !t)
			{
				return;
			}
			sfxLoop.SetActive(value: false);
			if (t)
			{
				for (int k = 0; k < detatchSFX.Length; k++)
				{
					detatchSFX[k].Play(stickyBlob.position);
				}
			}
			t = false;
		}

		public void FixedUpdate()
		{
			if ((bool)attachedLimb)
			{
				Vector3 vector = targetBlobPos - attachedLimb.position;
				vector = Vector3.ClampMagnitude(vector, 5f);
				vector *= pullForce;
				vector *= Mathf.Clamp01(sinceGrab * sinceGrabTimeMul);
				attachedLimb.AddForce(vector * attachedLimbMul, ForceMode.Acceleration);
				playerAttached.refs.ragdoll.AddForce(vector * allRigMul, ForceMode.Acceleration);
				playerAttached.data.sinceGrounded = Mathf.Clamp(playerAttached.data.sinceGrounded, 0f, maxSinceGrounded);
				stickyBlob.position = attachedLimb.position;
				stickyBlob.rotation.SetLookRotation((attachedLimb.position - base.transform.position).normalized, Vector3.up);
				if (sinceGrab > 0.25f)
				{
					playerAttached.SetPhysicsCamera(1f);
					playerAttached.refs.ragdoll.ExtraDrag(playerDrag);
					playerAttached.data.rotationOvveride = Quaternion.LookRotation(playerAttached.data.lookDirection.Flat(), Vector3.down);
					playerAttached.data.rotationOvverideStr = Mathf.Clamp01(sinceGrab);
				}
			}
		}

		private bool FindSpawnSpot()
		{
			List<PatrolPoint> pointsInGroups = Level.currentLevel.GetPointsInGroups(new List<PatrolPoint.PatrolGroup> { PatrolPoint.PatrolGroup.Bear });
			int num = 50;
			for (int i = 0; i < num; i++)
			{
				PatrolPoint rnd = pointsInGroups.GetRnd();
				base.transform.root.position = rnd.transform.position;
				if (CanSpawn())
				{
					return true;
				}
			}
			bot_g.Destroy();
			return false;
		}

		private bool CanSpawn()
		{
			RaycastHit raycastHit = HelperFunctions.LineCheck(base.transform.position, base.transform.position + Vector3.up * maxLength, HelperFunctions.LayerType.TerrainProp);
			if (raycastHit.collider == null)
			{
				return false;
			}
			base.transform.root.position = raycastHit.point;
			Physics.SyncTransforms();
			Collider[] array = Physics.OverlapSphere(base.transform.root.position, 2.25f);
			foreach (Collider collider in array)
			{
				if (collider.gameObject.layer != LayerMask.NameToLayer("Terrain"))
				{
					if (collider.gameObject.layer == LayerMask.NameToLayer("Prop"))
					{
						return false;
					}
					Bot_Slurper componentInChildren = collider.transform.root.GetComponentInChildren<Bot_Slurper>();
					if (componentInChildren != null && componentInChildren != this)
					{
						VerboseDebug.Log("Already a slurper at spawn. Finding new spot");
						collideCount += 1f;
						return false;
					}
				}
			}
			RaycastHit raycastHit2 = HelperFunctions.LineCheck(base.transform.position, base.transform.position + Vector3.down * maxLength, HelperFunctions.LayerType.TerrainProp);
			if (raycastHit2.collider == null)
			{
				return false;
			}
			targetReelLevel = GetNewReelTarget();
			lineEndPoint = raycastHit2.point;
			view_g.RPC("RPCA_SyncStart", RpcTarget.AllBuffered, base.transform.root.position, lineEndPoint, targetReelLevel, UnityEngine.Random.Range(0f, 360f), UnityEngine.Random.Range(0, 1000) < 3);
			return true;
		}

		private float GetNewReelTarget()
		{
			return Mathf.Clamp01(minMaxIdleReelTarget.PRndRange());
		}

		[PunRPC]
		private void RPCA_SyncStart(Vector3 pos, Vector3 lineEndPosition, float targetReel, float rotation, bool attackMonsters)
		{
			base.transform.root.position = pos;
			Vector3 localEulerAngles = base.transform.root.localEulerAngles;
			localEulerAngles.y = rotation;
			base.transform.root.localEulerAngles = localEulerAngles;
			lineEndPoint = lineEndPosition;
			targetReelLevel = targetReel;
			this.attackMonsters = attackMonsters;
			Hide();
		}

		public void Hide()
		{
			lineRenderer_g.sharedMaterial = hiddenMaterial;
			stickyBlobRenderer.sharedMaterial = hiddenMaterial;
			handRenderer.sharedMaterial = hiddenMaterial;
		}

		public void Show()
		{
			lineRenderer_g.sharedMaterial = visibleMaterial;
			stickyBlobRenderer.sharedMaterial = visibleMaterial;
			handRenderer.sharedMaterial = visibleMaterial;
		}

		[PunRPC]
		public void RPCA_ReleasePlayer()
		{
			if ((bool)playerAttached)
			{
				playerAttached.data.isHangingUpsideDown = false;
			}
			playerAttached = null;
			attachedLimb = null;
			targetReelLevel = 0.2f;
			StartCoroutine(EnableAfterTime(cooldown));
			Show();
			IEnumerator EnableAfterTime(float time)
			{
				isSticky = false;
				float elapsed = 0f;
				while (elapsed < time)
				{
					elapsed += Time.deltaTime;
					yield return null;
				}
				targetReelLevel = GetNewReelTarget();
				isSticky = true;
				Hide();
			}
		}

		[PunRPC]
		public void RPCA_AttachBlob(int viewID, int bodyPartID)
		{
			sinceGrab = 0f;
			Show();
			Player player = PlayerHandler.instance.TryGetPlayerFromViewID(viewID);
			if (!(player == null) && (!player.ai || attackMonsters))
			{
				Bodypart bodypartFromID = player.refs.ragdoll.GetBodypartFromID(bodyPartID);
				if (bodypartFromID == null)
				{
					Debug.Log("Tried to slurp null bodypart");
				}
				timeSinceDamage = 1f;
				attachedLimb = bodypartFromID.rig;
				playerAttached = player;
				playerAttached.data.isHangingUpsideDown = true;
				targetReelLevel = 0.2f;
			}
		}

		private void ReleasePlayer()
		{
			HitByThrowable(null);
		}

		public void HitByThrowable(ItemInstance item)
		{
			if (view_g.IsMine)
			{
				view_g.RPC("RPCA_ReleasePlayer", RpcTarget.All);
			}
		}
	}
}
