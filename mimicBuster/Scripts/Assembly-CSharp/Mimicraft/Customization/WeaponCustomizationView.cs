using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Mimicraft.Analytics;
using Mimicraft.Cameras;
using Mimicraft.Gameplay;
using Mimicraft.Localization;
using Mimicraft.Settings;
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
	public class WeaponCustomizationView : MonoBehaviour, IModelEditSession
	{
		[Tooltip("Düzenlenecek silahlar - sahneye elle yerleştirdiğin modeller. Her birinin üzerinde WeaponSkinAssembler olmalı.")]
		[SerializeField]
		private WeaponSkinAssembler[] weapons;

		[Tooltip("Silahları gösteren ve döndürülen kamera. OrbitCamera bileşeni de bunun üzerinde olmalı.")]
		[SerializeField]
		private Camera editorCamera;

		[Tooltip("Additive olarak yüklenecek editör sahnesi - karakter ekranıyla AYNI sahne.")]
		[AssetSelectorPopup("SceneAsset", false)]
		[SerializeField]
		private string editorSceneName = "Editor";

		[Tooltip("Bu ekran kapanınca geri açılacak objeler - ana menü paneli.")]
		[SerializeField]
		private GameObject[] showOnClose;

		[Tooltip("Bu ekranın kapatma butonu. Bağlanırsa ESC tam olarak bu butona basar - yani klavyeyle çıkmak ile tıklayarak çıkmak aynı şeyi yapar. Boş bırakılırsa ESC sadece ekranı kapatır ve menüyü geri getirmek Show On Close'un işi olur.")]
		[SerializeField]
		private Button closeButton;

		[Tooltip("Escape ile çıkılabilsin mi.")]
		[SerializeField]
		private bool closeOnEscape = true;

		[Tooltip("Bu ekran açıkken Depth of Field'ı kapatılacak Volume'lar. Boş bırakılırsa sahnedeki bütün Volume'lar taranır.")]
		[SerializeField]
		private Volume[] depthOfFieldVolumes;

		[Tooltip("Kaydetme çubuğu - senin tasarladığın obje. Boş bırakılırsa bu objenin altında aranır, yani çubuğu buranın çocuğu yapmak yeterli.")]
		[SerializeField]
		private WeaponSaveBarView saveBar;

		[Header("Ses Tipi")]
		[Tooltip("Basınca seçili silahın ses tipini Normal/Susturuculu arasında değiştiren buton. Karakterlerdeki Erkek/Kadın ses düğmesinin silah karşılığı.")]
		[SerializeField]
		private Button soundTypeButton;

		[Tooltip("Ses tipi NORMALKEN butonda görünen sprite.")]
		[SerializeField]
		private Sprite normalSoundSprite;

		[Tooltip("Ses tipi SUSTURUCULUYKEN butonda görünen sprite.")]
		[SerializeField]
		private Sprite suppressedSoundSprite;

		[Tooltip("Sprite'ın konacağı Image. Boş bırakılırsa butonun kendi Image'ı kullanılır - ikon butonun içinde ayrı bir çocuksa onu buraya sürükle.")]
		[SerializeField]
		private Image soundTypeIcon;

		[Tooltip("Basınca tezgâhtaki silahın bir atışını oynatan buton: sesi ve namlu alevi birlikte.\n\nSes, SEÇİLİ ses tipinden gelir ve oyundaki atışın aynısıdır - aynı klip havuzundan rastgele biri, silahın kendi ses seviyesiyle. Alev, modelin kendi MuzzleFlash'ıdır ve Muzzle noktasında oynar; noktayı sürükleyince alev de oraya gider.")]
		[SerializeField]
		private Button previewButton;

		[Tooltip("Hazir model listesi. Bos birakilirsa bu ekranin altinda aranir; hic yoksa preset butonu hicbir sey yapmaz.")]
		[SerializeField]
		private PresetPickerView presetPicker;

		[Header("Referans noktaları")]
		[Tooltip("Tutamak oklarının uzunluğu, metre.")]
		[SerializeField]
		[Min(0.01f)]
		private float handleArmLength = 0.12f;

		[Tooltip("Noktaların hangi adıma oturacağı, metre. Boş bırak: noktalar yarım voxel adımlarla modelin kendi gridine oturur, yani ya bir küpün köşesine ya da tam ortasına - arada bir yere düşemezler. Değer yazarsan adım o olur, başlangıcı yine modelin köşesidir.")]
		[SerializeField]
		[Min(0f)]
		private float handleSnap;

		[Tooltip("Tutamak oklarının kalınlığı. 1 = editörün normal oku; referans noktaları için çok kalın kalıyor, 0.2 civarı iyi bir başlangıç.")]
		[SerializeField]
		[Min(0.01f)]
		private float handleThickness = 0.25f;

		[Tooltip("Noktanın kendisini gösteren kürenin çapı, metre. 0 = küre yok.")]
		[SerializeField]
		[Min(0f)]
		private float handlePointSize = 0.015f;

		[Tooltip("Üzerine gelince çıkan isim yazısının büyüklüğü. 0 = yazı yok.")]
		[SerializeField]
		[Min(0f)]
		private float handleLabelSize = 0.02f;

		[Tooltip("Nokta isimlerinin fontu. Boş bırakılırsa TMP'nin kendi varsayılanı kullanılır.\n\nÖlçü yazıları (kutu kenarlarındaki sayılar) buradan gelmiyor - onlar Editor sahnesindeki DimensionLabels bileşeninin kendi Font Override alanından geliyor, çünkü o bileşen sahne genelinde tek ve iki ekran tarafından da paylaşılıyor.")]
		[SerializeField]
		private TMP_FontAsset handleLabelFont;

		[Header("Nokta ikonları")]
		[Tooltip("Sol El noktasının merkezinde gösterilecek ikon. Atanınca o noktanın küresi gizlenir, ikon onun yerini alır. Boşsa küre olduğu gibi kalır.")]
		[SerializeField]
		private Sprite leftHandIcon;

		[Tooltip("Sağ El noktasının ikonu.")]
		[SerializeField]
		private Sprite rightHandIcon;

		[Tooltip("Namlu noktasının ikonu.")]
		[SerializeField]
		private Sprite muzzleIcon;

		[Tooltip("ADS Pos noktasının ikonu.")]
		[SerializeField]
		private Sprite adsPosIcon;

		[Tooltip("İkonların büyüklüğü - uzun kenarı, metre. Sprite'ın kendi piksel boyutundan ve Pixels Per Unit ayarından bağımsız. Her zaman modelin üstünde çizilir. 0 = ikon yok.")]
		[SerializeField]
		[Min(0f)]
		private float handleIconSize = 0.03f;

		[Header("Editör gizmo'ları")]
		[Tooltip("Silah düzenlenirken kullanılacak gizmo çarpanları. Silahın kendi prefabinde değil burada: bunlar bu EKRANIN özelliği, her silahın değil - prefabe koymak aynı dört sayıyı silah başına bir kez girmek demek olurdu.")]
		[SerializeField]
		private EditorGizmoSizes gizmoSizes = new EditorGizmoSizes();

		private readonly Dictionary<DepthOfField, bool> suppressedDepthOfField = new Dictionary<DepthOfField, bool>();

		private OrbitCamera orbitCamera;

		private bool loadedEditorScene;

		private bool cameraLive;

		private bool restoreCameraActive = true;

		private bool restoreCameraEnabled;

		private bool restoreOrbitEnabled;

		private Vector3 restoreCameraPosition;

		private Quaternion restoreCameraRotation = Quaternion.identity;

		private PointHandle leftHandle;

		private PointHandle rightHandle;

		private PointHandle muzzleHandle;

		private PointHandle adsHandle;

		private bool hasFramed;

		private bool warnedAboutNoReturn;

		private string[] openPaths;

		private string[] openNames;

		private WeaponSkinData[] openOriginals;

		private AudioSource previewSource;

		private UITooltipTrigger soundTooltip;

		private bool warnedMissingSprite;

		private bool warnedAboutIds;

		public int SelectedIndex { get; private set; }

		public WeaponSkinAssembler Selected
		{
			get
			{
				if (SelectedIndex < 0 || SelectedIndex >= WeaponCount)
				{
					return null;
				}
				return weapons[SelectedIndex];
			}
		}

		public string SelectedName
		{
			get
			{
				if (!(Selected != null))
				{
					return "";
				}
				return Selected.DisplayName;
			}
		}

		public bool CanResetModel
		{
			get
			{
				if (SelectedIndex >= 0 && SelectedIndex < WeaponCount)
				{
					return Selected != null;
				}
				return false;
			}
		}

		public bool SupportsPrimitives => false;

		private int WeaponCount
		{
			get
			{
				if (weapons == null)
				{
					return 0;
				}
				return weapons.Length;
			}
		}

		public string CurrentFilePath
		{
			get
			{
				EnsureLibraryState();
				if (SelectedIndex < 0 || SelectedIndex >= WeaponCount)
				{
					return "";
				}
				return openPaths[SelectedIndex];
			}
		}

		public string CurrentSkinName
		{
			get
			{
				EnsureLibraryState();
				if (SelectedIndex < 0 || SelectedIndex >= WeaponCount)
				{
					return "";
				}
				string text = openNames[SelectedIndex];
				if (!string.IsNullOrEmpty(text))
				{
					return text;
				}
				return SelectedName;
			}
		}

		public WeaponSoundKind CurrentSound
		{
			get
			{
				if (!(Selected != null))
				{
					return WeaponSoundKind.Normal;
				}
				return Selected.Sound;
			}
		}

		public string SelectedWeaponId => SelectedId();

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
			WireSoundControls();
			StartCoroutine(OpenRoutine());
			Telemetry.Send("menu_action", ("menu_item", "customize_weapon"), ("session_seconds", Telemetry.SessionSeconds));
		}

		private void OnDisable()
		{
			hasFramed = false;
			ModelEditSession.End(this);
			DestroyHandles();
			SetEditing(editing: false);
			for (int i = 0; i < WeaponCount; i++)
			{
				if (weapons[i] != null)
				{
					weapons[i].gameObject.SetActive(value: true);
				}
			}
			UnloadEditorScene();
		}

		private IEnumerator OpenRoutine()
		{
			yield return LoadEditorScene();
			LoadSaved();
			Select(SelectedIndex);
			BindSaveBar();
		}

		public void Select(int index)
		{
			if (WeaponCount == 0 || (index != SelectedIndex && !GuardUnsaved(delegate
			{
				Select(index);
			})))
			{
				return;
			}
			SelectedIndex = Mathf.Clamp(index, 0, WeaponCount - 1);
			if (presetPicker != null && presetPicker.IsOpen && !presetPicker.IsOpenFor(SelectedIndex))
			{
				presetPicker.Hide();
			}
			for (int num = 0; num < WeaponCount; num++)
			{
				if (!(weapons[num] == null))
				{
					bool flag = num == SelectedIndex;
					weapons[num].SetEditable(flag, editorCamera, gizmoSizes);
					weapons[num].gameObject.SetActive(flag);
				}
			}
			SetEditing(editing: true);
			BuildHandles();
			if (!hasFramed)
			{
				hasFramed = true;
				FrameSelected();
			}
			RefreshSoundControls();
		}

		private void FrameSelected()
		{
			WeaponSkinAssembler selected = Selected;
			if (!(selected == null) && !(OrbitCamera == null))
			{
				WeaponSkinBox box = selected.Box;
				Vector3 point = selected.transform.position;
				float num = 0.5f;
				if (box != null)
				{
					Vector3 vector = (Vector3)box.BoxSize * box.VoxelSize;
					point = selected.transform.TransformPoint(box.LocalCorner + vector * 0.5f);
					num = vector.magnitude * 0.5f;
				}
				OrbitCamera.IgnoreRoot = selected.transform;
				OrbitCamera.CollisionEnabled = false;
				OrbitCamera.SetDistanceLimits(num * 0.6f, num * 12f);
				OrbitCamera.Frame(point, num);
			}
		}

		public void SelectNext()
		{
			Select((WeaponCount != 0) ? ((SelectedIndex + 1) % WeaponCount) : 0);
		}

		public void SelectPrevious()
		{
			Select((WeaponCount != 0) ? ((SelectedIndex + WeaponCount - 1) % WeaponCount) : 0);
		}

		private void BuildHandles()
		{
			DestroyHandles();
			WeaponSkinAssembler weapon = Selected;
			if (weapon == null)
			{
				return;
			}
			Transform boxSpace = weapon.BoxSpace;
			if (boxSpace == null)
			{
				return;
			}
			float snap = ((handleSnap > 0f) ? handleSnap : weapon.PointGridStep);
			Vector3 pointGridOrigin = weapon.PointGridOrigin;
			Vector3 vector = ((weapon.Box != null) ? weapon.Box.LocalCorner : Vector3.zero);
			Vector3 vector2 = ((weapon.Box != null) ? (weapon.Box.LocalCorner + (Vector3)weapon.Box.BoxSize * weapon.Box.VoxelSize) : Vector3.zero);
			leftHandle = PointHandle.Create(boxSpace, Loc.Get("Socket.LeftHand"), new Color(0.5f, 0.9f, 1f), handleArmLength, handleThickness, handlePointSize, handleLabelSize, snap, handleLabelFont);
			rightHandle = PointHandle.Create(boxSpace, Loc.Get("Socket.RightHand"), new Color(1f, 0.75f, 0.4f), handleArmLength, handleThickness, handlePointSize, handleLabelSize, snap, handleLabelFont);
			muzzleHandle = PointHandle.Create(boxSpace, Loc.Get("Socket.Muzzle"), new Color(1f, 0.4f, 0.4f), handleArmLength, handleThickness, handlePointSize, handleLabelSize, snap, handleLabelFont);
			leftHandle.SetSnapOrigin(pointGridOrigin);
			rightHandle.SetSnapOrigin(pointGridOrigin);
			muzzleHandle.SetSnapOrigin(pointGridOrigin);
			leftHandle.SetIcon(leftHandIcon, handleIconSize);
			rightHandle.SetIcon(rightHandIcon, handleIconSize);
			muzzleHandle.SetIcon(muzzleIcon, handleIconSize);
			if (vector2 != vector)
			{
				leftHandle.SetBounds(vector, vector2);
				rightHandle.SetBounds(vector, vector2);
				muzzleHandle.SetBounds(vector, vector2);
			}
			leftHandle.SetPoint(weapon.LeftGrip);
			rightHandle.SetPoint(weapon.RightGrip);
			muzzleHandle.SetPoint(weapon.Muzzle);
			PointHandle pointHandle = leftHandle;
			pointHandle.Moved = (Action<Vector3>)Delegate.Combine(pointHandle.Moved, (Action<Vector3>)delegate(Vector3 p)
			{
				weapon.LeftGrip = p;
			});
			PointHandle pointHandle2 = rightHandle;
			pointHandle2.Moved = (Action<Vector3>)Delegate.Combine(pointHandle2.Moved, (Action<Vector3>)delegate(Vector3 p)
			{
				weapon.RightGrip = p;
			});
			PointHandle pointHandle3 = muzzleHandle;
			pointHandle3.Moved = (Action<Vector3>)Delegate.Combine(pointHandle3.Moved, (Action<Vector3>)delegate(Vector3 p)
			{
				weapon.Muzzle = p;
			});
			if (Features.AimDownSights)
			{
				adsHandle = PointHandle.Create(boxSpace, Loc.Get("Socket.AdsPos"), new Color(0.6f, 1f, 0.5f), handleArmLength, handleThickness, handlePointSize, handleLabelSize, snap, handleLabelFont);
				adsHandle.SetSnapOrigin(pointGridOrigin);
				if (vector2 != vector)
				{
					adsHandle.SetBounds(vector, vector2);
				}
				adsHandle.SetIcon(adsPosIcon, handleIconSize);
				adsHandle.SetPoint(weapon.AdsPos);
				PointHandle pointHandle4 = adsHandle;
				pointHandle4.Moved = (Action<Vector3>)Delegate.Combine(pointHandle4.Moved, (Action<Vector3>)delegate(Vector3 p)
				{
					weapon.AdsPos = p;
				});
			}
		}

		private void DestroyHandles()
		{
			if (leftHandle != null)
			{
				UnityEngine.Object.Destroy(leftHandle.gameObject);
			}
			if (rightHandle != null)
			{
				UnityEngine.Object.Destroy(rightHandle.gameObject);
			}
			if (muzzleHandle != null)
			{
				UnityEngine.Object.Destroy(muzzleHandle.gameObject);
			}
			if (adsHandle != null)
			{
				UnityEngine.Object.Destroy(adsHandle.gameObject);
			}
			leftHandle = null;
			rightHandle = null;
			muzzleHandle = null;
			adsHandle = null;
		}

		private void LateUpdate()
		{
			if (VoxelEditorSettings.CurrentTool != EditorState.Transform)
			{
				leftHandle?.Hide();
				rightHandle?.Hide();
				muzzleHandle?.Hide();
				adsHandle?.Hide();
				return;
			}
			leftHandle?.Tick(editorCamera);
			rightHandle?.Tick(editorCamera);
			muzzleHandle?.Tick(editorCamera);
			WeaponSkinAssembler selected = Selected;
			if (adsHandle != null && selected != null && !adsHandle.IsDragging && !selected.HasPlacedAdsPos)
			{
				adsHandle.SetPoint(selected.AdsPos);
			}
			adsHandle?.Tick(editorCamera);
		}

		private void BindSaveBar()
		{
			if (saveBar == null)
			{
				saveBar = GetComponentInChildren<WeaponSaveBarView>(includeInactive: true);
			}
			if (presetPicker == null)
			{
				presetPicker = GetComponentInChildren<PresetPickerView>(includeInactive: true);
			}
			if (saveBar != null)
			{
				saveBar.Bind(this);
			}
		}

		private void LoadSaved()
		{
			for (int i = 0; i < WeaponCount; i++)
			{
				LoadOne(i);
			}
		}

		private void LoadOne(int index)
		{
			WeaponSkinAssembler weaponSkinAssembler = weapons[index];
			if (weaponSkinAssembler == null)
			{
				return;
			}
			EnsureLibraryState();
			string text = IdOf(index);
			WeaponSkinData weaponSkinData = null;
			if (!string.IsNullOrEmpty(text))
			{
				string text2 = WeaponSkinStorage.SelectedPath(text);
				if (!string.IsNullOrEmpty(text2))
				{
					weaponSkinData = WeaponSkinStorage.LoadFile(text2, out var _, out var skinName);
					if (weaponSkinData != null)
					{
						openPaths[index] = text2;
						openNames[index] = skinName;
					}
				}
				if (weaponSkinData == null)
				{
					weaponSkinData = WeaponSkinStorage.Load(text);
				}
			}
			weaponSkinAssembler.Apply(weaponSkinData ?? weaponSkinAssembler.DefaultSkin);
		}

		public void Save()
		{
			SaveSkin();
		}

		public void ShowPresets()
		{
			WeaponSkinAssembler selected = Selected;
			if (presetPicker == null || selected == null)
			{
				return;
			}
			IReadOnlyList<WeaponSkinDefinition> presets = selected.Presets;
			if (presets.Count == 0)
			{
				return;
			}
			List<PresetPickerData> list = new List<PresetPickerData>(presets.Count);
			foreach (WeaponSkinDefinition item in presets)
			{
				list.Add((item != null) ? new PresetPickerData
				{
					name = item.DisplayName,
					icon = item.icon
				} : new PresetPickerData
				{
					name = "-",
					icon = null
				});
			}
			int index = SelectedIndex;
			presetPicker.Toggle(index, Loc.Format("Preset.WeaponTitle", selected.DisplayName), list, delegate(int chosen)
			{
				ApplyPreset(index, chosen);
			});
		}

		private void ApplyPreset(int weaponIndex, int presetIndex)
		{
			EnsureLibraryState();
			if (weaponIndex < 0 || weaponIndex >= WeaponCount)
			{
				return;
			}
			WeaponSkinAssembler weaponSkinAssembler = weapons[weaponIndex];
			if (weaponSkinAssembler == null)
			{
				return;
			}
			IReadOnlyList<WeaponSkinDefinition> presets = weaponSkinAssembler.Presets;
			if (presetIndex < 0 || presetIndex >= presets.Count || presets[presetIndex] == null)
			{
				return;
			}
			WeaponSkinDefinition weaponSkinDefinition = presets[presetIndex];
			if (!GuardUnsaved(delegate
			{
				ApplyPreset(weaponIndex, presetIndex);
			}))
			{
				return;
			}
			if (weaponSkinDefinition.asset == null)
			{
				Debug.LogWarning("[WeaponCustomizationView] '" + weaponSkinAssembler.DisplayName + "' silahinin '" + weaponSkinDefinition.name + "' preseti bos - WeaponDefinition'daki Skin Presets listesinde Asset alanini doldur.", this);
				return;
			}
			weaponSkinAssembler.Apply(weaponSkinDefinition.asset.ToSkinData());
			openPaths[weaponIndex] = null;
			string displayName = weaponSkinDefinition.DisplayName;
			openNames[weaponIndex] = (string.IsNullOrWhiteSpace(displayName) ? weaponSkinAssembler.DisplayName : displayName);
			openOriginals[weaponIndex] = weaponSkinDefinition.asset.ToSkinData();
			ModelEditSession.MarkSaved();
			Select(weaponIndex);
			if (saveBar != null)
			{
				saveBar.RefreshForWeapon(Loc.Format("WeaponSkin.PresetStarted", openNames[weaponIndex]));
			}
		}

		public void ResetSelected()
		{
			string text = ((SelectedIndex >= 0 && SelectedIndex < WeaponCount) ? IdOf(SelectedIndex) : "");
			if (!string.IsNullOrEmpty(text))
			{
				WeaponSkinStorage.Delete(text);
				LoadOne(SelectedIndex);
				Select(SelectedIndex);
			}
		}

		public void ResetAll()
		{
			for (int i = 0; i < WeaponCount; i++)
			{
				string text = IdOf(i);
				if (!string.IsNullOrEmpty(text))
				{
					WeaponSkinStorage.Delete(text);
				}
			}
			SetEditing(editing: false);
			LoadSaved();
			Select(SelectedIndex);
		}

		public void ResetModel()
		{
			EnsureLibraryState();
			if (CanResetModel)
			{
				int selectedIndex = SelectedIndex;
				Selected.Apply(openOriginals[selectedIndex] ?? Selected.DefaultSkin);
				Select(selectedIndex);
				ModelEditSession.MarkSaved();
			}
		}

		public void SaveModel()
		{
			SaveSkin();
		}

		public void ApplyPrimitive(VoxelPrimitive shape)
		{
		}

		[Tooltip("Kaydedilmemiş değişiklik varken çıkarken veya başka bir model açarken çıkacak üç butonlu kutu. Boş bırakılırsa bu ekranın altında aranır; hiç yoksa uyarı gösterilmez ve değişiklikler sessizce kaybolur.")]
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

		private void CloseFromEscape()
		{
			if (closeButton != null && closeButton.isActiveAndEnabled)
			{
				closeButton.onClick.Invoke();
				return;
			}
			WarnIfNothingComesBack();
			Close();
		}

		private void WarnIfNothingComesBack()
		{
			if (warnedAboutNoReturn || closeButton != null)
			{
				return;
			}
			bool flag = false;
			if (showOnClose != null)
			{
				GameObject[] array = showOnClose;
				foreach (GameObject gameObject in array)
				{
					flag |= gameObject != null;
				}
			}
			if (!flag)
			{
				warnedAboutNoReturn = true;
				Debug.LogWarning("[" + GetType().Name + "] ESC bu ekrani kapatiyor ama geri gelecek hicbir sey yok - Close Button ya da Show On Close alanini doldur.", this);
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

		private void Update()
		{
			if (closeOnEscape && Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
			{
				GameMenuState.RequestEscape(20, CloseFromEscape);
			}
		}

		private void SetEditing(bool editing)
		{
			if (!editing)
			{
				for (int i = 0; i < WeaponCount; i++)
				{
					if (weapons[i] != null)
					{
						weapons[i].SetEditable(editable: false, editorCamera);
					}
				}
			}
			VoxelEditorSettings.MenuEditing = editing;
			VoxelEditorSettings.StandaloneEditing = editing;
			VoxelEditorSettings.EditorCamera = (editing ? editorCamera : null);
			SetCameraLive(editing);
			SetDepthOfFieldEnabled(!editing);
			if (editing)
			{
				VoxelEditorSettings.IsMovementMode = false;
			}
		}

		private void EnsureLibraryState()
		{
			if (openPaths == null || openPaths.Length != WeaponCount || openOriginals == null || openOriginals.Length != WeaponCount)
			{
				openPaths = new string[WeaponCount];
				openNames = new string[WeaponCount];
				openOriginals = new WeaponSkinData[WeaponCount];
			}
		}

		public void SetSkinName(string name)
		{
			EnsureLibraryState();
			if (SelectedIndex >= 0 && SelectedIndex < WeaponCount)
			{
				openNames[SelectedIndex] = name;
			}
		}

		public List<WeaponSkinListEntry> ListSkins()
		{
			string text = SelectedId();
			if (!string.IsNullOrEmpty(text))
			{
				return WeaponSkinStorage.List(text);
			}
			return new List<WeaponSkinListEntry>();
		}

		public void ToggleSound()
		{
			if (!(Selected == null))
			{
				Selected.Sound = ((Selected.Sound != WeaponSoundKind.Suppressed) ? WeaponSoundKind.Suppressed : WeaponSoundKind.Normal);
				ModelEditSession.MarkChanged();
				RefreshSoundControls();
			}
		}

		public void Preview()
		{
			WeaponSkinAssembler selected = Selected;
			WeaponDefinition weaponDefinition = ((selected != null) ? selected.Weapon : null);
			if (!(weaponDefinition == null))
			{
				WeaponVisual visual = selected.Visual;
				if (visual != null)
				{
					visual.PlayMuzzleFlash();
				}
				WeaponSoundKind currentSound = CurrentSound;
				AudioClip audioClip = weaponDefinition.PickFireClip(currentSound);
				if (audioClip == null)
				{
					audioClip = ((AudioLibrary.Instance != null) ? AudioLibrary.Instance.shotClip : null);
				}
				if (!(audioClip == null))
				{
					PreviewSource().PlayOneShot(audioClip, AudioLibrary.VolumeOf(audioClip) * weaponDefinition.FireVolumeFor(currentSound));
				}
			}
		}

		private AudioSource PreviewSource()
		{
			if (previewSource == null)
			{
				previewSource = base.gameObject.AddComponent<AudioSource>();
				previewSource.playOnAwake = false;
				previewSource.spatialBlend = 0f;
				SpatialAudio.Route(previewSource);
			}
			return previewSource;
		}

		public string PreviewReport()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("--- weapon preview ---");
			stringBuilder.AppendLine($"screen active: {base.isActiveAndEnabled}");
			if (previewButton == null)
			{
				stringBuilder.AppendLine("Preview Button: NOT ASSIGNED on this component.");
				stringBuilder.AppendLine("  Nothing is wired, so pressing anything in the scene cannot reach this code.");
			}
			else
			{
				stringBuilder.AppendLine("Preview Button: '" + previewButton.name + "' " + $"interactable={previewButton.interactable} " + $"activeInHierarchy={previewButton.gameObject.activeInHierarchy} " + $"listeners={previewButton.onClick.GetPersistentEventCount()} persistent " + "(runtime ones are not counted by Unity)");
			}
			WeaponSkinAssembler selected = Selected;
			stringBuilder.AppendLine($"weapons on bench: {WeaponCount}   selected index: {SelectedIndex}");
			if (selected == null)
			{
				stringBuilder.AppendLine("selected assembler: NULL - PreviewSound returns immediately.");
				stringBuilder.Append(AudioLibrary.OutputReport());
				return stringBuilder.ToString();
			}
			WeaponDefinition weapon = selected.Weapon;
			stringBuilder.AppendLine("selected: '" + selected.DisplayName + "'   definition: " + ((weapon != null) ? ("'" + weapon.name + "'") : "NULL - the assembler has no Weapon asset"));
			if (weapon == null)
			{
				stringBuilder.Append(AudioLibrary.OutputReport());
				return stringBuilder.ToString();
			}
			WeaponSoundKind currentSound = CurrentSound;
			AudioClip audioClip = weapon.PickFireClip(currentSound);
			stringBuilder.AppendLine($"sound kind: {currentSound}   weapon volume: {weapon.FireVolumeFor(currentSound):0.00}");
			stringBuilder.AppendLine((audioClip != null) ? $"clip picked: '{audioClip.name}' length={audioClip.length:0.00}s loadState={audioClip.loadState}" : "clip picked: NULL - this weapon has no Fire Clips, the shared library shot is used");
			WeaponVisual visual = selected.Visual;
			if (visual == null)
			{
				stringBuilder.AppendLine("muzzle flash: no WeaponVisual on the model - nothing to play");
			}
			else if (!visual.HasMuzzleFlash)
			{
				stringBuilder.AppendLine("muzzle flash: '" + visual.name + "' has no Muzzle Flash object assigned");
			}
			else
			{
				Transform muzzleFlashTransform = visual.MuzzleFlashTransform;
				stringBuilder.AppendLine("muzzle flash: '" + muzzleFlashTransform.name + "' parent='" + ((muzzleFlashTransform.parent != null) ? muzzleFlashTransform.parent.name : "none") + "' " + $"(Point_Muzzle means ApplyPoints moved it) lifetime={visual.MuzzleFlashLifetime:0.00}s");
				stringBuilder.AppendLine($"  world pos {muzzleFlashTransform.position:F2}   muzzle point {selected.Muzzle:F2}");
				stringBuilder.AppendLine($"  active={muzzleFlashTransform.gameObject.activeInHierarchy} (off between shots is correct) " + $"scale={muzzleFlashTransform.lossyScale:F2}");
				int layer = muzzleFlashTransform.gameObject.layer;
				Camera camera = editorCamera;
				bool flag = camera != null && (camera.cullingMask & (1 << layer)) != 0;
				stringBuilder.AppendLine($"  layer={layer} ({LayerMask.LayerToName(layer)})   " + ((camera == null) ? "no editor camera assigned" : (flag ? ("'" + camera.name + "' draws that layer") : ("'" + camera.name + "' DOES NOT draw that layer - it plays and nobody sees it"))));
			}
			AudioSource audioSource = PreviewSource();
			stringBuilder.AppendLine($"preview source: enabled={audioSource.enabled} mute={audioSource.mute} " + $"volume={audioSource.volume:0.00} spatialBlend={audioSource.spatialBlend:0.00}");
			stringBuilder.AppendLine("  mixer group: " + ((audioSource.outputAudioMixerGroup != null) ? audioSource.outputAudioMixerGroup.name : "NONE - master bus, the effects slider will not move it"));
			stringBuilder.Append(AudioLibrary.OutputReport());
			stringBuilder.AppendLine("playing it now...");
			Preview();
			return stringBuilder.ToString();
		}

		private void WireSoundControls()
		{
			if (previewButton != null)
			{
				previewButton.onClick.RemoveListener(Preview);
				previewButton.onClick.AddListener(Preview);
				UITooltipTrigger component = previewButton.GetComponent<UITooltipTrigger>();
				if (component != null && string.IsNullOrEmpty(component.Key))
				{
					component.Key = "Tooltip.WeaponPreview";
				}
			}
			if (soundTypeButton != null)
			{
				soundTypeButton.onClick.RemoveListener(ToggleSound);
				soundTypeButton.onClick.AddListener(ToggleSound);
				soundTooltip = soundTypeButton.GetComponent<UITooltipTrigger>();
				if (soundTooltip != null && string.IsNullOrEmpty(soundTooltip.Key))
				{
					soundTooltip.Key = "Tooltip.WeaponSound";
				}
			}
			RefreshSoundControls();
		}

		private void RefreshSoundControls()
		{
			Image image = ((soundTypeIcon != null) ? soundTypeIcon : ((soundTypeButton != null) ? soundTypeButton.image : null));
			bool flag = CurrentSound == WeaponSoundKind.Suppressed;
			Sprite sprite = (flag ? suppressedSoundSprite : normalSoundSprite);
			if (image != null && sprite != null)
			{
				image.sprite = sprite;
			}
			else if (image != null && !warnedMissingSprite)
			{
				warnedMissingSprite = true;
				Debug.LogWarning("[WeaponCustomizationView] '" + base.name + "' uzerinde " + (flag ? "Suppressed Sound Sprite" : "Normal Sound Sprite") + " alani bos - ses tipi ikonu degismeden kalir.", this);
			}
			if (soundTypeButton != null)
			{
				soundTypeButton.interactable = Selected != null;
			}
			if (previewButton != null)
			{
				previewButton.interactable = Selected != null;
			}
			if (!(soundTooltip == null))
			{
				soundTooltip.Suffix = Loc.Format("WeaponSound.Current", Loc.Get(flag ? "WeaponSound.Suppressed" : "WeaponSound.Normal"));
			}
		}

		public void SaveSkin()
		{
			EnsureLibraryState();
			string text = SelectedId();
			if (!string.IsNullOrEmpty(text) && !(Selected == null))
			{
				WeaponSkinData skin = Selected.Capture();
				string text2 = WeaponSkinStorage.SaveFile(openPaths[SelectedIndex], text, CurrentSkinName, skin);
				openPaths[SelectedIndex] = text2;
				WeaponSkinStorage.Select(text, text2);
				openOriginals[SelectedIndex] = WeaponSkinStorage.LoadFile(text2, out var _, out var _);
				ModelEditSession.MarkSaved();
				WeaponSkinPortraitService.Request(text2, text, skin);
			}
		}

		public void SaveAsNewSkin(string name)
		{
			EnsureLibraryState();
			if (!string.IsNullOrEmpty(SelectedId()) && !(Selected == null))
			{
				openNames[SelectedIndex] = name;
				openPaths[SelectedIndex] = null;
				SaveSkin();
			}
		}

		public void LoadSkin(string filePath)
		{
			EnsureLibraryState();
			string weaponId;
			string skinName;
			WeaponSkinData weaponSkinData = WeaponSkinStorage.LoadFile(filePath, out weaponId, out skinName);
			if (weaponSkinData == null || !GuardUnsaved(delegate
			{
				LoadSkin(filePath);
			}))
			{
				return;
			}
			int num = IndexOfWeapon(weaponId);
			if (num >= 0)
			{
				if (num != SelectedIndex)
				{
					Select(num);
				}
				openPaths[num] = filePath;
				openNames[num] = skinName;
				openOriginals[num] = WeaponSkinStorage.LoadFile(filePath, out var _, out var _);
				ModelEditSession.MarkSaved();
				weapons[num].Apply(weaponSkinData);
				WeaponSkinStorage.Select(weaponId, filePath);
				Select(num);
			}
		}

		public void DuplicateSkin(string filePath, string newName)
		{
			string weaponId;
			string skinName;
			WeaponSkinData weaponSkinData = WeaponSkinStorage.LoadFile(filePath, out weaponId, out skinName);
			if (weaponSkinData != null)
			{
				string text = WeaponSkinStorage.SaveFile(null, weaponId, newName, weaponSkinData);
				WeaponSkinPortraitService.Request(text, weaponId, weaponSkinData);
				LoadSkin(text);
			}
		}

		public void DeleteSkin(string filePath)
		{
			EnsureLibraryState();
			WeaponSkinStorage.DeleteFile(filePath);
			SavedThumbnails.Delete(filePath);
			for (int i = 0; i < WeaponCount; i++)
			{
				if (!(openPaths[i] != filePath))
				{
					openPaths[i] = null;
					openNames[i] = null;
					if (weapons[i] != null)
					{
						weapons[i].Apply(weapons[i].DefaultSkin);
					}
					if (i == SelectedIndex)
					{
						Select(i);
					}
				}
			}
		}

		private string SelectedId()
		{
			if (SelectedIndex < 0 || SelectedIndex >= WeaponCount)
			{
				return "";
			}
			return IdOf(SelectedIndex);
		}

		private int IndexOfWeapon(string weaponId)
		{
			for (int i = 0; i < WeaponCount; i++)
			{
				if (IdOf(i) == weaponId)
				{
					return i;
				}
			}
			return -1;
		}

		private string IdOf(int index)
		{
			string obj = ((weapons[index] != null) ? weapons[index].WeaponId : "");
			if (string.IsNullOrEmpty(obj) && !warnedAboutIds)
			{
				warnedAboutIds = true;
				Debug.LogWarning("[WeaponCustomizationView] Bir silahin WeaponSkinAssembler'inda Weapon alani bos - o silah kaydedilmeyecek.", this);
			}
			return obj;
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
				Debug.LogError("[WeaponCustomizationView] '" + editorSceneName + "' yuklenemedi - sahne Build Settings'te ekli mi?", this);
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
	}
}
