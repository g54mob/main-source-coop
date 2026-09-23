using System.Collections.Generic;
using Mimicraft.Gameplay;
using Mimicraft.VoxelEditor;
using Mimicraft.VoxelEditor.Core;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Mimicraft.Customization
{
	public class CharacterAssembler : MonoBehaviour
	{
		public readonly struct BuiltPart
		{
			public readonly CharacterPartDefinition Definition;

			public readonly VoxelModel Model;

			public BuiltPart(CharacterPartDefinition definition, VoxelModel model)
			{
				Definition = definition;
				Model = model;
			}
		}

		[Tooltip("Bu karakterin parça listesi.")]
		[SerializeField]
		private CharacterRigDefinition rig;

		[Tooltip("Kemikleri sağlayan Animator. Boş bırakılırsa bu objede ve altında aranır. Humanoid olmalı - parçalar kemikleri isimle değil HumanBodyBones ile buluyor.")]
		[SerializeField]
		private Animator animator;

		[Tooltip("Voxel parçalarının materyalleri - VoxelModel'in Unlit/Lit çifti.")]
		[SerializeField]
		private Material unlitMaterial;

		[SerializeField]
		private Material litMaterial;

		[Tooltip("Parçalar kurulduğunda gizlenecek varsayılan görünüm. Boş bırakılırsa bu objenin altındaki, parçalara ait OLMAYAN bütün renderer'lar bulunur.")]
		[SerializeField]
		private Renderer[] defaultRenderers;

		[Tooltip("Açıkken, verisi olmayan parçalar dolu bir blok olarak kurulur - zorunlu çekirdek tanımlıysa o kadarı, değilse kutunun tamamı. Yontulacak kil. Kapatınca verisi olmayan parça hiç oluşturulmaz ve o slot boş kalır.")]
		[SerializeField]
		private bool buildMissingParts = true;

		[Tooltip("Açıkken her parça, mevcut karakter mesh'inin O KEMİK etrafındaki kutusunun tam ortasına yerleşir ve Anchor Offset bunun üstüne eklenir - yani offset sıfırdan bir konum değil, küçük bir düzeltme olur. Kemiğin altındaki MeshRenderer'lar ölçülür; skinned bir modelde ise bone weight'ler kullanılır (o durumda Read/Write Enabled gerekir). Ölçülecek bir şey bulunamazsa yerleşim yalnızca Anchor Offset'e kalır ve konsola bir kez yazılır.")]
		[SerializeField]
		private bool centreOnBoneMesh = true;

		[Header("Düzenleme gizmo'ları")]
		[Tooltip("Parçalar düzenlenirken kullanılacak gizmo çarpanları - VoxelEditorController'ın kendi Inspector alanlarının aynısı, parçalar çalışma anında oluştuğu için buradan veriliyor.\n\nVoxelSize'dan BAĞIMSIZ: editör okları dünya ölçeğinde çiziyor (parent scale'i sceneCompensation ile iptal ediliyor), yani 1 yazmak voxel boyu ne olursa olsun aynı büyüklüğü verir.")]
		[SerializeField]
		[Min(0.01f)]
		private float gizmoSize = 1f;

		[Tooltip("Extrude okunun uzunluk çarpanı. Ayrı tutuluyor çünkü o okun boyu modelin kutusuna değil çekilen adım sayısına bağlı.")]
		[SerializeField]
		[Min(0.01f)]
		private float extrudeGizmoSize = 1f;

		[Tooltip("Ok gövdesi/başı kalınlığı, uzunluktan bağımsız.")]
		[SerializeField]
		[Min(0.01f)]
		private float gizmoThickness = 1f;

		[Tooltip("Merkezdeki sarı işaretçi küresinin büyüklüğü. Diğer gizmo'lardan ayrı: dünya çapı bir Modelci modeline göre yazılmış, karakter parçasında çok daha küçük olması gerekiyor.")]
		[SerializeField]
		[Min(0.01f)]
		private float centerMarkerSize = 1f;

		[Tooltip("Açarsan parçalar Transform aracıyla taşınıp döndürülebilir. Kapalı olması gerekiyor: parçanın yeri kemikten geliyor, kaydırılabilen bir uzuv özelleştirilmiş değil bozulmuş bir karakter demek. Ayrıca 14 parçanın taşıma okları üst üste bindiği için odak sürekli iki parça arasında gidip geliyor.")]
		[SerializeField]
		private bool allowPartTransform;

		[Tooltip("Sahne açılır açılmaz giydirilecek karakter - projeye attığın bir .character dosyası. Oyun içi yollarda kullanılmaz; bir sanat/test sahnesinde karakteri elle Apply etmeden görebilmek için. Boş bırakılırsa hiçbir şey uygulanmaz ve gövde prefabındaki hâliyle kalır.")]
		[SerializeField]
		private CharacterAsset applyOnStart;

		private readonly List<BuiltPart> built = new List<BuiltPart>();

		private bool warnedAboutEmptyBuild;

		private bool warnedAboutRig;

		private readonly HashSet<string> hiddenParts = new HashSet<string>();

		private bool editable;

		private const int MaxHeals = 4;

		private const float HealBudgetResetSeconds = 60f;

		private CharacterData lastApplied;

		private int heals;

		private float lastHealTime;

		private bool gaveUpHealing;

		private bool applyWhenActive;

		private int missingBonesThisApply;

		private readonly Dictionary<HumanBodyBones, Transform> boneCache = new Dictionary<HumanBodyBones, Transform>();

		private BoneMeshBounds boneBoundsTable;

		private bool reportedBounds;

		private bool reportingThisApply;

		private bool warnedAboutReferences;

		private readonly List<string> derivedSizes = new List<string>();

		public bool HasCustomParts => built.Count > 0;

		public bool HasLiveParts
		{
			get
			{
				if (built.Count > 0)
				{
					return built[0].Model != null;
				}
				return false;
			}
		}

		public IReadOnlyList<BuiltPart> BuiltParts => built;

		public CharacterRigDefinition Rig => rig;

		public CharacterData DefaultCharacterData
		{
			get
			{
				if (!(rig != null) || !(rig.DefaultCharacter != null))
				{
					return null;
				}
				return rig.DefaultCharacter.ToCharacterData(rig);
			}
		}

		public int BuildVersion { get; private set; }

		public void SetEditable(bool editable, Camera editorCamera)
		{
			this.editable = editable;
			foreach (BuiltPart item in built)
			{
				if (item.Model == null)
				{
					continue;
				}
				if (editable)
				{
					DetachFromRagdollBody(item.Model.gameObject);
				}
				MeshCollider component = item.Model.GetComponent<MeshCollider>();
				if (component != null)
				{
					component.enabled = editable;
					if (editable)
					{
						item.Model.RefreshCollider();
					}
				}
				if (!editable)
				{
					ReattachToRagdollBody(item.Model.gameObject);
				}
				VoxelEditorController voxelEditorController = item.Model.GetComponent<VoxelEditorController>();
				if (editable)
				{
					if (voxelEditorController == null)
					{
						voxelEditorController = item.Model.gameObject.AddComponent<VoxelEditorController>();
					}
					voxelEditorController.SetCamera(editorCamera);
					voxelEditorController.enabled = true;
					voxelEditorController.AllowTransform = allowPartTransform;
					voxelEditorController.GizmoSize = gizmoSize;
					voxelEditorController.ExtrudeGizmoSize = extrudeGizmoSize;
					voxelEditorController.GizmoThickness = gizmoThickness;
					voxelEditorController.CenterMarkerSize = centerMarkerSize;
					item.Model.EditBounds = new PartEditBounds(item.Definition, item.Definition.PartId);
				}
				else if (voxelEditorController != null)
				{
					voxelEditorController.enabled = false;
					item.Model.EditBounds = null;
				}
			}
			ApplyPartVisibility();
		}

		private static void DetachFromRagdollBody(GameObject part)
		{
			if (!(part.transform.parent == null) && !(part.transform.parent.GetComponentInParent<Rigidbody>() == null))
			{
				Rigidbody rigidbody = part.GetComponent<Rigidbody>();
				if (rigidbody == null)
				{
					rigidbody = part.AddComponent<Rigidbody>();
				}
				rigidbody.isKinematic = true;
				rigidbody.useGravity = false;
			}
		}

		private static void ReattachToRagdollBody(GameObject part)
		{
			Rigidbody component = part.GetComponent<Rigidbody>();
			if (component != null)
			{
				Object.Destroy(component);
			}
		}

		public bool IsPartVisible(string partId)
		{
			return !hiddenParts.Contains(partId);
		}

		public void SetPartVisible(string partId, bool visible)
		{
			if (!string.IsNullOrEmpty(partId))
			{
				if (visible)
				{
					hiddenParts.Remove(partId);
				}
				else
				{
					hiddenParts.Add(partId);
				}
				ApplyPartVisibility();
			}
		}

		private void ApplyPartVisibility()
		{
			foreach (BuiltPart item in built)
			{
				if (!(item.Model == null) && !(item.Definition == null))
				{
					bool flag = IsPartVisible(item.Definition.PartId);
					MeshRenderer component = item.Model.GetComponent<MeshRenderer>();
					if (component != null)
					{
						component.enabled = flag;
					}
					MeshCollider component2 = item.Model.GetComponent<MeshCollider>();
					if (component2 != null && !flag)
					{
						component2.enabled = false;
					}
					else if (component2 != null && editable)
					{
						component2.enabled = true;
					}
					VoxelEditorController component3 = item.Model.GetComponent<VoxelEditorController>();
					if (component3 != null)
					{
						component3.enabled = editable && flag;
					}
				}
			}
		}

		public CharacterData Capture()
		{
			CharacterData characterData = new CharacterData((rig != null) ? rig.RigId : "");
			foreach (BuiltPart item in built)
			{
				if (item.Model != null && item.Definition != null)
				{
					characterData.Set(item.Definition.PartId, item.Model.Grid);
				}
			}
			return characterData;
		}

		private void Awake()
		{
			if (animator == null)
			{
				animator = GetComponentInChildren<Animator>(includeInactive: true);
			}
			if (defaultRenderers == null || defaultRenderers.Length == 0)
			{
				defaultRenderers = GetComponentsInChildren<Renderer>(includeInactive: true);
			}
		}

		[ContextMenu("Apply Start Character")]
		private void Start()
		{
			if (applyOnStart != null)
			{
				Apply(applyOnStart.ToCharacterData(rig));
			}
		}

		public void Apply(CharacterData data)
		{
			lastApplied = data;
			Clear();
			BuildVersion++;
			if (rig == null || animator == null)
			{
				if (!warnedAboutReferences)
				{
					warnedAboutReferences = true;
					Debug.LogWarning("[CharacterAssembler] Karakter kurulamiyor - Rig=" + ((rig != null) ? rig.name : "BOS") + ", Animator=" + ((animator != null) ? animator.name : "BOS") + ". Ikisi de dolu olmali; parcalar kemikleri Animator uzerinden buluyor ve hangi parcalarin olduğunu Rig soyluyor. Bu objedeki CharacterAssembler'in bu iki alanini doldur.", this);
				}
				SetDefaultRenderersVisible(visible: true);
				return;
			}
			if (!animator.isHuman)
			{
				if (!warnedAboutRig)
				{
					warnedAboutRig = true;
					Debug.LogWarning("[CharacterAssembler] '" + animator.name + "' uzerindeki Animator humanoid degil - Avatar atanmamis ya da Avatar'i Generic. Parcalar kemikleri HumanBodyBones ile buluyor, yani hicbiri kurulamaz. Modelin Import Settings > Rig > Animation Type'ini Humanoid yap ve olusan Avatar'i bu Animator'e ata.", this);
				}
				SetDefaultRenderersVisible(visible: true);
				return;
			}
			MeasureBones();
			missingBonesThisApply = 0;
			foreach (CharacterPartDefinition part in rig.Parts)
			{
				VoxelGrid voxelGrid = ResolveGrid(part, data);
				if (voxelGrid != null && voxelGrid.Count != 0)
				{
					VoxelModel voxelModel = BuildPart(part, voxelGrid);
					if (voxelModel != null)
					{
						built.Add(new BuiltPart(part, voxelModel));
					}
				}
			}
			ReportDerivedSizes();
			if (missingBonesThisApply > 0 && !animator.gameObject.activeInHierarchy)
			{
				applyWhenActive = true;
				SetDefaultRenderersVisible(built.Count == 0);
				NotifyCameraRig();
				return;
			}
			ApplyPartVisibility();
			SetDefaultRenderersVisible(built.Count == 0);
			NotifyCameraRig();
			if (built.Count == 0 && rig.Parts.Count > 0 && !warnedAboutEmptyBuild)
			{
				warnedAboutEmptyBuild = true;
				Debug.LogWarning($"[CharacterAssembler] '{rig.name}' icindeki {rig.Parts.Count} parcanin " + "hicbiri kurulamadi. Kayitli karakter yoksa Build Missing Parts acik mi, ve Animator humanoid mi (parcalar kemikleri HumanBodyBones ile buluyor)?", this);
			}
		}

		private void LateUpdate()
		{
			if (applyWhenActive && animator != null && animator.gameObject.activeInHierarchy)
			{
				applyWhenActive = false;
				Apply(lastApplied);
			}
			else if (built.Count == 0 || built[0].Model != null)
			{
				if (heals > 0 && Time.unscaledTime - lastHealTime > 60f)
				{
					heals = 0;
				}
			}
			else if (heals >= 4)
			{
				if (!gaveUpHealing)
				{
					gaveUpHealing = true;
					Debug.LogError($"[CharacterAssembler] Parcalar {4} kez yeniden kuruldu ve her " + "seferinde yok edildi - vazgeciliyor. Karakter varsayilan gorunumde kalacak.", this);
					Clear();
				}
			}
			else
			{
				heals++;
				lastHealTime = Time.unscaledTime;
				Debug.LogWarning($"[CharacterAssembler] Parcalar disaridan yok edilmis ({built.Count} adet, " + $"frame {Time.frameCount}, sahne '{base.gameObject.scene.name}') - karakter yeniden " + $"kuruluyor ({heals}/{4}). Muhtemel sebep: parcalarin olusturuldugu sahne " + "bosaltildi, ya da oyuncunun altindaki VoxelModel'leri toplu silen bir kod.", this);
				Apply(lastApplied);
			}
		}

		public void Clear()
		{
			foreach (BuiltPart item in built)
			{
				if (item.Model != null)
				{
					Object.Destroy(item.Model.gameObject);
				}
			}
			built.Clear();
			BuildVersion++;
			SetDefaultRenderersVisible(visible: true);
			NotifyCameraRig();
		}

		private VoxelGrid ResolveGrid(CharacterPartDefinition part, CharacterData data)
		{
			CharacterPartData characterPartData = data?.Find(part.PartId);
			if (characterPartData?.Grid != null && characterPartData.Grid.Count > 0)
			{
				return characterPartData.Grid;
			}
			if (!buildMissingParts)
			{
				return null;
			}
			VoxelGrid voxelGrid = new VoxelGrid();
			Vector3Int vector3Int = (part.HasRequiredCore ? part.RequiredMin : Vector3Int.zero);
			Vector3Int vector3Int2 = (part.HasRequiredCore ? (vector3Int + part.RequiredSize) : part.BoxSize);
			Color32 color = new Color32(200, 200, 205, byte.MaxValue);
			for (int i = vector3Int.x; i < vector3Int2.x; i++)
			{
				for (int j = vector3Int.y; j < vector3Int2.y; j++)
				{
					for (int k = vector3Int.z; k < vector3Int2.z; k++)
					{
						voxelGrid.Set(new Vector3Int(i, j, k), new VoxelData(color));
					}
				}
			}
			return voxelGrid;
		}

		private Transform ResolveBone(HumanBodyBones bone)
		{
			if (boneCache.TryGetValue(bone, out var value) && value != null)
			{
				return value;
			}
			Transform transform = ((animator != null && animator.isHuman) ? animator.GetBoneTransform(bone) : null);
			if (transform != null)
			{
				boneCache[bone] = transform;
			}
			return transform;
		}

		private VoxelModel BuildPart(CharacterPartDefinition part, VoxelGrid grid)
		{
			Transform transform = ResolveBone(part.Bone);
			if (transform == null)
			{
				missingBonesThisApply++;
				if (animator != null && !animator.gameObject.activeInHierarchy)
				{
					return null;
				}
				Debug.LogWarning($"[CharacterAssembler] '{part.DisplayName}' icin {part.Bone} kemigi " + "bulunamadi - Animator humanoid mi? Bu parca cizilmeyecek.", this);
				return null;
			}
			float num = ResolveVoxelSize(part, transform);
			GameObject gameObject = new GameObject("Part_" + part.PartId);
			gameObject.layer = base.gameObject.layer;
			Scene scene = transform.gameObject.scene;
			if (scene.IsValid() && gameObject.scene != scene)
			{
				SceneManager.MoveGameObjectToScene(gameObject, scene);
			}
			Transform transform2 = gameObject.transform;
			transform2.SetParent(transform, worldPositionStays: false);
			transform2.localRotation = part.AnchorRotation;
			transform2.localPosition = ResolveAnchor(part, transform, num, transform2.localRotation);
			gameObject.AddComponent<MeshFilter>();
			gameObject.AddComponent<MeshRenderer>();
			gameObject.AddComponent<MeshCollider>().enabled = false;
			VoxelModel voxelModel = gameObject.AddComponent<VoxelModel>();
			voxelModel.ConfigureVoxelSize(num);
			voxelModel.ConfigureMaterials(unlitMaterial, litMaterial);
			voxelModel.SetUnlit(unlit: false);
			voxelModel.Grid.Clear();
			foreach (KeyValuePair<Vector3Int, VoxelData> voxel in grid.Voxels)
			{
				voxelModel.Grid.Set(voxel.Key, voxel.Value);
			}
			foreach (KeyValuePair<(Vector3Int, int), Color32> faceColor in grid.FaceColors)
			{
				voxelModel.Grid.SetFaceColor(faceColor.Key.Item1, faceColor.Key.Item2, faceColor.Value);
			}
			voxelModel.RebuildMesh(updateCollider: false);
			return voxelModel;
		}

		private Vector3 ResolveAnchor(CharacterPartDefinition part, Transform bone, float voxelSize, Quaternion rotation)
		{
			if (!centreOnBoneMesh || boneBoundsTable == null)
			{
				return part.AnchorOffset;
			}
			if (!boneBoundsTable.TryGet(bone, out var box))
			{
				return part.AnchorOffset;
			}
			Vector3 vector = rotation * ((Vector3)part.BoxSize * voxelSize * 0.5f);
			return box.Center - vector + part.AnchorOffset;
		}

		private float ResolveVoxelSize(CharacterPartDefinition part, Transform bone)
		{
			if (part.OverrideVoxelSize || !centreOnBoneMesh || boneBoundsTable == null)
			{
				return part.VoxelSize;
			}
			if (!boneBoundsTable.TryGet(bone, out var box))
			{
				return part.VoxelSize;
			}
			Vector3 axisAlignedSize = box.AxisAlignedSize;
			Vector3Int boxSize = part.BoxSize;
			float num = Mathf.Min(axisAlignedSize.x / (float)Mathf.Max(boxSize.x, 1), axisAlignedSize.y / (float)Mathf.Max(boxSize.y, 1), axisAlignedSize.z / (float)Mathf.Max(boxSize.z, 1));
			if (num <= 0.0001f)
			{
				return part.VoxelSize;
			}
			if (reportingThisApply)
			{
				Vector3 vector = boxSize;
				derivedSizes.Add($"{part.DisplayName}: kutu {axisAlignedSize.x:0.###}x{axisAlignedSize.y:0.###}x{axisAlignedSize.z:0.###}m " + $"/ {vector.x:0}x{vector.y:0}x{vector.z:0} voxel -> {num:0.####}" + (box.Oriented ? "" : " [zarf]"));
			}
			return num;
		}

		private void MeasureBones()
		{
			if (!centreOnBoneMesh)
			{
				boneBoundsTable = null;
			}
			else
			{
				if (boneBoundsTable != null && boneBoundsTable.MeasuredBoneCount > 0)
				{
					return;
				}
				List<Transform> list = new List<Transform>();
				if (animator.isHuman)
				{
					foreach (CharacterPartDefinition part in rig.Parts)
					{
						Transform transform = ResolveBone(part.Bone);
						if (transform != null)
						{
							list.Add(transform);
						}
					}
				}
				boneBoundsTable = BoneMeshBounds.Measure(base.gameObject, list, (Renderer renderer) => renderer.GetComponentInParent<VoxelModel>() != null);
				if (!reportedBounds)
				{
					reportedBounds = true;
					reportingThisApply = true;
					if (boneBoundsTable.MeasuredBoneCount == 0)
					{
						Debug.LogWarning("[CharacterAssembler] Centre On Bone Mesh acik ama olculecek mesh " + $"bulunamadi ({boneBoundsTable.RendererCount} renderer tarandi, 0 kemik olculdu). " + "Parcalar sadece Anchor Offset ile yerlesecek. Karakterin mesh'leri kemiklerin ALTINDA mi duruyor?", this);
					}
					else
					{
						Debug.Log($"[CharacterAssembler] {boneBoundsTable.RendererCount} renderer tarandi, " + $"{boneBoundsTable.MeasuredBoneCount}/{list.Count} kemik olculdu, bunlarin " + $"{boneBoundsTable.OrientedBoneCount} tanesi kendi ekseninde (tam) olculdu.", this);
					}
				}
			}
		}

		private void ReportDerivedSizes()
		{
			if (reportingThisApply)
			{
				reportingThisApply = false;
				if (derivedSizes.Count > 0)
				{
					Debug.Log("[CharacterAssembler] Olculen kutular ve turetilen voxel boylari -- " + string.Join("  //  ", derivedSizes), this);
				}
				derivedSizes.Clear();
			}
		}

		private void SetDefaultRenderersVisible(bool visible)
		{
			if (defaultRenderers == null)
			{
				return;
			}
			Renderer[] array = defaultRenderers;
			foreach (Renderer renderer in array)
			{
				if (renderer != null && renderer.enabled != visible)
				{
					renderer.enabled = visible;
				}
			}
		}

		private void NotifyCameraRig()
		{
			PlayerCameraRig componentInParent = GetComponentInParent<PlayerCameraRig>();
			if (componentInParent != null)
			{
				componentInParent.RefreshCharacterRenderers();
			}
		}
	}
}
