using System.Collections;
using System.Collections.Generic;
using Mirror;
using Mirror.RemoteCalls;
using UnityEngine;

[RequireComponent(typeof(NetworkIdentity))]
public class PoopPickup : NetworkBehaviour
{
	[Header("Yeme")]
	[Tooltip("Üstünde Eat state'inde kesintisiz bu kadar sn kalınca kaka yenmiş sayılır")]
	public float eatDuration = 1.2f;

	[Header("Optimizasyon (otomatik yok olma)")]
	[Tooltip("Game sahnesinde: üstünden geçen farklı oyuncu/bot sayısı bunu geçerse yok edilir")]
	public int maxPassCount = 15;

	[Tooltip("Lobide bunun yerine kullanılır — kakayla oynanan minigame'ler için çok daha yüksek, pratikte neredeyse hiç tetiklenmesin diye")]
	public int lobbyMaxPassCount = 250;

	[Tooltip("Game sahnesinde: kimse yemese/geçmese bile en fazla bu kadar sn sahnede kalır. Lobide HİÇ uygulanmaz (kakayla oynanan minigame'ler süresiz kalmalı)")]
	public float maxLifetime = 180f;

	private bool _isLobby;

	[Header("Ses")]
	[Tooltip("Spawn anında bu listeden rastgele biri çalınır — hem konum belli etsin diye hem de geri bildirim")]
	public AudioSource spawnSfxSource;

	public AudioClip[] spawnSfxClips;

	[Tooltip("Yenince (eatDuration doldu) çalınır")]
	public AudioClip[] eatSfxClips;

	public float minPitch = 0.95f;

	public float maxPitch = 1.05f;

	[Tooltip("Yenince mesh/collider hemen kapanır, ses bittikten sonra obje tamamen yok olsun diye bu kadar sn beklenir")]
	public float eatDestroyDelay = 3f;

	[Header("Spawn Animasyonu")]
	[Tooltip("Spawn olunca ufak bir scale pop-in (hafif taşıp yerine oturur) — basit coroutine, spawn sonrası kendini durdurur")]
	public float popDuration = 0.25f;

	private readonly HashSet<Collider> _countedPassers = new HashSet<Collider>();

	private readonly Dictionary<Collider, float> _eatingTimers = new Dictionary<Collider, float>();

	private int _passCount;

	private Vector3 _baseScale;

	private bool _eaten;

	private bool _popping;

	private MeshRenderer _meshRenderer;

	private Collider _collider;

	private void Awake()
	{
		if (spawnSfxSource == null)
		{
			spawnSfxSource = GetComponent<AudioSource>();
		}
		if (spawnSfxSource == null)
		{
			spawnSfxSource = base.gameObject.AddComponent<AudioSource>();
		}
		spawnSfxSource.playOnAwake = false;
		spawnSfxSource.spatialBlend = 1f;
		spawnSfxSource.rolloffMode = AudioRolloffMode.Linear;
		_baseScale = base.transform.localScale;
		_meshRenderer = GetComponent<MeshRenderer>();
		_collider = GetComponent<Collider>();
	}

	public override void OnStartServer()
	{
		_isLobby = MyNetworkManager.Singleton == null || !MyNetworkManager.Singleton.IsInGameScene;
		if (!_isLobby)
		{
			Invoke("ServerDespawnTimeout", maxLifetime);
		}
	}

	public override void OnStartClient()
	{
		StartCoroutine(PopIn());
		if (!(spawnSfxSource == null) && spawnSfxClips != null && spawnSfxClips.Length != 0)
		{
			AudioClip audioClip = spawnSfxClips[Random.Range(0, spawnSfxClips.Length)];
			if (!(audioClip == null))
			{
				spawnSfxSource.pitch = Random.Range(minPitch, maxPitch);
				spawnSfxSource.PlayOneShot(audioClip);
			}
		}
	}

	private IEnumerator PopIn()
	{
		base.transform.localScale = Vector3.zero;
		float t = 0f;
		while (t < popDuration)
		{
			t += Time.deltaTime;
			float num = EaseOutBack(Mathf.Clamp01(t / popDuration));
			base.transform.localScale = _baseScale * num;
			yield return null;
		}
		base.transform.localScale = _baseScale;
	}

	private static float EaseOutBack(float t)
	{
		float num = t - 1f;
		return 1f + 2.70158f * num * num * num + 1.70158f * num * num;
	}

	[ClientRpc]
	private void RpcPlayPopOut()
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		SendRPCInternal("System.Void PoopPickup::RpcPlayPopOut()", -1899947479, writer, 0, includeOwner: true);
		NetworkWriterPool.Return(writer);
	}

	private IEnumerator PopOut()
	{
		Vector3 start = base.transform.localScale;
		float t = 0f;
		while (t < popDuration)
		{
			t += Time.deltaTime;
			base.transform.localScale = Vector3.Lerp(start, Vector3.zero, Mathf.Clamp01(t / popDuration));
			yield return null;
		}
		base.transform.localScale = Vector3.zero;
	}

	[Server]
	private void ServerDespawnTimeout()
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void PoopPickup::ServerDespawnTimeout()' called when server was not active");
		}
		else if (!_eaten)
		{
			ServerDespawnWithPop();
		}
	}

	[ServerCallback]
	private void OnTriggerEnter(Collider other)
	{
		if (NetworkServer.active && !_eaten && _countedPassers.Add(other))
		{
			_passCount++;
			int num = (_isLobby ? lobbyMaxPassCount : maxPassCount);
			if (_passCount >= num)
			{
				ServerDespawnWithPop();
			}
		}
	}

	[Server]
	private void ServerDespawnWithPop()
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void PoopPickup::ServerDespawnWithPop()' called when server was not active");
		}
		else if (!_eaten && !_popping)
		{
			_popping = true;
			if (_collider != null)
			{
				_collider.enabled = false;
			}
			RpcPlayPopOut();
			Invoke("ServerFinishDestroy", popDuration);
		}
	}

	[ServerCallback]
	private void OnTriggerExit(Collider other)
	{
		if (NetworkServer.active)
		{
			_eatingTimers.Remove(other);
		}
	}

	[ServerCallback]
	private void OnTriggerStay(Collider other)
	{
		if (!NetworkServer.active || _eaten)
		{
			return;
		}
		if (!IsEating(other))
		{
			_eatingTimers.Remove(other);
			return;
		}
		float value;
		float num = (_eatingTimers.TryGetValue(other, out value) ? value : 0f) + Time.deltaTime;
		_eatingTimers[other] = num;
		if (num >= eatDuration)
		{
			ServerEaten(other);
		}
	}

	[Server]
	private void ServerEaten(Collider eater)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void PoopPickup::ServerEaten(UnityEngine.Collider)' called when server was not active");
		}
		else if (!_eaten)
		{
			_eaten = true;
			eater.GetComponentInParent<AnimalEnergyController>()?.ServerRefillFull();
			RpcPlayEatEffect();
			Invoke("ServerFinishDestroy", eatDestroyDelay);
		}
	}

	[Server]
	private void ServerFinishDestroy()
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void PoopPickup::ServerFinishDestroy()' called when server was not active");
		}
		else if (base.gameObject != null)
		{
			NetworkServer.Destroy(base.gameObject);
		}
	}

	[ClientRpc]
	private void RpcPlayEatEffect()
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		SendRPCInternal("System.Void PoopPickup::RpcPlayEatEffect()", 1666941841, writer, 0, includeOwner: true);
		NetworkWriterPool.Return(writer);
	}

	private static bool IsEating(Collider other)
	{
		AnimalBotController componentInParent = other.GetComponentInParent<AnimalBotController>();
		if (componentInParent != null)
		{
			return componentInParent.IsEating;
		}
		AnimalEatController componentInParent2 = other.GetComponentInParent<AnimalEatController>();
		if (componentInParent2 != null)
		{
			return componentInParent2.IsEating;
		}
		return false;
	}

	public override bool Weaved()
	{
		return true;
	}

	protected void UserCode_RpcPlayPopOut()
	{
		StopAllCoroutines();
		StartCoroutine(PopOut());
	}

	protected static void InvokeUserCode_RpcPlayPopOut(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("RPC RpcPlayPopOut called on server.");
		}
		else
		{
			((PoopPickup)obj).UserCode_RpcPlayPopOut();
		}
	}

	protected void UserCode_RpcPlayEatEffect()
	{
		if (_meshRenderer != null)
		{
			_meshRenderer.enabled = false;
		}
		if (_collider != null)
		{
			_collider.enabled = false;
		}
		if (!(spawnSfxSource == null) && eatSfxClips != null && eatSfxClips.Length != 0)
		{
			AudioClip audioClip = eatSfxClips[Random.Range(0, eatSfxClips.Length)];
			if (!(audioClip == null))
			{
				spawnSfxSource.pitch = Random.Range(minPitch, maxPitch);
				spawnSfxSource.PlayOneShot(audioClip);
			}
		}
	}

	protected static void InvokeUserCode_RpcPlayEatEffect(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("RPC RpcPlayEatEffect called on server.");
		}
		else
		{
			((PoopPickup)obj).UserCode_RpcPlayEatEffect();
		}
	}

	static PoopPickup()
	{
		RemoteProcedureCalls.RegisterRpc(typeof(PoopPickup), "System.Void PoopPickup::RpcPlayPopOut()", InvokeUserCode_RpcPlayPopOut);
		RemoteProcedureCalls.RegisterRpc(typeof(PoopPickup), "System.Void PoopPickup::RpcPlayEatEffect()", InvokeUserCode_RpcPlayEatEffect);
	}
}
