using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Mirror;
using UnityEngine;

public class BarnIntroController : NetworkBehaviour
{
	public enum DoorState
	{
		Closed = 0,
		Opening = 1,
		Open = 2,
		Closing = 3
	}

	[CompilerGenerated]
	private sealed class _003CCloseDoor_003Ed__36 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public BarnIntroController _003C_003E4__this;

		object IEnumerator<object>.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		[DebuggerHidden]
		public _003CCloseDoor_003Ed__36(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
		}

		private bool MoveNext()
		{
			int num = _003C_003E1__state;
			BarnIntroController barnIntroController = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				barnIntroController.Network_doorState = DoorState.Closing;
				_003C_003E2__current = new WaitForSeconds(barnIntroController.doorAnimDuration);
				_003C_003E1__state = 1;
				return true;
			case 1:
				_003C_003E1__state = -1;
				if (barnIntroController._pendingQueue.Count > 0)
				{
					barnIntroController._doorRoutine = barnIntroController.StartCoroutine(barnIntroController.OpenThenProcessQueue());
					return false;
				}
				barnIntroController.Network_doorState = DoorState.Closed;
				barnIntroController._doorRoutine = null;
				return false;
			}
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}
	}

	[CompilerGenerated]
	private sealed class _003COpenThenProcessQueue_003Ed__31 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public BarnIntroController _003C_003E4__this;

		object IEnumerator<object>.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		[DebuggerHidden]
		public _003COpenThenProcessQueue_003Ed__31(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
		}

		private bool MoveNext()
		{
			int num = _003C_003E1__state;
			BarnIntroController barnIntroController = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				barnIntroController.Network_doorState = DoorState.Opening;
				_003C_003E2__current = new WaitForSeconds(barnIntroController.doorAnimDuration);
				_003C_003E1__state = 1;
				return true;
			case 1:
				_003C_003E1__state = -1;
				barnIntroController.Network_doorState = DoorState.Open;
				barnIntroController.ProcessQueue();
				barnIntroController._doorRoutine = null;
				return false;
			}
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}
	}

	[CompilerGenerated]
	private sealed class _003CServerReconfirmTeleport_003Ed__30 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public NetworkConnectionToClient conn;

		public NetworkTransformBase nt;

		public Vector3 pos;

		public Quaternion rot;

		object IEnumerator<object>.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		[DebuggerHidden]
		public _003CServerReconfirmTeleport_003Ed__30(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
		}

		private bool MoveNext()
		{
			switch (_003C_003E1__state)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				_003C_003E2__current = new WaitForSeconds(0.35f);
				_003C_003E1__state = 1;
				return true;
			case 1:
				_003C_003E1__state = -1;
				if (conn == null || conn.identity == null || nt == null)
				{
					return false;
				}
				nt.ServerTeleport(pos, rot);
				return false;
			}
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}
	}

	[CompilerGenerated]
	private sealed class _003CWalkerSafetyTimeout_003Ed__34 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public BarnIntroController _003C_003E4__this;

		public NetworkConnectionToClient conn;

		object IEnumerator<object>.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		[DebuggerHidden]
		public _003CWalkerSafetyTimeout_003Ed__34(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
		}

		private bool MoveNext()
		{
			int num = _003C_003E1__state;
			BarnIntroController barnIntroController = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				_003C_003E2__current = new WaitForSeconds(barnIntroController.walkTimeoutSafety);
				_003C_003E1__state = 1;
				return true;
			case 1:
				_003C_003E1__state = -1;
				barnIntroController.ServerCompleteWalker(conn);
				return false;
			}
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}
	}

	[Header("Referanslar")]
	public Animator barnAnimator;

	[Header("Zamanlama (saniye)")]
	[Tooltip("Barn_Opening / Barn_Closing klip uzunluğuyla eşleşmeli")]
	public float doorAnimDuration = 0.5f;

	public float walkOutDuration = 1.5f;

	[Tooltip("Client CmdIntroComplete göndermezse (bağlantı kopması vb.) kapı yine de bu süre sonunda kapanır — LobbyIntroController'ın toplam süresinden (yürüyüş + animasyon yumuşatma + kamera fade'leri) uzun tutulmalı")]
	public float walkTimeoutSafety = 6f;

	private static readonly int OpeningHash = Animator.StringToHash("Barn_Opening");

	private static readonly int ClosingHash = Animator.StringToHash("Barn_Closing");

	private static readonly int IdleHash = Animator.StringToHash("Barn_Idle");

	[SyncVar(hook = "OnDoorStateChanged")]
	private DoorState _doorState;

	public static readonly Vector3 BarnInsidePosition = new Vector3(30.42f, 0.17385048f, -51.97f);

	public static readonly Quaternion BarnInsideRotation = Quaternion.Euler(0f, 299.27844f, 0f);

	public static readonly Vector3 BarnOutsidePosition = new Vector3(25.73923f, 0.18161213f, -49.432236f);

	public static readonly Quaternion BarnOutsideRotation = BarnInsideRotation;

	private readonly Queue<NetworkConnectionToClient> _pendingQueue = new Queue<NetworkConnectionToClient>();

	private readonly HashSet<NetworkConnectionToClient> _activeWalkers = new HashSet<NetworkConnectionToClient>();

	private Coroutine _doorRoutine;

	private AudioSource audioSource;

	public AudioClip OpeningSFX;

	public AudioClip ClosingSFX;

	public CanvasGroup NameTagCanvasGroup;

	public Action<DoorState, DoorState> _Mirror_SyncVarHookDelegate__doorState;

	public static BarnIntroController Instance { get; private set; }

	public DoorState Network_doorState
	{
		get
		{
			return _doorState;
		}
		[param: In]
		set
		{
			GeneratedSyncVarSetter(value, ref _doorState, 1uL, _Mirror_SyncVarHookDelegate__doorState);
		}
	}

	private void Awake()
	{
		Instance = this;
		if (barnAnimator == null)
		{
			barnAnimator = GetComponent<Animator>();
		}
		audioSource = GetComponent<AudioSource>();
		NameTagCanvasGroup.alpha = 0f;
	}

	private void OnDestroy()
	{
		if (Instance == this)
		{
			Instance = null;
		}
	}

	private void OnDoorStateChanged(DoorState _, DoorState state)
	{
		ApplyDoorVisual(state);
	}

	private void ApplyDoorVisual(DoorState state)
	{
		if (!(barnAnimator == null))
		{
			switch (state)
			{
			case DoorState.Closed:
				barnAnimator.speed = 1f;
				barnAnimator.Play(IdleHash, 0, 0f);
				NameTagCanvasGroup.alpha = 1f;
				break;
			case DoorState.Opening:
				barnAnimator.speed = 1f;
				barnAnimator.Play(OpeningHash, 0, 0f);
				audioSource.PlayOneShot(OpeningSFX);
				NameTagCanvasGroup.alpha = 0f;
				break;
			case DoorState.Open:
				barnAnimator.Play(OpeningHash, 0, 1f);
				barnAnimator.speed = 0f;
				break;
			case DoorState.Closing:
				barnAnimator.speed = 1f;
				barnAnimator.Play(ClosingHash, 0, 0f);
				audioSource.PlayOneShot(ClosingSFX);
				break;
			}
		}
	}

	[Server]
	public void RequestIntro(NetworkConnectionToClient conn)
	{
		if (!NetworkServer.active)
		{
			UnityEngine.Debug.LogWarning("[Server] function 'System.Void BarnIntroController::RequestIntro(Mirror.NetworkConnectionToClient)' called when server was not active");
		}
		else
		{
			if (conn == null || conn.identity == null)
			{
				return;
			}
			TeleportToInside(conn);
			conn.identity.GetComponent<LobbyIntroController>()?.TargetActivateCinematic(conn);
			_pendingQueue.Enqueue(conn);
			if (_doorState == DoorState.Closed || _doorState == DoorState.Closing)
			{
				if (_doorRoutine != null)
				{
					StopCoroutine(_doorRoutine);
				}
				_doorRoutine = StartCoroutine(OpenThenProcessQueue());
			}
			else if (_doorState == DoorState.Open)
			{
				ProcessQueue();
			}
		}
	}

	[Server]
	private void TeleportToInside(NetworkConnectionToClient conn)
	{
		if (!NetworkServer.active)
		{
			UnityEngine.Debug.LogWarning("[Server] function 'System.Void BarnIntroController::TeleportToInside(Mirror.NetworkConnectionToClient)' called when server was not active");
			return;
		}
		NetworkTransformBase component = conn.identity.GetComponent<NetworkTransformBase>();
		if (component != null)
		{
			component.ServerTeleport(BarnInsidePosition, BarnInsideRotation);
			StartCoroutine(ServerReconfirmTeleport(conn, component, BarnInsidePosition, BarnInsideRotation));
		}
		else
		{
			conn.identity.transform.SetPositionAndRotation(BarnInsidePosition, BarnInsideRotation);
		}
		conn.identity.GetComponent<PlayerController>()?.ServerResetVelocity();
	}

	[IteratorStateMachine(typeof(_003CServerReconfirmTeleport_003Ed__30))]
	[Server]
	private IEnumerator ServerReconfirmTeleport(NetworkConnectionToClient conn, NetworkTransformBase nt, Vector3 pos, Quaternion rot)
	{
		if (!NetworkServer.active)
		{
			UnityEngine.Debug.LogWarning("[Server] function 'System.Collections.IEnumerator BarnIntroController::ServerReconfirmTeleport(Mirror.NetworkConnectionToClient,Mirror.NetworkTransformBase,UnityEngine.Vector3,UnityEngine.Quaternion)' called when server was not active");
			return null;
		}
		return new _003CServerReconfirmTeleport_003Ed__30(0)
		{
			conn = conn,
			nt = nt,
			pos = pos,
			rot = rot
		};
	}

	[IteratorStateMachine(typeof(_003COpenThenProcessQueue_003Ed__31))]
	[Server]
	private IEnumerator OpenThenProcessQueue()
	{
		if (!NetworkServer.active)
		{
			UnityEngine.Debug.LogWarning("[Server] function 'System.Collections.IEnumerator BarnIntroController::OpenThenProcessQueue()' called when server was not active");
			return null;
		}
		return new _003COpenThenProcessQueue_003Ed__31(0)
		{
			_003C_003E4__this = this
		};
	}

	[Server]
	private void ProcessQueue()
	{
		if (!NetworkServer.active)
		{
			UnityEngine.Debug.LogWarning("[Server] function 'System.Void BarnIntroController::ProcessQueue()' called when server was not active");
			return;
		}
		while (_pendingQueue.Count > 0)
		{
			NetworkConnectionToClient networkConnectionToClient = _pendingQueue.Dequeue();
			if (networkConnectionToClient != null && !(networkConnectionToClient.identity == null))
			{
				_activeWalkers.Add(networkConnectionToClient);
				LobbyIntroController component = networkConnectionToClient.identity.GetComponent<LobbyIntroController>();
				if (component != null)
				{
					component.TargetBeginWalk(networkConnectionToClient, GetExitPosition(), BarnOutsideRotation, walkOutDuration);
				}
				StartCoroutine(WalkerSafetyTimeout(networkConnectionToClient));
			}
		}
	}

	[Server]
	private Vector3 GetExitPosition()
	{
		if (!NetworkServer.active)
		{
			UnityEngine.Debug.LogWarning("[Server] function 'UnityEngine.Vector3 BarnIntroController::GetExitPosition()' called when server was not active");
			return default(Vector3);
		}
		return BarnOutsidePosition;
	}

	[IteratorStateMachine(typeof(_003CWalkerSafetyTimeout_003Ed__34))]
	[Server]
	private IEnumerator WalkerSafetyTimeout(NetworkConnectionToClient conn)
	{
		if (!NetworkServer.active)
		{
			UnityEngine.Debug.LogWarning("[Server] function 'System.Collections.IEnumerator BarnIntroController::WalkerSafetyTimeout(Mirror.NetworkConnectionToClient)' called when server was not active");
			return null;
		}
		return new _003CWalkerSafetyTimeout_003Ed__34(0)
		{
			_003C_003E4__this = this,
			conn = conn
		};
	}

	[Server]
	public void ServerCompleteWalker(NetworkConnectionToClient conn)
	{
		if (!NetworkServer.active)
		{
			UnityEngine.Debug.LogWarning("[Server] function 'System.Void BarnIntroController::ServerCompleteWalker(Mirror.NetworkConnectionToClient)' called when server was not active");
		}
		else
		{
			if (!_activeWalkers.Remove(conn))
			{
				return;
			}
			if (conn != null && conn.identity != null)
			{
				NetworkTransformBase component = conn.identity.GetComponent<NetworkTransformBase>();
				if (component != null)
				{
					component.ServerTeleport(BarnOutsidePosition, BarnOutsideRotation);
				}
			}
			if (_activeWalkers.Count == 0 && _pendingQueue.Count == 0 && _doorState == DoorState.Open)
			{
				if (_doorRoutine != null)
				{
					StopCoroutine(_doorRoutine);
				}
				_doorRoutine = StartCoroutine(CloseDoor());
			}
		}
	}

	[IteratorStateMachine(typeof(_003CCloseDoor_003Ed__36))]
	[Server]
	private IEnumerator CloseDoor()
	{
		if (!NetworkServer.active)
		{
			UnityEngine.Debug.LogWarning("[Server] function 'System.Collections.IEnumerator BarnIntroController::CloseDoor()' called when server was not active");
			return null;
		}
		return new _003CCloseDoor_003Ed__36(0)
		{
			_003C_003E4__this = this
		};
	}

	public BarnIntroController()
	{
		_Mirror_SyncVarHookDelegate__doorState = OnDoorStateChanged;
	}

	public override bool Weaved()
	{
		return true;
	}

	public override void SerializeSyncVars(NetworkWriter writer, bool forceAll)
	{
		base.SerializeSyncVars(writer, forceAll);
		if (forceAll)
		{
			GeneratedNetworkCode._Write_BarnIntroController_002FDoorState(writer, _doorState);
			return;
		}
		writer.WriteVarULong(syncVarDirtyBits);
		if ((syncVarDirtyBits & 1L) != 0L)
		{
			GeneratedNetworkCode._Write_BarnIntroController_002FDoorState(writer, _doorState);
		}
	}

	public override void DeserializeSyncVars(NetworkReader reader, bool initialState)
	{
		base.DeserializeSyncVars(reader, initialState);
		if (initialState)
		{
			GeneratedSyncVarDeserialize(ref _doorState, _Mirror_SyncVarHookDelegate__doorState, GeneratedNetworkCode._Read_BarnIntroController_002FDoorState(reader));
			return;
		}
		long num = (long)reader.ReadVarULong();
		if ((num & 1L) != 0L)
		{
			GeneratedSyncVarDeserialize(ref _doorState, _Mirror_SyncVarHookDelegate__doorState, GeneratedNetworkCode._Read_BarnIntroController_002FDoorState(reader));
		}
	}
}
