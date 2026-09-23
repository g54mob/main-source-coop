using System;
using System.Collections.Generic;
using Mimicraft.Gameplay;
using Mimicraft.Localization;
using Mimicraft.UI;
using Mimicraft.VoxelEditor;
using Mimicraft.VoxelEditor.Core;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Mimicraft.Customization
{
	public class WeaponSkinAssembler : MonoBehaviour
	{
		[Tooltip("Bu silahın tanımı - kutu boyu, voxel boyu ve offset oradan geliyor. Tek yerde durmasının sebebi: aynı silahın FPS prefab'ı, TPS prefab'ı ve menüdeki kopyası AYNI kutuyu kullanmak zorunda. Ayrı ayrı yazılsalardı üçünü aynı anda değiştirmeyi hatırlamak gerekirdi, ve unutulan kopya menüde yontulanı oyunda başka bir kutuya oturturdu.")]
		[SerializeField]
		private WeaponDefinition weapon;

		[Tooltip("Silahın kendi mesh'inin durduğu obje. Voxel model TAM BURAYA takılıyor ve bu objenin katmanını alıyor. Boş bırakılırsa ilk renderer'ın objesi kullanılır. Köke değil buraya takılmasının sebebi: FPS view model'inde kök ile mesh arasında animasyonlu transform'lar var (WeaponPivot, sway, geri tepme). Köke takılan bir model onlarla birlikte hareket etmez ve silahtan ayrı düşer.")]
		[SerializeField]
		internal Transform modelRoot;

		[Tooltip("Voxel modelin materyalleri - VoxelModel'in Unlit/Lit çifti.")]
		[SerializeField]
		private Material unlitMaterial;

		[SerializeField]
		private Material litMaterial;

		[Tooltip("Bir model kurulduğunda gizlenecek varsayılan görünüm. Boş bırakılırsa bu objenin altındaki, voxel modele ait OLMAYAN bütün renderer'lar bulunur.")]
		[SerializeField]
		private Renderer[] defaultRenderers;

		[Tooltip("Açıkken, verisi olmayan bir silah kutusunu dolduran bir blok olarak kurulur - yontulacak kil. Kapatınca özelleştirilmemiş silah kendi modelinde kalır.")]
		[SerializeField]
		private bool buildWhenEmpty;

		private VoxelModel model;

		private WeaponSkinBox box;

		[Tooltip("Açıkken silah kutusunun TAM ORTASINA oturur ve kayıtlı model de kutunun ortasına hizalanır - tezgâhta çevirip yontmak için. Kapalıyken (varsayılan, ve oyundaki doğru hâli) kutunun arka yüzü Model Root'un orijinine dayanır ve öne doğru uzar; her silah vücutta aynı noktadan başlar.\n\nSADECE Customization sahnesindeki kopyalarda aç. FPS/TPS prefablerinde kapalı kalmalı.")]
		[SerializeField]
		private bool centreInBox;

		private bool hasApplied;

		private string lastReport;

		private VoxelGrid topCentreGrid;

		private int topCentreVersion = -1;

		private int topCentreLayer;

		private float topCentreX;

		private Vector3 subVoxelShift;

		private Transform leftAnchor;

		private Transform rightAnchor;

		private Transform muzzleAnchor;

		private WeaponSkinData points;

		private Vector3 pendingLeft;

		private Vector3 pendingRight;

		private Vector3 pendingMuzzle;

		private WeaponEditBounds editBounds;

		private bool watchingEdits;

		private bool revertPending;

		private Vector3Int gridShift;

		public bool CentreInBox
		{
			get
			{
				return centreInBox;
			}
			set
			{
				if (centreInBox != value)
				{
					centreInBox = value;
					box = null;
				}
			}
		}

		public WeaponSkinBox Box => box ?? Measure();

		public WeaponDefinition Weapon => weapon;

		public WeaponVisual Visual => GetComponentInChildren<WeaponVisual>(includeInactive: true);

		public VoxelModel Model => model;

		public int BuildVersion { get; private set; }

		public WeaponSoundKind Sound { get; set; }

		public bool HasModel => model != null;

		public Vector3 LeftGrip
		{
			get
			{
				return SnapToPointGrid((points != null && points.HasPoints) ? (points.LeftGrip + PointOffset) : PrefabPoint((WeaponVisual v) => v.LeftGrip));
			}
			set
			{
				SetPoint(ref pendingLeft, value);
			}
		}

		public Vector3 RightGrip
		{
			get
			{
				return SnapToPointGrid((points != null && points.HasPoints) ? (points.RightGrip + PointOffset) : PrefabPoint((WeaponVisual v) => v.RightGrip));
			}
			set
			{
				SetPoint(ref pendingRight, value);
			}
		}

		public Vector3 Muzzle
		{
			get
			{
				return SnapToPointGrid((points != null && points.HasPoints) ? (points.Muzzle + PointOffset) : PrefabPoint((WeaponVisual v) => v.Muzzle));
			}
			set
			{
				SetPoint(ref pendingMuzzle, value);
			}
		}

		public Vector3 AdsPos
		{
			get
			{
				if (points != null && points.HasAdsPos)
				{
					return SnapToPointGrid(points.AdsPos + PointOffset);
				}
				if (TryGetDefaultAdsPos(out var position))
				{
					return SnapToPointGrid(position);
				}
				WeaponVisual componentInChildren = GetComponentInChildren<WeaponVisual>(includeInactive: true);
				if (!(componentInChildren != null))
				{
					return Vector3.zero;
				}
				return SnapToPointGrid(componentInChildren.AuthoredAdsPivotPosition);
			}
			set
			{
				if (points == null)
				{
					points = new WeaponSkinData((model != null) ? model.Grid : null);
				}
				points.SetAdsPos(SnapToPointGrid(value) - PointOffset);
				ApplyPoints();
			}
		}

		public string AdsReport
		{
			get
			{
				bool flag = points != null && points.HasAdsPos;
				Vector3 position;
				string text = (TryGetDefaultAdsPos(out position) ? position.ToString("F3") : "yok (model yok)");
				return $"yerlestirilmis={flag}" + (flag ? (" kayitli(kutuya gore)=" + points.AdsPos.ToString("F3") + " modelRoot'ta=" + AdsPos.ToString("F3")) : "") + " varsayilan=" + text + " model=" + ((model != null) ? model.name : "YOK") + " pointOffset=" + PointOffset.ToString("F3");
			}
		}

		public bool HasPlacedAdsPos
		{
			get
			{
				if (points != null)
				{
					return points.HasAdsPos;
				}
				return false;
			}
		}

		private Vector3 BoxCorner
		{
			get
			{
				if (box == null)
				{
					return Vector3.zero;
				}
				return box.LocalCorner;
			}
		}

		private Vector3 PointOffset => BoxCorner + (Vector3)gridShift * ((box != null) ? box.VoxelSize : 0f) + subVoxelShift;

		public Vector3 PointGridOrigin => PointOffset;

		public float PointGridStep
		{
			get
			{
				if (box == null)
				{
					return 0f;
				}
				return box.VoxelSize * 0.5f;
			}
		}

		public Transform BoxSpace => ModelRoot;

		private GameObject MeasurementSource
		{
			get
			{
				if (weapon == null)
				{
					return base.gameObject;
				}
				if (!(weapon.TpsPrefab != null))
				{
					if (!(weapon.HeldPrefab != null))
					{
						return base.gameObject;
					}
					return weapon.HeldPrefab;
				}
				return weapon.TpsPrefab;
			}
		}

		private Transform ModelRoot
		{
			get
			{
				if (modelRoot != null)
				{
					return modelRoot;
				}
				Renderer[] componentsInChildren = GetComponentsInChildren<Renderer>(includeInactive: true);
				foreach (Renderer renderer in componentsInChildren)
				{
					if (renderer != null && renderer.GetComponentInParent<VoxelModel>() == null)
					{
						return renderer.transform;
					}
				}
				return null;
			}
		}

		public string WeaponId
		{
			get
			{
				if (!(weapon != null))
				{
					return "";
				}
				return weapon.WeaponId;
			}
		}

		public string DisplayName
		{
			get
			{
				if (!(weapon != null))
				{
					return base.name;
				}
				return weapon.DisplayName;
			}
		}

		public WeaponSkinData DefaultSkin
		{
			get
			{
				if (!(weapon != null) || !(weapon.DefaultSkin != null))
				{
					return null;
				}
				return weapon.DefaultSkin.ToSkinData();
			}
		}

		public IReadOnlyList<WeaponSkinDefinition> Presets
		{
			get
			{
				if (!(weapon != null))
				{
					return Array.Empty<WeaponSkinDefinition>();
				}
				return weapon.SkinPresets;
			}
		}

		private void Awake()
		{
			if (defaultRenderers == null || defaultRenderers.Length == 0)
			{
				defaultRenderers = GetComponentsInChildren<Renderer>(includeInactive: true);
			}
		}

		private void Start()
		{
			if (!hasApplied)
			{
				Apply(null);
			}
		}

		public void Apply(WeaponSkinData skin)
		{
			hasApplied = true;
			points = skin;
			Sound = skin?.Sound ?? WeaponSoundKind.Normal;
			VoxelGrid voxelGrid = skin?.Grid;
			Clear();
			BuildVersion++;
			WeaponSkinBox weaponSkinBox = Box;
			if (weaponSkinBox == null)
			{
				ApplyPoints();
				return;
			}
			gridShift = ((voxelGrid != null && voxelGrid.Count > 0) ? CentringShift(voxelGrid, weaponSkinBox.BoxSize) : Vector3Int.zero);
			VoxelGrid voxelGrid2 = ((voxelGrid != null && voxelGrid.Count > 0) ? voxelGrid : (buildWhenEmpty ? SolidBlock(weaponSkinBox) : null));
			subVoxelShift = ((voxelGrid2 != null) ? SubVoxelCentring(voxelGrid2, weaponSkinBox) : Vector3.zero);
			ApplyPoints();
			if (voxelGrid2 == null)
			{
				Report($"kutu {weaponSkinBox.BoxSize.x}x{weaponSkinBox.BoxSize.y}x{weaponSkinBox.BoxSize.z} voxel " + $"({weaponSkinBox.VoxelSize:0.####}m/voxel) ama gosterilecek voxel yok - " + "Build When Empty kapali ve kayitli bir model gelmedi.");
				return;
			}
			model = Build(weaponSkinBox, voxelGrid2);
			SetDefaultRenderersVisible(model == null);
			WeaponVisual componentInChildren = GetComponentInChildren<WeaponVisual>(includeInactive: true);
			if (componentInChildren != null)
			{
				ApplyAdsAim(componentInChildren);
			}
			Report($"kutu {weaponSkinBox.BoxSize.x}x{weaponSkinBox.BoxSize.y}x{weaponSkinBox.BoxSize.z} voxel " + $"({weaponSkinBox.VoxelSize:0.####}m/voxel), {voxelGrid2.Count} voxel kuruldu.");
		}

		private void Report(string message, bool warning = false)
		{
			if (!(message == lastReport))
			{
				lastReport = message;
				if (warning)
				{
					Debug.LogWarning("[WeaponSkinAssembler] " + base.name + ": " + message, this);
				}
				else
				{
					Debug.Log("[WeaponSkinAssembler] " + base.name + ": " + message, this);
				}
			}
		}

		public void Clear()
		{
			if (model != null)
			{
				UnityEngine.Object.Destroy(model.gameObject);
			}
			model = null;
			subVoxelShift = Vector3.zero;
			BuildVersion++;
			SetDefaultRenderersVisible(visible: true);
		}

		public WeaponSkinData Capture()
		{
			WeaponSkinData weaponSkinData = new WeaponSkinData((model != null) ? model.Grid : null);
			weaponSkinData.SetPoints(LeftGrip - PointOffset, RightGrip - PointOffset, Muzzle - PointOffset);
			if (points != null && points.HasAdsPos)
			{
				weaponSkinData.SetAdsPos(AdsPos - PointOffset);
			}
			weaponSkinData.Sound = Sound;
			return weaponSkinData;
		}

		public bool TryGetDefaultAdsPos(out Vector3 position)
		{
			position = Vector3.zero;
			Transform transform = ModelRoot;
			if (model == null || transform == null || !model.Grid.TryGetBounds(out var min, out var max))
			{
				return false;
			}
			float num = min.z;
			float num2 = (float)max.z + 1f;
			Vector3 position2 = new Vector3(TopLayerCentreX(model.Grid, max.y, min, max), (float)max.y + 1f, num + (num2 - num) / 3f);
			position = transform.InverseTransformPoint(model.transform.TransformPoint(position2));
			return true;
		}

		private float TopLayerCentreX(VoxelGrid grid, int top, Vector3Int min, Vector3Int max)
		{
			if (grid == topCentreGrid && grid.Version == topCentreVersion && top == topCentreLayer)
			{
				return topCentreX;
			}
			int num = int.MaxValue;
			int num2 = int.MinValue;
			foreach (Vector3Int position in grid.Positions)
			{
				if (position.y == top)
				{
					if (position.x < num)
					{
						num = position.x;
					}
					if (position.x > num2)
					{
						num2 = position.x;
					}
				}
			}
			topCentreX = ((num <= num2) ? (((float)(num + num2) + 1f) * 0.5f) : (((float)(min.x + max.x) + 1f) * 0.5f));
			topCentreGrid = grid;
			topCentreVersion = grid.Version;
			topCentreLayer = top;
			return topCentreX;
		}

		private void ApplyAdsAim(WeaponVisual visual)
		{
			Transform root = ModelRoot;
			Transform pivot = visual.AimPivot;
			Vector3? vector = ResolvedAdsOverride();
			if (!vector.HasValue || root == null || pivot == null)
			{
				visual.SetAdsAimPoint(null, Vector3.zero);
				return;
			}
			Vector3 position;
			Vector3 modelRootPoint = (TryGetDefaultAdsPos(out position) ? position : vector.Value);
			visual.SetAdsAimPoint(ToPivot(vector.Value), ToPivot(modelRootPoint));
			Vector3 ToPivot(Vector3 position2)
			{
				return pivot.InverseTransformPoint(root.TransformPoint(position2));
			}
		}

		private Vector3? ResolvedAdsOverride()
		{
			if (points != null && points.HasAdsPos)
			{
				return points.AdsPos + PointOffset;
			}
			if (!TryGetDefaultAdsPos(out var position))
			{
				return null;
			}
			return position;
		}

		public Vector3 SnapToPointGrid(Vector3 value)
		{
			float pointGridStep = PointGridStep;
			if (pointGridStep <= 0f)
			{
				return value;
			}
			Vector3 pointGridOrigin = PointGridOrigin;
			return new Vector3(pointGridOrigin.x + Mathf.Round((value.x - pointGridOrigin.x) / pointGridStep) * pointGridStep, pointGridOrigin.y + Mathf.Round((value.y - pointGridOrigin.y) / pointGridStep) * pointGridStep, pointGridOrigin.z + Mathf.Round((value.z - pointGridOrigin.z) / pointGridStep) * pointGridStep);
		}

		private void SetPoint(ref Vector3 slot, Vector3 value)
		{
			SeedPendingPoints();
			slot = SnapToPointGrid(value);
			if (points == null)
			{
				points = new WeaponSkinData((model != null) ? model.Grid : null);
			}
			points.SetPoints(pendingLeft - PointOffset, pendingRight - PointOffset, pendingMuzzle - PointOffset);
			ApplyPoints();
		}

		private void SeedPendingPoints()
		{
			pendingLeft = LeftGrip;
			pendingRight = RightGrip;
			pendingMuzzle = Muzzle;
		}

		private void ApplyPoints()
		{
			SeedPendingPoints();
			WeaponVisual componentInChildren = GetComponentInChildren<WeaponVisual>(includeInactive: true);
			if (!(componentInChildren == null))
			{
				Transform left = (HasSource(componentInChildren.LeftGrip) ? Anchor(ref leftAnchor, "Point_LeftGrip", pendingLeft) : null);
				Transform right = (HasSource(componentInChildren.RightGrip) ? Anchor(ref rightAnchor, "Point_RightGrip", pendingRight) : null);
				Transform transform = (HasSource((componentInChildren.Muzzle == componentInChildren.transform) ? null : componentInChildren.Muzzle) ? Anchor(ref muzzleAnchor, "Point_Muzzle", pendingMuzzle) : null);
				componentInChildren.OverridePoints(left, right, transform);
				AttachFlash(componentInChildren, transform);
				ApplyAdsAim(componentInChildren);
			}
		}

		private bool HasSource(Transform authored)
		{
			if (points == null || !points.HasPoints)
			{
				return authored != null;
			}
			return true;
		}

		private Transform Anchor(ref Transform slot, string name, Vector3 boxPoint)
		{
			Transform boxSpace = BoxSpace;
			if (boxSpace == null)
			{
				return null;
			}
			if (slot == null)
			{
				slot = new GameObject(name).transform;
			}
			if (slot.parent != boxSpace)
			{
				slot.SetParent(boxSpace, worldPositionStays: false);
			}
			slot.localPosition = boxPoint;
			slot.localRotation = Quaternion.identity;
			return slot;
		}

		private void AttachFlash(WeaponVisual visual, Transform barrel)
		{
			Transform muzzleFlashTransform = visual.MuzzleFlashTransform;
			if (!(muzzleFlashTransform == null))
			{
				Transform boxSpace = BoxSpace;
				if (boxSpace != null)
				{
					SetLayerRecursively(muzzleFlashTransform, boxSpace.gameObject.layer);
				}
				if ((object)barrel == null)
				{
					barrel = ((visual.Muzzle != visual.transform) ? visual.Muzzle : null);
				}
				if (!(barrel == null) && !(muzzleFlashTransform == barrel) && !(muzzleFlashTransform.parent == barrel))
				{
					Quaternion localRotation = Quaternion.Inverse(barrel.rotation) * muzzleFlashTransform.rotation;
					Vector3 lossyScale = muzzleFlashTransform.lossyScale;
					muzzleFlashTransform.SetParent(barrel, worldPositionStays: false);
					muzzleFlashTransform.localPosition = Vector3.zero;
					muzzleFlashTransform.localRotation = localRotation;
					Vector3 lossyScale2 = barrel.lossyScale;
					muzzleFlashTransform.localScale = new Vector3((lossyScale2.x > 0.0001f) ? (lossyScale.x / lossyScale2.x) : lossyScale.x, (lossyScale2.y > 0.0001f) ? (lossyScale.y / lossyScale2.y) : lossyScale.y, (lossyScale2.z > 0.0001f) ? (lossyScale.z / lossyScale2.z) : lossyScale.z);
				}
			}
		}

		private static void SetLayerRecursively(Transform root, int layer)
		{
			if (root.gameObject.layer != layer)
			{
				Transform[] componentsInChildren = root.GetComponentsInChildren<Transform>(includeInactive: true);
				for (int i = 0; i < componentsInChildren.Length; i++)
				{
					componentsInChildren[i].gameObject.layer = layer;
				}
			}
		}

		private Vector3 PrefabPoint(Func<WeaponVisual, Transform> pick)
		{
			Transform boxSpace = BoxSpace;
			if (boxSpace == null)
			{
				return Vector3.zero;
			}
			WeaponVisual componentInChildren = GetComponentInChildren<WeaponVisual>(includeInactive: true);
			if (componentInChildren != null)
			{
				Transform transform = pick(componentInChildren);
				if (transform != null && transform != componentInChildren.transform)
				{
					return boxSpace.InverseTransformPoint(transform.position);
				}
			}
			GameObject gameObject = ((weapon != null) ? (weapon.TpsPrefab ?? weapon.HeldPrefab) : null);
			if (gameObject == null)
			{
				return Vector3.zero;
			}
			WeaponVisual componentInChildren2 = gameObject.GetComponentInChildren<WeaponVisual>(includeInactive: true);
			Transform transform2 = ((componentInChildren2 != null) ? pick(componentInChildren2) : null);
			if (transform2 == null || transform2 == componentInChildren2.transform)
			{
				return Vector3.zero;
			}
			return PrefabModelRoot(gameObject).transform.InverseTransformPoint(transform2.position);
		}

		private static GameObject PrefabModelRoot(GameObject prefab)
		{
			WeaponSkinAssembler componentInChildren = prefab.GetComponentInChildren<WeaponSkinAssembler>(includeInactive: true);
			if (componentInChildren != null && componentInChildren.modelRoot != null)
			{
				return componentInChildren.modelRoot.gameObject;
			}
			Renderer[] componentsInChildren = prefab.GetComponentsInChildren<Renderer>(includeInactive: true);
			foreach (Renderer renderer in componentsInChildren)
			{
				if (renderer != null && renderer.GetComponentInParent<VoxelModel>() == null)
				{
					return renderer.gameObject;
				}
			}
			return prefab;
		}

		public void SetEditable(bool editable, Camera editorCamera, EditorGizmoSizes gizmoSizes = null)
		{
			if (model == null)
			{
				return;
			}
			MeshCollider component = model.GetComponent<MeshCollider>();
			if (component != null)
			{
				component.enabled = editable;
				if (editable)
				{
					model.RefreshCollider();
				}
			}
			VoxelEditorController voxelEditorController = model.GetComponent<VoxelEditorController>();
			if (editable)
			{
				if (voxelEditorController == null)
				{
					voxelEditorController = model.gameObject.AddComponent<VoxelEditorController>();
				}
				voxelEditorController.SetCamera(editorCamera);
				voxelEditorController.enabled = true;
				gizmoSizes?.ApplyTo(voxelEditorController);
				voxelEditorController.AllowTransform = true;
				voxelEditorController.AllowTransformGizmos = false;
				editBounds = new WeaponEditBounds(Box, model, (weapon != null) ? weapon.SkinMinimumVoxels : 0);
				model.EditBounds = editBounds;
				WatchEdits(watch: true);
			}
			else if (voxelEditorController != null)
			{
				voxelEditorController.enabled = false;
				model.EditBounds = null;
				editBounds = null;
				WatchEdits(watch: false);
			}
		}

		private void WatchEdits(bool watch)
		{
			if (watch != watchingEdits)
			{
				watchingEdits = watch;
				if (watch)
				{
					UndoManager.EditStepApplied += OnEditStep;
				}
				else
				{
					UndoManager.EditStepApplied -= OnEditStep;
				}
				revertPending = false;
			}
		}

		private void OnDisable()
		{
			WatchEdits(watch: false);
		}

		private void OnEditStep(IUndoableCommand command, UndoManager.EditStepKind kind)
		{
			if (kind != UndoManager.EditStepKind.Undo && editBounds != null && !editBounds.IsSatisfied)
			{
				revertPending = true;
			}
		}

		private void LateUpdate()
		{
			if (!revertPending)
			{
				return;
			}
			revertPending = false;
			if (editBounds != null && !editBounds.IsSatisfied && !(model == null))
			{
				VoxelEditorController component = model.GetComponent<VoxelEditorController>();
				if (component != null)
				{
					component.AbandonGesture();
				}
				UndoManager.Undo();
				AudioLibrary.PlayOneShotClip((AudioLibrary.Instance != null) ? AudioLibrary.Instance.editDeniedClip : null);
				if (ToastView.Instance != null)
				{
					ToastView.Instance.Show(string.Format(Loc.Get("WeaponSkin.MinimumVoxels"), editBounds.MinimumVoxels));
				}
			}
		}

		private static Transform MeasurementFrame(GameObject source)
		{
			if (source == null)
			{
				return null;
			}
			WeaponSkinAssembler componentInChildren = source.GetComponentInChildren<WeaponSkinAssembler>(includeInactive: true);
			if (!(componentInChildren != null) || !(componentInChildren.modelRoot != null))
			{
				return source.transform;
			}
			return componentInChildren.modelRoot;
		}

		private WeaponSkinBox Measure()
		{
			if (weapon == null)
			{
				Report("Weapon alani bos - kutu olculemiyor, bu silah ozellestirilemez.");
				return null;
			}
			Transform transform = ModelRoot;
			if (transform == null)
			{
				Report("silahin mesh'i bulunamadi - Model Root'u elle ver.");
				return null;
			}
			GameObject measurementSource = MeasurementSource;
			Transform measureFrame = MeasurementFrame(measurementSource);
			string explanation;
			bool flag = WeaponSkinBox.TryCreate(measurementSource, weapon.DisplayName, weapon.SkinBoxSize, weapon.SkinVoxelSize, weapon.SkinAnchorOffset, out box, out explanation, centreInBox, measureFrame);
			Report("kutu -> " + explanation + " (olculen: " + ((measurementSource == null) ? "YOK" : measurementSource.name) + ", Model Root: " + transform.name + ")", !flag);
			return box;
		}

		private VoxelModel Build(WeaponSkinBox measured, VoxelGrid source)
		{
			Transform transform = ModelRoot;
			if (transform == null)
			{
				return null;
			}
			GameObject gameObject = new GameObject("WeaponSkin");
			gameObject.layer = transform.gameObject.layer;
			Scene scene = base.gameObject.scene;
			if (scene.IsValid() && gameObject.scene != scene)
			{
				SceneManager.MoveGameObjectToScene(gameObject, scene);
			}
			Transform obj = gameObject.transform;
			obj.SetParent(transform, worldPositionStays: false);
			obj.localPosition = measured.LocalCorner + subVoxelShift;
			obj.localRotation = Quaternion.identity;
			gameObject.AddComponent<MeshFilter>();
			gameObject.AddComponent<MeshRenderer>();
			gameObject.AddComponent<MeshCollider>().enabled = false;
			VoxelModel voxelModel = gameObject.AddComponent<VoxelModel>();
			voxelModel.ConfigureVoxelSize(measured.VoxelSize);
			voxelModel.ConfigureMaterials(unlitMaterial, litMaterial);
			voxelModel.SetUnlit(unlit: false);
			Vector3Int vector3Int = gridShift;
			voxelModel.Grid.Clear();
			foreach (KeyValuePair<Vector3Int, VoxelData> voxel in source.Voxels)
			{
				voxelModel.Grid.Set(voxel.Key + vector3Int, voxel.Value);
			}
			foreach (KeyValuePair<(Vector3Int, int), Color32> faceColor in source.FaceColors)
			{
				voxelModel.Grid.SetFaceColor(faceColor.Key.Item1 + vector3Int, faceColor.Key.Item2, faceColor.Value);
			}
			voxelModel.RebuildMesh(updateCollider: false);
			ReportPlacement(source, voxelModel, measured, vector3Int);
			return voxelModel;
		}

		private void ReportPlacement(VoxelGrid source, VoxelModel built, WeaponSkinBox measured, Vector3Int shift)
		{
			if (Occupied(source, out var min, out var max) && Occupied(built.Grid, out var min2, out var max2))
			{
				Vector3 vector = (Vector3)(max2 - min2 + Vector3Int.one) * measured.VoxelSize;
				Vector3 vector2 = built.transform.localPosition + (min2 + (Vector3)(max2 - min2 + Vector3Int.one) * 0.5f) * measured.VoxelSize;
				Vector3 measuredCentre = measured.MeasuredCentre;
				Vector3 vector3 = vector2 - measuredCentre;
				Report($"yerlesim -> dolu hucreler {min}..{max} + kaydirma {shift} = " + $"{min2}..{max2} (kutu {measured.BoxSize}), " + $"dolu boyut ({vector.x:0.###}x{vector.y:0.###}x{vector.z:0.###}m), " + $"model merkezi ({vector2.x:0.####}, {vector2.y:0.####}, {vector2.z:0.####}), " + $"mesh merkezi ({measuredCentre.x:0.####}, {measuredCentre.y:0.####}, {measuredCentre.z:0.####}), " + $"KACIK ({vector3.x:0.####}, {vector3.y:0.####}, {vector3.z:0.####}), " + $"centreInBox={centreInBox}");
			}
		}

		private static bool Occupied(VoxelGrid grid, out Vector3Int min, out Vector3Int max)
		{
			min = new Vector3Int(int.MaxValue, int.MaxValue, int.MaxValue);
			max = new Vector3Int(int.MinValue, int.MinValue, int.MinValue);
			bool result = false;
			foreach (KeyValuePair<Vector3Int, VoxelData> voxel in grid.Voxels)
			{
				result = true;
				min = Vector3Int.Min(min, voxel.Key);
				max = Vector3Int.Max(max, voxel.Key);
			}
			return result;
		}

		private Vector3 SubVoxelCentring(VoxelGrid source, WeaponSkinBox measured)
		{
			if (centreInBox || !Occupied(source, out var min, out var max))
			{
				return Vector3.zero;
			}
			Vector3Int vector3Int = max - min + Vector3Int.one;
			return new Vector3(Remainder(measured.BoxSize.x - vector3Int.x) * measured.VoxelSize, Remainder(measured.BoxSize.y - vector3Int.y) * measured.VoxelSize, 0f);
		}

		private static float Remainder(int surplus)
		{
			if (surplus <= 0 || (surplus & 1) != 1)
			{
				return 0f;
			}
			return 0.5f;
		}

		private Vector3Int CentringShift(VoxelGrid source, Vector3Int box)
		{
			Vector3Int vector3Int = new Vector3Int(int.MaxValue, int.MaxValue, int.MaxValue);
			Vector3Int vector3Int2 = new Vector3Int(int.MinValue, int.MinValue, int.MinValue);
			bool flag = false;
			foreach (KeyValuePair<Vector3Int, VoxelData> voxel in source.Voxels)
			{
				flag = true;
				vector3Int = Vector3Int.Min(vector3Int, voxel.Key);
				vector3Int2 = Vector3Int.Max(vector3Int2, voxel.Key);
			}
			if (!flag)
			{
				return Vector3Int.zero;
			}
			Vector3Int vector3Int3 = vector3Int2 - vector3Int + Vector3Int.one;
			return new Vector3Int(Mathf.Max(0, (box.x - vector3Int3.x) / 2) - vector3Int.x, Mathf.Max(0, (box.y - vector3Int3.y) / 2) - vector3Int.y, centreInBox ? (Mathf.Max(0, (box.z - vector3Int3.z) / 2) - vector3Int.z) : (-vector3Int.z));
		}

		private static VoxelGrid SolidBlock(WeaponSkinBox measured)
		{
			VoxelGrid voxelGrid = new VoxelGrid();
			Color32 color = new Color32(150, 150, 160, byte.MaxValue);
			for (int i = 0; i < measured.BoxSize.x; i++)
			{
				for (int j = 0; j < measured.BoxSize.y; j++)
				{
					for (int k = 0; k < measured.BoxSize.z; k++)
					{
						voxelGrid.Set(new Vector3Int(i, j, k), new VoxelData(color));
					}
				}
			}
			return voxelGrid;
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
	}
}
