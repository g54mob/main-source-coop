using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Mirror;
using Mirror.RemoteCalls;
using UnityEngine;
using UnityEngine.AI;

public class GameManager : NetworkBehaviour
{
	[CompilerGenerated]
	private sealed class _003CNewGameCountdownRoutine_003Ed__96 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public GameManager _003C_003E4__this;

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
		public _003CNewGameCountdownRoutine_003Ed__96(int _003C_003E1__state)
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
			GameManager CS_0024_003C_003E8__locals2 = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				_003C_003E2__current = new WaitUntil(() => NetworkTime.time >= CS_0024_003C_003E8__locals2._newGameEndTime);
				_003C_003E1__state = 1;
				return true;
			case 1:
				_003C_003E1__state = -1;
				CS_0024_003C_003E8__locals2.RestartRound();
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
	private sealed class _003CReadyBarrierTimeoutRoutine_003Ed__74 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public GameManager _003C_003E4__this;

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
		public _003CReadyBarrierTimeoutRoutine_003Ed__74(int _003C_003E1__state)
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
			GameManager gameManager = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				_003C_003E2__current = new WaitForSeconds(gameManager.readyBarrierTimeoutSeconds);
				_003C_003E1__state = 1;
				return true;
			case 1:
				_003C_003E1__state = -1;
				gameManager._readyBarrierTimeoutRoutine = null;
				if (gameManager._gameStarted)
				{
					return false;
				}
				UnityEngine.Debug.LogWarning($"[GameManager] Ready bariyeri {gameManager.readyBarrierTimeoutSeconds}sn'de dolmadı " + $"({gameManager._readySet.Count}/{gameManager._connections.Count}) — round ZORLA başlatılıyor.");
				gameManager.StartCoroutine(gameManager.StartGame());
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
	private sealed class _003CServerReconfirmTeleport_003Ed__105 : IEnumerator<object>, IEnumerator, IDisposable
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
		public _003CServerReconfirmTeleport_003Ed__105(int _003C_003E1__state)
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
	private sealed class _003CSpawnAnimalsForPlayer_003Ed__108 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public GameManager _003C_003E4__this;

		public AnimalType type;

		private GameObject _003Cprefab_003E5__2;

		private int _003Ci_003E5__3;

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
		public _003CSpawnAnimalsForPlayer_003Ed__108(int _003C_003E1__state)
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
			GameManager gameManager = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
			{
				_003C_003E1__state = -1;
				_003Cprefab_003E5__2 = null;
				AnimalPrefabEntry[] animalPrefabs = gameManager.animalPrefabs;
				foreach (AnimalPrefabEntry animalPrefabEntry in animalPrefabs)
				{
					if (animalPrefabEntry.type == type)
					{
						_003Cprefab_003E5__2 = animalPrefabEntry.prefab;
						break;
					}
				}
				if (_003Cprefab_003E5__2 == null)
				{
					UnityEngine.Debug.LogWarning($"[GameManager] Prefab bulunamadı: {type}");
					return false;
				}
				_003Ci_003E5__3 = 0;
				break;
			}
			case 1:
				_003C_003E1__state = -1;
				_003Ci_003E5__3++;
				break;
			}
			if (_003Ci_003E5__3 < gameManager.animalsPerPlayer)
			{
				Vector3 navMeshPoint = gameManager.GetNavMeshPoint();
				GameObject gameObject = UnityEngine.Object.Instantiate(_003Cprefab_003E5__2, navMeshPoint, Quaternion.identity);
				AnimalBotController component = gameObject.GetComponent<AnimalBotController>();
				if (component != null)
				{
					component.wanderZone = gameManager.wanderZone;
					component.additionalWanderZones = gameManager.additionalWanderZones;
					component.minInterval = gameManager._configuredAnimalSoundMin;
					component.maxInterval = gameManager._configuredAnimalSoundMax;
					component.soundIntervalSkew = gameManager._configuredAnimalSoundSkew;
				}
				NetworkServer.Spawn(gameObject);
				gameManager._spawnedAnimals.Add(gameObject);
				_003C_003E2__current = null;
				_003C_003E1__state = 1;
				return true;
			}
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

	[CompilerGenerated]
	private sealed class _003CStartGame_003Ed__79 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public GameManager _003C_003E4__this;

		private List<NetworkConnectionToClient>.Enumerator _003C_003E7__wrap1;

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
		public _003CStartGame_003Ed__79(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			int num = _003C_003E1__state;
			if (num == -3 || num == 1)
			{
				try
				{
				}
				finally
				{
					_003C_003Em__Finally1();
				}
			}
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				GameManager gameManager = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
				{
					_003C_003E1__state = -1;
					gameManager.Network_gameStarted = true;
					gameManager.GameStarted = true;
					UnityEngine.Debug.Log("[GameManager] Herkes hazır, oyun başlıyor!");
					List<NetworkConnectionToClient> list = new List<NetworkConnectionToClient>(gameManager._connections);
					_003C_003E7__wrap1 = list.GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_00d8;
				}
				case 1:
					_003C_003E1__state = -3;
					goto IL_00d8;
				case 2:
					{
						_003C_003E1__state = -1;
						gameManager.Network_hunterReleaseEndTime = -1.0;
						gameManager.SetHunterDoorOpen(open: true);
						gameManager.StartGameTimer();
						return false;
					}
					IL_00d8:
					while (_003C_003E7__wrap1.MoveNext())
					{
						NetworkConnectionToClient current = _003C_003E7__wrap1.Current;
						if (current != null && !(current.identity == null))
						{
							PlayerRoleData component = current.identity.GetComponent<PlayerRoleData>();
							if (!(component == null) && component.Role != PlayerRole.Hunter)
							{
								_003C_003E2__current = gameManager.StartCoroutine(gameManager.SpawnAnimalsForPlayer(component.AssignedAnimal));
								_003C_003E1__state = 1;
								return true;
							}
						}
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = default(List<NetworkConnectionToClient>.Enumerator);
					gameManager.Network_hunterReleaseEndTime = NetworkTime.time + (double)gameManager.hunterDoorOpenDelay;
					_003C_003E2__current = new WaitForSeconds(gameManager.hunterDoorOpenDelay);
					_003C_003E1__state = 2;
					return true;
				}
			}
			catch
			{
				//try-fault
				((IDisposable)this).Dispose();
				throw;
			}
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		private void _003C_003Em__Finally1()
		{
			_003C_003E1__state = -1;
			((IDisposable)_003C_003E7__wrap1/*cast due to .constrained prefix*/).Dispose();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}
	}

	[Header("Ayarlar")]
	public int animalsPerPlayer = 8;

	public float spawnRadius = 25f;

	public Transform spawnCenter;

	[Tooltip("Bot spawn noktası seçilirken bu yarıçap içinde katı bir engel (kaya/eşya) OLMAMALI — sadece spawn noktasının kendisi değil, çevresi de boş olsun diye agent radius'undan kasıtlı daha geniş")]
	public float spawnClearanceRadius = 1.5f;

	[Tooltip("Bot spawn noktası bir NavMeshObstacle'a bu mesafeden daha yakın OLAMAZ — obstacle'lar her zaman fiziksel bir collider taşımayabildiği için spawnClearanceRadius (Physics tabanlı) bunları yakalayamayabiliyor, bot obstacle'ın içinde/hemen yanında doğup NavMeshAgent'ı bozabiliyordu")]
	public float obstacleAvoidDistance = 5f;

	[Header("Hayvan Prefabları")]
	public AnimalPrefabEntry[] animalPrefabs;

	private readonly List<NetworkConnectionToClient> _connections = new List<NetworkConnectionToClient>();

	private readonly HashSet<NetworkConnectionToClient> _readySet = new HashSet<NetworkConnectionToClient>();

	private readonly List<GameObject> _spawnedAnimals = new List<GameObject>();

	[SyncVar]
	public bool _gameStarted;

	[SyncVar]
	public int ConfiguredHunterAmmo = 4;

	[SyncVar]
	public int ConfiguredBuzzingInterval = 30;

	[SyncVar]
	public float ConfiguredHunterPenaltySeconds = 30f;

	[SyncVar]
	public float ConfiguredMaxRecordSeconds = 3f;

	private float _configuredAnimalSoundMin = 35f;

	private float _configuredAnimalSoundMax = 90f;

	private float _configuredAnimalSoundSkew = 5f;

	[Header("Oyun Süresi")]
	[Tooltip("Toplam oyun süresi (saniye) — default 5dk")]
	public float gameDurationSeconds = 300f;

	[SyncVar(hook = "OnGameTimeChanged")]
	private double _gameEndTime = -1.0;

	private bool _gameTimerRunning;

	private bool _last30Warned;

	[Header("Oyun Sonu")]
	[Tooltip("Oyun bitince yeni round'un otomatik başlamasına kadarki geri sayım")]
	public float newGameCountdownSeconds = 10f;

	[SyncVar]
	private bool _huntersWon;

	[SyncVar(hook = "OnGameEndedChanged")]
	private bool _gameEnded;

	[SyncVar]
	private double _newGameEndTime = -1.0;

	private int _initialHunterCount;

	private int _initialAnimalCount;

	[Header("Bot Dolaşma")]
	public WanderZone wanderZone;

	[Tooltip("Eğimli/karmaşık haritalarda (ör. Forest) tek dörtgen yetmeyebilir — buraya ek WanderZone'lar eklenebilir, spawn edilen her bot'a wanderZone ile birlikte aktarılır.")]
	public WanderZone[] additionalWanderZones;

	[Header("Oyuncu Spawn (her maç başı sıfırlanır)")]
	[Tooltip("Hayvanlar (ve hunterSpawnPoint atanmamışsa hunter'lar da) her round başında bu noktanın etrafında küçük bir dairede spawn olur")]
	public Transform playerSpawnPoint;

	[Tooltip("Atanırsa hunter'lar hayvanlardan ayrı, bu noktanın etrafında spawn olur")]
	public Transform hunterSpawnPoint;

	[Tooltip("Spawn dairesinin yarıçapı (hunterSpawnBoxSize atanmamışsa hunter'lar için de bu kullanılır)")]
	public float spawnCircleRadius = 3f;

	[Tooltip("Atanırsa (sıfırdan farklıysa) hunter'lar dairesel yerine bu KUTU içinde (hunterSpawnPoint'in yerel eksenlerine göre, tam boyut — X/Y/Z) rastgele bir noktada spawn olur. Kutunun dışına asla taşmaz — Scene view'da gizmo olarak görünür.")]
	public Vector3 hunterSpawnBoxSize = Vector3.zero;

	[Tooltip("Hunter'lar kutunun X/Z kenarlarından bu kadar (metre) içeride kalacak şekilde spawn olur — kenara/duvara çok yakın doğmasınlar diye.")]
	public float hunterSpawnEdgeMargin = 0.5f;

	[Header("Hunter Kapısı")]
	[Tooltip("Avcı kulübesinin kapısı — hunter release süresi bitip oyun başlayınca açılır, yeni round başında kapanır")]
	public Transform HunterDoor;

	[Tooltip("Kapı AÇIK konumunda HunterDoor'un local position/rotation (Euler) değerleri")]
	public Vector3 HunterDoorOpenPos;

	public Vector3 HunterDoorOpenRot;

	[Tooltip("Kapı KAPALI konumunda HunterDoor'un local position/rotation (Euler) değerleri")]
	public Vector3 HunterDoorClosedPos;

	public Vector3 HunterDoorClosedRot;

	[Tooltip("Açılma/kapanma animasyon süresi (sn)")]
	public float hunterDoorMoveDuration = 1.2f;

	[Tooltip("Herkes hazır olduktan (tüm ses kayıtları tamamlandıktan) VEYA kayıt süresi dolup otomatik hazır sayıldıktan SONRA, kapı açılmadan önce beklenecek süre (sn)")]
	public float hunterDoorOpenDelay = 10f;

	[SyncVar]
	private double _hunterReleaseEndTime = -1.0;

	private Coroutine _hunterDoorRoutine;

	[SyncVar(hook = "OnHunterDoorOpenChanged")]
	private bool _hunterDoorOpen;

	private static readonly HashSet<AnimalType> ForestFeaturedAnimals;

	private static readonly HashSet<AnimalType> FarmFeaturedAnimals;

	private readonly Dictionary<int, PlayerRole> _lastRole = new Dictionary<int, PlayerRole>();

	private readonly Dictionary<int, AnimalType> _lastAnimal = new Dictionary<int, AnimalType>();

	[Header("Güvenlik Ağı")]
	[Tooltip("Rol atamasından sonra bu süre içinde TÜM oyuncular hazır olmazsa (bkz. CmdPlayerReady/_readySet), round kim hazır olursa olsun ZORLA başlatılır. RoleAssignmentUI'daki en uzun panel süresini (hunterWaitSeconds, varsayılan 60sn) rahatça aşacak kadar cömert tutulmalı.")]
	public float readyBarrierTimeoutSeconds = 90f;

	private Coroutine _readyBarrierTimeoutRoutine;

	public Action<double, double> _Mirror_SyncVarHookDelegate__gameEndTime;

	public Action<bool, bool> _Mirror_SyncVarHookDelegate__gameEnded;

	public Action<bool, bool> _Mirror_SyncVarHookDelegate__hunterDoorOpen;

	public static GameManager Instance { get; private set; }

	public bool GameStarted { get; private set; }

	public bool IsGameEnded => _gameEnded;

	public bool IsHunterDoorOpen => _hunterDoorOpen;

	public double RoundStartTime { get; private set; }

	public bool Network_gameStarted
	{
		get
		{
			return _gameStarted;
		}
		[param: In]
		set
		{
			GeneratedSyncVarSetter(value, ref _gameStarted, 1uL, null);
		}
	}

	public int NetworkConfiguredHunterAmmo
	{
		get
		{
			return ConfiguredHunterAmmo;
		}
		[param: In]
		set
		{
			GeneratedSyncVarSetter(value, ref ConfiguredHunterAmmo, 2uL, null);
		}
	}

	public int NetworkConfiguredBuzzingInterval
	{
		get
		{
			return ConfiguredBuzzingInterval;
		}
		[param: In]
		set
		{
			GeneratedSyncVarSetter(value, ref ConfiguredBuzzingInterval, 4uL, null);
		}
	}

	public float NetworkConfiguredHunterPenaltySeconds
	{
		get
		{
			return ConfiguredHunterPenaltySeconds;
		}
		[param: In]
		set
		{
			GeneratedSyncVarSetter(value, ref ConfiguredHunterPenaltySeconds, 8uL, null);
		}
	}

	public float NetworkConfiguredMaxRecordSeconds
	{
		get
		{
			return ConfiguredMaxRecordSeconds;
		}
		[param: In]
		set
		{
			GeneratedSyncVarSetter(value, ref ConfiguredMaxRecordSeconds, 16uL, null);
		}
	}

	public double Network_gameEndTime
	{
		get
		{
			return _gameEndTime;
		}
		[param: In]
		set
		{
			GeneratedSyncVarSetter(value, ref _gameEndTime, 32uL, _Mirror_SyncVarHookDelegate__gameEndTime);
		}
	}

	public bool Network_huntersWon
	{
		get
		{
			return _huntersWon;
		}
		[param: In]
		set
		{
			GeneratedSyncVarSetter(value, ref _huntersWon, 64uL, null);
		}
	}

	public bool Network_gameEnded
	{
		get
		{
			return _gameEnded;
		}
		[param: In]
		set
		{
			GeneratedSyncVarSetter(value, ref _gameEnded, 128uL, _Mirror_SyncVarHookDelegate__gameEnded);
		}
	}

	public double Network_newGameEndTime
	{
		get
		{
			return _newGameEndTime;
		}
		[param: In]
		set
		{
			GeneratedSyncVarSetter(value, ref _newGameEndTime, 256uL, null);
		}
	}

	public double Network_hunterReleaseEndTime
	{
		get
		{
			return _hunterReleaseEndTime;
		}
		[param: In]
		set
		{
			GeneratedSyncVarSetter(value, ref _hunterReleaseEndTime, 512uL, null);
		}
	}

	public bool Network_hunterDoorOpen
	{
		get
		{
			return _hunterDoorOpen;
		}
		[param: In]
		set
		{
			GeneratedSyncVarSetter(value, ref _hunterDoorOpen, 1024uL, _Mirror_SyncVarHookDelegate__hunterDoorOpen);
		}
	}

	private void Awake()
	{
		Instance = this;
	}

	public override void OnStartServer()
	{
		MyNetworkManager singleton = MyNetworkManager.Singleton;
		if (!(singleton == null))
		{
			gameDurationSeconds = singleton.GameDurationSeconds;
			animalsPerPlayer = LobbyGameSettingsUtil.GetNpcPopulationCount(singleton.NpcPopulation);
			NetworkConfiguredHunterAmmo = Mathf.Max(1, singleton.HunterAmmo);
			NetworkConfiguredBuzzingInterval = Mathf.Max(0, singleton.BuzzingInterval);
			NetworkConfiguredHunterPenaltySeconds = Mathf.Clamp(singleton.HunterPenaltySeconds, 0f, 120f);
			NetworkConfiguredMaxRecordSeconds = Mathf.Clamp(singleton.MaxRecordSeconds, 1f, 5f);
			_configuredAnimalSoundMin = Mathf.Max(1f, (float)ConfiguredBuzzingInterval - 5f);
			float animalSoundMax = LobbyGameSettingsUtil.GetAnimalSoundMax(singleton.AnimalSound);
			_configuredAnimalSoundMax = Mathf.Max(_configuredAnimalSoundMin + 5f, animalSoundMax);
			_configuredAnimalSoundSkew = LobbyGameSettingsUtil.GetAnimalSoundSkew(singleton.AnimalSound);
		}
	}

	private void OnDrawGizmosSelected()
	{
		if (!(hunterSpawnPoint == null) && !(hunterSpawnBoxSize.sqrMagnitude <= 0.0001f))
		{
			Gizmos.matrix = Matrix4x4.TRS(hunterSpawnPoint.position, hunterSpawnPoint.rotation, Vector3.one);
			Gizmos.color = new Color(1f, 0.55f, 0f, 0.25f);
			Gizmos.DrawCube(Vector3.zero, hunterSpawnBoxSize);
			Gizmos.color = new Color(1f, 0.55f, 0f, 1f);
			Gizmos.DrawWireCube(Vector3.zero, hunterSpawnBoxSize);
		}
	}

	[Server]
	private void SetHunterDoorOpen(bool open)
	{
		if (!NetworkServer.active)
		{
			UnityEngine.Debug.LogWarning("[Server] function 'System.Void GameManager::SetHunterDoorOpen(System.Boolean)' called when server was not active");
		}
		else
		{
			Network_hunterDoorOpen = open;
		}
	}

	private void OnHunterDoorOpenChanged(bool _, bool open)
	{
		if (!(HunterDoor == null))
		{
			if (_hunterDoorRoutine != null)
			{
				StopCoroutine(_hunterDoorRoutine);
			}
			_hunterDoorRoutine = StartCoroutine(AnimateHunterDoor(open));
		}
	}

	private void Start()
	{
		if (HunterDoor != null)
		{
			HunterDoor.localPosition = HunterDoorClosedPos;
			HunterDoor.localRotation = Quaternion.Euler(HunterDoorClosedRot);
		}
	}

	private IEnumerator AnimateHunterDoor(bool open)
	{
		Vector3 fromPos = HunterDoor.localPosition;
		Quaternion fromRot = HunterDoor.localRotation;
		Vector3 toPos = (open ? HunterDoorOpenPos : HunterDoorClosedPos);
		Quaternion toRot = Quaternion.Euler(open ? HunterDoorOpenRot : HunterDoorClosedRot);
		float t = 0f;
		while (t < hunterDoorMoveDuration)
		{
			t += Time.deltaTime;
			float t2 = 1f - Mathf.Pow(1f - Mathf.Clamp01(t / hunterDoorMoveDuration), 3f);
			HunterDoor.localPosition = Vector3.Lerp(fromPos, toPos, t2);
			HunterDoor.localRotation = Quaternion.Slerp(fromRot, toRot, t2);
			yield return null;
		}
		HunterDoor.localPosition = toPos;
		HunterDoor.localRotation = toRot;
		_hunterDoorRoutine = null;
	}

	private float AnimalTypeWeight(AnimalType type, AnimalType[] pool)
	{
		MyNetworkManager singleton = MyNetworkManager.Singleton;
		AnimalType[] array;
		if (singleton != null && singleton.SelectedMap == MapType.Forest)
		{
			bool flag = ForestFeaturedAnimals.Contains(type);
			int num = 0;
			int num2 = 0;
			array = pool;
			foreach (AnimalType item in array)
			{
				if (ForestFeaturedAnimals.Contains(item))
				{
					num++;
				}
				else
				{
					num2++;
				}
			}
			if (flag)
			{
				if (num <= 0)
				{
					return 0.0001f;
				}
				return 0.75f / (float)num;
			}
			if (num2 <= 0)
			{
				return 0.0001f;
			}
			return 0.25f / (float)num2;
		}
		if (FarmFeaturedAnimals.Contains(type))
		{
			return 0.1f;
		}
		if (ForestFeaturedAnimals.Contains(type))
		{
			return 0.0001f;
		}
		int num3 = 0;
		array = pool;
		foreach (AnimalType item2 in array)
		{
			if (!FarmFeaturedAnimals.Contains(item2) && !ForestFeaturedAnimals.Contains(item2))
			{
				num3++;
			}
		}
		if (num3 <= 0)
		{
			return 1f;
		}
		return 0.7f / (float)num3;
	}

	[Server]
	public void RegisterPlayer(NetworkConnectionToClient conn)
	{
		if (!NetworkServer.active)
		{
			UnityEngine.Debug.LogWarning("[Server] function 'System.Void GameManager::RegisterPlayer(Mirror.NetworkConnectionToClient)' called when server was not active");
		}
		else if (!_connections.Contains(conn))
		{
			_connections.Add(conn);
		}
	}

	[Server]
	public void UnregisterPlayer(NetworkConnectionToClient conn)
	{
		if (!NetworkServer.active)
		{
			UnityEngine.Debug.LogWarning("[Server] function 'System.Void GameManager::UnregisterPlayer(Mirror.NetworkConnectionToClient)' called when server was not active");
			return;
		}
		_connections.Remove(conn);
		_readySet.Remove(conn);
	}

	public AnimalType[] GetAvailableAnimals()
	{
		if (animalPrefabs == null)
		{
			return new AnimalType[0];
		}
		List<AnimalType> list = new List<AnimalType>();
		AnimalPrefabEntry[] array = animalPrefabs;
		foreach (AnimalPrefabEntry animalPrefabEntry in array)
		{
			if (animalPrefabEntry != null && !(animalPrefabEntry.prefab == null) && animalPrefabEntry.type != AnimalType.None && !list.Contains(animalPrefabEntry.type))
			{
				list.Add(animalPrefabEntry.type);
			}
		}
		return list.ToArray();
	}

	[Server]
	public void AssignRolesWithVolunteers(List<int> volunteerConnIds)
	{
		if (!NetworkServer.active)
		{
			UnityEngine.Debug.LogWarning("[Server] function 'System.Void GameManager::AssignRolesWithVolunteers(System.Collections.Generic.List`1<System.Int32>)' called when server was not active");
			return;
		}
		AnimalType[] available = GetAvailableAnimals();
		if (available.Length == 0)
		{
			UnityEngine.Debug.LogError("[GameManager] animalPrefabs boş! Rol atanamıyor.");
			return;
		}
		List<NetworkConnectionToClient> list = new List<NetworkConnectionToClient>();
		HashSet<int> hashSet = new HashSet<int>();
		foreach (NetworkConnectionToClient value2 in NetworkServer.connections.Values)
		{
			if (value2 != null && !(value2.identity == null) && !(value2.identity.GetComponent<PlayerRoleData>() == null) && !hashSet.Contains(value2.connectionId))
			{
				hashSet.Add(value2.connectionId);
				list.Add(value2);
			}
		}
		int count = list.Count;
		if (count == 0)
		{
			UnityEngine.Debug.LogWarning("[GameManager] Geçerli oyuncu yok, rol atanmadı.");
			return;
		}
		List<AnimalType> list2 = WeightedPickWithoutReplacement(new List<AnimalType>(available), available.Length, (AnimalType t) => AnimalTypeWeight(t, available));
		int count2 = list2.Count;
		MyNetworkManager singleton = MyNetworkManager.Singleton;
		int a = Mathf.Clamp((!(singleton != null)) ? 1 : singleton.HunterCount, 0, Mathf.Max(0, count - 1));
		if (count >= 2)
		{
			a = Mathf.Max(a, 1);
		}
		int b = Mathf.Max(0, count - count2);
		a = Mathf.Max(a, b);
		a = Mathf.Clamp(a, 0, count);
		List<NetworkConnectionToClient> list3 = new List<NetworkConnectionToClient>();
		List<NetworkConnectionToClient> list4 = new List<NetworkConnectionToClient>();
		foreach (NetworkConnectionToClient item in list)
		{
			if (volunteerConnIds != null && volunteerConnIds.Contains(item.connectionId))
			{
				list3.Add(item);
			}
			else
			{
				list4.Add(item);
			}
		}
		a = Mathf.Clamp(a, 0, Mathf.Max(0, count - 1));
		HashSet<NetworkConnectionToClient> hunters = new HashSet<NetworkConnectionToClient>(PickHuntersAvoidingRepeat(list3, a));
		if (hunters.Count < a)
		{
			List<NetworkConnectionToClient> pool = list4.FindAll((NetworkConnectionToClient c) => !hunters.Contains(c));
			foreach (NetworkConnectionToClient item2 in PickHuntersAvoidingRepeat(pool, a - hunters.Count))
			{
				hunters.Add(item2);
			}
		}
		Dictionary<NetworkConnectionToClient, AnimalType> dictionary = new Dictionary<NetworkConnectionToClient, AnimalType>();
		List<NetworkConnectionToClient> list5 = new List<NetworkConnectionToClient>();
		foreach (NetworkConnectionToClient item3 in list)
		{
			if (!hunters.Contains(item3))
			{
				list5.Add(item3);
			}
		}
		int num = Mathf.Min(list2.Count, list5.Count);
		for (int num2 = 0; num2 < num; num2++)
		{
			AnimalType animal = list2[num2];
			List<NetworkConnectionToClient> list6 = list5.FindAll((NetworkConnectionToClient c) => !_lastAnimal.TryGetValue(c.connectionId, out var value) || value != animal);
			List<NetworkConnectionToClient> source = ((list6.Count > 0) ? list6 : list5);
			List<NetworkConnectionToClient> list7 = WeightedPickWithoutReplacement(source, 1, (NetworkConnectionToClient c) => 1f);
			if (list7.Count == 0)
			{
				break;
			}
			dictionary[list7[0]] = animal;
			list5.Remove(list7[0]);
		}
		int num3 = 0;
		int num4 = 0;
		foreach (NetworkConnectionToClient item4 in list)
		{
			PlayerRoleData component = item4.identity.GetComponent<PlayerRoleData>();
			if (!(component == null))
			{
				bool flag = hunters.Contains(item4);
				if (!flag && !dictionary.ContainsKey(item4))
				{
					flag = true;
					UnityEngine.Debug.Log($"[GameManager] Hayvan bitti, {item4.connectionId} avcı yapıldı.");
				}
				AnimalType animalType = AnimalType.None;
				if (flag)
				{
					component.ServerSetRole(PlayerRole.Hunter, AnimalType.None);
					num4++;
				}
				else
				{
					animalType = dictionary[item4];
					component.ServerSetRole(PlayerRole.Animal, animalType);
					num3++;
				}
				_lastRole[item4.connectionId] = ((!flag) ? PlayerRole.Animal : PlayerRole.Hunter);
				_lastAnimal[item4.connectionId] = animalType;
			}
		}
		_initialHunterCount = num4;
		_initialAnimalCount = num3;
		UnityEngine.Debug.Log($"[GameManager] Roller atandı. Oyuncu: {count}, " + $"Avcı: {num4}, Hayvan: {num3} " + $"(benzersiz hayvan havuzu: {count2})");
		RepositionPlayersToSpawn();
		if (_readyBarrierTimeoutRoutine != null)
		{
			StopCoroutine(_readyBarrierTimeoutRoutine);
		}
		_readyBarrierTimeoutRoutine = StartCoroutine(ReadyBarrierTimeoutRoutine());
	}

	[IteratorStateMachine(typeof(_003CReadyBarrierTimeoutRoutine_003Ed__74))]
	[Server]
	private IEnumerator ReadyBarrierTimeoutRoutine()
	{
		if (!NetworkServer.active)
		{
			UnityEngine.Debug.LogWarning("[Server] function 'System.Collections.IEnumerator GameManager::ReadyBarrierTimeoutRoutine()' called when server was not active");
			return null;
		}
		return new _003CReadyBarrierTimeoutRoutine_003Ed__74(0)
		{
			_003C_003E4__this = this
		};
	}

	[Server]
	public void AssignRoleToLateJoiner(NetworkConnectionToClient conn)
	{
		if (!NetworkServer.active)
		{
			UnityEngine.Debug.LogWarning("[Server] function 'System.Void GameManager::AssignRoleToLateJoiner(Mirror.NetworkConnectionToClient)' called when server was not active");
		}
		else
		{
			if (conn == null || conn.identity == null)
			{
				return;
			}
			PlayerRoleData component = conn.identity.GetComponent<PlayerRoleData>();
			if (component == null)
			{
				return;
			}
			AnimalType[] available = GetAvailableAnimals();
			if (available.Length == 0)
			{
				return;
			}
			int num = 0;
			HashSet<AnimalType> used = new HashSet<AnimalType>();
			foreach (NetworkConnectionToClient value in NetworkServer.connections.Values)
			{
				if (value == null || value.identity == null || value == conn)
				{
					continue;
				}
				PlayerRoleData component2 = value.identity.GetComponent<PlayerRoleData>();
				if (!(component2 == null) && component2.RolesLocked)
				{
					if (component2.Role == PlayerRole.Hunter)
					{
						num++;
					}
					else
					{
						used.Add(component2.AssignedAnimal);
					}
				}
			}
			int count = _connections.Count;
			MyNetworkManager singleton = MyNetworkManager.Singleton;
			int num2 = Mathf.Clamp((!(singleton != null)) ? 1 : singleton.HunterCount, 0, Mathf.Max(0, count - 1));
			if (count >= 2)
			{
				num2 = Mathf.Max(num2, 1);
			}
			if (num < num2)
			{
				component.ServerSetRole(PlayerRole.Hunter, AnimalType.None);
				_initialHunterCount++;
				_lastRole[conn.connectionId] = PlayerRole.Hunter;
				_lastAnimal[conn.connectionId] = AnimalType.None;
				UnityEngine.Debug.Log($"[GameManager] Geç giren oyuncu avcı yapıldı (hedef: {num2}, mevcut: {num}).");
				RepositionPlayerToSpawn(conn);
				return;
			}
			List<AnimalType> list = new List<AnimalType>(available);
			list.RemoveAll((AnimalType a) => used.Contains(a));
			if (list.Count == 0)
			{
				component.ServerSetRole(PlayerRole.Hunter, AnimalType.None);
				_initialHunterCount++;
				_lastRole[conn.connectionId] = PlayerRole.Hunter;
				_lastAnimal[conn.connectionId] = AnimalType.None;
				UnityEngine.Debug.Log("[GameManager] Geç giren oyuncuya benzersiz hayvan kalmadı, avcı yapıldı.");
				RepositionPlayerToSpawn(conn);
				return;
			}
			List<AnimalType> list2 = WeightedPickWithoutReplacement(list, 1, (AnimalType t) => AnimalTypeWeight(t, available));
			AnimalType animalType = ((list2.Count > 0) ? list2[0] : list[UnityEngine.Random.Range(0, list.Count)]);
			component.ServerSetRole(PlayerRole.Animal, animalType);
			_initialAnimalCount++;
			_lastRole[conn.connectionId] = PlayerRole.Animal;
			_lastAnimal[conn.connectionId] = animalType;
			UnityEngine.Debug.Log($"[GameManager] Geç giren oyuncuya hayvan atandı: {component.AssignedAnimal}");
			RepositionPlayerToSpawn(conn);
		}
	}

	[Command(requiresAuthority = false)]
	public void CmdPlayerReady(NetworkConnectionToClient conn = null)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		SendCommandInternal("System.Void GameManager::CmdPlayerReady(Mirror.NetworkConnectionToClient)", -20711235, writer, 0, requiresAuthority: false);
		NetworkWriterPool.Return(writer);
	}

	[Server]
	private bool AllAnimalsReady()
	{
		if (!NetworkServer.active)
		{
			UnityEngine.Debug.LogWarning("[Server] function 'System.Boolean GameManager::AllAnimalsReady()' called when server was not active");
			return default(bool);
		}
		foreach (NetworkConnectionToClient connection in _connections)
		{
			if (connection != null && !(connection.identity == null))
			{
				PlayerRoleData component = connection.identity.GetComponent<PlayerRoleData>();
				if (!(component == null) && component.Role == PlayerRole.Animal && !_readySet.Contains(connection))
				{
					return false;
				}
			}
		}
		return true;
	}

	[ClientRpc]
	private void RpcForceCloseHunterPanels()
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		SendRPCInternal("System.Void GameManager::RpcForceCloseHunterPanels()", 1475478223, writer, 0, includeOwner: true);
		NetworkWriterPool.Return(writer);
	}

	[IteratorStateMachine(typeof(_003CStartGame_003Ed__79))]
	[Server]
	private IEnumerator StartGame()
	{
		if (!NetworkServer.active)
		{
			UnityEngine.Debug.LogWarning("[Server] function 'System.Collections.IEnumerator GameManager::StartGame()' called when server was not active");
			return null;
		}
		return new _003CStartGame_003Ed__79(0)
		{
			_003C_003E4__this = this
		};
	}

	[Server]
	private void StartGameTimer()
	{
		if (!NetworkServer.active)
		{
			UnityEngine.Debug.LogWarning("[Server] function 'System.Void GameManager::StartGameTimer()' called when server was not active");
			return;
		}
		Network_gameEndTime = NetworkTime.time + (double)gameDurationSeconds;
		RoundStartTime = NetworkTime.time;
		_gameTimerRunning = true;
		_last30Warned = false;
		UnityEngine.Debug.Log($"[GameManager] Oyun süresi başladı: {gameDurationSeconds}sn");
		RpcPlayGameStartedSfx();
	}

	[ClientRpc]
	private void RpcPlayGameStartedSfx()
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		SendRPCInternal("System.Void GameManager::RpcPlayGameStartedSfx()", -1574225281, writer, 0, includeOwner: true);
		NetworkWriterPool.Return(writer);
	}

	[ClientRpc]
	private void RpcPlayLast30SecondsSfx()
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		SendRPCInternal("System.Void GameManager::RpcPlayLast30SecondsSfx()", -2118239966, writer, 0, includeOwner: true);
		NetworkWriterPool.Return(writer);
	}

	[ClientRpc]
	private void RpcPlayGameOverSfx()
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		SendRPCInternal("System.Void GameManager::RpcPlayGameOverSfx()", 256315658, writer, 0, includeOwner: true);
		NetworkWriterPool.Return(writer);
	}

	[Server]
	private void AddGameTime(float seconds)
	{
		if (!NetworkServer.active)
		{
			UnityEngine.Debug.LogWarning("[Server] function 'System.Void GameManager::AddGameTime(System.Single)' called when server was not active");
		}
		else if (_gameTimerRunning && !(_gameEndTime <= 0.0))
		{
			Network_gameEndTime = _gameEndTime + (double)seconds;
			UnityEngine.Debug.Log($"[GameManager] Admin: oyun süresine {seconds}sn eklendi.");
		}
	}

	[Server]
	public void ServerAddGameTime(float seconds)
	{
		if (!NetworkServer.active)
		{
			UnityEngine.Debug.LogWarning("[Server] function 'System.Void GameManager::ServerAddGameTime(System.Single)' called when server was not active");
		}
		else
		{
			AddGameTime(seconds);
		}
	}

	private void Update()
	{
		if (base.isServer && _gameStarted && !_gameEnded)
		{
			CheckWinConditions();
		}
		if (base.isServer && _gameTimerRunning && _gameEndTime > 0.0)
		{
			double num = _gameEndTime - NetworkTime.time;
			if (!_last30Warned && num <= 30.0 && num > 0.0)
			{
				_last30Warned = true;
				RpcPlayLast30SecondsSfx();
			}
			if (num <= 0.0)
			{
				_gameTimerRunning = false;
				OnGameTimeUp();
			}
		}
		if (_gameEndTime > 0.0)
		{
			double num2 = _gameEndTime - NetworkTime.time;
			PlayerHUD.Instance?.UpdateGameCountdown(Mathf.Max(0f, (float)num2));
		}
		PlayerHUD.Instance?.SetTopCountdownVisible(_gameEndTime > 0.0);
		double num3 = ((_hunterReleaseEndTime > 0.0) ? (_hunterReleaseEndTime - NetworkTime.time) : 0.0);
		PlayerHUD.Instance?.UpdateHunterReleaseCountdown(Mathf.Max(0f, (float)num3));
		bool waitingForRecordingVisible = !_gameEnded && RoleAssignmentUI.LocalPanelClosedForRound && !IsHunterDoorOpen && _hunterReleaseEndTime <= 0.0;
		PlayerHUD.Instance?.SetWaitingForRecordingVisible(waitingForRecordingVisible);
		if (_gameEnded)
		{
			double num4 = ((_newGameEndTime > 0.0) ? (_newGameEndTime - NetworkTime.time) : 0.0);
			PlayerHUD.Instance?.ShowGameEnd(_huntersWon, Mathf.Max(0f, (float)num4));
			PlayerHUD.Instance?.SetHostReturnPromptVisible(base.isServer);
			if (base.isServer && Input.GetKeyDown(KeyCode.E))
			{
				MyNetworkManager.Singleton?.ReturnToLobby();
			}
		}
		else
		{
			PlayerHUD.Instance?.HideGameEnd();
		}
	}

	[Server]
	private void CheckWinConditions()
	{
		if (!NetworkServer.active)
		{
			UnityEngine.Debug.LogWarning("[Server] function 'System.Void GameManager::CheckWinConditions()' called when server was not active");
		}
		else
		{
			if (_connections.Count == 0)
			{
				return;
			}
			int num = 0;
			int num2 = 0;
			foreach (NetworkConnectionToClient connection in _connections)
			{
				if (connection == null || connection.identity == null)
				{
					continue;
				}
				PlayerRoleData component = connection.identity.GetComponent<PlayerRoleData>();
				if (component == null || !component.RolesLocked)
				{
					continue;
				}
				if (component.Role == PlayerRole.Hunter)
				{
					num2++;
					continue;
				}
				Health component2 = connection.identity.GetComponent<Health>();
				if (component2 == null || !component2.IsDead)
				{
					num++;
				}
			}
			if (_initialHunterCount > 0 && num2 == 0)
			{
				EndGame(huntersWon: false);
			}
			else if (_initialAnimalCount > 0 && num == 0)
			{
				EndGame(huntersWon: true);
			}
		}
	}

	[Server]
	private void OnGameTimeUp()
	{
		if (!NetworkServer.active)
		{
			UnityEngine.Debug.LogWarning("[Server] function 'System.Void GameManager::OnGameTimeUp()' called when server was not active");
			return;
		}
		UnityEngine.Debug.Log("[GameManager] Süre doldu — en az bir hayvan hayatta, hayvanlar kazandı.");
		EndGame(huntersWon: false);
	}

	[Server]
	private void EndGame(bool huntersWon)
	{
		if (!NetworkServer.active)
		{
			UnityEngine.Debug.LogWarning("[Server] function 'System.Void GameManager::EndGame(System.Boolean)' called when server was not active");
		}
		else if (!_gameEnded)
		{
			Network_gameEnded = true;
			Network_huntersWon = huntersWon;
			_gameTimerRunning = false;
			Network_gameEndTime = -1.0;
			Network_newGameEndTime = NetworkTime.time + (double)newGameCountdownSeconds;
			CheckRoundEndAchievements(huntersWon);
			VoiceNetwork.Instance?.ServerClearAllRecords();
			UnityEngine.Debug.Log("[GameManager] OYUN BİTTİ! Kazanan: " + (huntersWon ? "Avcılar" : "Hayvanlar"));
			RpcPlayGameOverSfx();
			StartCoroutine(NewGameCountdownRoutine());
		}
	}

	[Server]
	private void CheckRoundEndAchievements(bool huntersWon)
	{
		if (!NetworkServer.active)
		{
			UnityEngine.Debug.LogWarning("[Server] function 'System.Void GameManager::CheckRoundEndAchievements(System.Boolean)' called when server was not active");
			return;
		}
		if (!huntersWon)
		{
			NetworkConnectionToClient networkConnectionToClient = null;
			int num = 0;
			foreach (NetworkConnectionToClient connection in _connections)
			{
				if (connection == null || connection.identity == null)
				{
					continue;
				}
				PlayerRoleData component = connection.identity.GetComponent<PlayerRoleData>();
				if (!(component == null) && component.Role == PlayerRole.Animal)
				{
					Health component2 = connection.identity.GetComponent<Health>();
					if (!(component2 != null) || !component2.IsDead)
					{
						num++;
						networkConnectionToClient = connection;
					}
				}
			}
			if (num == 1 && networkConnectionToClient != null)
			{
				TargetUnlockAchievement(networkConnectionToClient, "ACH_ESCAPE_ARTIST");
			}
			return;
		}
		foreach (NetworkConnectionToClient connection2 in _connections)
		{
			if (connection2 == null || connection2.identity == null)
			{
				continue;
			}
			PlayerRoleData component3 = connection2.identity.GetComponent<PlayerRoleData>();
			if (!(component3 == null) && component3.Role == PlayerRole.Hunter)
			{
				HunterShotgun component4 = connection2.identity.GetComponent<HunterShotgun>();
				if (component4 != null && !component4.HadBadShotThisRound && component4.KillCount > 0)
				{
					TargetUnlockAchievement(connection2, "ACH_ONE_SHOT_ONE_COW");
				}
			}
		}
	}

	[TargetRpc]
	private void TargetUnlockAchievement(NetworkConnectionToClient target, string achievementApiName)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		writer.WriteString(achievementApiName);
		SendTargetRPCInternal(target, "System.Void GameManager::TargetUnlockAchievement(Mirror.NetworkConnectionToClient,System.String)", 1571697843, writer, 0);
		NetworkWriterPool.Return(writer);
	}

	[IteratorStateMachine(typeof(_003CNewGameCountdownRoutine_003Ed__96))]
	[Server]
	private IEnumerator NewGameCountdownRoutine()
	{
		if (!NetworkServer.active)
		{
			UnityEngine.Debug.LogWarning("[Server] function 'System.Collections.IEnumerator GameManager::NewGameCountdownRoutine()' called when server was not active");
			return null;
		}
		return new _003CNewGameCountdownRoutine_003Ed__96(0)
		{
			_003C_003E4__this = this
		};
	}

	[Server]
	private void RestartRound()
	{
		if (!NetworkServer.active)
		{
			UnityEngine.Debug.LogWarning("[Server] function 'System.Void GameManager::RestartRound()' called when server was not active");
			return;
		}
		if (_connections.Count == 0)
		{
			UnityEngine.Debug.Log("[GameManager] Kimse kalmadı, yeni round başlatılmadı.");
			return;
		}
		UnityEngine.Debug.Log("[GameManager] Yeni round başlıyor...");
		Network_gameEnded = false;
		Network_huntersWon = false;
		Network_newGameEndTime = -1.0;
		Network_gameEndTime = -1.0;
		_gameTimerRunning = false;
		_last30Warned = false;
		_readySet.Clear();
		Network_gameStarted = false;
		GameStarted = false;
		SetHunterDoorOpen(open: false);
		DespawnAllAnimals();
		ReviveAllPlayers();
		PrepareNewRound();
		AssignRolesWithVolunteers(null);
	}

	[Server]
	private void DespawnAllAnimals()
	{
		if (!NetworkServer.active)
		{
			UnityEngine.Debug.LogWarning("[Server] function 'System.Void GameManager::DespawnAllAnimals()' called when server was not active");
			return;
		}
		foreach (GameObject spawnedAnimal in _spawnedAnimals)
		{
			if (spawnedAnimal != null)
			{
				NetworkServer.Destroy(spawnedAnimal);
			}
		}
		_spawnedAnimals.Clear();
	}

	[Server]
	public void StopSoundsForAnimalType(AnimalType type)
	{
		if (!NetworkServer.active)
		{
			UnityEngine.Debug.LogWarning("[Server] function 'System.Void GameManager::StopSoundsForAnimalType(AnimalType)' called when server was not active");
			return;
		}
		foreach (GameObject spawnedAnimal in _spawnedAnimals)
		{
			if (!(spawnedAnimal == null))
			{
				AnimalBotController component = spawnedAnimal.GetComponent<AnimalBotController>();
				if (component != null && component.animalType == type)
				{
					component.ServerStopMakingSound();
				}
			}
		}
	}

	[Server]
	private void ReviveAllPlayers()
	{
		if (!NetworkServer.active)
		{
			UnityEngine.Debug.LogWarning("[Server] function 'System.Void GameManager::ReviveAllPlayers()' called when server was not active");
			return;
		}
		foreach (NetworkConnectionToClient connection in _connections)
		{
			if (connection != null && !(connection.identity == null))
			{
				connection.identity.GetComponent<Health>()?.ServerRevive();
				connection.identity.GetComponent<Health>()?.ServerResetForNewRound();
				connection.identity.GetComponent<HunterShotgun>()?.ServerResetForNewRound();
				connection.identity.GetComponent<PlayerVoiceMonitor>()?.ServerResetSoundsMade();
				connection.identity.GetComponent<PlayerVoiceMonitor>()?.ServerResetBuzzingState();
				connection.identity.GetComponent<AnimalEnergyController>()?.ServerResetForNewRound();
			}
		}
	}

	[Server]
	public void PrepareNewRound()
	{
		if (!NetworkServer.active)
		{
			UnityEngine.Debug.LogWarning("[Server] function 'System.Void GameManager::PrepareNewRound()' called when server was not active");
		}
		else
		{
			RpcShowLoading();
		}
	}

	[ClientRpc]
	private void RpcShowLoading()
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		SendRPCInternal("System.Void GameManager::RpcShowLoading()", -666149448, writer, 0, includeOwner: true);
		NetworkWriterPool.Return(writer);
	}

	[Server]
	private void RepositionPlayersToSpawn()
	{
		if (!NetworkServer.active)
		{
			UnityEngine.Debug.LogWarning("[Server] function 'System.Void GameManager::RepositionPlayersToSpawn()' called when server was not active");
			return;
		}
		if (playerSpawnPoint == null)
		{
			UnityEngine.Debug.LogError("[GameManager] playerSpawnPoint ATANMAMIŞ! Oyuncular spawn noktasına ışınlanamıyor (mevcut/boş konumlarında kalıyorlar). Inspector'dan bir Transform ata.");
		}
		foreach (NetworkConnectionToClient connection in _connections)
		{
			RepositionPlayerToSpawn(connection);
		}
	}

	[Server]
	private void RepositionPlayerToSpawn(NetworkConnectionToClient conn)
	{
		if (!NetworkServer.active)
		{
			UnityEngine.Debug.LogWarning("[Server] function 'System.Void GameManager::RepositionPlayerToSpawn(Mirror.NetworkConnectionToClient)' called when server was not active");
		}
		else
		{
			if (conn == null || conn.identity == null)
			{
				return;
			}
			NetworkTransformBase component = conn.identity.GetComponent<NetworkTransformBase>();
			if (component == null)
			{
				return;
			}
			PlayerRoleData component2 = conn.identity.GetComponent<PlayerRoleData>();
			bool flag = component2 != null && component2.Role == PlayerRole.Hunter;
			Transform transform = ((flag && hunterSpawnPoint != null) ? hunterSpawnPoint : playerSpawnPoint);
			if (transform != null)
			{
				bool flag2 = flag && transform == hunterSpawnPoint && hunterSpawnBoxSize.sqrMagnitude > 0.0001f;
				Vector3 vector = hunterSpawnBoxSize * 0.5f;
				Vector3 vector2;
				float maxDistance;
				if (flag2)
				{
					float num = Mathf.Max(0f, vector.x - hunterSpawnEdgeMargin);
					float num2 = Mathf.Max(0f, vector.z - hunterSpawnEdgeMargin);
					Vector3 position = new Vector3(UnityEngine.Random.Range(0f - num, num), 0f, UnityEngine.Random.Range(0f - num2, num2));
					vector2 = transform.TransformPoint(position);
					maxDistance = Mathf.Sqrt(vector.x * vector.x + vector.z * vector.z) + 2f;
				}
				else
				{
					Vector2 vector3 = UnityEngine.Random.insideUnitCircle * spawnCircleRadius;
					vector2 = transform.position + new Vector3(vector3.x, 0f, vector3.y);
					maxDistance = spawnCircleRadius + 2f;
				}
				Vector3 vector4 = vector2;
				NavMeshHit hit2;
				if (NavMesh.SamplePosition(vector2, out var hit, maxDistance, -1))
				{
					vector4 = hit.position;
					if (flag2)
					{
						Vector3 vector5 = transform.InverseTransformPoint(vector4);
						if (Mathf.Abs(vector5.x) > vector.x + 0.1f || Mathf.Abs(vector5.z) > vector.z + 0.1f)
						{
							vector4 = vector2;
						}
					}
				}
				else if (NavMesh.SamplePosition(transform.position, out hit2, 10f, -1))
				{
					vector4 = hit2.position;
				}
				else
				{
					UnityEngine.Debug.LogWarning("[GameManager] spawnPoint ('" + transform.name + "') yakınında NavMesh bulunamadı — ham pozisyon kullanılıyor, oyuncu boşlukta/zemin dışında kalabilir. NavMesh'in bu noktayı kapsadığından emin ol (Window > AI > Navigation > Bake).");
				}
				vector4.y = vector2.y;
				component.ServerTeleport(vector4, transform.rotation);
				StartCoroutine(ServerReconfirmTeleport(conn, component, vector4, transform.rotation));
			}
			else
			{
				Vector3 position2 = conn.identity.transform.position;
				Quaternion rotation = conn.identity.transform.rotation;
				component.ServerTeleport(position2, rotation);
				StartCoroutine(ServerReconfirmTeleport(conn, component, position2, rotation));
			}
			conn.identity.GetComponent<PlayerController>()?.ServerResetVelocity();
		}
	}

	[IteratorStateMachine(typeof(_003CServerReconfirmTeleport_003Ed__105))]
	[Server]
	private IEnumerator ServerReconfirmTeleport(NetworkConnectionToClient conn, NetworkTransformBase nt, Vector3 pos, Quaternion rot)
	{
		if (!NetworkServer.active)
		{
			UnityEngine.Debug.LogWarning("[Server] function 'System.Collections.IEnumerator GameManager::ServerReconfirmTeleport(Mirror.NetworkConnectionToClient,Mirror.NetworkTransformBase,UnityEngine.Vector3,UnityEngine.Quaternion)' called when server was not active");
			return null;
		}
		return new _003CServerReconfirmTeleport_003Ed__105(0)
		{
			conn = conn,
			nt = nt,
			pos = pos,
			rot = rot
		};
	}

	private void OnGameTimeChanged(double _, double newVal)
	{
	}

	private void OnGameEndedChanged(bool _, bool ended)
	{
		if (!ended)
		{
			return;
		}
		AchievementManager.Instance?.IncrementStatAndUnlock("Stat.MatchesPlayed", 100, "ACH_VETERAN_FARMER");
		NetworkIdentity localPlayer = NetworkClient.localPlayer;
		if (localPlayer == null)
		{
			return;
		}
		PlayerRoleData component = localPlayer.GetComponent<PlayerRoleData>();
		if (component == null)
		{
			return;
		}
		if (_huntersWon && component.Role == PlayerRole.Hunter)
		{
			AchievementManager.Instance?.Unlock("ACH_NATURAL_HUNTER");
			AchievementManager.Instance?.IncrementStatAndUnlock("Stat.HunterWins", 10, "ACH_SHERIFF_IN_TOWN");
		}
		else if (!_huntersWon && component.Role == PlayerRole.Animal)
		{
			AchievementManager.Instance?.Unlock("ACH_MASTER_OF_DISGUISE");
			AchievementManager.Instance?.IncrementStatAndUnlock("Stat.AnimalWins", 10, "ACH_UNTOUCHABLE");
			Health component2 = localPlayer.GetComponent<Health>();
			if (component2 != null && !component2.WasShotThisRound)
			{
				AchievementManager.Instance?.Unlock("ACH_MOOVE_LIKE_NOTHING_HAPPENED");
			}
		}
	}

	[IteratorStateMachine(typeof(_003CSpawnAnimalsForPlayer_003Ed__108))]
	[Server]
	private IEnumerator SpawnAnimalsForPlayer(AnimalType type)
	{
		if (!NetworkServer.active)
		{
			UnityEngine.Debug.LogWarning("[Server] function 'System.Collections.IEnumerator GameManager::SpawnAnimalsForPlayer(AnimalType)' called when server was not active");
			return null;
		}
		return new _003CSpawnAnimalsForPlayer_003Ed__108(0)
		{
			_003C_003E4__this = this,
			type = type
		};
	}

	private Vector3 GetNavMeshPoint()
	{
		Vector3 vector = ((spawnCenter != null) ? spawnCenter.position : Vector3.zero);
		for (int i = 0; i < 30; i++)
		{
			Vector2 vector2 = UnityEngine.Random.insideUnitCircle * spawnRadius;
			Vector3 vector3 = vector + new Vector3(vector2.x, 0f, vector2.y);
			if (WanderZone.TryGetTerrainHeight(vector3, out var groundY))
			{
				vector3.y = groundY;
			}
			if (NavMesh.SamplePosition(vector3, out var hit, 5f, -1))
			{
				Vector3 vector4 = WanderZone.SnapToTerrain(hit.position);
				if (!AnimalBotController.IsNearNavMeshObstacle(vector4, obstacleAvoidDistance) && AnimalBotController.IsGoodGroundPoint(vector4, spawnClearanceRadius))
				{
					return vector4;
				}
			}
		}
		Vector3 vector5 = vector;
		if (WanderZone.TryGetTerrainHeight(vector5, out var groundY2))
		{
			vector5.y = groundY2;
		}
		if (NavMesh.SamplePosition(vector5, out var hit2, 10f, -1))
		{
			return WanderZone.SnapToTerrain(hit2.position);
		}
		return vector5;
	}

	private List<NetworkConnectionToClient> PickHuntersAvoidingRepeat(List<NetworkConnectionToClient> pool, int count)
	{
		if (count <= 0 || pool == null || pool.Count == 0)
		{
			return new List<NetworkConnectionToClient>();
		}
		List<NetworkConnectionToClient> source = pool.FindAll((NetworkConnectionToClient c) => !_lastRole.TryGetValue(c.connectionId, out var value) || value != PlayerRole.Hunter);
		List<NetworkConnectionToClient> source2 = pool.FindAll((NetworkConnectionToClient c) => _lastRole.TryGetValue(c.connectionId, out var value) && value == PlayerRole.Hunter);
		List<NetworkConnectionToClient> list = new List<NetworkConnectionToClient>(WeightedPickWithoutReplacement(source, count, (NetworkConnectionToClient c) => 1f));
		if (list.Count < count)
		{
			list.AddRange(WeightedPickWithoutReplacement(source2, count - list.Count, (NetworkConnectionToClient c) => 1f));
		}
		return list;
	}

	private List<T> WeightedPickWithoutReplacement<T>(List<T> source, int count, Func<T, float> weightFn)
	{
		List<T> list = new List<T>();
		if (source == null || source.Count == 0 || count <= 0)
		{
			return list;
		}
		List<T> list2 = new List<T>(source);
		count = Mathf.Min(count, list2.Count);
		for (int i = 0; i < count; i++)
		{
			float num = 0f;
			for (int j = 0; j < list2.Count; j++)
			{
				num += Mathf.Max(0.0001f, weightFn(list2[j]));
			}
			float num2 = UnityEngine.Random.Range(0f, num);
			float num3 = 0f;
			int index = list2.Count - 1;
			for (int k = 0; k < list2.Count; k++)
			{
				num3 += Mathf.Max(0.0001f, weightFn(list2[k]));
				if (num2 <= num3)
				{
					index = k;
					break;
				}
			}
			list.Add(list2[index]);
			list2.RemoveAt(index);
		}
		return list;
	}

	public GameManager()
	{
		_Mirror_SyncVarHookDelegate__gameEndTime = OnGameTimeChanged;
		_Mirror_SyncVarHookDelegate__gameEnded = OnGameEndedChanged;
		_Mirror_SyncVarHookDelegate__hunterDoorOpen = OnHunterDoorOpenChanged;
	}

	static GameManager()
	{
		ForestFeaturedAnimals = new HashSet<AnimalType>
		{
			AnimalType.Lion,
			AnimalType.Giraffe,
			AnimalType.Gorilla,
			AnimalType.Fox,
			AnimalType.Deer,
			AnimalType.Panda,
			AnimalType.Wolf,
			AnimalType.Raccoon
		};
		FarmFeaturedAnimals = new HashSet<AnimalType>
		{
			AnimalType.Wolf,
			AnimalType.Fox,
			AnimalType.Raccoon
		};
		RemoteProcedureCalls.RegisterCommand(typeof(GameManager), "System.Void GameManager::CmdPlayerReady(Mirror.NetworkConnectionToClient)", InvokeUserCode_CmdPlayerReady__NetworkConnectionToClient, requiresAuthority: false);
		RemoteProcedureCalls.RegisterRpc(typeof(GameManager), "System.Void GameManager::RpcForceCloseHunterPanels()", InvokeUserCode_RpcForceCloseHunterPanels);
		RemoteProcedureCalls.RegisterRpc(typeof(GameManager), "System.Void GameManager::RpcPlayGameStartedSfx()", InvokeUserCode_RpcPlayGameStartedSfx);
		RemoteProcedureCalls.RegisterRpc(typeof(GameManager), "System.Void GameManager::RpcPlayLast30SecondsSfx()", InvokeUserCode_RpcPlayLast30SecondsSfx);
		RemoteProcedureCalls.RegisterRpc(typeof(GameManager), "System.Void GameManager::RpcPlayGameOverSfx()", InvokeUserCode_RpcPlayGameOverSfx);
		RemoteProcedureCalls.RegisterRpc(typeof(GameManager), "System.Void GameManager::RpcShowLoading()", InvokeUserCode_RpcShowLoading);
		RemoteProcedureCalls.RegisterRpc(typeof(GameManager), "System.Void GameManager::TargetUnlockAchievement(Mirror.NetworkConnectionToClient,System.String)", InvokeUserCode_TargetUnlockAchievement__NetworkConnectionToClient__String);
	}

	public override bool Weaved()
	{
		return true;
	}

	protected void UserCode_CmdPlayerReady__NetworkConnectionToClient(NetworkConnectionToClient conn)
	{
		if (conn != null && !_gameStarted)
		{
			_readySet.Add(conn);
			UnityEngine.Debug.Log($"[GameManager] Ready: {_readySet.Count}/{_connections.Count}");
			if (AllAnimalsReady())
			{
				RpcForceCloseHunterPanels();
			}
			if (_readySet.Count >= _connections.Count && _connections.Count > 0)
			{
				StartCoroutine(StartGame());
			}
		}
	}

	protected static void InvokeUserCode_CmdPlayerReady__NetworkConnectionToClient(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			UnityEngine.Debug.LogError("Command CmdPlayerReady called on client.");
		}
		else
		{
			((GameManager)obj).UserCode_CmdPlayerReady__NetworkConnectionToClient(senderConnection);
		}
	}

	protected void UserCode_RpcForceCloseHunterPanels()
	{
		NetworkIdentity localPlayer = NetworkClient.localPlayer;
		if (!(localPlayer == null))
		{
			PlayerRoleData component = localPlayer.GetComponent<PlayerRoleData>();
			if (!(component == null) && component.Role == PlayerRole.Hunter)
			{
				RoleAssignmentUI.Instance?.ForceCloseHunterPanel();
			}
		}
	}

	protected static void InvokeUserCode_RpcForceCloseHunterPanels(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			UnityEngine.Debug.LogError("RPC RpcForceCloseHunterPanels called on server.");
		}
		else
		{
			((GameManager)obj).UserCode_RpcForceCloseHunterPanels();
		}
	}

	protected void UserCode_RpcPlayGameStartedSfx()
	{
		GameAudioManager.Instance?.PlayGameStarted();
	}

	protected static void InvokeUserCode_RpcPlayGameStartedSfx(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			UnityEngine.Debug.LogError("RPC RpcPlayGameStartedSfx called on server.");
		}
		else
		{
			((GameManager)obj).UserCode_RpcPlayGameStartedSfx();
		}
	}

	protected void UserCode_RpcPlayLast30SecondsSfx()
	{
		GameAudioManager.Instance?.PlayLast30Seconds();
	}

	protected static void InvokeUserCode_RpcPlayLast30SecondsSfx(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			UnityEngine.Debug.LogError("RPC RpcPlayLast30SecondsSfx called on server.");
		}
		else
		{
			((GameManager)obj).UserCode_RpcPlayLast30SecondsSfx();
		}
	}

	protected void UserCode_RpcPlayGameOverSfx()
	{
		GameAudioManager.Instance?.PlayGameOver();
	}

	protected static void InvokeUserCode_RpcPlayGameOverSfx(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			UnityEngine.Debug.LogError("RPC RpcPlayGameOverSfx called on server.");
		}
		else
		{
			((GameManager)obj).UserCode_RpcPlayGameOverSfx();
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
			((GameManager)obj).UserCode_TargetUnlockAchievement__NetworkConnectionToClient__String(null, reader.ReadString());
		}
	}

	protected void UserCode_RpcShowLoading()
	{
		PlayerHUD.Instance?.ShowLoading();
	}

	protected static void InvokeUserCode_RpcShowLoading(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			UnityEngine.Debug.LogError("RPC RpcShowLoading called on server.");
		}
		else
		{
			((GameManager)obj).UserCode_RpcShowLoading();
		}
	}

	public override void SerializeSyncVars(NetworkWriter writer, bool forceAll)
	{
		base.SerializeSyncVars(writer, forceAll);
		if (forceAll)
		{
			writer.WriteBool(_gameStarted);
			writer.WriteVarInt(ConfiguredHunterAmmo);
			writer.WriteVarInt(ConfiguredBuzzingInterval);
			writer.WriteFloat(ConfiguredHunterPenaltySeconds);
			writer.WriteFloat(ConfiguredMaxRecordSeconds);
			writer.WriteDouble(_gameEndTime);
			writer.WriteBool(_huntersWon);
			writer.WriteBool(_gameEnded);
			writer.WriteDouble(_newGameEndTime);
			writer.WriteDouble(_hunterReleaseEndTime);
			writer.WriteBool(_hunterDoorOpen);
			return;
		}
		writer.WriteVarULong(syncVarDirtyBits);
		if ((syncVarDirtyBits & 1L) != 0L)
		{
			writer.WriteBool(_gameStarted);
		}
		if ((syncVarDirtyBits & 2L) != 0L)
		{
			writer.WriteVarInt(ConfiguredHunterAmmo);
		}
		if ((syncVarDirtyBits & 4L) != 0L)
		{
			writer.WriteVarInt(ConfiguredBuzzingInterval);
		}
		if ((syncVarDirtyBits & 8L) != 0L)
		{
			writer.WriteFloat(ConfiguredHunterPenaltySeconds);
		}
		if ((syncVarDirtyBits & 0x10L) != 0L)
		{
			writer.WriteFloat(ConfiguredMaxRecordSeconds);
		}
		if ((syncVarDirtyBits & 0x20L) != 0L)
		{
			writer.WriteDouble(_gameEndTime);
		}
		if ((syncVarDirtyBits & 0x40L) != 0L)
		{
			writer.WriteBool(_huntersWon);
		}
		if ((syncVarDirtyBits & 0x80L) != 0L)
		{
			writer.WriteBool(_gameEnded);
		}
		if ((syncVarDirtyBits & 0x100L) != 0L)
		{
			writer.WriteDouble(_newGameEndTime);
		}
		if ((syncVarDirtyBits & 0x200L) != 0L)
		{
			writer.WriteDouble(_hunterReleaseEndTime);
		}
		if ((syncVarDirtyBits & 0x400L) != 0L)
		{
			writer.WriteBool(_hunterDoorOpen);
		}
	}

	public override void DeserializeSyncVars(NetworkReader reader, bool initialState)
	{
		base.DeserializeSyncVars(reader, initialState);
		if (initialState)
		{
			GeneratedSyncVarDeserialize(ref _gameStarted, null, reader.ReadBool());
			GeneratedSyncVarDeserialize(ref ConfiguredHunterAmmo, null, reader.ReadVarInt());
			GeneratedSyncVarDeserialize(ref ConfiguredBuzzingInterval, null, reader.ReadVarInt());
			GeneratedSyncVarDeserialize(ref ConfiguredHunterPenaltySeconds, null, reader.ReadFloat());
			GeneratedSyncVarDeserialize(ref ConfiguredMaxRecordSeconds, null, reader.ReadFloat());
			GeneratedSyncVarDeserialize(ref _gameEndTime, _Mirror_SyncVarHookDelegate__gameEndTime, reader.ReadDouble());
			GeneratedSyncVarDeserialize(ref _huntersWon, null, reader.ReadBool());
			GeneratedSyncVarDeserialize(ref _gameEnded, _Mirror_SyncVarHookDelegate__gameEnded, reader.ReadBool());
			GeneratedSyncVarDeserialize(ref _newGameEndTime, null, reader.ReadDouble());
			GeneratedSyncVarDeserialize(ref _hunterReleaseEndTime, null, reader.ReadDouble());
			GeneratedSyncVarDeserialize(ref _hunterDoorOpen, _Mirror_SyncVarHookDelegate__hunterDoorOpen, reader.ReadBool());
			return;
		}
		long num = (long)reader.ReadVarULong();
		if ((num & 1L) != 0L)
		{
			GeneratedSyncVarDeserialize(ref _gameStarted, null, reader.ReadBool());
		}
		if ((num & 2L) != 0L)
		{
			GeneratedSyncVarDeserialize(ref ConfiguredHunterAmmo, null, reader.ReadVarInt());
		}
		if ((num & 4L) != 0L)
		{
			GeneratedSyncVarDeserialize(ref ConfiguredBuzzingInterval, null, reader.ReadVarInt());
		}
		if ((num & 8L) != 0L)
		{
			GeneratedSyncVarDeserialize(ref ConfiguredHunterPenaltySeconds, null, reader.ReadFloat());
		}
		if ((num & 0x10L) != 0L)
		{
			GeneratedSyncVarDeserialize(ref ConfiguredMaxRecordSeconds, null, reader.ReadFloat());
		}
		if ((num & 0x20L) != 0L)
		{
			GeneratedSyncVarDeserialize(ref _gameEndTime, _Mirror_SyncVarHookDelegate__gameEndTime, reader.ReadDouble());
		}
		if ((num & 0x40L) != 0L)
		{
			GeneratedSyncVarDeserialize(ref _huntersWon, null, reader.ReadBool());
		}
		if ((num & 0x80L) != 0L)
		{
			GeneratedSyncVarDeserialize(ref _gameEnded, _Mirror_SyncVarHookDelegate__gameEnded, reader.ReadBool());
		}
		if ((num & 0x100L) != 0L)
		{
			GeneratedSyncVarDeserialize(ref _newGameEndTime, null, reader.ReadDouble());
		}
		if ((num & 0x200L) != 0L)
		{
			GeneratedSyncVarDeserialize(ref _hunterReleaseEndTime, null, reader.ReadDouble());
		}
		if ((num & 0x400L) != 0L)
		{
			GeneratedSyncVarDeserialize(ref _hunterDoorOpen, _Mirror_SyncVarHookDelegate__hunterDoorOpen, reader.ReadBool());
		}
	}
}
