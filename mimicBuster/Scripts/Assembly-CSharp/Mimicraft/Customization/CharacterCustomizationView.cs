using System;
using System.Collections;
using System.Collections.Generic;
using Mimicraft.Analytics;
using Mimicraft.Cameras;
using Mimicraft.Localization;
using Mimicraft.UI;
using Mimicraft.VoxelEditor;
using Mimicraft.VoxelEditor.Core;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Mimicraft.Customization
{
	public class CharacterCustomizationView : MonoBehaviour, IModelEditSession
	{
		[Tooltip("Düzenlenecek karakter. Menü sahnesindeki örnek - oyundaki oyuncu değil.")]
		[SerializeField]
		private CharacterAssembler character;

		[Tooltip("Karakteri gösteren ve döndürülen kamera. OrbitCamera bileşeni de bunun üzerinde olmalı - yörünge zaten bir kamerayı hareket ettiriyor, ayrı bir obje değil.")]
		[SerializeField]
		private Camera editorCamera;

		[Tooltip("Additive olarak yüklenecek editör sahnesi - araç çubuğu, boya paneli ve şablon tarayıcısı orada yaşıyor. Oyun içi Modelci editörüyle AYNI sahne, bilerek: UI'ın ikinci bir kopyası, ikisini de aynı anda güncellemeyi hatırlamak demek olurdu. Boş bırakılırsa hiç yüklenmez ve editör UI'ının bu sahnede olması beklenir.")]
		[AssetSelectorPopup("SceneAsset", false)]
		[SerializeField]
		private string editorSceneName = "Editor";

		[Tooltip("Ekran açıldığında yüklenecek karakter dosyası. Boş bırakılırsa en son kaydedilen yüklenir; hiç kayıt yoksa varsayılan bloklarla başlanır.")]
		[SerializeField]
		private string startupFilePath = "";

		[Tooltip("Kameranın modele yaklaşabileceği en küçük mesafe. Karakter kamerasının kendi Inspector değeri bütün bir karakteri çerçevelemek için ayarlı; boyama ve küçük şekiller santimetrelerden bakmayı gerektiriyor.")]
		[SerializeField]
		[Min(0.01f)]
		private float closeZoomDistance = 0.25f;

		[Tooltip("Bu ekran kapanınca geri açılacak objeler - ana menü paneli. Boş bırakılırsa bu obje sadece kapanır ve menüyü geri getirmek başkasının işi olur.")]
		[SerializeField]
		private GameObject[] showOnClose;

		[Tooltip("Bu ekranın kapatma butonu. Bağlanırsa ESC tam olarak bu butona basar - yani klavyeyle çıkmak ile tıklayarak çıkmak aynı şeyi yapar. Boş bırakılırsa ESC sadece ekranı kapatır ve menüyü geri getirmek Show On Close'un işi olur.")]
		[SerializeField]
		private Button closeButton;

		[Tooltip("Escape ile çıkılabilsin mi.")]
		[SerializeField]
		private bool closeOnEscape = true;

		[Header("Post processing")]
		[Tooltip("Bu ekran açıkken Depth of Field'ı kapatılacak Volume'lar. Boş bırakılırsa sahnedeki bütün Volume'lar taranır. Kapanışta hepsi bulunduğu hâle geri döner.")]
		[SerializeField]
		private Volume[] depthOfFieldVolumes;

		[Header("Paneller")]
		[Tooltip("Vücut parçası listesi paneli - senin tasarladığın obje. İsteğe bağlı.")]
		[SerializeField]
		private CharacterPartListView partList;

		[Tooltip("Kaydetme çubuğu - senin tasarladığın obje. İsteğe bağlı.")]
		[SerializeField]
		private CharacterSaveBarView saveBar;

		[Header("Ses Tipi")]
		[Tooltip("Basınca karakterin ses tipini Erkek/Kadın arasında değiştiren buton.")]
		[SerializeField]
		private Button voiceTypeButton;

		[Tooltip("Ses tipi ERKEKKEN butonda görünen sprite.")]
		[SerializeField]
		private Sprite maleVoiceSprite;

		[Tooltip("Ses tipi KADINKEN butonda görünen sprite.")]
		[SerializeField]
		private Sprite femaleVoiceSprite;

		[Tooltip("Sprite'ın konacağı Image. Boş bırakılırsa butonun kendi Image'ı kullanılır - ikon butonun içinde ayrı bir çocuksa onu buraya sürükle.")]
		[SerializeField]
		private Image voiceTypeIcon;

		private UITooltipTrigger voiceTooltip;

		private CharacterData openOriginal;

		private bool warnedAboutNoReturn;

		private bool loadedEditorScene;

		private readonly Dictionary<DepthOfField, bool> suppressedDepthOfField = new Dictionary<DepthOfField, bool>();

		private bool cameraLive;

		private bool restoreCameraActive = true;

		private Vector3 restoreCameraPosition;

		private Quaternion restoreCameraRotation = Quaternion.identity;

		private bool restoreCameraEnabled;

		private bool restoreOrbitEnabled;

		private bool warnedAboutCameraFight;

		private bool hasFocused;

		private OrbitCamera orbitCamera;

		private bool warnedAboutOrbit;

		public string CurrentFilePath { get; private set; }

		public string CurrentName { get; private set; } = "Karakter";

		public CharacterVoice CurrentVoice { get; private set; }

		public bool CanResetModel => character != null;

		public bool SupportsPrimitives => false;

		public IReadOnlyList<CharacterAssetDefinition> Presets
		{
			get
			{
				if (!(character != null) || !(character.Rig != null))
				{
					return Array.Empty<CharacterAssetDefinition>();
				}
				return character.Rig.Presets;
			}
		}

		private OrbitCamera OrbitCamera
		{
			get
			{
				if (orbitCamera != null || editorCamera == null)
				{
					return orbitCamera;
				}
				orbitCamera = editorCamera.GetComponentInParent<OrbitCamera>(includeInactive: true);
				if (orbitCamera == null && !warnedAboutOrbit)
				{
					warnedAboutOrbit = true;
					Debug.LogWarning("[CharacterCustomizationView] '" + editorCamera.name + "' uzerinde OrbitCamera yok - kamera hic hareket etmeyecek. O kameraya OrbitCamera bileseni ekle (sag tik surukle: dondur, tekerlek: yakinlas, orta tik: kaydir).", this);
				}
				return orbitCamera;
			}
		}

		public CharacterRigDefinition Rig
		{
			get
			{
				if (!(character != null))
				{
					return null;
				}
				return character.Rig;
			}
		}

		public void ToggleVoice()
		{
			SetVoice((CurrentVoice != CharacterVoice.Female) ? CharacterVoice.Female : CharacterVoice.Male, markChanged: true);
		}

		private void SetVoice(CharacterVoice voice, bool markChanged)
		{
			bool flag = voice != CurrentVoice;
			CurrentVoice = voice;
			if (markChanged && flag)
			{
				ModelEditSession.MarkChanged();
			}
			RefreshVoiceControls();
		}

		private void WireVoiceControls()
		{
			if (voiceTypeButton != null)
			{
				voiceTypeButton.onClick.RemoveListener(ToggleVoice);
				voiceTypeButton.onClick.AddListener(ToggleVoice);
				voiceTooltip = voiceTypeButton.GetComponent<UITooltipTrigger>();
				if (voiceTooltip != null && string.IsNullOrEmpty(voiceTooltip.Key))
				{
					voiceTooltip.Key = "Tooltip.VoiceType";
				}
			}
			RefreshVoiceControls();
		}

		private void RefreshVoiceControls()
		{
			Image image = ((voiceTypeIcon != null) ? voiceTypeIcon : ((voiceTypeButton != null) ? voiceTypeButton.image : null));
			Sprite sprite = ((CurrentVoice == CharacterVoice.Female) ? femaleVoiceSprite : maleVoiceSprite);
			if (image != null && sprite != null)
			{
				image.sprite = sprite;
			}
			RefreshVoiceTooltip();
		}

		private void RefreshVoiceTooltip()
		{
			if (!(voiceTooltip == null))
			{
				string text = Loc.Get((CurrentVoice == CharacterVoice.Female) ? "CharacterVoice.Female" : "CharacterVoice.Male");
				voiceTooltip.Suffix = "\n<size=88%>" + Loc.Format("Tooltip.VoiceTypeCurrent", text) + "</size>";
				TooltipView.Instance?.RefreshOwnedBy((RectTransform)voiceTooltip.transform, voiceTooltip.Text);
			}
		}

		private void OnEnable()
		{
			ModelEditSession.Begin(this);
			WireVoiceControls();
			Telemetry.Send("menu_action", ("menu_item", "customize_character"), ("session_seconds", Telemetry.SessionSeconds));
			Loc.Changed += RefreshVoiceControls;
			StartCoroutine(OpenRoutine());
		}

		public void ResetModel()
		{
			if (!(character == null))
			{
				SetEditing(editing: false);
				character.Apply(openOriginal ?? character.DefaultCharacterData);
				SetVoice((openOriginal != null) ? openOriginal.VoiceType : CharacterVoice.Male, markChanged: false);
				SetEditing(editing: true);
				ModelEditSession.MarkSaved();
			}
		}

		public void SaveModel()
		{
			Save();
		}

		public void ApplyPrimitive(VoxelPrimitive shape)
		{
		}

		private void MarkOpened(CharacterData original)
		{
			openOriginal = original;
			ModelEditSession.MarkSaved();
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

		private void OnDisable()
		{
			hasFocused = false;
			Loc.Changed -= RefreshVoiceControls;
			ModelEditSession.End(this);
			SetEditing(editing: false);
			CursorManager.ResetToDefault();
			UnloadEditorScene();
			CharacterMenuPreview.RefreshAll();
		}

		private IEnumerator OpenRoutine()
		{
			yield return LoadEditorScene();
			LoadStartupCharacter();
			SetEditing(editing: true);
			BindPanels();
		}

		private void BindPanels()
		{
			if (partList != null)
			{
				partList.Bind(character, FocusPart);
			}
			if (saveBar != null)
			{
				saveBar.Bind(this);
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
				Debug.LogError("[CharacterCustomizationView] '" + editorSceneName + "' yuklenemedi - sahne Build Settings'te ekli mi?", this);
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

		private void LoadStartupCharacter()
		{
			if (!(character == null))
			{
				string text = (CurrentFilePath = (string.IsNullOrEmpty(startupFilePath) ? CharacterSelection.Resolve() : startupFilePath));
				if (!string.IsNullOrEmpty(text))
				{
					CurrentName = NameFor(text, CurrentName);
				}
				CharacterData characterData = (string.IsNullOrEmpty(text) ? character.DefaultCharacterData : CharacterStorage.Load(text, Rig));
				character.Apply(characterData);
				MarkOpened(characterData);
				SetVoice(characterData?.VoiceType ?? CharacterVoice.Male, markChanged: false);
			}
		}

		private void SetEditing(bool editing)
		{
			if (character == null)
			{
				return;
			}
			character.SetEditable(editing, editorCamera);
			VoxelEditorSettings.MenuEditing = editing;
			VoxelEditorSettings.StandaloneEditing = editing;
			VoxelEditorSettings.EditorCamera = (editing ? editorCamera : null);
			SetCameraLive(editing);
			SetDepthOfFieldEnabled(!editing);
			if (editing)
			{
				VoxelEditorSettings.IsMovementMode = false;
				if (OrbitCamera != null)
				{
					OrbitCamera.SetMinDistance(closeZoomDistance);
					OrbitCamera.IgnoreRoot = ((character != null) ? character.transform : null);
					OrbitCamera.CollisionEnabled = false;
				}
				if (!hasFocused)
				{
					hasFocused = true;
					FocusWholeCharacter();
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

		private void LateUpdate()
		{
			if (!cameraLive || editorCamera == null)
			{
				return;
			}
			bool num = !editorCamera.enabled;
			bool flag = orbitCamera != null && !orbitCamera.enabled;
			if (num || flag)
			{
				editorCamera.enabled = true;
				if (orbitCamera != null)
				{
					orbitCamera.enabled = true;
				}
				if (!warnedAboutCameraFight)
				{
					warnedAboutCameraFight = true;
					Debug.LogWarning("[CharacterCustomizationView] '" + editorCamera.name + "' baskasi tarafindan kapatildi ve geri acildi. Menude Camera.main olarak etiketlenmis kamera hangisi? Editor sahnesindeki PlayModeManager onu kendi orbit kamerasi sanip kapatabiliyor.", this);
				}
			}
		}

		public void FocusPart(string partId)
		{
			if (character == null || OrbitCamera == null)
			{
				return;
			}
			foreach (CharacterAssembler.BuiltPart builtPart in character.BuiltParts)
			{
				if (!(builtPart.Definition == null) && !(builtPart.Definition.PartId != partId) && !(builtPart.Model == null))
				{
					OrbitCamera.SetFocusPoint(builtPart.Model.transform.TransformPoint(builtPart.Model.GetCurrentBoundsCenterLocal()));
					break;
				}
			}
		}

		private void FocusWholeCharacter()
		{
			if (character == null || OrbitCamera == null)
			{
				return;
			}
			Bounds bounds = default(Bounds);
			bool flag = false;
			foreach (CharacterAssembler.BuiltPart builtPart in character.BuiltParts)
			{
				if (builtPart.Model == null)
				{
					continue;
				}
				Renderer component = builtPart.Model.GetComponent<Renderer>();
				if (!(component == null))
				{
					if (!flag)
					{
						flag = true;
						bounds = component.bounds;
					}
					else
					{
						bounds.Encapsulate(component.bounds);
					}
				}
			}
			if (flag)
			{
				OrbitCamera.SetFocusPoint(bounds.center);
			}
			else
			{
				FocusFirstPart();
			}
		}

		private void FocusFirstPart()
		{
			foreach (CharacterAssembler.BuiltPart builtPart in character.BuiltParts)
			{
				if (builtPart.Definition != null)
				{
					FocusPart(builtPart.Definition.PartId);
					break;
				}
			}
		}

		public void Save()
		{
			if (!(character == null))
			{
				CharacterData characterData = character.Capture();
				characterData.VoiceType = CurrentVoice;
				CurrentFilePath = CharacterStorage.Save(CurrentFilePath, CurrentName, characterData, character.Rig);
				CharacterSelection.SelectedPath = CurrentFilePath;
				MarkOpened(characterData);
				CharacterPortraitService.Request(CurrentFilePath, characterData);
			}
		}

		public void CreateNew(string characterName)
		{
			CreateNew(characterName, null);
		}

		public void CreateNew(string characterName, CharacterAsset preset)
		{
			if (!(character == null) && GuardUnsaved(delegate
			{
				CreateNew(characterName, preset);
			}))
			{
				SetEditing(editing: false);
				CurrentFilePath = "";
				CurrentName = ((!string.IsNullOrWhiteSpace(characterName)) ? characterName : ((preset != null) ? preset.CharacterName : ""));
				if (string.IsNullOrWhiteSpace(CurrentName))
				{
					CurrentName = "Yeni Karakter";
				}
				CharacterData characterData = ((preset != null) ? preset.ToCharacterData(character.Rig) : character.DefaultCharacterData);
				character.Apply(characterData);
				MarkOpened(characterData);
				SetVoice(characterData?.VoiceType ?? CharacterVoice.Male, markChanged: false);
				SetEditing(editing: true);
			}
		}

		public string Duplicate(string filePath)
		{
			if (character == null || string.IsNullOrEmpty(filePath))
			{
				return "";
			}
			CharacterData characterData = CharacterStorage.Load(filePath, Rig);
			if (characterData == null)
			{
				return "";
			}
			SetEditing(editing: false);
			CurrentFilePath = "";
			CurrentName = NameFor(filePath, CurrentName) + " (kopya)";
			character.Apply(characterData);
			SetVoice(characterData.VoiceType, markChanged: false);
			SetEditing(editing: true);
			Save();
			return CurrentFilePath;
		}

		public void Load(string filePath)
		{
			if (!(character == null) && !string.IsNullOrEmpty(filePath) && GuardUnsaved(delegate
			{
				Load(filePath);
			}))
			{
				SetEditing(editing: false);
				CurrentFilePath = filePath;
				CurrentName = NameFor(filePath, CurrentName);
				CharacterData characterData = CharacterStorage.Load(filePath, Rig);
				character.Apply(characterData);
				MarkOpened(characterData);
				SetVoice(characterData?.VoiceType ?? CharacterVoice.Male, markChanged: false);
				SetEditing(editing: true);
				CharacterSelection.SelectedPath = filePath;
			}
		}

		public void Delete(string filePath)
		{
			CharacterStorage.Delete(filePath);
			SavedThumbnails.Delete(filePath);
			CharacterSelection.ForgetIfSelected(filePath);
			if (CurrentFilePath == filePath)
			{
				CurrentFilePath = "";
			}
		}

		public void SetName(string characterName)
		{
			CurrentName = characterName;
		}

		private static string NameFor(string filePath, string fallback)
		{
			foreach (CharacterListEntry item in CharacterStorage.List())
			{
				if (item.FilePath == filePath)
				{
					return item.CharacterName;
				}
			}
			return fallback;
		}
	}
}
