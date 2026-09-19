using System;
using System.Collections;
using System.Runtime.InteropServices;
using Mirror;
using Mirror.RemoteCalls;
using UnityEngine;

public class AnimalEatController : NetworkBehaviour
{
	[Header("Girdi")]
	public KeyCode eatKey = KeyCode.Mouse0;

	[Tooltip("Eat tuşu basılı tutulduktan sonra gerçekten Eat state'e (animasyon + sfx) girmeden önce beklenecek süre — anlık dokunuşla kase önünden geçerken yemiş sayılmayı engeller.")]
	public float eatStartDelay = 1f;

	[Header("Yemek Yeme Sesi")]
	public AudioSource eatSfxSource;

	public AudioClip eatSfx;

	public float minPitch = 0.95f;

	public float maxPitch = 1.05f;

	[Tooltip("Eat animasyonu 2x hızda oynatıldığı için 2.5sn (giriş+yeme+çıkış) — ses tam yeme anında çalsın diye ilk ses bu kadar gecikmeli başlar")]
	public float eatSfxStartDelay = 0.5f;

	[Tooltip("Eating devam ettiği sürece ses bu aralıkla tekrarlanır")]
	public float eatSfxRepeatInterval = 1.5f;

	[Header("Referanslar")]
	public PlayerRoleData roleData;

	private static readonly int _hashEat;

	[SyncVar(hook = "OnEatingChanged")]
	private bool _syncedEating;

	private PlayerModelController _modelController;

	private bool _wantEatingLocal;

	private float _eatHoldTimer;

	private Coroutine _eatSfxRoutine;

	private Health _health;

	public Action<bool, bool> _Mirror_SyncVarHookDelegate__syncedEating;

	public bool IsEating => _syncedEating;

	private Animator _anim
	{
		get
		{
			if (!(_modelController != null))
			{
				return null;
			}
			return _modelController.ActiveAnimator;
		}
	}

	public bool Network_syncedEating
	{
		get
		{
			return _syncedEating;
		}
		[param: In]
		set
		{
			GeneratedSyncVarSetter(value, ref _syncedEating, 1uL, _Mirror_SyncVarHookDelegate__syncedEating);
		}
	}

	private void Awake()
	{
		if (roleData == null)
		{
			roleData = GetComponent<PlayerRoleData>();
		}
		_modelController = GetComponent<PlayerModelController>();
		_health = GetComponent<Health>();
		if (eatSfxSource == null)
		{
			eatSfxSource = base.gameObject.AddComponent<AudioSource>();
		}
		eatSfxSource.playOnAwake = false;
		eatSfxSource.spatialBlend = 1f;
		eatSfxSource.maxDistance = 25f;
		eatSfxSource.rolloffMode = AudioRolloffMode.Linear;
	}

	private void Update()
	{
		if (!base.isLocalPlayer || roleData == null || roleData.Role != PlayerRole.Animal)
		{
			return;
		}
		if (_health != null && _health.IsDead)
		{
			_eatHoldTimer = 0f;
			if (_wantEatingLocal)
			{
				_wantEatingLocal = false;
				CmdSetEating(eating: false);
			}
		}
		else if (CursorManager.Instance != null && CursorManager.Instance.AnyUIOpen)
		{
			_eatHoldTimer = 0f;
			if (_wantEatingLocal)
			{
				_wantEatingLocal = false;
				CmdSetEating(eating: false);
			}
		}
		else if (!Input.GetKey(eatKey))
		{
			_eatHoldTimer = 0f;
			if (_wantEatingLocal)
			{
				_wantEatingLocal = false;
				CmdSetEating(eating: false);
			}
		}
		else if (!_wantEatingLocal)
		{
			_eatHoldTimer += Time.deltaTime;
			if (_eatHoldTimer >= eatStartDelay)
			{
				_wantEatingLocal = true;
				CmdSetEating(eating: true);
			}
		}
	}

	[Command]
	private void CmdSetEating(bool eating)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		writer.WriteBool(eating);
		SendCommandInternal("System.Void AnimalEatController::CmdSetEating(System.Boolean)", -79848546, writer, 0);
		NetworkWriterPool.Return(writer);
	}

	private void OnEatingChanged(bool _, bool eating)
	{
		if (_anim != null)
		{
			_anim.SetBool(_hashEat, eating);
		}
		if (_eatSfxRoutine != null)
		{
			StopCoroutine(_eatSfxRoutine);
			_eatSfxRoutine = null;
		}
		if (eating)
		{
			_eatSfxRoutine = StartCoroutine(EatSfxLoop());
		}
	}

	private IEnumerator EatSfxLoop()
	{
		yield return new WaitForSeconds(eatSfxStartDelay);
		while (true)
		{
			if (eatSfxSource != null && eatSfx != null)
			{
				eatSfxSource.pitch = UnityEngine.Random.Range(minPitch, maxPitch);
				eatSfxSource.PlayOneShot(eatSfx);
			}
			yield return new WaitForSeconds(eatSfxRepeatInterval);
		}
	}

	[Server]
	public void ServerForceStopEating()
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void AnimalEatController::ServerForceStopEating()' called when server was not active");
		}
		else
		{
			Network_syncedEating = false;
		}
	}

	public AnimalEatController()
	{
		_Mirror_SyncVarHookDelegate__syncedEating = OnEatingChanged;
	}

	static AnimalEatController()
	{
		_hashEat = Animator.StringToHash("Eat_b");
		RemoteProcedureCalls.RegisterCommand(typeof(AnimalEatController), "System.Void AnimalEatController::CmdSetEating(System.Boolean)", InvokeUserCode_CmdSetEating__Boolean, requiresAuthority: true);
	}

	public override bool Weaved()
	{
		return true;
	}

	protected void UserCode_CmdSetEating__Boolean(bool eating)
	{
		if (!eating || !(_health != null) || !_health.IsDead)
		{
			Network_syncedEating = eating;
		}
	}

	protected static void InvokeUserCode_CmdSetEating__Boolean(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdSetEating called on client.");
		}
		else
		{
			((AnimalEatController)obj).UserCode_CmdSetEating__Boolean(reader.ReadBool());
		}
	}

	public override void SerializeSyncVars(NetworkWriter writer, bool forceAll)
	{
		base.SerializeSyncVars(writer, forceAll);
		if (forceAll)
		{
			writer.WriteBool(_syncedEating);
			return;
		}
		writer.WriteVarULong(syncVarDirtyBits);
		if ((syncVarDirtyBits & 1L) != 0L)
		{
			writer.WriteBool(_syncedEating);
		}
	}

	public override void DeserializeSyncVars(NetworkReader reader, bool initialState)
	{
		base.DeserializeSyncVars(reader, initialState);
		if (initialState)
		{
			GeneratedSyncVarDeserialize(ref _syncedEating, _Mirror_SyncVarHookDelegate__syncedEating, reader.ReadBool());
			return;
		}
		long num = (long)reader.ReadVarULong();
		if ((num & 1L) != 0L)
		{
			GeneratedSyncVarDeserialize(ref _syncedEating, _Mirror_SyncVarHookDelegate__syncedEating, reader.ReadBool());
		}
	}
}
