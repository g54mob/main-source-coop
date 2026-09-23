using System;
using System.Collections;
using System.Collections.Generic;
using Mimicraft.Analytics;
using Mimicraft.Cameras;
using Mimicraft.Gameplay;
using Mimicraft.Localization;
using Mimicraft.UI;
using Mimicraft.VoxelEditor;
using Mimicraft.VoxelEditor.Core;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Mimicraft.Customization
{
	public class MimicCustomizationView : MonoBehaviour, IModelEditSession
	{
		[Tooltip("Düzenlenecek model - menü sahnesine elle koyduğun, üzerinde VoxelModel ve VoxelEditorController olan obje. Oyundaki gövde değil.")]
		[SerializeField]
		private VoxelEditorController mimic;

		[Tooltip("Modeli gösteren ve döndürülen kamera. OrbitCamera bileşeni de bunun üzerinde olmalı.")]
		[SerializeField]
		private Camera editorCamera;

		[Tooltip("Additive olarak yüklenecek editör sahnesi - karakter ve silah ekranlarıyla AYNI sahne.")]
		[AssetSelectorPopup("SceneAsset", false)]
		[SerializeField]
		private string editorSceneName = "Editor";

		[Tooltip("Kaydetme çubuğu. Boş bırakılırsa bu objenin altında aranır.")]
		[SerializeField]
		private MimicSaveBarView saveBar;

		[Tooltip("Taunt sesi panelini açıp kapatan buton. İsteğe bağlı.")]
		[SerializeField]
		private Button voiceButton;

		[Tooltip("O butonun açıp kapadığı panel - üzerinde ModelerVoiceView olan obje. Boş bırakılırsa bu ekranın altında aranır.")]
		[SerializeField]
		private GameObject voicePanel;

		[Header("Uzuvlar")]
		[Tooltip("Uzuvları açıp kapatan buton. İsteğe bağlı. Yazısı varsa duruma göre 'göster/gizle' diye yazılır.")]
		[SerializeField]
		private Button limbsButton;

		[Tooltip("Uzuv sürücüsü - modelin KÖK objesinde HiderLimbs, yanında ModelerLimbs prefab'ı ve MimicLimbBody. Boş bırakılırsa bu ekranın altında aranır. Bileşen KAPALI durmalı: kendi Update'i koşmayan bir oyuncu görür ve uzuvları hep geri çeker; burada ekran sürer (DevTick), tıpkı replay hayaletinde olduğu gibi.")]
		[SerializeField]
		private HiderLimbs limbs;

		[Tooltip("Bu ekran kapanınca geri açılacak objeler - ana menü paneli.")]
		[SerializeField]
		private GameObject[] showOnClose;

		[Tooltip("Bu ekranın kapatma butonu. Bağlanırsa ESC tam olarak bu butona basar.")]
		[SerializeField]
		private Button closeButton;

		[Tooltip("Escape ile çıkılabilsin mi.")]
		[SerializeField]
		private bool closeOnEscape = true;

		[Tooltip("Bu ekran açıkken Depth of Field'ı kapatılacak Volume'lar. Boş bırakılırsa sahnedeki bütün Volume'lar taranır.")]
		[SerializeField]
		private Volume[] depthOfFieldVolumes;

		[Tooltip("Kameranın modele yaklaşabileceği en küçük mesafe. Boyama santimetrelerden bakmayı gerektiriyor; kameranın kendi değeri bütün modeli çerçevelemek için ayarlı.")]
		[SerializeField]
		[Min(0.01f)]
		private float closeZoomDistance = 0.25f;

		[Header("Gövde ölçüsü (oyundakiyle aynı)")]
		[Tooltip("Bir voxel'in dünya boyu, metre. Oyundaki gövde 0.0625 (16 voxel = 1 m). Ekran açılınca modele bunu yazar - sahnedeki objenin kendi değeri ne olursa olsun. VoxelModel'in kod varsayılanı 0.3, ve onunla kurulan bir obje oyundakinden beş kat büyük voxel'lerle açılır.")]
		[SerializeField]
		[Min(0.001f)]
		private float bodyVoxelSize = 0.0625f;

		[Tooltip("Başlangıç küpünün kenarı, voxel. Oyundaki gövde 16. Kod varsayılanı 3, ki bu gövde kurallarının alt sınırının (4) altında: küp daha açılırken kural dışı görünür.")]
		[SerializeField]
		[Min(1f)]
		private int bodyInitialSize = 16;

		[Tooltip("Gizmo çarpanları. Oyundaki gövdenin değerleri (0.05 / 0.075 / 0.5 / 1). Kod varsayılanı hepsi 1, yani yirmi kat büyük oklar.")]
		[SerializeField]
		private EditorGizmoSizes gizmoSizes = new EditorGizmoSizes
		{
			gizmoSize = 0.05f,
			extrudeGizmoSize = 0.075f,
			gizmoThickness = 0.5f,
			centerMarkerSize = 1f
		};

		[Tooltip("Uzuvların gövdeye nereden bağlandığı ve koşuda ne kadar kalktığı - oyundaki gövdenin sayıları (0.05 / 0.05 / 0.28 / 1). Sahnedeki HiderLimbs'e açılışta yazılır; kod varsayılanlarıyla kurulmuş bir sürücü uzuvları gövdeden uzağa koyuyor ve az kaldırıyordu.")]
		[SerializeField]
		private HiderLimbs.LimbTuning limbTuning = new HiderLimbs.LimbTuning();

		private readonly Dictionary<DepthOfField, bool> suppressedDepthOfField = new Dictionary<DepthOfField, bool>();

		private bool limbsShown;

		private Vector3 restLocalPosition;

		private Quaternion restLocalRotation = Quaternion.identity;

		private bool hasRestPose;

		private OrbitCamera orbitCamera;

		private bool loadedEditorScene;

		private bool cameraLive;

		private bool hasFramed;

		private bool restoreCameraActive = true;

		private bool restoreCameraEnabled;

		private bool restoreOrbitEnabled;

		private Vector3 restoreCameraPosition;

		private Quaternion restoreCameraRotation = Quaternion.identity;

		public VoxelEditorController Mimic => mimic;

		public VoxelModel Model
		{
			get
			{
				if (!(mimic != null))
				{
					return null;
				}
				return mimic.Model;
			}
		}

		public Camera EditorCamera => editorCamera;

		public bool CanResetModel => mimic != null;

		public bool SupportsPrimitives => true;

		private OrbitCamera OrbitCamera
		{
			get
			{
				if (orbitCamera == null && editorCamera != null)
				{
					orbitCamera = editorCamera.GetComponentInParent<OrbitCamera>(includeInactive: true);
				}
				return orbitCamera;
			}
		}

		private void OnEnable()
		{
			ModelEditSession.Begin(this);
			WireButtons();
			Telemetry.Send("menu_action", ("menu_item", "customize_mimic"), ("session_seconds", Telemetry.SessionSeconds));
			StartCoroutine(OpenRoutine());
		}

		private void OnDisable()
		{
			hasFramed = false;
			limbsShown = false;
			if (limbs != null)
			{
				limbs.DevCollapse();
			}
			ApplyLimbsCaption();
			ModelEditSession.End(this);
			SetEditing(editing: false);
			CursorManager.ResetToDefault();
			UnloadEditorScene();
		}

		private void Update()
		{
			if (limbs != null)
			{
				limbs.DevTick(limbsShown);
			}
			if (closeOnEscape && Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
			{
				GameMenuState.RequestEscape(20, CloseFromEscape);
			}
		}

		private void WireButtons()
		{
			if (voiceButton != null)
			{
				voiceButton.onClick.RemoveListener(ToggleVoicePanel);
				voiceButton.onClick.AddListener(ToggleVoicePanel);
			}
			if (limbsButton != null)
			{
				limbsButton.onClick.RemoveListener(ToggleLimbs);
				limbsButton.onClick.AddListener(ToggleLimbs);
			}
			ResolveLimbs();
			ApplyLimbsCaption();
			UITooltipTrigger.AttachKey(voiceButton, "Tooltip.Mimic.Voice");
			UITooltipTrigger.AttachKey(limbsButton, "Tooltip.Mimic.Limbs");
			UITooltipTrigger.AttachKey(closeButton, "Tooltip.Close");
		}

		public void ToggleLimbs()
		{
			if (!(ResolveLimbs() == null))
			{
				limbsShown = !limbsShown;
				ApplyLimbsCaption();
			}
		}

		private HiderLimbs ResolveLimbs()
		{
			if (limbs == null)
			{
				limbs = GetComponentInChildren<HiderLimbs>(includeInactive: true);
			}
			if (limbs == null)
			{
				return null;
			}
			if (limbs.enabled)
			{
				limbs.enabled = false;
			}
			limbs.ApplyTuning(limbTuning);
			return limbs;
		}

		private void ApplyLimbsCaption()
		{
			if (!(limbsButton == null))
			{
				TMP_Text componentInChildren = limbsButton.GetComponentInChildren<TMP_Text>(includeInactive: true);
				if (componentInChildren != null)
				{
					componentInChildren.text = Loc.Get(limbsShown ? "Mimic.LimbsHide" : "Mimic.LimbsShow");
				}
			}
		}

		public void NotifyBodyChanged()
		{
			if (!(limbs == null))
			{
				MimicLimbBody component = limbs.GetComponent<MimicLimbBody>();
				if (component != null)
				{
					component.MarkChanged();
				}
			}
		}

		public void ToggleVoicePanel()
		{
			GameObject gameObject = ResolveVoicePanel();
			if (gameObject != null)
			{
				gameObject.SetActive(!gameObject.activeSelf);
			}
		}

		private GameObject ResolveVoicePanel()
		{
			if (voicePanel != null)
			{
				return voicePanel;
			}
			ModelerVoiceView componentInChildren = GetComponentInChildren<ModelerVoiceView>(includeInactive: true);
			if (componentInChildren != null)
			{
				voicePanel = componentInChildren.gameObject;
			}
			return voicePanel;
		}

		public void ResetModel()
		{
			LoadStartupMimic();
			ModelEditSession.MarkSaved();
		}

		public void SaveModel()
		{
			if (saveBar != null)
			{
				saveBar.Save();
			}
		}

		public void ApplyPrimitive(VoxelPrimitive shape)
		{
			if (mimic == null || mimic.Model == null)
			{
				return;
			}
			if (shape == VoxelPrimitive.StartingModel)
			{
				ResetModel();
				return;
			}
			VoxelGrid voxelGrid = VoxelPrimitives.Build(shape, mimic.Model.InitialSize, mimic.Model.InitialColor);
			if (voxelGrid == null)
			{
				return;
			}
			PutLimbsDown();
			RemoveExtraPieces();
			VoxelGrid grid = mimic.Model.Grid;
			grid.Clear();
			foreach (KeyValuePair<Vector3Int, VoxelData> voxel in voxelGrid.Voxels)
			{
				grid.Set(voxel.Key, voxel.Value);
			}
			mimic.Model.RebuildMesh();
			UndoManager.Clear();
			ModelEditSession.MarkChanged();
			NotifyBodyChanged();
		}

		private void RemoveExtraPieces()
		{
			foreach (VoxelEditorController item in VoxelFocusManager.GetAllUsable())
			{
				if (item != mimic)
				{
					item.gameObject.SetActive(value: false);
				}
			}
			VoxelFocusManager.SetStickyFocus(mimic);
		}

		private IEnumerator OpenRoutine()
		{
			yield return LoadEditorScene();
			ApplyBodyScale();
			CaptureRestPose();
			LoadStartupMimic();
			SetEditing(editing: true);
			BindPanels();
		}

		private void ApplyBodyScale()
		{
			if (!(mimic == null) && !(mimic.Model == null))
			{
				mimic.Model.ConfigureVoxelSize(bodyVoxelSize);
				mimic.Model.ConfigureInitialSize(bodyInitialSize);
				gizmoSizes.ApplyTo(mimic);
			}
		}

		private void CaptureRestPose()
		{
			if (!(mimic == null) && !hasRestPose)
			{
				restLocalPosition = mimic.transform.localPosition;
				restLocalRotation = mimic.transform.localRotation;
				hasRestPose = true;
			}
		}

		public void PutLimbsDown()
		{
			if (limbs != null)
			{
				limbs.DevCollapse();
			}
		}

		private void BindPanels()
		{
			if (saveBar == null)
			{
				saveBar = GetComponentInChildren<MimicSaveBarView>(includeInactive: true);
			}
			if (saveBar != null)
			{
				saveBar.Bind(this);
			}
		}

		public void LoadStartupMimic()
		{
			if (mimic == null)
			{
				return;
			}
			PutLimbsDown();
			RemoveExtraPieces();
			string text = StartingMimic.Resolve();
			if (!string.IsNullOrEmpty(text))
			{
				foreach (TemplateListEntry item in StartingMimic.List())
				{
					if (!(item.FilePath != text))
					{
						if (LoadTemplate(item))
						{
							return;
						}
						break;
					}
				}
			}
			NewMimic();
		}

		public bool LoadTemplate(TemplateListEntry entry)
		{
			if (mimic == null || mimic.Model == null)
			{
				return false;
			}
			TemplateModel templateModel = TemplateStorage.LoadTemplate(entry.FilePath);
			if (templateModel == null || templateModel.Pieces.Count == 0)
			{
				return false;
			}
			PutLimbsDown();
			RemoveExtraPieces();
			if (hasRestPose)
			{
				mimic.transform.SetLocalPositionAndRotation(restLocalPosition, restLocalRotation);
			}
			TemplateStorage.ApplyPiece(templateModel.Pieces[0], mimic.Model.Grid);
			mimic.Model.RebuildMesh();
			UndoManager.Clear();
			VoxelEditorSettings.CurrentTemplateFilePath = entry.FilePath;
			VoxelEditorSettings.CurrentTemplateName = entry.ModelName;
			VoxelEditorSettings.CurrentTemplateTag = entry.Tag;
			VoxelEditorSettings.CurrentTemplateCategory = entry.Category;
			if (templateModel.Pieces.Count > 1 && ToastView.Instance != null)
			{
				ToastView.Instance.Show(Loc.Format("Mimic.MultiPieceTrimmed", templateModel.Pieces.Count), 5f);
			}
			NotifyBodyChanged();
			return true;
		}

		public void NewMimic()
		{
			if (!(mimic == null))
			{
				PutLimbsDown();
				RemoveExtraPieces();
				if (hasRestPose)
				{
					mimic.transform.SetLocalPositionAndRotation(restLocalPosition, restLocalRotation);
				}
				else
				{
					mimic.transform.localRotation = Quaternion.identity;
				}
				mimic.Model.ResetToDefaultCube();
				mimic.Model.DisplayName = null;
				TemplateSession.ForgetOpenTemplate();
				UndoManager.Clear();
				StartingMimic.Clear();
				ModelEditSession.MarkSaved();
				NotifyBodyChanged();
			}
		}

		public void Close()
		{
			if (!GuardUnsaved(Close))
			{
				return;
			}
			if (showOnClose != null)
			{
				GameObject[] array = showOnClose;
				foreach (GameObject gameObject in array)
				{
					if (gameObject != null)
					{
						gameObject.SetActive(value: true);
					}
				}
			}
			base.gameObject.SetActive(value: false);
		}

		private void CloseFromEscape()
		{
			if (closeButton != null && closeButton.isActiveAndEnabled)
			{
				closeButton.onClick.Invoke();
			}
			else
			{
				Close();
			}
		}

		private bool GuardUnsaved(Action proceed)
		{
			if (!ModelEditSession.HasUnsavedChanges)
			{
				return true;
			}
			DialogView.Show(new DialogRequest(Loc.Get("UnsavedChanges"), Loc.Get("UnsavedChanges.Save"), Loc.Get("Common.Cancel"), Loc.Get("UnsavedChanges.Discard"), wantsInput: false, null, null, inputIsPassword: false, requiresInput: false, alternateIsDestructive: true), delegate(DialogAnswer answer, string _)
			{
				switch (answer)
				{
				case DialogAnswer.Confirm:
					SaveModel();
					proceed();
					break;
				case DialogAnswer.Alternate:
					ModelEditSession.MarkSaved();
					proceed();
					break;
				}
			});
			return false;
		}

		private void SetEditing(bool editing)
		{
			if (mimic != null)
			{
				mimic.enabled = editing;
				mimic.AllowTransform = !editing;
			}
			if (editing && VoxelEditorSettings.CurrentTool == EditorState.Transform)
			{
				VoxelEditorSettings.CurrentTool = EditorState.Extrude;
			}
			VoxelEditorSettings.MenuEditing = editing;
			VoxelEditorSettings.SinglePieceEditing = editing;
			VoxelEditorSettings.EditorCamera = (editing ? editorCamera : null);
			SetCameraLive(editing);
			SetDepthOfFieldEnabled(!editing);
			if (!editing)
			{
				return;
			}
			VoxelEditorSettings.IsMovementMode = false;
			if (!(OrbitCamera == null))
			{
				OrbitCamera.SetMinDistance(closeZoomDistance);
				OrbitCamera.IgnoreRoot = ((mimic != null) ? mimic.transform : null);
				OrbitCamera.CollisionEnabled = false;
				if (!hasFramed && !(mimic == null) && !(mimic.Model == null))
				{
					hasFramed = true;
					OrbitCamera.Frame(mimic.transform.TransformPoint(mimic.Model.GetCurrentBoundsCenterLocal()), mimic.Model.InitialWorldSize);
				}
			}
		}

		private void SetCameraLive(bool live)
		{
			if (editorCamera == null)
			{
				return;
			}
			if (live)
			{
				if (!cameraLive)
				{
					restoreCameraActive = editorCamera.gameObject.activeSelf;
					restoreCameraEnabled = editorCamera.enabled;
					restoreOrbitEnabled = OrbitCamera != null && OrbitCamera.enabled;
					restoreCameraPosition = editorCamera.transform.position;
					restoreCameraRotation = editorCamera.transform.rotation;
				}
				editorCamera.gameObject.SetActive(value: true);
				editorCamera.enabled = true;
				if (OrbitCamera != null)
				{
					OrbitCamera.enabled = true;
				}
			}
			else
			{
				if (orbitCamera != null)
				{
					orbitCamera.enabled = restoreOrbitEnabled;
				}
				editorCamera.transform.SetPositionAndRotation(restoreCameraPosition, restoreCameraRotation);
				editorCamera.enabled = restoreCameraEnabled;
				editorCamera.gameObject.SetActive(restoreCameraActive);
			}
			cameraLive = live;
		}

		private void SetDepthOfFieldEnabled(bool enabled)
		{
			if (enabled)
			{
				foreach (KeyValuePair<DepthOfField, bool> item in suppressedDepthOfField)
				{
					if (item.Key != null)
					{
						item.Key.active = item.Value;
					}
				}
				suppressedDepthOfField.Clear();
				return;
			}
			Volume[] array = ((depthOfFieldVolumes != null && depthOfFieldVolumes.Length != 0) ? depthOfFieldVolumes : UnityEngine.Object.FindObjectsByType<Volume>(FindObjectsInactive.Exclude, FindObjectsSortMode.None));
			foreach (Volume volume in array)
			{
				if (!(volume == null) && !(volume.sharedProfile == null) && volume.profile.TryGet<DepthOfField>(out var component))
				{
					if (!suppressedDepthOfField.ContainsKey(component))
					{
						suppressedDepthOfField[component] = component.active;
					}
					component.active = false;
				}
			}
		}

		private IEnumerator LoadEditorScene()
		{
			if (string.IsNullOrWhiteSpace(editorSceneName))
			{
				yield break;
			}
			Scene sceneByName = SceneManager.GetSceneByName(editorSceneName);
			if (sceneByName.IsValid() && sceneByName.isLoaded)
			{
				loadedEditorScene = false;
				yield break;
			}
			AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(editorSceneName, LoadSceneMode.Additive);
			if (asyncOperation == null)
			{
				Debug.LogError("[MimicCustomizationView] '" + editorSceneName + "' yuklenemedi - sahne Build Settings'te ekli mi?", this);
				yield break;
			}
			yield return asyncOperation;
			loadedEditorScene = true;
		}

		private void UnloadEditorScene()
		{
			if (loadedEditorScene && !string.IsNullOrWhiteSpace(editorSceneName))
			{
				loadedEditorScene = false;
				Scene sceneByName = SceneManager.GetSceneByName(editorSceneName);
				if (sceneByName.IsValid() && sceneByName.isLoaded)
				{
					SceneManager.UnloadSceneAsync(sceneByName);
				}
			}
		}
	}
}
