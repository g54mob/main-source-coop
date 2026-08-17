using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Mirror.RemoteCalls;
using UnityEngine;

namespace Mirror.Examples.TopDownShooter
{
	public class PlayerTopDown : NetworkBehaviour
	{
		public static readonly List<PlayerTopDown> playerList;

		private Camera mainCamera;

		private CameraTopDown cameraTopDown;

		private CanvasTopDown canvasTopDown;

		public float moveSpeed = 5f;

		public CharacterController characterController;

		public GameObject leftFoot;

		public GameObject rightFoot;

		private Vector3 previousPosition;

		private Quaternion previousRotation;

		[SyncVar(hook = "OnFlashLightChanged")]
		public bool flashLightStatus = true;

		public Light flashLight;

		[SyncVar(hook = "OnKillsChanged")]
		public int kills;

		[SyncVar(hook = "OnPlayerStatusChanged")]
		public int playerStatus;

		public GameObject[] objectsToHideOnDeath;

		public float shootDistance = 100f;

		public LayerMask hitLayers;

		public GameObject muzzleFlash;

		public AudioSource soundGunShot;

		public AudioSource soundDeath;

		public AudioSource soundFlashLight;

		public AudioSource soundLeftFoot;

		public AudioSource soundRightFoot;

		public Action<bool, bool> _Mirror_SyncVarHookDelegate_flashLightStatus;

		public Action<int, int> _Mirror_SyncVarHookDelegate_kills;

		public Action<int, int> _Mirror_SyncVarHookDelegate_playerStatus;

		public bool NetworkflashLightStatus
		{
			get
			{
				return flashLightStatus;
			}
			[param: In]
			set
			{
				GeneratedSyncVarSetter(value, ref flashLightStatus, 1uL, _Mirror_SyncVarHookDelegate_flashLightStatus);
			}
		}

		public int Networkkills
		{
			get
			{
				return kills;
			}
			[param: In]
			set
			{
				GeneratedSyncVarSetter(value, ref kills, 2uL, _Mirror_SyncVarHookDelegate_kills);
			}
		}

		public int NetworkplayerStatus
		{
			get
			{
				return playerStatus;
			}
			[param: In]
			set
			{
				GeneratedSyncVarSetter(value, ref playerStatus, 4uL, _Mirror_SyncVarHookDelegate_playerStatus);
			}
		}

		public override void OnStartLocalPlayer()
		{
			mainCamera = Camera.main;
			cameraTopDown = mainCamera.GetComponent<CameraTopDown>();
			cameraTopDown.playerTransform = base.transform;
			cameraTopDown.offset.y = 20f;
			canvasTopDown.playerTopDown = this;
			mainCamera.GetComponent<AudioListener>().enabled = false;
			base.gameObject.AddComponent<AudioListener>();
		}

		private void Awake()
		{
			canvasTopDown = UnityEngine.Object.FindAnyObjectByType<CanvasTopDown>();
		}

		public void Start()
		{
			playerList.Add(this);
			MonoBehaviour.print("Player joined, total players: " + playerList.Count);
			if (base.isClient)
			{
				InvokeRepeating("AnimatePlayer", 0.2f, 0.2f);
			}
		}

		public void OnDestroy()
		{
			playerList.Remove(this);
			MonoBehaviour.print("Player removed, total players: " + playerList.Count);
			if ((bool)mainCamera)
			{
				mainCamera.GetComponent<AudioListener>().enabled = true;
			}
		}

		[ClientCallback]
		private void Update()
		{
			if (NetworkClient.active && Application.isFocused && base.isOwned && playerStatus == 0)
			{
				float axis = Input.GetAxis("Horizontal");
				float axis2 = Input.GetAxis("Vertical");
				Vector3 vector = new Vector3(axis, 0f, axis2);
				if (vector.magnitude > 1f)
				{
					vector.Normalize();
				}
				characterController.Move(vector * moveSpeed * Time.deltaTime);
				RotatePlayerToMouse();
				if (Input.GetKeyUp(KeyCode.F))
				{
					CmdFlashLight();
				}
				if (Input.GetMouseButtonDown(0))
				{
					Shoot();
				}
			}
		}

		[ClientCallback]
		private void RotatePlayerToMouse()
		{
			if (NetworkClient.active)
			{
				Plane plane = new Plane(Vector3.up, base.transform.position);
				Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
				if (plane.Raycast(ray, out var enter))
				{
					Quaternion b = Quaternion.LookRotation(ray.GetPoint(enter) - base.transform.position);
					base.transform.rotation = Quaternion.Slerp(base.transform.rotation, b, moveSpeed * Time.deltaTime);
				}
			}
		}

		[ClientCallback]
		private void Shoot()
		{
			if (NetworkClient.active && Physics.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition), out var hitInfo, shootDistance, hitLayers))
			{
				canvasTopDown.shotMarker.transform.position = hitInfo.point;
				if (hitInfo.collider.gameObject.GetComponent<NetworkIdentity>() != null)
				{
					CmdShoot(hitInfo.collider.gameObject);
				}
				else
				{
					CmdShoot(null);
				}
			}
		}

		private IEnumerator GunShotEffect()
		{
			soundGunShot.Play();
			muzzleFlash.SetActive(value: true);
			if (base.isLocalPlayer)
			{
				canvasTopDown.shotMarker.SetActive(value: true);
			}
			yield return new WaitForSeconds(0.1f);
			muzzleFlash.SetActive(value: false);
			if (base.isLocalPlayer)
			{
				canvasTopDown.shotMarker.SetActive(value: false);
			}
		}

		[Command]
		public void CmdFlashLight()
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			SendCommandInternal("System.Void Mirror.Examples.TopDownShooter.PlayerTopDown::CmdFlashLight()", 1582609334, writer, 0);
			NetworkWriterPool.Return(writer);
		}

		private void OnFlashLightChanged(bool _Old, bool _New)
		{
			Debug.Log($"OnFlashLightChanged: {_New}");
			flashLight.enabled = _New;
			soundFlashLight.Play();
		}

		[Command]
		public void CmdShoot(GameObject target)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			writer.WriteGameObject(target);
			SendCommandInternal("System.Void Mirror.Examples.TopDownShooter.PlayerTopDown::CmdShoot(UnityEngine.GameObject)", 101798243, writer, 0);
			NetworkWriterPool.Return(writer);
		}

		[ClientRpc]
		private void RpcShoot()
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			SendRPCInternal("System.Void Mirror.Examples.TopDownShooter.PlayerTopDown::RpcShoot()", 616059940, writer, 0, includeOwner: true);
			NetworkWriterPool.Return(writer);
		}

		private void OnKillsChanged(int _Old, int _New)
		{
			if (base.isLocalPlayer)
			{
				canvasTopDown.UpdateKillsUI(kills);
			}
		}

		[ClientCallback]
		private void AnimatePlayer()
		{
			if (!NetworkClient.active)
			{
				return;
			}
			if (base.transform.position == previousPosition && Quaternion.Angle(base.transform.rotation, previousRotation) < 20f)
			{
				rightFoot.SetActive(value: false);
				leftFoot.SetActive(value: false);
				return;
			}
			if (rightFoot.activeInHierarchy)
			{
				leftFoot.SetActive(value: true);
				rightFoot.SetActive(value: false);
				soundLeftFoot.Play();
			}
			else
			{
				leftFoot.SetActive(value: false);
				rightFoot.SetActive(value: true);
				soundRightFoot.Play();
			}
			previousPosition = base.transform.position;
			previousRotation = base.transform.rotation;
		}

		[Command]
		public void CmdRespawnPlayer()
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			SendCommandInternal("System.Void Mirror.Examples.TopDownShooter.PlayerTopDown::CmdRespawnPlayer()", 782582827, writer, 0);
			NetworkWriterPool.Return(writer);
		}

		private void OnPlayerStatusChanged(int _Old, int _New)
		{
			if (playerStatus == 0)
			{
				GameObject[] array = objectsToHideOnDeath;
				for (int i = 0; i < array.Length; i++)
				{
					array[i].SetActive(value: true);
				}
				characterController.enabled = true;
				if (base.isLocalPlayer)
				{
					base.transform.position = NetworkManager.startPositions[UnityEngine.Random.Range(0, NetworkManager.startPositions.Count)].position;
					canvasTopDown.buttonRespawnPlayer.gameObject.SetActive(value: false);
				}
			}
			else if (playerStatus == 1)
			{
				GameObject[] array = objectsToHideOnDeath;
				for (int i = 0; i < array.Length; i++)
				{
					array[i].SetActive(value: false);
				}
				characterController.enabled = false;
				if (base.isLocalPlayer)
				{
					canvasTopDown.buttonRespawnPlayer.gameObject.SetActive(value: true);
				}
			}
		}

		[ServerCallback]
		public void Kill()
		{
			if (NetworkServer.active)
			{
				NetworkplayerStatus = 1;
				RpcKill();
			}
		}

		[ClientRpc]
		private void RpcKill()
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			SendRPCInternal("System.Void Mirror.Examples.TopDownShooter.PlayerTopDown::RpcKill()", 451527875, writer, 0, includeOwner: true);
			NetworkWriterPool.Return(writer);
		}

		public PlayerTopDown()
		{
			_Mirror_SyncVarHookDelegate_flashLightStatus = OnFlashLightChanged;
			_Mirror_SyncVarHookDelegate_kills = OnKillsChanged;
			_Mirror_SyncVarHookDelegate_playerStatus = OnPlayerStatusChanged;
		}

		static PlayerTopDown()
		{
			playerList = new List<PlayerTopDown>();
			RemoteProcedureCalls.RegisterCommand(typeof(PlayerTopDown), "System.Void Mirror.Examples.TopDownShooter.PlayerTopDown::CmdFlashLight()", InvokeUserCode_CmdFlashLight, requiresAuthority: true);
			RemoteProcedureCalls.RegisterCommand(typeof(PlayerTopDown), "System.Void Mirror.Examples.TopDownShooter.PlayerTopDown::CmdShoot(UnityEngine.GameObject)", InvokeUserCode_CmdShoot__GameObject, requiresAuthority: true);
			RemoteProcedureCalls.RegisterCommand(typeof(PlayerTopDown), "System.Void Mirror.Examples.TopDownShooter.PlayerTopDown::CmdRespawnPlayer()", InvokeUserCode_CmdRespawnPlayer, requiresAuthority: true);
			RemoteProcedureCalls.RegisterRpc(typeof(PlayerTopDown), "System.Void Mirror.Examples.TopDownShooter.PlayerTopDown::RpcShoot()", InvokeUserCode_RpcShoot);
			RemoteProcedureCalls.RegisterRpc(typeof(PlayerTopDown), "System.Void Mirror.Examples.TopDownShooter.PlayerTopDown::RpcKill()", InvokeUserCode_RpcKill);
		}

		public override bool Weaved()
		{
			return true;
		}

		protected void UserCode_CmdFlashLight()
		{
			NetworkflashLightStatus = !flashLightStatus;
		}

		protected static void InvokeUserCode_CmdFlashLight(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdFlashLight called on client.");
			}
			else
			{
				((PlayerTopDown)obj).UserCode_CmdFlashLight();
			}
		}

		protected void UserCode_CmdShoot__GameObject(GameObject target)
		{
			RpcShoot();
			if (!target)
			{
				return;
			}
			if (target.name.Contains("Enemy"))
			{
				target.GetComponent<EnemyTopDown>().Kill();
			}
			else if (CompareTag("Player"))
			{
				if (target.GetComponent<PlayerTopDown>().playerStatus != 0 || target == base.gameObject)
				{
					return;
				}
				target.GetComponent<PlayerTopDown>().Kill();
			}
			Networkkills = kills + 1;
		}

		protected static void InvokeUserCode_CmdShoot__GameObject(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdShoot called on client.");
			}
			else
			{
				((PlayerTopDown)obj).UserCode_CmdShoot__GameObject(reader.ReadGameObject());
			}
		}

		protected void UserCode_RpcShoot()
		{
			StartCoroutine(GunShotEffect());
		}

		protected static void InvokeUserCode_RpcShoot(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkClient.active)
			{
				Debug.LogError("RPC RpcShoot called on server.");
			}
			else
			{
				((PlayerTopDown)obj).UserCode_RpcShoot();
			}
		}

		protected void UserCode_CmdRespawnPlayer()
		{
			if (playerStatus == 0)
			{
				NetworkplayerStatus = 1;
			}
			else
			{
				NetworkplayerStatus = 0;
			}
		}

		protected static void InvokeUserCode_CmdRespawnPlayer(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdRespawnPlayer called on client.");
			}
			else
			{
				((PlayerTopDown)obj).UserCode_CmdRespawnPlayer();
			}
		}

		protected void UserCode_RpcKill()
		{
			soundDeath.Play();
			UnityEngine.Object.Destroy(UnityEngine.Object.Instantiate(canvasTopDown.deathSplatter, base.transform.position, base.transform.rotation), 5f);
		}

		protected static void InvokeUserCode_RpcKill(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkClient.active)
			{
				Debug.LogError("RPC RpcKill called on server.");
			}
			else
			{
				((PlayerTopDown)obj).UserCode_RpcKill();
			}
		}

		public override void SerializeSyncVars(NetworkWriter writer, bool forceAll)
		{
			base.SerializeSyncVars(writer, forceAll);
			if (forceAll)
			{
				writer.WriteBool(flashLightStatus);
				writer.WriteVarInt(kills);
				writer.WriteVarInt(playerStatus);
				return;
			}
			writer.WriteVarULong(syncVarDirtyBits);
			if ((syncVarDirtyBits & 1L) != 0L)
			{
				writer.WriteBool(flashLightStatus);
			}
			if ((syncVarDirtyBits & 2L) != 0L)
			{
				writer.WriteVarInt(kills);
			}
			if ((syncVarDirtyBits & 4L) != 0L)
			{
				writer.WriteVarInt(playerStatus);
			}
		}

		public override void DeserializeSyncVars(NetworkReader reader, bool initialState)
		{
			base.DeserializeSyncVars(reader, initialState);
			if (initialState)
			{
				GeneratedSyncVarDeserialize(ref flashLightStatus, _Mirror_SyncVarHookDelegate_flashLightStatus, reader.ReadBool());
				GeneratedSyncVarDeserialize(ref kills, _Mirror_SyncVarHookDelegate_kills, reader.ReadVarInt());
				GeneratedSyncVarDeserialize(ref playerStatus, _Mirror_SyncVarHookDelegate_playerStatus, reader.ReadVarInt());
				return;
			}
			long num = (long)reader.ReadVarULong();
			if ((num & 1L) != 0L)
			{
				GeneratedSyncVarDeserialize(ref flashLightStatus, _Mirror_SyncVarHookDelegate_flashLightStatus, reader.ReadBool());
			}
			if ((num & 2L) != 0L)
			{
				GeneratedSyncVarDeserialize(ref kills, _Mirror_SyncVarHookDelegate_kills, reader.ReadVarInt());
			}
			if ((num & 4L) != 0L)
			{
				GeneratedSyncVarDeserialize(ref playerStatus, _Mirror_SyncVarHookDelegate_playerStatus, reader.ReadVarInt());
			}
		}
	}
}
