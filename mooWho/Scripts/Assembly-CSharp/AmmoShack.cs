using System.Collections.Generic;
using Mirror;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class AmmoShack : MonoBehaviour
{
	[Header("Referans")]
	[Tooltip("Kulübe binasındaki QuickOutline bileşeni — sadece mermisi biten avcının kendi ekranında görünür olmalı")]
	public Outline shackOutline;

	[Header("Outline Görünümü")]
	public Color outlineColor = new Color(1f, 0.8f, 0.2f);

	public float outlineWidth = 6f;

	[Header("Hayvan Engeli")]
	[Tooltip("Kulübe girişindeki katı (trigger OLMAYAN) collider objesi ('AnimalBlock') — SADECE Animal rolündeki LOCAL oyuncuda aktif tutulur, hayvanların kulübeye girmesini fiziksel olarak engeller. Hunter'da kapalıdır. Tamamen local/client-taraflı bir engel (ör. MyNetworkManager.Awake'teki Physics.IgnoreLayerCollision deseniyle aynı mantık — CharacterController çarpışması her zaman kendi client'ında işlenir) — networked bir obje DEĞİL, senkronizasyon gerekmez.")]
	public GameObject animalBlock;

	[Header("Sunucu — Geometrik Overlap Yedeği")]
	[Tooltip("BUG (bulundu — kullanıcı raporu: 'avcı ilk kulübede doğunca içeride sıkışıp mermi dolmuyor'): Mirror'ın NetworkTransform'u gelen pozisyonu transform.position'a DOĞRUDAN atar (CharacterController.Move() ÜZERİNDEN DEĞİL) — Unity'nin CharacterController trigger algılaması (OnTriggerEnter/Stay) SADECE .Move() çağrılınca güvenilir çalışır. Sunucuda, HOST OLMAYAN bir avcının CharacterController'ı hiçbir zaman .Move() ile hareket ettirilmez (PlayerController.Update() isLocalPlayer korumalı — sadece owner'ın kendi client'ında çalışır), bu yüzden sunucu tarafında o avcı için OnTriggerEnter/Stay HİÇ TETİKLENMEYEBİLİR — özellikle round başı ServerTeleport ile (yine doğrudan pozisyon ataması) kulübenin içine ışınlandığında. Bu yüzden sunucu, Unity'nin trigger sistemine hiç güvenmeden, bu aralıkla (sn) doğrudan geometrik bir overlap kontrolü de yapar — kesin/garanti çalışan yedek.")]
	public float overlapCheckInterval = 0.5f;

	private BoxCollider _box;

	private float _overlapCheckTimer;

	private readonly HashSet<HunterShotgun> _serverKnownInside = new HashSet<HunterShotgun>();

	public static bool LocalHunterInShack { get; private set; }

	private void Reset()
	{
		GetComponent<BoxCollider>().isTrigger = true;
	}

	private void Awake()
	{
		_box = GetComponent<BoxCollider>();
		if (shackOutline != null)
		{
			shackOutline.OutlineColor = outlineColor;
			shackOutline.OutlineWidth = outlineWidth;
			shackOutline.enabled = false;
		}
	}

	private void Update()
	{
		if (NetworkServer.active)
		{
			_overlapCheckTimer -= Time.deltaTime;
			if (_overlapCheckTimer <= 0f)
			{
				_overlapCheckTimer = overlapCheckInterval;
				ServerCheckOverlaps();
			}
		}
		NetworkIdentity localPlayer = NetworkClient.localPlayer;
		PlayerRoleData playerRoleData = ((localPlayer != null) ? localPlayer.GetComponent<PlayerRoleData>() : null);
		UpdateAnimalBlock(playerRoleData);
		if (shackOutline == null)
		{
			return;
		}
		if (localPlayer == null)
		{
			shackOutline.enabled = false;
			return;
		}
		HunterShotgun component = localPlayer.GetComponent<HunterShotgun>();
		Health component2 = localPlayer.GetComponent<Health>();
		bool flag = playerRoleData != null && playerRoleData.Role == PlayerRole.Hunter && component != null && component.Ammo <= 0 && (component2 == null || !component2.IsDead) && !LocalHunterInShack;
		if (shackOutline.enabled != flag)
		{
			shackOutline.enabled = flag;
		}
	}

	private void UpdateAnimalBlock(PlayerRoleData roleData)
	{
		if (!(animalBlock == null))
		{
			bool flag = !(roleData != null) || roleData.Role != PlayerRole.Hunter;
			if (animalBlock.activeSelf != flag)
			{
				animalBlock.SetActive(flag);
			}
		}
	}

	[Server]
	private void ServerCheckOverlaps()
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void AmmoShack::ServerCheckOverlaps()' called when server was not active");
			return;
		}
		Vector3 center = base.transform.TransformPoint(_box.center);
		Vector3 halfExtents = Vector3.Scale(_box.size * 0.5f, base.transform.lossyScale);
		Collider[] array = Physics.OverlapBox(center, halfExtents, base.transform.rotation, -1, QueryTriggerInteraction.Collide);
		HashSet<HunterShotgun> insideNow = new HashSet<HunterShotgun>();
		Collider[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			HunterShotgun componentInParent = array2[i].GetComponentInParent<HunterShotgun>();
			if (componentInParent != null)
			{
				insideNow.Add(componentInParent);
			}
		}
		foreach (HunterShotgun item in insideNow)
		{
			item.ServerSetInShack(inShack: true);
		}
		_serverKnownInside.RemoveWhere(delegate(HunterShotgun shotgun)
		{
			int num;
			if (shotgun != null)
			{
				num = (insideNow.Contains(shotgun) ? 1 : 0);
				if (num != 0)
				{
					goto IL_002b;
				}
			}
			else
			{
				num = 0;
			}
			if (shotgun != null)
			{
				shotgun.ServerSetInShack(inShack: false);
			}
			goto IL_002b;
			IL_002b:
			return num == 0;
		});
		_serverKnownInside.UnionWith(insideNow);
	}

	private void OnTriggerEnter(Collider other)
	{
		HunterShotgun componentInParent = other.GetComponentInParent<HunterShotgun>();
		if (!(componentInParent == null))
		{
			if (NetworkServer.active)
			{
				componentInParent.ServerSetInShack(inShack: true);
			}
			if (componentInParent.isLocalPlayer)
			{
				LocalHunterInShack = true;
			}
		}
	}

	private void OnTriggerExit(Collider other)
	{
		HunterShotgun componentInParent = other.GetComponentInParent<HunterShotgun>();
		if (!(componentInParent == null))
		{
			if (NetworkServer.active)
			{
				componentInParent.ServerSetInShack(inShack: false);
			}
			if (componentInParent.isLocalPlayer)
			{
				LocalHunterInShack = false;
			}
		}
	}

	private void OnTriggerStay(Collider other)
	{
		HunterShotgun componentInParent = other.GetComponentInParent<HunterShotgun>();
		if (!(componentInParent == null))
		{
			if (NetworkServer.active)
			{
				componentInParent.ServerSetInShack(inShack: true);
			}
			if (componentInParent.isLocalPlayer)
			{
				LocalHunterInShack = true;
			}
		}
	}
}
