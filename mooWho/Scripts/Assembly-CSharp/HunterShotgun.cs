using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Mirror;
using Mirror.RemoteCalls;
using UnityEngine;

public class HunterShotgun : NetworkBehaviour
{
	[CompilerGenerated]
	private sealed class _003CServerRefillLoop_003Ed__63 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public HunterShotgun _003C_003E4__this;

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
		public _003CServerRefillLoop_003Ed__63(int _003C_003E1__state)
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
			HunterShotgun hunterShotgun = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				break;
			case 1:
				_003C_003E1__state = -1;
				if (!hunterShotgun._inShack)
				{
					return false;
				}
				if (hunterShotgun._serverAmmo < hunterShotgun.MaxAmmo)
				{
					hunterShotgun._serverAmmo = Mathf.Min(hunterShotgun._serverAmmo + 1, hunterShotgun.MaxAmmo);
					hunterShotgun.TargetSyncAmmo(hunterShotgun.connectionToClient, hunterShotgun._serverAmmo);
					hunterShotgun.RpcPlayReload();
				}
				break;
			}
			if (hunterShotgun._inShack)
			{
				_003C_003E2__current = new WaitForSeconds(hunterShotgun.shackRefillInterval);
				_003C_003E1__state = 1;
				return true;
			}
			hunterShotgun._refillRoutine = null;
			return false;
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

	[Header("Ateş Ayarları")]
	[Tooltip("Maksimum mermi hakkı fallback default'u — GameManager.Instance mevcutsa Lobby Game Settings'teki ConfiguredHunterAmmo kullanılır (EffectiveMaxAmmo)")]
	public int maxAmmo = 4;

	[Tooltip("Atışlar arası bekleme (sn)")]
	public float fireDelay = 1.6f;

	[Tooltip("Pompalının maksimum etkili menzili — bunun ötesindeki hiçbir şeye isabet edilemez (çok uzağa sıkıp vuramama sorununu önler)")]
	public float range = 20f;

	public KeyCode fireKey = KeyCode.Mouse0;

	[Header("Aim")]
	[Tooltip("Aim (nişan) tuşu — sağ tık")]
	public KeyCode aimKey = KeyCode.Mouse1;

	[Tooltip("Koşarken aim alınabilir mi (false = sadece dur/yürü)")]
	public bool canAimWhileRunning;

	[Header("Hedefleme")]
	[Tooltip("Vurulabilir katmanlar (SADECE hayvan/oyuncu — duvar/zemin OLMASIN)")]
	public LayerMask hitMask;

	[Tooltip("İSTEK: düz bir Raycast yerine bu yarıçapta bir SphereCast kullanılır — ağaç gibi kalın collider'ların tam merkez hizasında olmayan (yakınında/arkasında duran) bir hedefi vurmayı piksel-hassasiyeti gerektirmeden kolaylaştıran küçük bir tolerans payı. Çok büyütme (0.1-0.2 civarı önerilir) — hedefe gerçekten yakın olmayan atışları da isabet saydırmasın.")]
	public float hitSphereRadius = 0.15f;

	[Tooltip("İSTEK: SphereCast yoluna hedeften ÖNCE bir engel (duvar, ağaç vb.) çıkarsa, engelin isabet mesafesi hedefin mesafesinden bu değerden DAHA FAZLA erkense atış gerçek bir duvar/engel sayılıp REDDEDİLİR. Engel hedefe bu payın İÇİNDE kadar yakınsa (ör. hedefin hemen yanındaki/arkasındaki bir ağaç gövdesi) atış yine de İSABET sayılır — 'duvar arkası vuramasın ama ağaç yanındaki/arkasındaki hedefe yakına sıkarsa saysın' kuralı.")]
	public float obstructionTolerance = 1.5f;

	[Tooltip("Yanlış (bot) hayvan vurulunca oyun süresinden düşülecek ceza (sn) — GameManager.Instance mevcutsa Lobby Game Settings'teki ConfiguredHunterPenaltySeconds (EffectiveWrongShotTimePenalty) kullanılır, bu sadece fallback default")]
	public float wrongShotTimePenalty = 30f;

	[Header("Ses Efektleri")]
	public AudioSource audioSource;

	public AudioClip fireSfx;

	public AudioClip emptyClickSfx;

	[Tooltip("Ammo Shack'te her mermi doldukça çalınır")]
	public AudioClip reloadSfx;

	[Tooltip("Sağ tık ile nişan alınca (silah çekilince) çalınır — SADECE atıcının kendi ekranında/kulağında (local), diğer oyuncular duymaz")]
	public AudioClip aimSfx;

	public float minPitch = 0.97f;

	public float maxPitch = 1.03f;

	[Tooltip("Avcı YANLIŞ hedefi (bot) vurunca çalınır — SADECE atışı yapan avcının kendi ekranında/kulağında (local), diğer oyuncular duymaz. Kendi clip/pitch/volume ayarları AudioSource'un kendi Inspector'ında yapılandırılır (PlayOneShot değil, doğrudan Play() çağrılır).")]
	public AudioSource WrongAnimalShootSource;

	[Header("Muzzle Flash")]
	public Transform muzzlePoint;

	public ParticleSystem muzzleFlash;

	[Header("Impact Efekti")]
	[Tooltip("Merminin gittiği yerde (isabet noktasında, ıskalarsa menzil sonunda) spawnlanır — TÜM client'larda görünür")]
	public GameObject impactParticlePrefab;

	[Tooltip("Spawn olduktan kaç saniye sonra yok edilir")]
	public float impactParticleLifetime = 4f;

	[Header("Animator")]
	[Tooltip("Nişan bool'u — true: silah yukarı (aim), false: silah aşağı")]
	public string aimBool = "Aim";

	[Header("Referanslar")]
	public PlayerRoleData roleData;

	public NetworkedCameraController cam;

	[SyncVar]
	private int _killCount;

	private int _ignoreRaycastLayer;

	private int _ammo;

	private float _lastFireTime = -999f;

	private bool _isAiming;

	private bool _lastAimState;

	private int _serverAmmo;

	private float _serverLastFire = -999f;

	private int _cleanKillStreak;

	private bool _hadBadShot;

	private PlayerModelController _modelController;

	private PlayerController _pc;

	private static int _hashAim;

	[Header("Kulübe (Ammo Shack) Mermi Doldurma")]
	[Tooltip("Kulübe içindeyken her bu kadar saniyede 1 mermi doldurulur — İSTEK: 2.5 kat hızlandırıldı (2.5sn → 1sn)")]
	public float shackRefillInterval = 1f;

	private bool _inShack;

	private Coroutine _refillRoutine;

	public float EffectiveWrongShotTimePenalty
	{
		get
		{
			if (!(GameManager.Instance != null))
			{
				return wrongShotTimePenalty;
			}
			return GameManager.Instance.ConfiguredHunterPenaltySeconds;
		}
	}

	public int KillCount => _killCount;

	public bool HadBadShotThisRound => _hadBadShot;

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

	public int Ammo => _ammo;

	public int MaxAmmo
	{
		get
		{
			if (!(GameManager.Instance != null))
			{
				return maxAmmo;
			}
			return GameManager.Instance.ConfiguredHunterAmmo;
		}
	}

	public bool IsAiming => _isAiming;

	public int Network_killCount
	{
		get
		{
			return _killCount;
		}
		[param: In]
		set
		{
			GeneratedSyncVarSetter(value, ref _killCount, 1uL, null);
		}
	}

	private void Awake()
	{
		if (roleData == null)
		{
			roleData = GetComponent<PlayerRoleData>();
		}
		if (cam == null)
		{
			cam = GetComponent<NetworkedCameraController>();
		}
		_pc = GetComponent<PlayerController>();
		_modelController = GetComponent<PlayerModelController>();
		_hashAim = Animator.StringToHash(aimBool);
		_ignoreRaycastLayer = LayerMask.NameToLayer("Ignore Raycast");
		_ammo = MaxAmmo;
		_serverAmmo = MaxAmmo;
		if (audioSource == null)
		{
			audioSource = GetComponent<AudioSource>();
		}
		if (audioSource == null)
		{
			audioSource = base.gameObject.AddComponent<AudioSource>();
		}
		audioSource.playOnAwake = false;
		audioSource.spatialBlend = 1f;
		audioSource.maxDistance = 40f;
		audioSource.rolloffMode = AudioRolloffMode.Linear;
	}

	private void Update()
	{
		if (base.isLocalPlayer && !(roleData == null) && roleData.Role == PlayerRole.Hunter)
		{
			UpdateAiming();
			if (_isAiming && Input.GetKeyDown(fireKey))
			{
				TryFire();
			}
		}
	}

	private void UpdateAiming()
	{
		bool isAiming = Input.GetKey(aimKey);
		if (_pc != null && _pc.IsRunning && !canAimWhileRunning)
		{
			isAiming = false;
		}
		_isAiming = isAiming;
		if (_lastAimState != _isAiming)
		{
			_lastAimState = _isAiming;
			if (_anim != null)
			{
				_anim.SetBool(_hashAim, _isAiming);
			}
			if (_isAiming)
			{
				PlayAimSfx();
			}
		}
		if (cam != null)
		{
			cam.SetAiming(_isAiming);
		}
	}

	private void PlayAimSfx()
	{
		if (!(aimSfx == null) && !(audioSource == null))
		{
			audioSource.pitch = UnityEngine.Random.Range(minPitch, maxPitch);
			audioSource.PlayOneShot(aimSfx, 0.1f);
		}
	}

	private void TryFire()
	{
		if (_isAiming)
		{
			if (GameManager.Instance == null || !GameManager.Instance.IsHunterDoorOpen)
			{
				PlayEmptyClick();
			}
			else if (_ammo <= 0)
			{
				PlayEmptyClick();
			}
			else if (!(Time.time - _lastFireTime < fireDelay))
			{
				Transform transform = ((cam != null) ? cam.CameraTransform : null);
				Vector3 origin = ((transform != null) ? transform.position : (base.transform.position + Vector3.up * 1.6f));
				Vector3 direction = ((transform != null) ? transform.forward : base.transform.forward);
				_lastFireTime = Time.time;
				AchievementManager.Instance?.IncrementStatAndUnlock("Stat.ShotsFired", 500, "ACH_TRIGGER_HAPPY");
				CmdFire(origin, direction);
			}
		}
	}

	[Server]
	public void ServerResetForNewRound()
	{
		if (!NetworkServer.active)
		{
			UnityEngine.Debug.LogWarning("[Server] function 'System.Void HunterShotgun::ServerResetForNewRound()' called when server was not active");
			return;
		}
		_serverAmmo = MaxAmmo;
		_serverLastFire = -999f;
		Network_killCount = 0;
		_cleanKillStreak = 0;
		_hadBadShot = false;
		ServerSetInShack(inShack: false);
		TargetResetAmmo(base.connectionToClient);
	}

	[TargetRpc]
	private void TargetResetAmmo(NetworkConnectionToClient target)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		SendTargetRPCInternal(target, "System.Void HunterShotgun::TargetResetAmmo(Mirror.NetworkConnectionToClient)", 1670060642, writer, 0);
		NetworkWriterPool.Return(writer);
	}

	[Server]
	public void ServerSetInShack(bool inShack)
	{
		if (!NetworkServer.active)
		{
			UnityEngine.Debug.LogWarning("[Server] function 'System.Void HunterShotgun::ServerSetInShack(System.Boolean)' called when server was not active");
		}
		else if (_inShack != inShack)
		{
			_inShack = inShack;
			if (_refillRoutine != null)
			{
				StopCoroutine(_refillRoutine);
				_refillRoutine = null;
			}
			if (_inShack)
			{
				_refillRoutine = StartCoroutine(ServerRefillLoop());
			}
		}
	}

	[IteratorStateMachine(typeof(_003CServerRefillLoop_003Ed__63))]
	[Server]
	private IEnumerator ServerRefillLoop()
	{
		if (!NetworkServer.active)
		{
			UnityEngine.Debug.LogWarning("[Server] function 'System.Collections.IEnumerator HunterShotgun::ServerRefillLoop()' called when server was not active");
			return null;
		}
		return new _003CServerRefillLoop_003Ed__63(0)
		{
			_003C_003E4__this = this
		};
	}

	[ClientRpc]
	private void RpcPlayReload()
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		SendRPCInternal("System.Void HunterShotgun::RpcPlayReload()", -1458279173, writer, 0, includeOwner: true);
		NetworkWriterPool.Return(writer);
	}

	public void ResetAimState()
	{
		_isAiming = false;
		_lastAimState = false;
		if (_anim != null)
		{
			_anim.SetBool(aimBool, value: false);
		}
		if (base.isLocalPlayer)
		{
			if (cam != null)
			{
				cam.SetAiming(aiming: false);
			}
			PlayerHUD.Instance?.SetShotgunGroupVisible(visible: false);
		}
	}

	[Command]
	private void CmdFire(Vector3 origin, Vector3 direction)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		writer.WriteVector3(origin);
		writer.WriteVector3(direction);
		SendCommandInternal("System.Void HunterShotgun::CmdFire(UnityEngine.Vector3,UnityEngine.Vector3)", -1427702985, writer, 0);
		NetworkWriterPool.Return(writer);
	}

	[ClientRpc]
	private void RpcSpawnImpactFX(Vector3 position, Vector3 direction)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		writer.WriteVector3(position);
		writer.WriteVector3(direction);
		SendRPCInternal("System.Void HunterShotgun::RpcSpawnImpactFX(UnityEngine.Vector3,UnityEngine.Vector3)", -460395039, writer, 0, includeOwner: true);
		NetworkWriterPool.Return(writer);
	}

	[Server]
	private void ProcessHit(GameObject target, Vector3 shotDir, Vector3 hitPoint)
	{
		if (!NetworkServer.active)
		{
			UnityEngine.Debug.LogWarning("[Server] function 'System.Void HunterShotgun::ProcessHit(UnityEngine.GameObject,UnityEngine.Vector3,UnityEngine.Vector3)' called when server was not active");
			return;
		}
		Health component = target.GetComponent<Health>();
		PlayerRoleData component2 = target.GetComponent<PlayerRoleData>();
		AnimalBotController component3 = target.GetComponent<AnimalBotController>();
		if (component2 == null && component3 == null)
		{
			_cleanKillStreak = 0;
		}
		else if (component2 != null)
		{
			if (component2.Role == PlayerRole.Hunter)
			{
				UnityEngine.Debug.Log("[Shotgun] Avcıya isabet — etkisiz.");
				TargetUnlockAchievement(base.connectionToClient, "ACH_FRIENDLY_FIRE");
				_hadBadShot = true;
				_cleanKillStreak = 0;
				return;
			}
			UnityEngine.Debug.Log($"[Shotgun] OYUNCU VURULDU! (hayvan: {component2.AssignedAnimal})");
			if (component != null)
			{
				component.ServerKill();
			}
			Network_killCount = _killCount + 1;
			TargetUnlockAchievement(base.connectionToClient, "ACH_BULLSEYE");
			_cleanKillStreak++;
			if (_cleanKillStreak >= 3)
			{
				TargetUnlockAchievement(base.connectionToClient, "ACH_EAGLE_EYE");
			}
			if (GameManager.Instance != null && NetworkTime.time - GameManager.Instance.RoundStartTime <= 10.0)
			{
				TargetUnlockAchievement(base.connectionToClient, "ACH_QUICK_DRAW");
			}
			_serverAmmo = Mathf.Min(_serverAmmo + 3, MaxAmmo);
			float effectiveWrongShotTimePenalty = EffectiveWrongShotTimePenalty;
			GameManager.Instance?.ServerAddGameTime(effectiveWrongShotTimePenalty);
			RpcCorrectShotTimeBonus(effectiveWrongShotTimePenalty);
		}
		else
		{
			UnityEngine.Debug.Log($"[Shotgun] HAYVAN (bot) vuruldu: {component3.animalType} — kaçıyor.");
			component3.ServerOnKilled();
			TargetUnlockAchievement(base.connectionToClient, "ACH_OOPS");
			TargetPlayWrongAnimalShootSfx(base.connectionToClient);
			_hadBadShot = true;
			_cleanKillStreak = 0;
			CheckMooWhoTrick(component3.animalType);
			float effectiveWrongShotTimePenalty2 = EffectiveWrongShotTimePenalty;
			GameManager.Instance?.ServerAddGameTime(0f - effectiveWrongShotTimePenalty2);
			RpcWrongShotTimePenalty(effectiveWrongShotTimePenalty2);
		}
	}

	[TargetRpc]
	private void TargetSyncAmmo(NetworkConnectionToClient target, int ammo)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		writer.WriteVarInt(ammo);
		SendTargetRPCInternal(target, "System.Void HunterShotgun::TargetSyncAmmo(Mirror.NetworkConnectionToClient,System.Int32)", 1391464439, writer, 0);
		NetworkWriterPool.Return(writer);
	}

	[Server]
	private void CheckMooWhoTrick(AnimalType botAnimalType)
	{
		if (!NetworkServer.active)
		{
			UnityEngine.Debug.LogWarning("[Server] function 'System.Void HunterShotgun::CheckMooWhoTrick(AnimalType)' called when server was not active");
			return;
		}
		foreach (NetworkConnectionToClient value in NetworkServer.connections.Values)
		{
			if (value == null || value.identity == null)
			{
				continue;
			}
			PlayerRoleData component = value.identity.GetComponent<PlayerRoleData>();
			if (!(component == null) && component.Role == PlayerRole.Animal && component.AssignedAnimal == botAnimalType)
			{
				Health component2 = value.identity.GetComponent<Health>();
				if (!(component2 != null) || !component2.IsDead)
				{
					TargetUnlockAchievement(value, "ACH_MOO_WHO");
				}
			}
		}
	}

	[ClientRpc]
	private void RpcWrongShotTimePenalty(float seconds)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		writer.WriteFloat(seconds);
		SendRPCInternal("System.Void HunterShotgun::RpcWrongShotTimePenalty(System.Single)", -1243354190, writer, 0, includeOwner: true);
		NetworkWriterPool.Return(writer);
	}

	[ClientRpc]
	private void RpcCorrectShotTimeBonus(float seconds)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		writer.WriteFloat(seconds);
		SendRPCInternal("System.Void HunterShotgun::RpcCorrectShotTimeBonus(System.Single)", 1899048979, writer, 0, includeOwner: true);
		NetworkWriterPool.Return(writer);
	}

	[TargetRpc]
	private void TargetUnlockAchievement(NetworkConnectionToClient target, string achievementApiName)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		writer.WriteString(achievementApiName);
		SendTargetRPCInternal(target, "System.Void HunterShotgun::TargetUnlockAchievement(Mirror.NetworkConnectionToClient,System.String)", -196962628, writer, 0);
		NetworkWriterPool.Return(writer);
	}

	[TargetRpc]
	private void TargetPlayWrongAnimalShootSfx(NetworkConnectionToClient target)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		SendTargetRPCInternal(target, "System.Void HunterShotgun::TargetPlayWrongAnimalShootSfx(Mirror.NetworkConnectionToClient)", 1642931202, writer, 0);
		NetworkWriterPool.Return(writer);
	}

	[ClientRpc]
	private void RpcPlayFire()
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		SendRPCInternal("System.Void HunterShotgun::RpcPlayFire()", 747455594, writer, 0, includeOwner: true);
		NetworkWriterPool.Return(writer);
	}

	private void PlayEmptyClick()
	{
		if (emptyClickSfx != null && audioSource != null)
		{
			audioSource.pitch = 1f;
			audioSource.PlayOneShot(emptyClickSfx);
		}
	}

	public override bool Weaved()
	{
		return true;
	}

	protected void UserCode_TargetResetAmmo__NetworkConnectionToClient(NetworkConnectionToClient target)
	{
		_ammo = MaxAmmo;
		_lastFireTime = -999f;
	}

	protected static void InvokeUserCode_TargetResetAmmo__NetworkConnectionToClient(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			UnityEngine.Debug.LogError("TargetRPC TargetResetAmmo called on server.");
		}
		else
		{
			((HunterShotgun)obj).UserCode_TargetResetAmmo__NetworkConnectionToClient(null);
		}
	}

	protected void UserCode_RpcPlayReload()
	{
		if (!(reloadSfx == null) && !(audioSource == null))
		{
			audioSource.pitch = UnityEngine.Random.Range(minPitch, maxPitch);
			audioSource.PlayOneShot(reloadSfx);
		}
	}

	protected static void InvokeUserCode_RpcPlayReload(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			UnityEngine.Debug.LogError("RPC RpcPlayReload called on server.");
		}
		else
		{
			((HunterShotgun)obj).UserCode_RpcPlayReload();
		}
	}

	protected void UserCode_CmdFire__Vector3__Vector3(Vector3 origin, Vector3 direction)
	{
		if (GameManager.Instance == null || !GameManager.Instance.IsHunterDoorOpen || _serverAmmo <= 0 || Time.time - _serverLastFire < fireDelay - 0.1f)
		{
			return;
		}
		_serverLastFire = Time.time;
		_serverAmmo--;
		direction.Normalize();
		RpcPlayFire();
		bool flag = false;
		Collider[] array = Physics.OverlapSphere(origin, 0.1f, hitMask, QueryTriggerInteraction.Collide);
		GameObject gameObject = null;
		Collider[] array2 = array;
		foreach (Collider collider in array2)
		{
			if (!(collider.GetComponentInParent<PoopPickup>() != null))
			{
				GameObject gameObject2 = collider.transform.root.gameObject;
				if (!(gameObject2 == base.gameObject))
				{
					gameObject = gameObject2;
					break;
				}
			}
		}
		if (gameObject != null)
		{
			RpcSpawnImpactFX(origin, direction);
			ProcessHit(gameObject, direction, origin);
			flag = true;
		}
		else
		{
			RaycastHit[] array3 = Physics.SphereCastAll(origin, hitSphereRadius, direction, range, hitMask, QueryTriggerInteraction.Collide);
			if (array3.Length != 0)
			{
				Array.Sort(array3, (RaycastHit a, RaycastHit b) => a.distance.CompareTo(b.distance));
				List<RaycastHit> list = new List<RaycastHit>();
				List<float> list2 = new List<float>();
				RaycastHit[] array4 = array3;
				for (int i = 0; i < array4.Length; i++)
				{
					RaycastHit item = array4[i];
					if (item.collider.GetComponentInParent<PoopPickup>() != null)
					{
						continue;
					}
					GameObject gameObject3 = item.collider.transform.root.gameObject;
					if (!(gameObject3 == base.gameObject))
					{
						if (gameObject3.GetComponent<PlayerRoleData>() != null || gameObject3.GetComponent<AnimalBotController>() != null)
						{
							list.Add(item);
						}
						else
						{
							list2.Add(item.distance);
						}
					}
				}
				int layerMask = ~(int)hitMask & ~(1 << _ignoreRaycastLayer);
				foreach (RaycastHit item2 in list)
				{
					bool flag2 = false;
					foreach (float item3 in list2)
					{
						if (item3 < item2.distance - obstructionTolerance)
						{
							flag2 = true;
							break;
						}
					}
					if (!flag2 && Physics.Raycast(origin, direction, out var hitInfo, Mathf.Max(0f, item2.distance - 0.01f), layerMask, QueryTriggerInteraction.Ignore) && hitInfo.distance < item2.distance - obstructionTolerance)
					{
						flag2 = true;
					}
					if (!flag2)
					{
						RpcSpawnImpactFX(item2.point, direction);
						ProcessHit(item2.collider.transform.root.gameObject, direction, item2.point);
						flag = true;
						break;
					}
				}
			}
		}
		if (!flag)
		{
			RpcSpawnImpactFX(origin + direction * range, direction);
			_cleanKillStreak = 0;
		}
		TargetSyncAmmo(base.connectionToClient, _serverAmmo);
	}

	protected static void InvokeUserCode_CmdFire__Vector3__Vector3(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			UnityEngine.Debug.LogError("Command CmdFire called on client.");
		}
		else
		{
			((HunterShotgun)obj).UserCode_CmdFire__Vector3__Vector3(reader.ReadVector3(), reader.ReadVector3());
		}
	}

	protected void UserCode_RpcSpawnImpactFX__Vector3__Vector3(Vector3 position, Vector3 direction)
	{
		if (!(impactParticlePrefab == null))
		{
			Quaternion rotation = Quaternion.FromToRotation(Vector3.up, -direction);
			UnityEngine.Object.Destroy(UnityEngine.Object.Instantiate(impactParticlePrefab, position, rotation), impactParticleLifetime);
		}
	}

	protected static void InvokeUserCode_RpcSpawnImpactFX__Vector3__Vector3(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			UnityEngine.Debug.LogError("RPC RpcSpawnImpactFX called on server.");
		}
		else
		{
			((HunterShotgun)obj).UserCode_RpcSpawnImpactFX__Vector3__Vector3(reader.ReadVector3(), reader.ReadVector3());
		}
	}

	protected void UserCode_TargetSyncAmmo__NetworkConnectionToClient__Int32(NetworkConnectionToClient target, int ammo)
	{
		_ammo = ammo;
	}

	protected static void InvokeUserCode_TargetSyncAmmo__NetworkConnectionToClient__Int32(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			UnityEngine.Debug.LogError("TargetRPC TargetSyncAmmo called on server.");
		}
		else
		{
			((HunterShotgun)obj).UserCode_TargetSyncAmmo__NetworkConnectionToClient__Int32(null, reader.ReadVarInt());
		}
	}

	protected void UserCode_RpcWrongShotTimePenalty__Single(float seconds)
	{
		PlayerHUD.Instance?.ShowWrongShotTimePenalty(seconds);
	}

	protected static void InvokeUserCode_RpcWrongShotTimePenalty__Single(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			UnityEngine.Debug.LogError("RPC RpcWrongShotTimePenalty called on server.");
		}
		else
		{
			((HunterShotgun)obj).UserCode_RpcWrongShotTimePenalty__Single(reader.ReadFloat());
		}
	}

	protected void UserCode_RpcCorrectShotTimeBonus__Single(float seconds)
	{
		PlayerHUD.Instance?.ShowCorrectShotTimeBonus(seconds);
	}

	protected static void InvokeUserCode_RpcCorrectShotTimeBonus__Single(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			UnityEngine.Debug.LogError("RPC RpcCorrectShotTimeBonus called on server.");
		}
		else
		{
			((HunterShotgun)obj).UserCode_RpcCorrectShotTimeBonus__Single(reader.ReadFloat());
		}
	}

	protected void UserCode_TargetUnlockAchievement__NetworkConnectionToClient__String(NetworkConnectionToClient target, string achievementApiName)
	{
		AchievementManager.Instance?.Unlock(achievementApiName);
	}

	protected static void InvokeUserCode_TargetUnlockAchievement__NetworkConnectionToClient__String(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			UnityEngine.Debug.LogError("TargetRPC TargetUnlockAchievement called on server.");
		}
		else
		{
			((HunterShotgun)obj).UserCode_TargetUnlockAchievement__NetworkConnectionToClient__String(null, reader.ReadString());
		}
	}

	protected void UserCode_TargetPlayWrongAnimalShootSfx__NetworkConnectionToClient(NetworkConnectionToClient target)
	{
		WrongAnimalShootSource?.Play();
	}

	protected static void InvokeUserCode_TargetPlayWrongAnimalShootSfx__NetworkConnectionToClient(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			UnityEngine.Debug.LogError("TargetRPC TargetPlayWrongAnimalShootSfx called on server.");
		}
		else
		{
			((HunterShotgun)obj).UserCode_TargetPlayWrongAnimalShootSfx__NetworkConnectionToClient(null);
		}
	}

	protected void UserCode_RpcPlayFire()
	{
		if (fireSfx != null && audioSource != null)
		{
			audioSource.pitch = UnityEngine.Random.Range(minPitch, maxPitch);
			audioSource.PlayOneShot(fireSfx);
		}
		if (muzzleFlash != null)
		{
			if (muzzlePoint != null)
			{
				muzzleFlash.transform.position = muzzlePoint.position;
				muzzleFlash.transform.rotation = muzzlePoint.rotation;
			}
			muzzleFlash.Play();
		}
		if (base.isLocalPlayer && cam != null)
		{
			cam.TriggerFireShake();
		}
	}

	protected static void InvokeUserCode_RpcPlayFire(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			UnityEngine.Debug.LogError("RPC RpcPlayFire called on server.");
		}
		else
		{
			((HunterShotgun)obj).UserCode_RpcPlayFire();
		}
	}

	static HunterShotgun()
	{
		RemoteProcedureCalls.RegisterCommand(typeof(HunterShotgun), "System.Void HunterShotgun::CmdFire(UnityEngine.Vector3,UnityEngine.Vector3)", InvokeUserCode_CmdFire__Vector3__Vector3, requiresAuthority: true);
		RemoteProcedureCalls.RegisterRpc(typeof(HunterShotgun), "System.Void HunterShotgun::RpcPlayReload()", InvokeUserCode_RpcPlayReload);
		RemoteProcedureCalls.RegisterRpc(typeof(HunterShotgun), "System.Void HunterShotgun::RpcSpawnImpactFX(UnityEngine.Vector3,UnityEngine.Vector3)", InvokeUserCode_RpcSpawnImpactFX__Vector3__Vector3);
		RemoteProcedureCalls.RegisterRpc(typeof(HunterShotgun), "System.Void HunterShotgun::RpcWrongShotTimePenalty(System.Single)", InvokeUserCode_RpcWrongShotTimePenalty__Single);
		RemoteProcedureCalls.RegisterRpc(typeof(HunterShotgun), "System.Void HunterShotgun::RpcCorrectShotTimeBonus(System.Single)", InvokeUserCode_RpcCorrectShotTimeBonus__Single);
		RemoteProcedureCalls.RegisterRpc(typeof(HunterShotgun), "System.Void HunterShotgun::RpcPlayFire()", InvokeUserCode_RpcPlayFire);
		RemoteProcedureCalls.RegisterRpc(typeof(HunterShotgun), "System.Void HunterShotgun::TargetResetAmmo(Mirror.NetworkConnectionToClient)", InvokeUserCode_TargetResetAmmo__NetworkConnectionToClient);
		RemoteProcedureCalls.RegisterRpc(typeof(HunterShotgun), "System.Void HunterShotgun::TargetSyncAmmo(Mirror.NetworkConnectionToClient,System.Int32)", InvokeUserCode_TargetSyncAmmo__NetworkConnectionToClient__Int32);
		RemoteProcedureCalls.RegisterRpc(typeof(HunterShotgun), "System.Void HunterShotgun::TargetUnlockAchievement(Mirror.NetworkConnectionToClient,System.String)", InvokeUserCode_TargetUnlockAchievement__NetworkConnectionToClient__String);
		RemoteProcedureCalls.RegisterRpc(typeof(HunterShotgun), "System.Void HunterShotgun::TargetPlayWrongAnimalShootSfx(Mirror.NetworkConnectionToClient)", InvokeUserCode_TargetPlayWrongAnimalShootSfx__NetworkConnectionToClient);
	}

	public override void SerializeSyncVars(NetworkWriter writer, bool forceAll)
	{
		base.SerializeSyncVars(writer, forceAll);
		if (forceAll)
		{
			writer.WriteVarInt(_killCount);
			return;
		}
		writer.WriteVarULong(syncVarDirtyBits);
		if ((syncVarDirtyBits & 1L) != 0L)
		{
			writer.WriteVarInt(_killCount);
		}
	}

	public override void DeserializeSyncVars(NetworkReader reader, bool initialState)
	{
		base.DeserializeSyncVars(reader, initialState);
		if (initialState)
		{
			GeneratedSyncVarDeserialize(ref _killCount, null, reader.ReadVarInt());
			return;
		}
		long num = (long)reader.ReadVarULong();
		if ((num & 1L) != 0L)
		{
			GeneratedSyncVarDeserialize(ref _killCount, null, reader.ReadVarInt());
		}
	}
}
