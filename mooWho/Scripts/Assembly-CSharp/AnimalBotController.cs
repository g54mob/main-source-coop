using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Mirror;
using Mirror.RemoteCalls;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
public class AnimalBotController : NetworkBehaviour
{
	private enum State
	{
		Idle = 0,
		Walk = 1,
		Run = 2,
		LookAround = 3,
		Slapped = 4,
		Spin = 5,
		Fleeing = 6,
		Eating = 7
	}

	[Header("Movement")]
	public float walkSpeed = 5f;

	public float runSpeed = 9f;

	public float rotSpeed = 8f;

	public float stoppingDist = 0.4f;

	[Header("Dolaşma Alanı")]
	public WanderZone wanderZone;

	[Tooltip("Eğimli/karmaşık haritalarda (ör. Forest) TEK bir dörtgen tüm alanı düzgün kapsayamayabilir (dik yamaçlar, vadiler) — buraya ek WanderZone'lar eklenebilir; her seferinde wanderZone + bunlardan rastgele biri seçilir, botlar haritaya daha iyi yayılır.")]
	public WanderZone[] additionalWanderZones;

	public float fallbackRadius = 30f;

	public float minTravelDistance = 8f;

	[Tooltip("Hedefe giderken ara nokta (waypoint) mesafesi — kademeli yol için")]
	public float waypointStep = 10f;

	[Tooltip("Ara noktalarda yanal sapma (zigzag hissi)")]
	public float lateralWander = 4f;

	[Header("Zamanlama")]
	public float minIdleTime = 0.5f;

	public float maxIdleTime = 2.5f;

	public float minWalkTime = 5f;

	public float maxWalkTime = 12f;

	public float runChance = 0.35f;

	public float lookAroundChance = 0.1f;

	[Header("Nadir Davranışlar (insansı)")]
	[Range(0f, 1f)]
	public float quickTurnChance = 0.03f;

	[Range(0f, 1f)]
	public float spinChance = 0.02f;

	[Range(0f, 1f)]
	public float suddenStopChance = 0.03f;

	[Header("Engel Yönetimi")]
	public float stuckTimeout = 1.2f;

	public float stuckSpeedThreshold = 0.2f;

	[Tooltip("CheckPathAhead — agent'ın steeringTarget'ı (yol üzerinde şu an gitmekte olduğu hemen önündeki nokta) bu yarıçapta katı bir collider'a denk gelirse (baked NavMesh ile mesh collider arasında uyumsuzluk — NavMesh 'gidilebilir' diyor ama fiziksel engel var) bot oraya varmadan rota değiştirir.")]
	public float pathAheadCheckRadius = 1f;

	[Header("Zemin Sabitleme")]
	[Tooltip("Y snap yumuşatma — düşük = daha smooth ama gecikmeli")]
	public float groundSnapSpeed = 8f;

	[Tooltip("Bu farktan küçük Y sapmalarını yok say (titreme önler)")]
	public float groundSnapDeadzone = 0.03f;

	[Header("Ölüm Kaçışı")]
	public float deathRunDuration = 5f;

	public float deathRunSpeed = 11f;

	[Header("Animator")]
	public float animDampTime = 0.12f;

	[Header("Ses")]
	public AnimalType animalType = AnimalType.Cow;

	public AudioSource audioSource;

	public float minPitch = 0.95f;

	public float maxPitch = 1.05f;

	public AudioClip defaultClip;

	[Header("Otomatik Ses")]
	public float minInterval = 10f;

	public float maxInterval = 40f;

	[Tooltip("ScheduleNextSound'daki çarpık dağılımın üssü — GameManager, Lobby Game Settings'teki Animal Sound (NPC Noise Frequency) seviyesine göre set eder (bkz. LobbyGameSettingsUtil.GetAnimalSoundSkew). Yüksek değer = minInterval'e yakın (sık ses) gelme ihtimali daha NADİR.")]
	public float soundIntervalSkew = 2.5f;

	[Header("Konuşma Efekti (ağız)")]
	[Tooltip("Ses çıkarırken ağız hizasından spawn edilecek VFX prefab")]
	public GameObject talkVfxPrefab;

	[Tooltip("Bulunan ağız/kafa noktasına ek yerel offset")]
	public Vector3 talkVfxLocalOffset = Vector3.zero;

	[Tooltip("Spawn edilen VFX kaç saniye sonra destroy edilir")]
	public float talkVfxLifetime = 5f;

	private Transform _mouthPoint;

	[Header("Yemek Yeme (Eat_b)")]
	[Tooltip("Eating state'ine girme aralığı (sn) — 10'a yakın olma ihtimali düşük (yüksek uca çarpık dağılım).")]
	public float eatIntervalMin = 10f;

	public float eatIntervalMax = 70f;

	[Tooltip("Yemek yeme süresi (sn) — 20'ye yakın olma ihtimali düşük (düşük uca çarpık dağılım, nadiren 15-20sn arası sürebilir).")]
	public float eatDurationMin = 2.5f;

	public float eatDurationMax = 20f;

	public AudioSource eatSfxSource;

	[Tooltip("Normal (ot/mera) yeme animasyonu sesi")]
	public AudioClip eatGrassSfx;

	[Tooltip("Kaka yerken çalınan ses efekti — TryStartSeekingPoop ile bir kakaya gidip EnterEating(eatingPoop: true) ile başlayan yeme evresinde kullanılır")]
	public AudioClip eatPooSfx;

	[Tooltip("Eat animasyonu 2x hızda oynatıldığı için 2.5sn (giriş+yeme+çıkış) — ses tam yeme anında çalsın diye ilk ses bu kadar gecikmeli başlar")]
	public float eatSfxStartDelay = 0.5f;

	[Tooltip("Eating devam ettiği sürece ses bu aralıkla tekrarlanır")]
	public float eatSfxRepeatInterval = 1.5f;

	private Coroutine _eatSfxRoutine;

	private bool _eatingPoop;

	[SyncVar]
	private float _syncedSpeed;

	[SyncVar(hook = "OnEatingChanged")]
	private bool _syncedEating;

	private bool _soundStopped;

	private NavMeshAgent _agent;

	private Animator _anim;

	private Vector3 _spawnPos;

	private State _state;

	private float _stateTimer;

	private float _lookTargetAngle;

	private float _lookTimer;

	private float _spinTargetY;

	private float _spinDoneY;

	private float _stuckTimer;

	private float _nextSoundTime;

	private float _deathRunTimer;

	private bool _hasScheduledSound;

	private bool _hasScheduledEat;

	private float _nextEatTime;

	private readonly Queue<Vector3> _waypoints = new Queue<Vector3>();

	private Vector3 _finalTarget;

	private float _smoothY;

	private bool _hasSmoothY;

	private static readonly int _hashSpeed;

	private static readonly int _hashEat;

	[Header("Kaka Arama (nadir)")]
	[Tooltip("Her kontrolde yakında kaka varsa ona gitme ihtimali — düşük tut")]
	[Range(0f, 1f)]
	public float seekPoopChance = 0.06f;

	[Tooltip("Bu mesafe içinde kaka varsa arama tetiklenebilir")]
	public float poopSeekRadius = 15f;

	[Tooltip("Kaka arama kontrolü ne sıklıkla yapılır (sn)")]
	public float poopSeekCheckInterval = 25f;

	private float _nextPoopSeekCheck;

	private bool _headingToPoop;

	private Transform _targetPoop;

	[Header("İlk Ses (Round Başı)")]
	[Tooltip("İSTEK: hunter release geri sayımı bitip (kapı açılıp) tam 5sn geçmeden İLK ses hiç planlanmaz (bkz. ScheduleFirstSound'un Time.time referansı — CanMakeBotSoundNow İLK true olduğu an, yani kapının açıldığı an). Bu 5sn'lik bekleme sonrası, bu dar aralıkta (normal min/max yerine) planlanır — amaç: herkesin kaydettiği sesi oyuncular erkenden duysun. Bu SADECE earlySoundChance ihtimalle gerçekleşir, geri kalan bot'lar normal (minInterval/maxInterval) aralığa düşer.")]
	public float firstSoundMinDelay = 5f;

	public float firstSoundMaxDelay = 15f;

	[Tooltip("İSTEK: eskiden HER bot kesin olarak erken ses çıkarıyordu — artık sadece bu ihtimalle (0..1) erken/dar aralık kullanılır. %20'ye çıkarıldı (sırasıyla eski: %20 → %10 → %20).")]
	[Range(0f, 1f)]
	public float earlySoundChance = 0.2f;

	private const float MapLimiterMargin = 2f;

	private static BoxCollider[] _mapLimiterCache;

	public Action<bool, bool> _Mirror_SyncVarHookDelegate__syncedEating;

	public bool IsEating => _syncedEating;

	private bool HasWanderZones
	{
		get
		{
			if (!(wanderZone != null))
			{
				if (additionalWanderZones != null)
				{
					return additionalWanderZones.Length != 0;
				}
				return false;
			}
			return true;
		}
	}

	public float Network_syncedSpeed
	{
		get
		{
			return _syncedSpeed;
		}
		[param: In]
		set
		{
			GeneratedSyncVarSetter(value, ref _syncedSpeed, 1uL, null);
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
			GeneratedSyncVarSetter(value, ref _syncedEating, 2uL, _Mirror_SyncVarHookDelegate__syncedEating);
		}
	}

	private void Awake()
	{
		_agent = GetComponent<NavMeshAgent>();
		_anim = GetComponent<Animator>();
		_agent.stoppingDistance = stoppingDist;
		_agent.angularSpeed = 0f;
		_agent.updateRotation = false;
		_agent.updateUpAxis = false;
		_agent.autoBraking = false;
		_agent.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;
		_agent.avoidancePriority = UnityEngine.Random.Range(30, 70);
		_agent.radius = Mathf.Max(_agent.radius, 0.3f);
		_agent.enabled = false;
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
		if (eatSfxSource == null)
		{
			eatSfxSource = base.gameObject.AddComponent<AudioSource>();
		}
		eatSfxSource.playOnAwake = false;
		eatSfxSource.spatialBlend = 1f;
		eatSfxSource.rolloffMode = AudioRolloffMode.Linear;
		SetupTalkVfx();
	}

	private void SetupTalkVfx()
	{
		_mouthPoint = MouthPointFinder.Find(_anim.transform);
	}

	private void PlayTalkVfx()
	{
		if (!(talkVfxPrefab == null) && !(_mouthPoint == null))
		{
			GameObject obj = UnityEngine.Object.Instantiate(talkVfxPrefab, _mouthPoint);
			obj.transform.localPosition = talkVfxLocalOffset;
			TalkVfxFacing talkVfxFacing = obj.AddComponent<TalkVfxFacing>();
			talkVfxFacing.facingReference = base.transform;
			talkVfxFacing.localCorrection = Quaternion.identity;
			obj.transform.rotation = talkVfxFacing.facingReference.rotation * talkVfxFacing.localCorrection;
			UnityEngine.Object.Destroy(obj, talkVfxLifetime);
		}
	}

	public override void OnStartServer()
	{
		StartCoroutine(DelayedStart());
	}

	public override void OnStopServer()
	{
		AnimalTickManager.Instance?.Unregister(this);
	}

	private IEnumerator DelayedStart()
	{
		yield return new WaitForSeconds(0.15f);
		_agent.enabled = true;
		yield return null;
		if (NavMesh.SamplePosition(base.transform.position, out var hit, 5f, -1))
		{
			Vector3 vector = WanderZone.SnapToTerrain(hit.position);
			_agent.Warp(vector);
			_spawnPos = vector;
			_smoothY = vector.y;
			_hasSmoothY = true;
			EnterIdle();
			AnimalTickManager.Instance?.Register(this);
		}
		else
		{
			Debug.LogWarning("[AnimalBot] " + base.name + " NavMesh yok, despawn.");
			NetworkServer.Destroy(base.gameObject);
		}
	}

	public void ManagedTick()
	{
		if (!_agent.isOnNavMesh)
		{
			if (NavMesh.SamplePosition(base.transform.position, out var hit, 10f, -1))
			{
				_agent.Warp(hit.position);
			}
			return;
		}
		TickFSM();
		CheckStuck();
		CheckObstruction();
		CheckPathAhead();
		if (_state != State.Fleeing && !_soundStopped && CanMakeBotSoundNow())
		{
			if (!_hasScheduledSound)
			{
				_hasScheduledSound = true;
				if (UnityEngine.Random.value < earlySoundChance)
				{
					ScheduleFirstSound();
				}
				else
				{
					ScheduleNextSound();
				}
			}
			else if (Time.time >= _nextSoundTime)
			{
				RpcPlaySound();
				ScheduleNextSound();
			}
		}
		if (CanMakeSoundNow())
		{
			if (!_hasScheduledEat)
			{
				_hasScheduledEat = true;
				ScheduleNextEat();
			}
			else if (CanStartEating() && Time.time >= _nextEatTime)
			{
				EnterEating();
			}
			if (!_headingToPoop && CanStartEating() && Time.time >= _nextPoopSeekCheck)
			{
				_nextPoopSeekCheck = Time.time + poopSeekCheckInterval;
				TryStartSeekingPoop();
			}
		}
	}

	[Server]
	private void TryStartSeekingPoop()
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void AnimalBotController::TryStartSeekingPoop()' called when server was not active");
		}
		else if (!(UnityEngine.Random.value > seekPoopChance))
		{
			PoopPickup poopPickup = FindNearestPoop();
			if (!(poopPickup == null))
			{
				_targetPoop = poopPickup.transform;
				_headingToPoop = true;
				_state = State.Walk;
				_agent.speed = walkSpeed;
				_waypoints.Clear();
				_agent.SetDestination(_targetPoop.position);
				_stateTimer = 30f;
			}
		}
	}

	private PoopPickup FindNearestPoop()
	{
		PoopPickup result = null;
		float num = poopSeekRadius * poopSeekRadius;
		PoopPickup[] array = UnityEngine.Object.FindObjectsOfType<PoopPickup>();
		foreach (PoopPickup poopPickup in array)
		{
			float sqrMagnitude = (poopPickup.transform.position - base.transform.position).sqrMagnitude;
			if (sqrMagnitude <= num)
			{
				num = sqrMagnitude;
				result = poopPickup;
			}
		}
		return result;
	}

	private static bool CanMakeSoundNow()
	{
		GameManager instance = GameManager.Instance;
		if (instance != null && instance._gameStarted)
		{
			return !instance.IsGameEnded;
		}
		return false;
	}

	private static bool CanMakeBotSoundNow()
	{
		if (CanMakeSoundNow())
		{
			return GameManager.Instance.IsHunterDoorOpen;
		}
		return false;
	}

	private void SnapToGround()
	{
		Vector3 origin = base.transform.position + Vector3.up * 3f;
		float y;
		if (TryFindGroundHit(origin, 10f, out var result))
		{
			y = result.point.y;
			if (_hasSmoothY && Mathf.Abs(y - _smoothY) > 1f && NavMesh.SamplePosition(base.transform.position, out var hit, 3f, -1) && Mathf.Abs(y - hit.position.y) > 0.5f)
			{
				y = hit.position.y;
			}
		}
		else
		{
			if (!NavMesh.SamplePosition(base.transform.position, out var hit2, 3f, -1))
			{
				return;
			}
			y = hit2.position.y;
		}
		if (!_hasSmoothY)
		{
			_smoothY = y;
			_hasSmoothY = true;
		}
		if (Mathf.Abs(_smoothY - y) > groundSnapDeadzone)
		{
			float t = 1f - Mathf.Exp((0f - groundSnapSpeed) * Time.deltaTime);
			_smoothY = Mathf.Lerp(_smoothY, y, t);
		}
		Vector3 position = base.transform.position;
		position.y = _smoothY;
		base.transform.position = position;
	}

	private bool TryFindGroundHit(Vector3 origin, float maxDistance, out RaycastHit result)
	{
		RaycastHit[] array = Physics.RaycastAll(origin, Vector3.down, maxDistance, -1, QueryTriggerInteraction.Ignore);
		Array.Sort(array, (RaycastHit a, RaycastHit b) => a.distance.CompareTo(b.distance));
		bool flag = false;
		bool flag2 = false;
		float num = 0f;
		RaycastHit[] array2 = array;
		for (int num2 = 0; num2 < array2.Length; num2++)
		{
			RaycastHit raycastHit = array2[num2];
			if (raycastHit.collider is TerrainCollider)
			{
				result = raycastHit;
				return true;
			}
			if (!flag)
			{
				flag = true;
				flag2 = NavMesh.SamplePosition(origin, out var hit, maxDistance + 5f, -1);
				num = hit.position.y;
			}
			if (flag2 && Mathf.Abs(raycastHit.point.y - num) <= 0.5f)
			{
				result = raycastHit;
				return true;
			}
		}
		result = default(RaycastHit);
		return false;
	}

	private void CheckStuck()
	{
		if (_state != State.Walk && _state != State.Run && _state != State.Fleeing)
		{
			_stuckTimer = 0f;
			return;
		}
		bool num = !_agent.pathPending && _agent.remainingDistance > stoppingDist;
		bool flag = _agent.velocity.magnitude < stuckSpeedThreshold;
		bool flag2 = _agent.pathStatus != NavMeshPathStatus.PathComplete;
		if ((num && flag) || flag2)
		{
			_stuckTimer += Time.deltaTime;
			if (!(_stuckTimer >= stuckTimeout))
			{
				return;
			}
			_stuckTimer = 0f;
			_waypoints.Clear();
			if (_state == State.Fleeing)
			{
				Vector3 vector = ((UnityEngine.Random.value < 0.5f) ? base.transform.right : (-base.transform.right));
				if (NavMesh.SamplePosition(base.transform.position + vector * 10f + base.transform.forward * 5f, out var hit, 8f, -1))
				{
					_agent.SetDestination(hit.position);
				}
			}
			else
			{
				SetNewWanderPath();
			}
		}
		else
		{
			_stuckTimer = 0f;
		}
	}

	private void CheckObstruction()
	{
		if ((_state == State.Walk || _state == State.Run || _state == State.Fleeing) && IsObstructed(base.transform.position, Mathf.Max(_agent.radius, 0.3f)))
		{
			_waypoints.Clear();
			if (_state == State.Fleeing)
			{
				BuildFleePath();
			}
			else
			{
				SetNewWanderPath();
			}
		}
	}

	private void CheckPathAhead()
	{
		if ((_state == State.Walk || _state == State.Run || _state == State.Fleeing) && _agent.hasPath && !_agent.pathPending && IsObstructed(_agent.steeringTarget, pathAheadCheckRadius))
		{
			_waypoints.Clear();
			if (_state == State.Fleeing)
			{
				BuildFleePath();
			}
			else
			{
				SetNewWanderPath();
			}
		}
	}

	private bool IsObstructed(Vector3 point, float radius)
	{
		return IsPointObstructed(point, radius, base.transform);
	}

	public static bool IsPointObstructed(Vector3 point, float radius, Transform ignoreRoot = null)
	{
		Collider[] array = Physics.OverlapSphere(point + Vector3.up * (radius + 0.1f), radius, -1, QueryTriggerInteraction.Ignore);
		foreach (Collider collider in array)
		{
			if (!(collider == null) && !(collider is TerrainCollider) && (!(ignoreRoot != null) || !collider.transform.IsChildOf(ignoreRoot)) && !(collider.GetComponentInParent<AnimalBotController>() != null) && !(collider.GetComponentInParent<PlayerController>() != null))
			{
				return true;
			}
		}
		return false;
	}

	public static bool IsNearNavMeshObstacle(Vector3 point, float radius)
	{
		NavMeshObstacle[] array = UnityEngine.Object.FindObjectsOfType<NavMeshObstacle>();
		foreach (NavMeshObstacle navMeshObstacle in array)
		{
			if (!(navMeshObstacle == null) && navMeshObstacle.enabled && Vector3.Distance(navMeshObstacle.transform.TransformPoint(navMeshObstacle.center), point) <= radius)
			{
				return true;
			}
		}
		return false;
	}

	private void Update()
	{
		if (base.isServer && _agent != null && _agent.enabled)
		{
			if (_agent.isOnNavMesh)
			{
				HandleRotation();
				UpdateSyncedSpeed();
			}
			SnapToGround();
		}
		SyncAnimator();
	}

	public void PlayAnimalSound()
	{
		if (!(audioSource == null))
		{
			AudioClip audioClip = ((VoiceClipStore.Instance != null) ? VoiceClipStore.Instance.GetClip(animalType) : null);
			if (!(audioClip == null))
			{
				audioSource.pitch = UnityEngine.Random.Range(minPitch, maxPitch);
				audioSource.PlayOneShot(audioClip);
				PlayTalkVfx();
			}
		}
	}

	[ClientRpc]
	private void RpcPlaySound()
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		SendRPCInternal("System.Void AnimalBotController::RpcPlaySound()", 1580327206, writer, 0, includeOwner: true);
		NetworkWriterPool.Return(writer);
	}

	[Server]
	private void ScheduleFirstSound()
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void AnimalBotController::ScheduleFirstSound()' called when server was not active");
		}
		else
		{
			_nextSoundTime = Time.time + UnityEngine.Random.Range(firstSoundMinDelay, firstSoundMaxDelay);
		}
	}

	[Server]
	private void ScheduleNextSound()
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void AnimalBotController::ScheduleNextSound()' called when server was not active");
			return;
		}
		float t = 1f - Mathf.Pow(UnityEngine.Random.value, soundIntervalSkew);
		_nextSoundTime = Time.time + Mathf.Lerp(minInterval, maxInterval, t);
	}

	[Server]
	public void ServerStopMakingSound()
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void AnimalBotController::ServerStopMakingSound()' called when server was not active");
		}
		else
		{
			_soundStopped = true;
		}
	}

	[Server]
	private void ScheduleNextEat()
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void AnimalBotController::ScheduleNextEat()' called when server was not active");
			return;
		}
		float t = 1f - Mathf.Pow(UnityEngine.Random.value, 3f);
		_nextEatTime = Time.time + Mathf.Lerp(eatIntervalMin, eatIntervalMax, t);
	}

	private bool CanStartEating()
	{
		if (_state != State.Idle && _state != State.Walk && _state != State.Run)
		{
			return _state == State.LookAround;
		}
		return true;
	}

	[Server]
	private void EnterEating(bool eatingPoop = false)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void AnimalBotController::EnterEating(System.Boolean)' called when server was not active");
			return;
		}
		_state = State.Eating;
		_eatingPoop = eatingPoop;
		float t = Mathf.Pow(UnityEngine.Random.value, 3f);
		_stateTimer = Mathf.Lerp(eatDurationMin, eatDurationMax, t);
		_agent.ResetPath();
		_waypoints.Clear();
		Network_syncedSpeed = 0f;
		Network_syncedEating = true;
	}

	private void PlayEatSound()
	{
		AudioClip audioClip = (_eatingPoop ? eatPooSfx : eatGrassSfx);
		if (!(eatSfxSource == null) && !(audioClip == null))
		{
			eatSfxSource.pitch = UnityEngine.Random.Range(minPitch, maxPitch);
			eatSfxSource.PlayOneShot(audioClip);
		}
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
			PlayEatSound();
			yield return new WaitForSeconds(eatSfxRepeatInterval);
		}
	}

	private void TickFSM()
	{
		switch (_state)
		{
		case State.Idle:
			_stateTimer -= Time.deltaTime;
			if (_stateTimer <= 0f)
			{
				DecideNextState();
			}
			break;
		case State.Walk:
		case State.Run:
		{
			_stateTimer -= Time.deltaTime;
			AdvanceWaypoints();
			if (!_headingToPoop && !IsPathActive() && UnityEngine.Random.value < suddenStopChance * Time.deltaTime)
			{
				EnterIdle();
				break;
			}
			bool flag = _waypoints.Count == 0 && !_agent.pathPending && _agent.remainingDistance <= stoppingDist;
			if (!flag && !(_stateTimer <= 0f))
			{
				break;
			}
			if (_headingToPoop)
			{
				_headingToPoop = false;
				_targetPoop = null;
				if (flag)
				{
					EnterEating(eatingPoop: true);
				}
				else
				{
					EnterIdle();
				}
			}
			else if (UnityEngine.Random.value < 0.75f)
			{
				SetNewWanderPath();
			}
			else
			{
				EnterIdle();
			}
			break;
		}
		case State.LookAround:
			_lookTimer -= Time.deltaTime;
			if (_lookTimer <= 0f)
			{
				EnterIdle();
			}
			break;
		case State.Spin:
		{
			float num = rotSpeed * 40f * Time.deltaTime;
			_spinDoneY += num;
			base.transform.Rotate(0f, num, 0f);
			if (_spinDoneY >= _spinTargetY)
			{
				EnterIdle();
			}
			break;
		}
		case State.Slapped:
			_stateTimer -= Time.deltaTime;
			if (_stateTimer <= 0f)
			{
				EnterIdle();
			}
			break;
		case State.Eating:
			_stateTimer -= Time.deltaTime;
			if (_stateTimer <= 0f)
			{
				Network_syncedEating = false;
				ScheduleNextEat();
				EnterIdle();
			}
			break;
		case State.Fleeing:
			_deathRunTimer -= Time.deltaTime;
			if (!_agent.pathPending && _agent.remainingDistance <= stoppingDist && _deathRunTimer > 0f)
			{
				BuildFleePath();
			}
			if (_deathRunTimer <= 0f)
			{
				_agent.speed = walkSpeed;
				_waypoints.Clear();
				EnterIdle();
			}
			break;
		}
	}

	private void DecideNextState()
	{
		float value = UnityEngine.Random.value;
		if (value < spinChance)
		{
			_spinTargetY = UnityEngine.Random.Range(180f, 360f);
			_spinDoneY = 0f;
			_state = State.Spin;
			_agent.ResetPath();
			Network_syncedSpeed = 0f;
		}
		else if (value < spinChance + quickTurnChance)
		{
			_lookTargetAngle = base.transform.eulerAngles.y + UnityEngine.Random.Range(90f, 270f);
			_lookTimer = 0.25f;
			_state = State.LookAround;
			_agent.ResetPath();
		}
		else if (UnityEngine.Random.value < lookAroundChance)
		{
			_lookTargetAngle = UnityEngine.Random.Range(0f, 360f);
			_lookTimer = UnityEngine.Random.Range(0.5f, 1.5f);
			_state = State.LookAround;
			_agent.ResetPath();
		}
		else
		{
			bool flag = UnityEngine.Random.value < runChance;
			_state = ((!flag) ? State.Walk : State.Run);
			_agent.speed = (flag ? runSpeed : walkSpeed);
			SetNewWanderPath();
			_stateTimer = UnityEngine.Random.Range(minWalkTime, maxWalkTime);
		}
	}

	private void EnterIdle()
	{
		_state = State.Idle;
		_stateTimer = UnityEngine.Random.Range(minIdleTime, maxIdleTime);
		_agent.ResetPath();
		_waypoints.Clear();
		Network_syncedSpeed = 0f;
	}

	private void SetNewWanderPath()
	{
		_finalTarget = PickWanderPoint();
		BuildWaypoints(_finalTarget);
	}

	private void BuildWaypoints(Vector3 finalTarget)
	{
		_waypoints.Clear();
		Vector3 position = base.transform.position;
		Vector3 vector = finalTarget - position;
		float magnitude = vector.magnitude;
		Vector3 normalized = vector.normalized;
		Vector3 normalized2 = Vector3.Cross(Vector3.up, normalized).normalized;
		int num = Mathf.Max(1, Mathf.FloorToInt(magnitude / waypointStep));
		for (int i = 1; i <= num; i++)
		{
			float num2 = (float)i / (float)num;
			Vector3 sourcePosition = position + normalized * (magnitude * num2);
			if (i < num)
			{
				float num3 = UnityEngine.Random.Range(0f - lateralWander, lateralWander);
				sourcePosition += normalized2 * num3;
			}
			if (NavMesh.SamplePosition(sourcePosition, out var hit, 4f, -1))
			{
				Vector3 vector2 = WanderZone.SnapToTerrain(hit.position);
				if (IsValidGroundPoint(vector2))
				{
					_waypoints.Enqueue(vector2);
				}
			}
		}
		if (_waypoints.Count == 0)
		{
			_waypoints.Enqueue(finalTarget);
		}
		_agent.SetDestination(_waypoints.Peek());
	}

	private void BuildFleePath()
	{
		_waypoints.Clear();
		Vector3 vector = base.transform.position + base.transform.forward * 25f;
		if (HasWanderZones)
		{
			Vector3 vector2 = PickWanderZone().GetRandomPoint();
			float num = -2f;
			Vector3 forward = base.transform.forward;
			for (int i = 0; i < 6; i++)
			{
				Vector3 randomPoint = PickWanderZone().GetRandomPoint();
				Vector3 normalized = (randomPoint - base.transform.position).normalized;
				float num2 = Vector3.Dot(forward, normalized);
				if (num2 > num)
				{
					num = num2;
					vector2 = randomPoint;
				}
			}
			vector = vector2;
		}
		if (NavMesh.SamplePosition(vector, out var hit, 10f, -1))
		{
			Vector3 vector3 = WanderZone.SnapToTerrain(hit.position);
			if (IsValidGroundPoint(vector3))
			{
				vector = vector3;
			}
			else if (HasWanderZones)
			{
				vector = TryFindFleeFallback(vector);
			}
		}
		else if (HasWanderZones)
		{
			vector = TryFindFleeFallback(vector);
		}
		_agent.SetDestination(vector);
	}

	private Vector3 TryFindFleeFallback(Vector3 defaultTarget)
	{
		for (int i = 0; i < 5; i++)
		{
			if (NavMesh.SamplePosition(PickWanderZone().GetRandomPoint(), out var hit, 5f, -1))
			{
				Vector3 vector = WanderZone.SnapToTerrain(hit.position);
				if (IsValidGroundPoint(vector))
				{
					return vector;
				}
			}
		}
		return defaultTarget;
	}

	private void AdvanceWaypoints()
	{
		if (_waypoints.Count != 0 && !_agent.pathPending && _agent.remainingDistance <= stoppingDist + 0.3f)
		{
			_waypoints.Dequeue();
			if (_waypoints.Count > 0)
			{
				_agent.SetDestination(_waypoints.Peek());
			}
		}
	}

	private bool IsPathActive()
	{
		if (!_agent.hasPath && !_agent.pathPending)
		{
			return _waypoints.Count > 0;
		}
		return true;
	}

	private void HandleRotation()
	{
		if (_state == State.Spin)
		{
			return;
		}
		if (_state == State.LookAround)
		{
			float y = Mathf.LerpAngle(base.transform.eulerAngles.y, _lookTargetAngle, Time.deltaTime * rotSpeed * 0.6f);
			base.transform.rotation = Quaternion.Euler(0f, y, 0f);
		}
		else if (_state == State.Fleeing)
		{
			Vector3 velocity = _agent.velocity;
			velocity.y = 0f;
			if (velocity.sqrMagnitude > 0.1f)
			{
				Quaternion b = Quaternion.LookRotation(velocity.normalized);
				base.transform.rotation = Quaternion.Slerp(base.transform.rotation, b, Time.deltaTime * rotSpeed * 0.5f);
			}
		}
		else
		{
			Vector3 desiredVelocity = _agent.desiredVelocity;
			desiredVelocity.y = 0f;
			if (desiredVelocity.sqrMagnitude > 0.05f)
			{
				Quaternion b2 = Quaternion.LookRotation(desiredVelocity.normalized);
				base.transform.rotation = Quaternion.Slerp(base.transform.rotation, b2, Time.deltaTime * rotSpeed);
			}
		}
	}

	private void UpdateSyncedSpeed()
	{
		float num = _state switch
		{
			State.Walk => 0.5f, 
			State.Run => 1f, 
			State.Fleeing => 1f, 
			_ => 0f, 
		};
		if (!Mathf.Approximately(_syncedSpeed, num))
		{
			Network_syncedSpeed = num;
		}
	}

	private void SyncAnimator()
	{
		float value = Mathf.MoveTowards(_anim.GetFloat(_hashSpeed), _syncedSpeed, Time.deltaTime / Mathf.Max(animDampTime, 0.001f));
		_anim.SetFloat(_hashSpeed, value);
	}

	[Server]
	public void OnSlapped()
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void AnimalBotController::OnSlapped()' called when server was not active");
			return;
		}
		_state = State.Slapped;
		_stateTimer = UnityEngine.Random.Range(0.6f, 1.2f);
		_agent.ResetPath();
		_waypoints.Clear();
		Network_syncedSpeed = 0f;
		Network_syncedEating = false;
		_headingToPoop = false;
		_targetPoop = null;
	}

	[Server]
	public void ServerOnKilled()
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void AnimalBotController::ServerOnKilled()' called when server was not active");
		}
		else if (_state != State.Fleeing)
		{
			_state = State.Fleeing;
			_deathRunTimer = deathRunDuration;
			RpcPlaySound();
			_agent.speed = deathRunSpeed;
			BuildFleePath();
			Network_syncedSpeed = 1f;
			Network_syncedEating = false;
			_headingToPoop = false;
			_targetPoop = null;
		}
	}

	private WanderZone PickWanderZone()
	{
		int num = ((additionalWanderZones != null) ? additionalWanderZones.Length : 0);
		int maxExclusive = ((wanderZone != null) ? 1 : 0) + num;
		int num2 = UnityEngine.Random.Range(0, maxExclusive);
		if (wanderZone != null)
		{
			if (num2 == 0)
			{
				return wanderZone;
			}
			num2--;
		}
		return additionalWanderZones[num2];
	}

	private Vector3 PickWanderPoint()
	{
		Vector3 position = base.transform.position;
		for (int i = 0; i < 30; i++)
		{
			if (NavMesh.SamplePosition(HasWanderZones ? PickWanderZone().GetRandomPoint() : (_spawnPos + (Vector3)(UnityEngine.Random.insideUnitCircle * fallbackRadius)), out var hit, 5f, -1))
			{
				Vector3 vector = WanderZone.SnapToTerrain(hit.position);
				if (Vector3.Distance(position, vector) >= minTravelDistance && IsValidGroundPoint(vector) && IsReachable(vector))
				{
					return vector;
				}
			}
		}
		for (int j = 0; j < 20; j++)
		{
			if (NavMesh.SamplePosition(HasWanderZones ? PickWanderZone().GetRandomPoint() : (_spawnPos + (Vector3)(UnityEngine.Random.insideUnitCircle * fallbackRadius)), out var hit2, 5f, -1))
			{
				Vector3 vector2 = WanderZone.SnapToTerrain(hit2.position);
				if (IsValidGroundPoint(vector2) && IsReachable(vector2))
				{
					return vector2;
				}
			}
		}
		return position;
	}

	private bool IsReachable(Vector3 target)
	{
		NavMeshPath navMeshPath = new NavMeshPath();
		if (_agent.CalculatePath(target, navMeshPath))
		{
			return navMeshPath.status == NavMeshPathStatus.PathComplete;
		}
		return false;
	}

	public static bool IsAboveTerrain(Vector3 navMeshPoint)
	{
		if (WanderZone.TryGetTerrainHeight(navMeshPoint, out var groundY))
		{
			return Mathf.Abs(navMeshPoint.y - groundY) <= 3f;
		}
		return false;
	}

	public static bool IsGoodGroundPoint(Vector3 point, float obstructionRadius, Transform ignoreRoot = null)
	{
		if (IsAboveTerrain(point) && !IsPointObstructed(point, obstructionRadius, ignoreRoot))
		{
			return !IsNearMapLimiter(point);
		}
		return false;
	}

	private bool IsValidGroundPoint(Vector3 point)
	{
		return IsGoodGroundPoint(point, Mathf.Max(_agent.radius, 0.3f), base.transform);
	}

	private static BoxCollider[] GetMapLimiters()
	{
		if (_mapLimiterCache != null && _mapLimiterCache.Length != 0 && _mapLimiterCache[0] != null)
		{
			return _mapLimiterCache;
		}
		GameObject gameObject = GameObject.Find("MapLimiters");
		_mapLimiterCache = ((gameObject != null) ? gameObject.GetComponentsInChildren<BoxCollider>() : new BoxCollider[0]);
		return _mapLimiterCache;
	}

	public static bool IsNearMapLimiter(Vector3 point)
	{
		BoxCollider[] mapLimiters = GetMapLimiters();
		foreach (BoxCollider boxCollider in mapLimiters)
		{
			if (!(boxCollider == null) && (boxCollider.ClosestPoint(point) - point).sqrMagnitude <= 4f)
			{
				return true;
			}
		}
		return false;
	}

	public AnimalBotController()
	{
		_Mirror_SyncVarHookDelegate__syncedEating = OnEatingChanged;
	}

	static AnimalBotController()
	{
		_hashSpeed = Animator.StringToHash("Speed_f");
		_hashEat = Animator.StringToHash("Eat_b");
		RemoteProcedureCalls.RegisterRpc(typeof(AnimalBotController), "System.Void AnimalBotController::RpcPlaySound()", InvokeUserCode_RpcPlaySound);
	}

	public override bool Weaved()
	{
		return true;
	}

	protected void UserCode_RpcPlaySound()
	{
		PlayAnimalSound();
	}

	protected static void InvokeUserCode_RpcPlaySound(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("RPC RpcPlaySound called on server.");
		}
		else
		{
			((AnimalBotController)obj).UserCode_RpcPlaySound();
		}
	}

	public override void SerializeSyncVars(NetworkWriter writer, bool forceAll)
	{
		base.SerializeSyncVars(writer, forceAll);
		if (forceAll)
		{
			writer.WriteFloat(_syncedSpeed);
			writer.WriteBool(_syncedEating);
			return;
		}
		writer.WriteVarULong(syncVarDirtyBits);
		if ((syncVarDirtyBits & 1L) != 0L)
		{
			writer.WriteFloat(_syncedSpeed);
		}
		if ((syncVarDirtyBits & 2L) != 0L)
		{
			writer.WriteBool(_syncedEating);
		}
	}

	public override void DeserializeSyncVars(NetworkReader reader, bool initialState)
	{
		base.DeserializeSyncVars(reader, initialState);
		if (initialState)
		{
			GeneratedSyncVarDeserialize(ref _syncedSpeed, null, reader.ReadFloat());
			GeneratedSyncVarDeserialize(ref _syncedEating, _Mirror_SyncVarHookDelegate__syncedEating, reader.ReadBool());
			return;
		}
		long num = (long)reader.ReadVarULong();
		if ((num & 1L) != 0L)
		{
			GeneratedSyncVarDeserialize(ref _syncedSpeed, null, reader.ReadFloat());
		}
		if ((num & 2L) != 0L)
		{
			GeneratedSyncVarDeserialize(ref _syncedEating, _Mirror_SyncVarHookDelegate__syncedEating, reader.ReadBool());
		}
	}
}
