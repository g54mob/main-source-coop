using System;
using Mirror;
using Mirror.RemoteCalls;
using UnityEngine;

public class AnimalPoopController : NetworkBehaviour
{
	[Header("Girdi")]
	public KeyCode poopKey = KeyCode.Mouse1;

	[Tooltip("İki kaka arasında minimum süre (sn)")]
	public float poopCooldown = 3f;

	[Header("Referanslar")]
	public PlayerRoleData roleData;

	[Tooltip("Varsayılan (ve genel oyunda kullanılan) kaka prefabı")]
	public GameObject poopPrefabBlack;

	[Tooltip("Sadece BowlWhite'tan yedikten sonra kullanılır (ileride lobi minigame'i için)")]
	public GameObject poopPrefabWhite;

	[Tooltip("Hayvan tipi listede yoksa kullanılan varsayılan yerel spawn offset'i — Y zemine raycast ile ayrıca düzeltilir")]
	public Vector3 spawnLocalOffset = new Vector3(0f, 0f, -0.8f);

	[Tooltip("Hayvan tipine göre Z offset — küçük hayvan (Chick vb.) neredeyse tam altına, büyük hayvan (Cow vb.) daha arkaya bırakır")]
	public AnimalPoopOffset[] offsetsByAnimal = new AnimalPoopOffset[22]
	{
		new AnimalPoopOffset
		{
			animal = AnimalType.Chick,
			zOffset = -0.15f
		},
		new AnimalPoopOffset
		{
			animal = AnimalType.Chicken,
			zOffset = -0.3f
		},
		new AnimalPoopOffset
		{
			animal = AnimalType.Rooster,
			zOffset = -0.3f
		},
		new AnimalPoopOffset
		{
			animal = AnimalType.Duck,
			zOffset = -0.3f
		},
		new AnimalPoopOffset
		{
			animal = AnimalType.Rabbit,
			zOffset = -0.3f
		},
		new AnimalPoopOffset
		{
			animal = AnimalType.Cat,
			zOffset = -0.3f
		},
		new AnimalPoopOffset
		{
			animal = AnimalType.Raccoon,
			zOffset = -0.45f
		},
		new AnimalPoopOffset
		{
			animal = AnimalType.Fox,
			zOffset = -0.45f
		},
		new AnimalPoopOffset
		{
			animal = AnimalType.Dog,
			zOffset = -0.5f
		},
		new AnimalPoopOffset
		{
			animal = AnimalType.Wolf,
			zOffset = -0.55f
		},
		new AnimalPoopOffset
		{
			animal = AnimalType.Pig,
			zOffset = -0.6f
		},
		new AnimalPoopOffset
		{
			animal = AnimalType.Sheep,
			zOffset = -0.6f
		},
		new AnimalPoopOffset
		{
			animal = AnimalType.Goat,
			zOffset = -0.6f
		},
		new AnimalPoopOffset
		{
			animal = AnimalType.Deer,
			zOffset = -0.6f
		},
		new AnimalPoopOffset
		{
			animal = AnimalType.Panda,
			zOffset = -0.6f
		},
		new AnimalPoopOffset
		{
			animal = AnimalType.Gorilla,
			zOffset = -0.65f
		},
		new AnimalPoopOffset
		{
			animal = AnimalType.Lion,
			zOffset = -0.75f
		},
		new AnimalPoopOffset
		{
			animal = AnimalType.Bear,
			zOffset = -0.8f
		},
		new AnimalPoopOffset
		{
			animal = AnimalType.Horse,
			zOffset = -1f
		},
		new AnimalPoopOffset
		{
			animal = AnimalType.Cow,
			zOffset = -1f
		},
		new AnimalPoopOffset
		{
			animal = AnimalType.Bull,
			zOffset = -1f
		},
		new AnimalPoopOffset
		{
			animal = AnimalType.Giraffe,
			zOffset = -1.3f
		}
	};

	[Header("Yerleşim")]
	[Tooltip("Aynı kakadan bu mesafe içinde yeni kaka spawn edilmez (iç içe girmesin diye)")]
	public float minPoopDistance = 1f;

	[Header("Kase (Bowl) — kaka rengi seçimi")]
	public string blackBowlTag = "BowlBlack";

	public string whiteBowlTag = "BowlWhite";

	[Tooltip("Kaseyle etkileşince (yerken) SADECE local'de çalınan geri bildirim sesi")]
	public AudioSource bowlEatSfxSource;

	public AudioClip[] bowlEatSfxClips;

	public float bowlEatMinPitch = 0.95f;

	public float bowlEatMaxPitch = 1.05f;

	private float _lastPoopTimeLocal = -999f;

	private float _serverLastPoopTime = -999f;

	private AnimalEatController _eatController;

	private bool _hasSetPoopColor;

	private bool _localWantsWhitePoop;

	private bool _serverWantsWhitePoop;

	private Health _health;

	private void Awake()
	{
		if (roleData == null)
		{
			roleData = GetComponent<PlayerRoleData>();
		}
		_eatController = GetComponent<AnimalEatController>();
		_health = GetComponent<Health>();
		if (bowlEatSfxSource == null)
		{
			bowlEatSfxSource = base.gameObject.AddComponent<AudioSource>();
		}
		bowlEatSfxSource.playOnAwake = false;
		bowlEatSfxSource.spatialBlend = 1f;
		bowlEatSfxSource.rolloffMode = AudioRolloffMode.Linear;
	}

	private void Update()
	{
		if (base.isLocalPlayer && !(roleData == null) && roleData.Role == PlayerRole.Animal && (!(_health != null) || !_health.IsDead) && (!(CursorManager.Instance != null) || !CursorManager.Instance.AnyUIOpen) && Input.GetKeyDown(poopKey) && Time.time - _lastPoopTimeLocal >= poopCooldown)
		{
			Vector3 position = spawnLocalOffset;
			position.z = GetZOffsetForAnimal(roleData.AssignedAnimal);
			Vector3 pos = base.transform.TransformPoint(position);
			if (TryGetGroundedPosition(pos, out var grounded))
			{
				_lastPoopTimeLocal = Time.time;
				CmdSpawnPoop(grounded, base.transform.rotation);
			}
		}
	}

	private void OnTriggerStay(Collider other)
	{
		if (!base.isLocalPlayer || _eatController == null || !_eatController.IsEating)
		{
			return;
		}
		bool flag;
		if (other.CompareTag(whiteBowlTag))
		{
			flag = true;
		}
		else
		{
			if (!other.CompareTag(blackBowlTag))
			{
				return;
			}
			flag = false;
		}
		if (!_hasSetPoopColor || _localWantsWhitePoop != flag)
		{
			_hasSetPoopColor = true;
			_localWantsWhitePoop = flag;
			CmdSetPoopColor(flag);
			PlayBowlEatSfx();
		}
	}

	private void PlayBowlEatSfx()
	{
		if (!(bowlEatSfxSource == null) && bowlEatSfxClips != null && bowlEatSfxClips.Length != 0)
		{
			AudioClip audioClip = bowlEatSfxClips[UnityEngine.Random.Range(0, bowlEatSfxClips.Length)];
			if (!(audioClip == null))
			{
				bowlEatSfxSource.pitch = UnityEngine.Random.Range(bowlEatMinPitch, bowlEatMaxPitch);
				bowlEatSfxSource.PlayOneShot(audioClip);
			}
		}
	}

	[Command]
	private void CmdSetPoopColor(bool white)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		writer.WriteBool(white);
		SendCommandInternal("System.Void AnimalPoopController::CmdSetPoopColor(System.Boolean)", 199928013, writer, 0);
		NetworkWriterPool.Return(writer);
	}

	[Command]
	private void CmdSpawnPoop(Vector3 spawnPos, Quaternion rotation)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		writer.WriteVector3(spawnPos);
		writer.WriteQuaternion(rotation);
		SendCommandInternal("System.Void AnimalPoopController::CmdSpawnPoop(UnityEngine.Vector3,UnityEngine.Quaternion)", 675203008, writer, 0);
		NetworkWriterPool.Return(writer);
	}

	[ClientRpc]
	private void RpcPlayPooEffect()
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		SendRPCInternal("System.Void AnimalPoopController::RpcPlayPooEffect()", -1413392763, writer, 0, includeOwner: true);
		NetworkWriterPool.Return(writer);
	}

	private float GetZOffsetForAnimal(AnimalType animal)
	{
		if (offsetsByAnimal != null)
		{
			AnimalPoopOffset[] array = offsetsByAnimal;
			foreach (AnimalPoopOffset animalPoopOffset in array)
			{
				if (animalPoopOffset.animal == animal)
				{
					return animalPoopOffset.zOffset;
				}
			}
		}
		return spawnLocalOffset.z;
	}

	private bool TryGetGroundedPosition(Vector3 pos, out Vector3 grounded)
	{
		grounded = pos;
		RaycastHit[] array = Physics.RaycastAll(pos + Vector3.up * 5f, Vector3.down, 20f, -1, QueryTriggerInteraction.Ignore);
		Array.Sort(array, (RaycastHit a, RaycastHit b) => a.distance.CompareTo(b.distance));
		RaycastHit[] array2 = array;
		for (int num = 0; num < array2.Length; num++)
		{
			RaycastHit raycastHit = array2[num];
			if (!raycastHit.collider.transform.IsChildOf(base.transform) && !(raycastHit.collider.transform == base.transform))
			{
				if (raycastHit.collider is TerrainCollider)
				{
					grounded.y = raycastHit.point.y;
					return true;
				}
				return false;
			}
		}
		return false;
	}

	public override bool Weaved()
	{
		return true;
	}

	protected void UserCode_CmdSetPoopColor__Boolean(bool white)
	{
		_serverWantsWhitePoop = white;
	}

	protected static void InvokeUserCode_CmdSetPoopColor__Boolean(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdSetPoopColor called on client.");
		}
		else
		{
			((AnimalPoopController)obj).UserCode_CmdSetPoopColor__Boolean(reader.ReadBool());
		}
	}

	protected void UserCode_CmdSpawnPoop__Vector3__Quaternion(Vector3 spawnPos, Quaternion rotation)
	{
		if (_health != null && _health.IsDead)
		{
			return;
		}
		GameObject gameObject = (_serverWantsWhitePoop ? poopPrefabWhite : poopPrefabBlack);
		if (gameObject == null || Time.time - _serverLastPoopTime < poopCooldown)
		{
			return;
		}
		PoopPickup[] array = UnityEngine.Object.FindObjectsOfType<PoopPickup>();
		for (int i = 0; i < array.Length; i++)
		{
			if (Vector3.Distance(array[i].transform.position, spawnPos) < minPoopDistance)
			{
				return;
			}
		}
		_serverLastPoopTime = Time.time;
		NetworkServer.Spawn(UnityEngine.Object.Instantiate(gameObject, spawnPos, rotation));
		RpcPlayPooEffect();
	}

	protected static void InvokeUserCode_CmdSpawnPoop__Vector3__Quaternion(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdSpawnPoop called on client.");
		}
		else
		{
			((AnimalPoopController)obj).UserCode_CmdSpawnPoop__Vector3__Quaternion(reader.ReadVector3(), reader.ReadQuaternion());
		}
	}

	protected void UserCode_RpcPlayPooEffect()
	{
		GetComponent<PlayerModelController>()?.PlayPooEffect();
	}

	protected static void InvokeUserCode_RpcPlayPooEffect(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("RPC RpcPlayPooEffect called on server.");
		}
		else
		{
			((AnimalPoopController)obj).UserCode_RpcPlayPooEffect();
		}
	}

	static AnimalPoopController()
	{
		RemoteProcedureCalls.RegisterCommand(typeof(AnimalPoopController), "System.Void AnimalPoopController::CmdSetPoopColor(System.Boolean)", InvokeUserCode_CmdSetPoopColor__Boolean, requiresAuthority: true);
		RemoteProcedureCalls.RegisterCommand(typeof(AnimalPoopController), "System.Void AnimalPoopController::CmdSpawnPoop(UnityEngine.Vector3,UnityEngine.Quaternion)", InvokeUserCode_CmdSpawnPoop__Vector3__Quaternion, requiresAuthority: true);
		RemoteProcedureCalls.RegisterRpc(typeof(AnimalPoopController), "System.Void AnimalPoopController::RpcPlayPooEffect()", InvokeUserCode_RpcPlayPooEffect);
	}
}
