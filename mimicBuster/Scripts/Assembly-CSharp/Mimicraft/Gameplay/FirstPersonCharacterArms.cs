using System.Collections.Generic;
using Mimicraft.Customization;
using UnityEngine;

namespace Mimicraft.Gameplay
{
	public class FirstPersonCharacterArms : MonoBehaviour
	{
		[Tooltip("Oyuncunun parçalarını kemiklerine takan bileşen. Boş bırakılırsa altında aranır.")]
		[SerializeField]
		private CharacterAssembler assembler;

		[Tooltip("Birinci şahıs view model'ini oluşturan bileşen. Boş bırakılırsa altında aranır.")]
		[SerializeField]
		private PlayerWeapons weapons;

		private readonly List<GameObject> spawned = new List<GameObject>();

		private readonly List<Renderer> hidden = new List<Renderer>();

		private GameObject fpsInstance;

		private int builtFromVersion = -1;

		private CharacterAssembler builtFromAssembler;

		private bool drivenExternally;

		private GameObject externalViewModel;

		private string lastReport;

		public void SetSources(CharacterAssembler character, GameObject viewModel)
		{
			drivenExternally = true;
			assembler = character;
			externalViewModel = viewModel;
		}

		private void Awake()
		{
			if (assembler == null)
			{
				assembler = GetComponentInChildren<CharacterAssembler>(includeInactive: true);
			}
			if (weapons == null)
			{
				weapons = GetComponentInChildren<PlayerWeapons>(includeInactive: true);
			}
		}

		private void LateUpdate()
		{
			GameObject gameObject = (drivenExternally ? externalViewModel : ((weapons != null) ? weapons.FirstPersonInstance : null));
			int num = ((assembler != null) ? assembler.BuildVersion : (-1));
			if (!(gameObject == fpsInstance) || num != builtFromVersion || !(assembler == builtFromAssembler))
			{
				fpsInstance = gameObject;
				builtFromVersion = num;
				builtFromAssembler = assembler;
				Rebuild();
			}
		}

		public void Rebuild()
		{
			Clear();
			if (fpsInstance == null || assembler == null)
			{
				Report("atlandi - fps rig " + ((fpsInstance == null) ? "YOK (silah kusanilmamis)" : "var") + ", assembler " + ((assembler == null) ? "YOK" : "var") + ", weapons " + ((weapons == null) ? "YOK" : "var"));
				return;
			}
			FirstPersonArmSlots componentInChildren = fpsInstance.GetComponentInChildren<FirstPersonArmSlots>(includeInactive: true);
			if (componentInChildren == null)
			{
				Report("'" + fpsInstance.name + "' uzerinde FirstPersonArmSlots yok - o view model'e kol kurulmayacak. Prefab'a bileseni ekleyip her parca icin Mount ver.");
				return;
			}
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			int num4 = 0;
			List<string> list = new List<string>();
			foreach (CharacterAssembler.BuiltPart builtPart in assembler.BuiltParts)
			{
				if (builtPart.Definition == null)
				{
					num2++;
					continue;
				}
				if (builtPart.Model == null)
				{
					num3++;
					continue;
				}
				FirstPersonArmSlots.Slot slot = componentInChildren.Find(builtPart.Definition.PartId);
				if (slot == null)
				{
					list.Add(builtPart.Definition.PartId);
					continue;
				}
				Hide(slot.Replaces);
				if (Spawn(builtPart.Model.transform, slot.Mount))
				{
					num++;
				}
				else
				{
					num4++;
				}
			}
			Report($"'{fpsInstance.name}': {num} kol kuruldu, {hidden.Count} varsayilan renderer " + $"gizlendi, {componentInChildren.Slots.Count} slot tanimli, karakterde {assembler.BuiltParts.Count} parca." + ((list.Count > 0) ? (" Slotu olmayan parcalar: " + string.Join(", ", list) + ".") : "") + ((num2 > 0) ? $" Tanimsiz parca: {num2}." : "") + ((num3 > 0) ? $" Modeli olmayan parca: {num3}." : "") + ((num4 > 0) ? $" Kurulamayan (Mount/mesh eksik): {num4}." : ""), num == 0 && componentInChildren.Slots.Count > 0 && assembler.BuiltParts.Count > 0);
		}

		private void OnDisable()
		{
			Clear();
			fpsInstance = null;
			builtFromVersion = -1;
			builtFromAssembler = null;
		}

		private bool Spawn(Transform source, Transform mount)
		{
			MeshFilter component = source.GetComponent<MeshFilter>();
			MeshRenderer component2 = source.GetComponent<MeshRenderer>();
			if (mount == null || component == null || component2 == null || component.sharedMesh == null)
			{
				Debug.LogWarning("[FirstPersonCharacterArms] '" + source.name + "' kurulamadi - mount " + ((mount == null) ? "YOK" : mount.name) + ", MeshFilter " + ((component == null) ? "YOK" : "var") + ", MeshRenderer " + ((component2 == null) ? "YOK" : "var") + ", mesh " + ((component == null || component.sharedMesh == null) ? "YOK" : "var") + ".", this);
				return false;
			}
			GameObject gameObject = new GameObject("FpsArm_" + source.name);
			gameObject.layer = mount.gameObject.layer;
			Transform obj = gameObject.transform;
			obj.SetParent(mount, worldPositionStays: false);
			obj.localPosition = source.localPosition;
			obj.localRotation = source.localRotation;
			obj.localScale = source.localScale;
			gameObject.AddComponent<MeshFilter>().sharedMesh = component.sharedMesh;
			gameObject.AddComponent<MeshRenderer>().sharedMaterials = component2.sharedMaterials;
			spawned.Add(gameObject);
			return true;
		}

		private void Hide(Renderer[] renderers)
		{
			if (renderers == null)
			{
				return;
			}
			foreach (Renderer renderer in renderers)
			{
				if (!(renderer == null) && renderer.enabled)
				{
					renderer.enabled = false;
					hidden.Add(renderer);
				}
			}
		}

		private void Clear()
		{
			foreach (GameObject item in spawned)
			{
				if (item != null)
				{
					Object.Destroy(item);
				}
			}
			spawned.Clear();
			foreach (Renderer item2 in hidden)
			{
				if (item2 != null)
				{
					item2.enabled = true;
				}
			}
			hidden.Clear();
		}

		private void Report(string message, bool warning = false)
		{
			if (!(message == lastReport))
			{
				lastReport = message;
				if (warning)
				{
					Debug.LogWarning("[FirstPersonCharacterArms] " + message, this);
				}
				else
				{
					Debug.Log("[FirstPersonCharacterArms] " + message, this);
				}
			}
		}
	}
}
