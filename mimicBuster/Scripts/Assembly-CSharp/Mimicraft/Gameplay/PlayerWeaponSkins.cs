using System.Collections.Generic;
using Mimicraft.Customization;
using Unity.Netcode;
using UnityEngine;

namespace Mimicraft.Gameplay
{
	public class PlayerWeaponSkins : MonoBehaviour
	{
		[Tooltip("Bu oyuncunun silahlarını oluşturan bileşen. Boş bırakılırsa altında aranır.")]
		[SerializeField]
		private PlayerWeapons weapons;

		[Tooltip("Birinci şahısta kendi gövdesini gizleyen kamera bileşeni. Boş bırakılırsa aranır.")]
		[SerializeField]
		private PlayerCameraRig cameraRig;

		private Dictionary<string, WeaponSkinData> skins;

		private GameObject appliedFps;

		private GameObject appliedTps;

		private string appliedId = "";

		private static readonly Dictionary<string, WeaponSkinData> NoSkins = new Dictionary<string, WeaponSkinData>();

		private bool resolvedOwnership;

		private NetworkObject netObject;

		private string lastReport;

		private bool appliedSkinHadVoxels;

		private void Awake()
		{
			if (weapons == null)
			{
				weapons = GetComponentInChildren<PlayerWeapons>(includeInactive: true);
			}
			if (cameraRig == null)
			{
				cameraRig = GetComponentInChildren<PlayerCameraRig>(includeInactive: true);
			}
		}

		private void OnEnable()
		{
			resolvedOwnership = false;
		}

		private void ResolveOwnership()
		{
			if (resolvedOwnership)
			{
				return;
			}
			netObject = GetComponentInParent<NetworkObject>();
			if (!(netObject != null) || netObject.IsSpawned)
			{
				resolvedOwnership = true;
				if (netObject == null || netObject.IsOwner)
				{
					skins = WeaponSkinStorage.LoadAll();
				}
			}
		}

		public WeaponSoundKind SoundFor(string weaponId)
		{
			if (skins == null || string.IsNullOrEmpty(weaponId))
			{
				return WeaponSoundKind.Normal;
			}
			if (!skins.TryGetValue(weaponId, out var value))
			{
				return WeaponSoundKind.Normal;
			}
			return value.Sound;
		}

		public void SetSkins(Dictionary<string, WeaponSkinData> incoming)
		{
			resolvedOwnership = true;
			skins = incoming ?? new Dictionary<string, WeaponSkinData>();
			appliedFps = null;
			appliedTps = null;
			appliedId = "";
		}

		public void Reload()
		{
			skins = WeaponSkinStorage.LoadAll();
			appliedFps = null;
			appliedTps = null;
			appliedId = "";
		}

		private void LateUpdate()
		{
			ResolveOwnership();
			if (weapons == null)
			{
				return;
			}
			IReadOnlyDictionary<string, WeaponSkinData> readOnlyDictionary = skins ?? NoSkins;
			GameObject firstPersonInstance = weapons.FirstPersonInstance;
			GameObject thirdPersonInstance = weapons.ThirdPersonInstance;
			string text = ((weapons.Shown != null) ? weapons.Shown.WeaponId : "");
			if (!(firstPersonInstance == appliedFps) || !(thirdPersonInstance == appliedTps) || !(text == appliedId) || WentMissing(firstPersonInstance) || WentMissing(thirdPersonInstance))
			{
				appliedFps = firstPersonInstance;
				appliedTps = thirdPersonInstance;
				appliedId = text;
				bool flag = !string.IsNullOrEmpty(text) && readOnlyDictionary.ContainsKey(text);
				Report("id='" + text + "', bilinen skin: " + ((skins == null) ? "-" : readOnlyDictionary.Count.ToString()) + ((readOnlyDictionary.Count > 0) ? (" (" + string.Join(", ", readOnlyDictionary.Keys) + ")") : "") + ", secilen: " + (flag ? "kendi modeli" : "silahin varsayilani") + ", fps=" + Describe(firstPersonInstance) + ", tps=" + Describe(thirdPersonInstance));
				WeaponSkinData value;
				WeaponSkinData weaponSkinData = ((!string.IsNullOrEmpty(text) && readOnlyDictionary.TryGetValue(text, out value)) ? value : (DefaultFor(firstPersonInstance) ?? DefaultFor(thirdPersonInstance)));
				appliedSkinHadVoxels = weaponSkinData != null && weaponSkinData.Grid != null && weaponSkinData.Grid.Count > 0;
				ApplyTo(firstPersonInstance, weaponSkinData);
				ApplyTo(thirdPersonInstance, weaponSkinData);
				if (cameraRig != null)
				{
					cameraRig.RefreshCharacterRenderers();
				}
			}
		}

		private static string Describe(GameObject instance)
		{
			if (instance == null)
			{
				return "yok";
			}
			if (!(instance.GetComponentInChildren<WeaponSkinAssembler>(includeInactive: true) != null))
			{
				return instance.name + " (assembler YOK)";
			}
			return instance.name;
		}

		private void Report(string message)
		{
			if (!(message == lastReport))
			{
				lastReport = message;
				string text = ((netObject != null && netObject.IsSpawned) ? $"owner {netObject.OwnerClientId}, yerel mi: {netObject.IsOwner}" : "aga bagli degil");
				Debug.Log("[PlayerWeaponSkins] (" + text + ") " + message, this);
			}
		}

		private bool WentMissing(GameObject instance)
		{
			if (!appliedSkinHadVoxels || instance == null)
			{
				return false;
			}
			WeaponSkinAssembler componentInChildren = instance.GetComponentInChildren<WeaponSkinAssembler>(includeInactive: true);
			if (componentInChildren != null)
			{
				return !componentInChildren.HasModel;
			}
			return false;
		}

		private static WeaponSkinData DefaultFor(GameObject instance)
		{
			if (instance == null)
			{
				return null;
			}
			WeaponSkinAssembler componentInChildren = instance.GetComponentInChildren<WeaponSkinAssembler>(includeInactive: true);
			if (!(componentInChildren != null))
			{
				return null;
			}
			return componentInChildren.DefaultSkin;
		}

		private static void ApplyTo(GameObject instance, WeaponSkinData skin)
		{
			if (!(instance == null))
			{
				WeaponSkinAssembler componentInChildren = instance.GetComponentInChildren<WeaponSkinAssembler>(includeInactive: true);
				if (componentInChildren != null)
				{
					componentInChildren.Apply(skin);
				}
			}
		}
	}
}
