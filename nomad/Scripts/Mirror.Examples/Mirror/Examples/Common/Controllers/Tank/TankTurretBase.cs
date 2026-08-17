using System;
using System.Runtime.InteropServices;
using Mirror.RemoteCalls;
using UnityEngine;

namespace Mirror.Examples.Common.Controllers.Tank
{
	[AddComponentMenu("")]
	[RequireComponent(typeof(NetworkIdentity))]
	[DisallowMultipleComponent]
	public class TankTurretBase : NetworkBehaviour
	{
		[Serializable]
		public struct OptionsKeys
		{
			public KeyCode MouseLock;

			public KeyCode AutoLevel;

			public KeyCode ToggleUI;
		}

		[Serializable]
		public struct MoveKeys
		{
			public KeyCode PitchUp;

			public KeyCode PitchDown;

			public KeyCode TurnLeft;

			public KeyCode TurnRight;
		}

		[Serializable]
		public struct OtherKeys
		{
			public KeyCode Shoot;
		}

		[Flags]
		public enum ControlOptions : byte
		{
			None = 0,
			MouseLock = 1,
			AutoLevel = 2,
			ShowUI = 4
		}

		[Serializable]
		public struct RuntimeData
		{
			[ReadOnly]
			[SerializeField]
			[Range(-300f, 300f)]
			private float _turretSpeed;

			[ReadOnly]
			[SerializeField]
			[Range(-180f, 180f)]
			private float _pitchAngle;

			[ReadOnly]
			[SerializeField]
			[Range(-180f, 180f)]
			private float _pitchSpeed;

			[ReadOnly]
			[SerializeField]
			[Range(-1f, 1f)]
			private float _mouseInputX;

			[ReadOnly]
			[SerializeField]
			[Range(0f, 30f)]
			private float _mouseSensitivity;

			[ReadOnly]
			[SerializeField]
			private double _lastShotTime;

			[ReadOnly]
			[SerializeField]
			private GameObject _turretUI;

			public float mouseInputX
			{
				get
				{
					return _mouseInputX;
				}
				internal set
				{
					_mouseInputX = value;
				}
			}

			public float mouseSensitivity
			{
				get
				{
					return _mouseSensitivity;
				}
				internal set
				{
					_mouseSensitivity = value;
				}
			}

			public float turretSpeed
			{
				get
				{
					return _turretSpeed;
				}
				internal set
				{
					_turretSpeed = value;
				}
			}

			public float pitchAngle
			{
				get
				{
					return _pitchAngle;
				}
				internal set
				{
					_pitchAngle = value;
				}
			}

			public float pitchSpeed
			{
				get
				{
					return _pitchSpeed;
				}
				internal set
				{
					_pitchSpeed = value;
				}
			}

			public double lastShotTime
			{
				get
				{
					return _lastShotTime;
				}
				internal set
				{
					_lastShotTime = value;
				}
			}

			public GameObject turretUI
			{
				get
				{
					return _turretUI;
				}
				internal set
				{
					_turretUI = value;
				}
			}
		}

		private const float BASE_DPI = 96f;

		private Material cachedMaterial;

		[Header("Prefabs")]
		public GameObject turretUIPrefab;

		public GameObject projectilePrefab;

		[Header("Components")]
		public Animator animator;

		public Transform turret;

		public Transform barrel;

		public Transform projectileMount;

		public CapsuleCollider barrelCollider;

		[Header("Seated Player")]
		public GameObject playerObject;

		[SyncVar(hook = "OnPlayerColorChanged")]
		public Color32 playerColor = Color.black;

		[Header("Configuration")]
		[SerializeField]
		public MoveKeys moveKeys = new MoveKeys
		{
			PitchUp = KeyCode.UpArrow,
			PitchDown = KeyCode.DownArrow,
			TurnLeft = KeyCode.LeftArrow,
			TurnRight = KeyCode.RightArrow
		};

		[SerializeField]
		public OtherKeys otherKeys = new OtherKeys
		{
			Shoot = KeyCode.Space
		};

		[SerializeField]
		public OptionsKeys optionsKeys = new OptionsKeys
		{
			MouseLock = KeyCode.M,
			AutoLevel = KeyCode.L,
			ToggleUI = KeyCode.U
		};

		[Space(5f)]
		public ControlOptions controlOptions = ControlOptions.AutoLevel | ControlOptions.ShowUI;

		[Header("Shooting")]
		[Tooltip("Cooldown time in seconds")]
		[Range(0f, 10f)]
		public byte cooldownTime = 1;

		[Header("Turret")]
		[Range(0f, 300f)]
		[Tooltip("Max Rotation in degrees per second")]
		public float maxTurretSpeed = 250f;

		[Range(0f, 30f)]
		[Tooltip("Rotation acceleration in degrees per second squared")]
		public float turretAcceleration = 10f;

		[Header("Barrel")]
		[Range(0f, 180f)]
		[Tooltip("Max Pitch in degrees per second")]
		public float maxPitchSpeed = 30f;

		[Range(0f, 40f)]
		[Tooltip("Max Pitch in degrees")]
		public float maxPitchUpAngle = 25f;

		[Range(0f, 20f)]
		[Tooltip("Max Pitch in degrees")]
		public float maxPitchDownAngle;

		[Range(0f, 10f)]
		[Tooltip("Pitch acceleration in degrees per second squared")]
		public float pitchAcceleration = 3f;

		[Header("Diagnostics")]
		public RuntimeData runtimeData;

		public Action<Color32, Color32> _Mirror_SyncVarHookDelegate_playerColor;

		private bool CanShoot => NetworkTime.time >= runtimeData.lastShotTime + (double)(int)cooldownTime;

		public Color32 NetworkplayerColor
		{
			get
			{
				return playerColor;
			}
			[param: In]
			set
			{
				GeneratedSyncVarSetter(value, ref playerColor, 1uL, _Mirror_SyncVarHookDelegate_playerColor);
			}
		}

		protected override void OnValidate()
		{
			if (!Application.isPlaying)
			{
				base.OnValidate();
				Reset();
			}
		}

		protected virtual void Reset()
		{
			syncDirection = SyncDirection.ClientToServer;
			if (animator == null)
			{
				animator = GetComponentInChildren<Animator>();
			}
			runtimeData.mouseSensitivity = turretAcceleration;
			if (turret == null)
			{
				turret = FindDeepChild(base.transform, "Turret");
			}
			if (barrel == null)
			{
				barrel = FindDeepChild(turret, "Barrel");
			}
			if (barrelCollider == null)
			{
				barrelCollider = barrel.GetComponent<CapsuleCollider>();
			}
			if (projectileMount == null)
			{
				projectileMount = FindDeepChild(turret, "ProjectileMount");
			}
			if (playerObject == null)
			{
				playerObject = FindDeepChild(turret, "SeatedPlayer").gameObject;
			}
			base.enabled = false;
			static Transform FindDeepChild(Transform aParent, string aName)
			{
				Transform transform = aParent.Find(aName);
				if (transform != null)
				{
					return transform;
				}
				foreach (Transform item in aParent)
				{
					transform = FindDeepChild(item, aName);
					if (transform != null)
					{
						return transform;
					}
				}
				return null;
			}
		}

		public override void OnStartLocalPlayer()
		{
			if (turretUIPrefab != null)
			{
				runtimeData.turretUI = UnityEngine.Object.Instantiate(turretUIPrefab);
			}
			if (runtimeData.turretUI != null)
			{
				if (runtimeData.turretUI.TryGetComponent<TurretUI>(out var component))
				{
					component.Refresh(moveKeys, optionsKeys);
				}
				runtimeData.turretUI.SetActive(controlOptions.HasFlag(ControlOptions.ShowUI));
			}
		}

		public override void OnStopLocalPlayer()
		{
			if (runtimeData.turretUI != null)
			{
				UnityEngine.Object.Destroy(runtimeData.turretUI);
			}
			runtimeData.turretUI = null;
		}

		public override void OnStartAuthority()
		{
			float num = ((Screen.dpi > 0f) ? (Screen.dpi / 96f) : 1f);
			runtimeData.mouseSensitivity = turretAcceleration * num;
			SetCursor(controlOptions.HasFlag(ControlOptions.MouseLock));
			base.enabled = true;
		}

		public override void OnStopAuthority()
		{
			SetCursor(locked: false);
			base.enabled = false;
		}

		private void Update()
		{
			float deltaTime = Time.deltaTime;
			HandleOptions();
			HandlePitch(deltaTime);
			if (controlOptions.HasFlag(ControlOptions.MouseLock))
			{
				HandleMouseTurret(deltaTime);
			}
			else
			{
				HandleTurning(deltaTime);
			}
			HandleShooting();
		}

		private void OnPlayerColorChanged(Color32 _, Color32 newColor)
		{
			if (cachedMaterial == null)
			{
				cachedMaterial = playerObject.GetComponent<Renderer>().material;
			}
			cachedMaterial.color = newColor;
			playerObject.SetActive(newColor != Color.black);
		}

		private void SetCursor(bool locked)
		{
			Cursor.lockState = (locked ? CursorLockMode.Locked : CursorLockMode.None);
			Cursor.visible = !locked;
		}

		private void HandleOptions()
		{
			if (optionsKeys.MouseLock != KeyCode.None && Input.GetKeyUp(optionsKeys.MouseLock))
			{
				controlOptions ^= ControlOptions.MouseLock;
				SetCursor(controlOptions.HasFlag(ControlOptions.MouseLock));
			}
			if (optionsKeys.AutoLevel != KeyCode.None && Input.GetKeyUp(optionsKeys.AutoLevel))
			{
				controlOptions ^= ControlOptions.AutoLevel;
			}
			if (optionsKeys.ToggleUI != KeyCode.None && Input.GetKeyUp(optionsKeys.ToggleUI))
			{
				controlOptions ^= ControlOptions.ShowUI;
				if (runtimeData.turretUI != null)
				{
					runtimeData.turretUI.SetActive(controlOptions.HasFlag(ControlOptions.ShowUI));
				}
			}
		}

		private void HandleTurning(float deltaTime)
		{
			float num = 0f;
			if (moveKeys.TurnLeft != KeyCode.None && Input.GetKey(moveKeys.TurnLeft))
			{
				num -= maxTurretSpeed;
			}
			if (moveKeys.TurnRight != KeyCode.None && Input.GetKey(moveKeys.TurnRight))
			{
				num += maxTurretSpeed;
			}
			runtimeData.turretSpeed = Mathf.MoveTowards(runtimeData.turretSpeed, num, turretAcceleration * maxTurretSpeed * deltaTime);
			turret.Rotate(0f, runtimeData.turretSpeed * deltaTime, 0f);
		}

		private void HandleMouseTurret(float deltaTime)
		{
			runtimeData.mouseInputX += Input.GetAxisRaw("Mouse X") * runtimeData.mouseSensitivity;
			runtimeData.mouseInputX = Mathf.Clamp(runtimeData.mouseInputX, -1f, 1f);
			float target = runtimeData.mouseInputX * maxTurretSpeed;
			runtimeData.turretSpeed = Mathf.MoveTowards(runtimeData.turretSpeed, target, runtimeData.mouseSensitivity * maxTurretSpeed * deltaTime);
			turret.Rotate(0f, runtimeData.turretSpeed * deltaTime, 0f);
			runtimeData.mouseInputX = Mathf.MoveTowards(runtimeData.mouseInputX, 0f, runtimeData.mouseSensitivity * deltaTime);
		}

		private void HandlePitch(float deltaTime)
		{
			float num = 0f;
			bool flag = false;
			if (moveKeys.PitchUp != KeyCode.None && Input.GetKey(moveKeys.PitchUp))
			{
				num -= maxPitchSpeed;
				flag = true;
			}
			if (moveKeys.PitchDown != KeyCode.None && Input.GetKey(moveKeys.PitchDown))
			{
				num += maxPitchSpeed;
				flag = true;
			}
			runtimeData.pitchSpeed = Mathf.MoveTowards(runtimeData.pitchSpeed, num, pitchAcceleration * maxPitchSpeed * deltaTime);
			runtimeData.pitchAngle += runtimeData.pitchSpeed * deltaTime;
			runtimeData.pitchAngle = Mathf.Clamp(runtimeData.pitchAngle, 0f - maxPitchUpAngle, maxPitchDownAngle);
			if (!flag && controlOptions.HasFlag(ControlOptions.AutoLevel))
			{
				runtimeData.pitchAngle = Mathf.MoveTowards(runtimeData.pitchAngle, 0f, maxPitchSpeed * deltaTime);
			}
			barrel.localRotation = Quaternion.Euler(-90f + runtimeData.pitchAngle, 0f, 180f);
		}

		private void HandleShooting()
		{
			if (CanShoot && otherKeys.Shoot != KeyCode.None && Input.GetKeyUp(otherKeys.Shoot))
			{
				CmdShoot();
				if (!base.isServer)
				{
					DoShoot();
				}
			}
		}

		[Command]
		private void CmdShoot()
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			SendCommandInternal("System.Void Mirror.Examples.Common.Controllers.Tank.TankTurretBase::CmdShoot()", 949663763, writer, 0);
			NetworkWriterPool.Return(writer);
		}

		[ClientRpc(includeOwner = false)]
		private void RpcShoot()
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			SendRPCInternal("System.Void Mirror.Examples.Common.Controllers.Tank.TankTurretBase::RpcShoot()", 219688798, writer, 0, includeOwner: false);
			NetworkWriterPool.Return(writer);
		}

		private void DoShoot()
		{
			Physics.IgnoreCollision(UnityEngine.Object.Instantiate(projectilePrefab, projectileMount.position, projectileMount.rotation).GetComponent<Collider>(), barrelCollider);
			runtimeData.lastShotTime = NetworkTime.time;
		}

		public TankTurretBase()
		{
			_Mirror_SyncVarHookDelegate_playerColor = OnPlayerColorChanged;
		}

		public override bool Weaved()
		{
			return true;
		}

		protected void UserCode_CmdShoot()
		{
			if (CanShoot)
			{
				RpcShoot();
				DoShoot();
			}
		}

		protected static void InvokeUserCode_CmdShoot(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdShoot called on client.");
			}
			else
			{
				((TankTurretBase)obj).UserCode_CmdShoot();
			}
		}

		protected void UserCode_RpcShoot()
		{
			if (!base.isServer)
			{
				DoShoot();
			}
		}

		protected static void InvokeUserCode_RpcShoot(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkClient.active)
			{
				Debug.LogError("RPC RpcShoot called on server.");
			}
			else
			{
				((TankTurretBase)obj).UserCode_RpcShoot();
			}
		}

		static TankTurretBase()
		{
			RemoteProcedureCalls.RegisterCommand(typeof(TankTurretBase), "System.Void Mirror.Examples.Common.Controllers.Tank.TankTurretBase::CmdShoot()", InvokeUserCode_CmdShoot, requiresAuthority: true);
			RemoteProcedureCalls.RegisterRpc(typeof(TankTurretBase), "System.Void Mirror.Examples.Common.Controllers.Tank.TankTurretBase::RpcShoot()", InvokeUserCode_RpcShoot);
		}

		public override void SerializeSyncVars(NetworkWriter writer, bool forceAll)
		{
			base.SerializeSyncVars(writer, forceAll);
			if (forceAll)
			{
				writer.WriteColor32(playerColor);
				return;
			}
			writer.WriteVarULong(syncVarDirtyBits);
			if ((syncVarDirtyBits & 1L) != 0L)
			{
				writer.WriteColor32(playerColor);
			}
		}

		public override void DeserializeSyncVars(NetworkReader reader, bool initialState)
		{
			base.DeserializeSyncVars(reader, initialState);
			if (initialState)
			{
				GeneratedSyncVarDeserialize(ref playerColor, _Mirror_SyncVarHookDelegate_playerColor, reader.ReadColor32());
				return;
			}
			long num = (long)reader.ReadVarULong();
			if ((num & 1L) != 0L)
			{
				GeneratedSyncVarDeserialize(ref playerColor, _Mirror_SyncVarHookDelegate_playerColor, reader.ReadColor32());
			}
		}
	}
}
