using System;
using Mirror;
using UnityEngine;

public class PlayerModelController : MonoBehaviour
{
	[Serializable]
	public class ModelEntry
	{
		public PlayerRole role = PlayerRole.Animal;

		public AnimalType animal;

		public GameObject modelObject;
	}

	[Header("Modeller")]
	public ModelEntry[] models;

	[Header("First Person")]
	[Tooltip("Local hunter'da gizlenecek kafa (kameranın içine girmesin)")]
	public GameObject hunterHead;

	[Header("Konuşma Efekti (ağız)")]
	[Tooltip("Ses/konuşma anında ağız hizasından spawn edilecek VFX prefab — bot'lardaki AnimalBotController.talkVfxPrefab ile aynı kullanım")]
	public GameObject talkVfxPrefab;

	[Tooltip("Bulunan ağız/kafa noktasına ek yerel offset")]
	public Vector3 talkVfxLocalOffset = Vector3.zero;

	[Tooltip("Spawn edilen VFX kaç saniye sonra destroy edilir")]
	public float talkVfxLifetime = 5f;

	[Header("Kaka Efekti (kalça)")]
	[Tooltip("Poo yapılınca kalça hizasında spawn edilecek VFX prefab — talkVfxPrefab ile aynı kullanım: her poo'da Instantiate edilir, pooVfxLifetime sonra Destroy edilir. Artık oyuncuya bağlı/yapışık kalmıyor.")]
	public GameObject pooParticleEffectPrefab;

	[Tooltip("Bulunan kalça noktasına ek yerel offset")]
	public Vector3 pooEffectLocalOffset = Vector3.zero;

	[Tooltip("Spawn edilen poo VFX'i kaç saniye sonra destroy edilir")]
	public float pooVfxLifetime = 2f;

	private AnimalType _currentAnimal = (AnimalType)(-999);

	private PlayerRole _currentRole = (PlayerRole)(-999);

	private GameObject _activeModel;

	private Transform _mouthPoint;

	private Transform _hipPoint;

	public Animator ActiveAnimator { get; private set; }

	private void Start()
	{
		if (_currentRole != (PlayerRole)(-999))
		{
			return;
		}
		ModelEntry[] array = models;
		foreach (ModelEntry modelEntry in array)
		{
			if (modelEntry.modelObject != null)
			{
				modelEntry.modelObject.SetActive(value: false);
			}
		}
	}

	public void ApplyModel(PlayerRole role, AnimalType animal)
	{
		if (role == _currentRole && animal == _currentAnimal)
		{
			return;
		}
		_currentRole = role;
		_currentAnimal = animal;
		GameObject gameObject = null;
		ModelEntry[] array = models;
		foreach (ModelEntry modelEntry in array)
		{
			if (!(modelEntry.modelObject == null))
			{
				bool flag = ((role != PlayerRole.Hunter) ? (modelEntry.role == PlayerRole.Animal && modelEntry.animal == animal) : (modelEntry.role == PlayerRole.Hunter));
				modelEntry.modelObject.SetActive(flag);
				if (flag)
				{
					gameObject = modelEntry.modelObject;
				}
			}
		}
		if (gameObject == null)
		{
			Debug.LogWarning($"[PlayerModelController] Model bulunamadı: role={role}, animal={animal}");
			return;
		}
		_activeModel = gameObject;
		ActiveAnimator = gameObject.GetComponent<Animator>();
		if (ActiveAnimator == null)
		{
			ActiveAnimator = gameObject.GetComponentInChildren<Animator>(includeInactive: true);
		}
		RefreshTalkVfxAnchor();
		RefreshPooEffectAnchor();
		PlayerOutline component = GetComponent<PlayerOutline>();
		if (component != null)
		{
			component.RefreshOutline();
		}
		if (role == PlayerRole.Hunter)
		{
			HunterShotgun component2 = GetComponent<HunterShotgun>();
			if (component2 != null)
			{
				component2.ResetAimState();
			}
		}
		if (GetComponent<NetworkIdentity>().isLocalPlayer && role == PlayerRole.Hunter)
		{
			if (hunterHead != null)
			{
				hunterHead.SetActive(value: false);
			}
		}
		else if (hunterHead != null)
		{
			hunterHead.SetActive(value: true);
		}
	}

	public void SetModelVisible(bool visible)
	{
		if (_activeModel != null)
		{
			_activeModel.SetActive(visible);
		}
	}

	private void RefreshTalkVfxAnchor()
	{
		if (ActiveAnimator == null)
		{
			_mouthPoint = null;
		}
		else
		{
			_mouthPoint = MouthPointFinder.Find(ActiveAnimator.transform);
		}
	}

	public void PlayTalkVfx()
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

	private void RefreshPooEffectAnchor()
	{
		if (ActiveAnimator == null)
		{
			_hipPoint = null;
		}
		else
		{
			_hipPoint = HipPointFinder.Find(ActiveAnimator.transform);
		}
	}

	public void PlayPooEffect()
	{
		if (!(pooParticleEffectPrefab == null) && !(_hipPoint == null))
		{
			Vector3 position = _hipPoint.TransformPoint(pooEffectLocalOffset);
			GameObject obj = UnityEngine.Object.Instantiate(pooParticleEffectPrefab, position, base.transform.rotation);
			ParticleSystem component = obj.GetComponent<ParticleSystem>();
			if (component != null)
			{
				component.Play();
			}
			UnityEngine.Object.Destroy(obj, pooVfxLifetime);
		}
	}

	public AnimalType GetFirstAnimal()
	{
		if (models == null)
		{
			return AnimalType.None;
		}
		ModelEntry[] array = models;
		foreach (ModelEntry modelEntry in array)
		{
			if (!(modelEntry.modelObject == null) && modelEntry.role == PlayerRole.Animal && modelEntry.animal != AnimalType.None)
			{
				return modelEntry.animal;
			}
		}
		return AnimalType.None;
	}

	public AnimalType GetRandomAnimal()
	{
		if (models == null)
		{
			return AnimalType.None;
		}
		AnimalType result = AnimalType.None;
		int num = 0;
		ModelEntry[] array = models;
		foreach (ModelEntry modelEntry in array)
		{
			if (!(modelEntry.modelObject == null) && modelEntry.role == PlayerRole.Animal && modelEntry.animal != AnimalType.None)
			{
				num++;
				if (UnityEngine.Random.Range(0, num) == 0)
				{
					result = modelEntry.animal;
				}
			}
		}
		return result;
	}
}
