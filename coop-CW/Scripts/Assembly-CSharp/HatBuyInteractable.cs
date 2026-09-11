using System.Linq;
using EPOOutline;
using TMPro;
using UnityEngine;
using pworld.Scripts.Extensions;

public class HatBuyInteractable : Interactable
{
	public Transform hatSpot;

	public Outlinable outlinable;

	public TextMeshPro nameText;

	public TextMeshPro priceText;

	private HatShop hatShop_gp;

	public GameObject hatPrefab;

	public Hat ihat;

	private string LOC_ALREADYOWN_TEXT;

	private string LOC_BUYHAT_TEXT;

	private string LOC_CANT_AFFORD_TEXT;

	private const string HAT_LOC_KEY = "{hatName}";

	public bool IsEmpty => ihat == null;

	public override string hoverText
	{
		get
		{
			if (!IsValid(Player.localPlayer))
			{
				return string.Empty;
			}
			if (ihat == null)
			{
				return string.Empty;
			}
			if (IsOwned)
			{
				return LOC_ALREADYOWN_TEXT.Replace("{hatName}", ihat.GetName());
			}
			if (MetaProgressionHandler.CanAffordPurchase(ihat.priceToday))
			{
				return LOC_BUYHAT_TEXT.Replace("{hatName}", ihat.GetName());
			}
			return LOC_CANT_AFFORD_TEXT.Replace("{hatName}", ihat.GetName());
		}
	}

	public bool IsOwned => MetaProgressionHandler.GetUnlockedHats().Any((int o) => o == ihat.runtimeHatIndex);

	protected override void Awake()
	{
		base.Awake();
		hatShop_gp = GetComponentInParent<HatShop>();
		outlinable = GetComponent<Outlinable>();
	}

	private void Start()
	{
		LOC_ALREADYOWN_TEXT = LocalizationKeys.GetLocalizedString(LocalizationKeys.Keys.HatShop_AlreadyOwn);
		LOC_BUYHAT_TEXT = LocalizationKeys.GetLocalizedString(LocalizationKeys.Keys.HatShop_Buy);
		LOC_CANT_AFFORD_TEXT = LocalizationKeys.GetLocalizedString(LocalizationKeys.Keys.HatShop_CantAfford);
	}

	public override bool IsValid(Player player)
	{
		return !IsEmpty;
	}

	public void LoadHat(GameObject hatPrefab, int price)
	{
		if (hatSpot == null)
		{
			Debug.LogError("hatSpot is null", base.gameObject);
		}
		this.hatPrefab = hatPrefab;
		hatSpot.KillAllChildren(destroyImmediate: true);
		ihat = HatDatabase.instance.InstantiateHat(hatPrefab);
		GameAPI.instance.objectSpawnedAction?.Invoke(ihat.gameObject);
		ihat.transform.parent = hatSpot;
		ihat.transform.localPosition = Vector3.zero;
		ihat.transform.localRotation = Quaternion.identity;
		float num = 0.3f;
		num /= ihat.transform.localScale.x;
		ihat.gameObject.AddComponent<SphereCollider>().radius = num;
		ihat.priceToday = price;
		ihat.gameObject.SetLayer(LayerMask.NameToLayer("Interactable"), includeChildren: true);
		nameText.text = ihat.GetName();
		priceText.text = ihat.priceToday + " MC";
		AddToOutlineable();
	}

	private void AddToOutlineable()
	{
		outlinable = GetComponent<Outlinable>();
		outlinable.ClearOutlineTargets();
		MeshRenderer[] componentsInChildren = hatSpot.GetComponentsInChildren<MeshRenderer>();
		foreach (MeshRenderer meshRenderer in componentsInChildren)
		{
			int subMeshCount = meshRenderer.GetComponent<MeshFilter>().sharedMesh.subMeshCount;
			for (int j = 0; j < subMeshCount; j++)
			{
				outlinable.TryAddTarget(new OutlineTarget(meshRenderer, j));
			}
		}
	}

	public void ClearHat()
	{
		if ((bool)ihat)
		{
			Object.Destroy(ihat.gameObject);
			ihat = null;
		}
	}

	public override void Interact(Player player)
	{
		Debug.Log("Interact");
		if (MetaProgressionHandler.CanAffordPurchase(ihat.priceToday) && !IsOwned)
		{
			hatShop_gp.HatBuyClicked(this);
		}
	}
}
