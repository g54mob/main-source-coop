using System.Collections.Generic;
using Mimicraft.Gameplay;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.UI;

namespace Mimicraft.Customization
{
	public class WeaponFpsPreview : MonoBehaviour
	{
		public enum PreviewMode
		{
			Fps = 0,
			Ads = 1,
			Tps = 2
		}

		[Tooltip("Hangi tezgâhın silahını gösterecek. Boşsa üst objelerde aranır.")]
		[SerializeField]
		private WeaponCustomizationView screen;

		[Tooltip("Görüntünün basılacağı RawImage - sağ alttaki 16:9 kutu. Boş bırakılırsa yalnızca Render Texture doldurulur, kendin bağlarsın.")]
		[SerializeField]
		private RawImage output;

		[Tooltip("İki kameranın da çizeceği doku. Boşsa aşağıdaki boyutta bir tane oluşturulur.")]
		[SerializeField]
		private RenderTexture renderTexture;

		[Tooltip("Kendi oluşturduğu dokunun boyutu, piksel. 16:9 tut.")]
		[SerializeField]
		private Vector2Int textureSize = new Vector2Int(960, 540);

		[Tooltip("Önizleme silahlarının ve TPS sahnesinin konacağı katman. Sadece önizleme kameraları bu katmanı çizmeli; sahnedeki diğer kameralarda kapalı olmalı.")]
		[SerializeField]
		private string previewLayer = "FPSVisual";

		[Tooltip("Açılıştaki görünüm.")]
		[SerializeField]
		private PreviewMode startMode;

		[Header("FPS / ADS")]
		[Tooltip("Birinci şahıs görünümünü çizen kamera. Silah prefabı bu kameranın ALTINA (0,0,0)'a konur - oyunda WeaponParent'ın FpsCamera altında durduğu yer. Görüş açısını Player prefabındaki WeaponCamera'yla aynı yap (85) ki önizleme oyundakiyle birebir olsun.")]
		[SerializeField]
		private Camera fpsCamera;

		[Tooltip("ADS pozuna gidiş/dönüş hızı - PlayerScope'takiyle aynı his için aynı değer.")]
		[SerializeField]
		[Min(1f)]
		private float adsMoveSpeed = 14f;

		[Header("TPS")]
		[Tooltip("Üçüncü şahıs görünümünü çizen kamera - sahnedeki karaktere bakan. FPS kamerasıyla aynı dokuya çizer; yalnızca seçili görünümün kamerası açık kalır.")]
		[SerializeField]
		private Camera tpsCamera;

		[Tooltip("Karakterin durduğu sahnenin kökü. TPS dışındaki görünümlerde kapatılır, TPS'de açılır ve bütünüyle Preview Layer'a alınır.")]
		[SerializeField]
		private GameObject tpsStage;

		[Tooltip("Üçüncü şahıs silahının takılacağı nokta - sahnedeki karakterin WeaponPivot'u (Player prefabındaki TPSSocket'in karşılığı). Boşsa sahnede \"TPSSocket\", o da yoksa \"WeaponPivot\" adlı obje aranır.")]
		[SerializeField]
		private Transform tpsSocket;

		[Tooltip("Karakterin Animator'ü - Armed parametresi true yapılır ki silahlı duruşta dursun. Boşsa sahnede aranır.")]
		[SerializeField]
		private Animator tpsAnimator;

		[Tooltip("Karakterin sol el IK hedefi (RigLayer_HandIK/LeftHandIK). Silahın sol kavrama noktasını takip eder - PlayerWeapons.FollowGrips'in yaptığı iş. Boşsa el animasyonda kaldığı yerde durur.")]
		[SerializeField]
		private Transform leftHandIkTarget;

		[Tooltip("Sağ el için aynısı (RigLayer_HandIK/RightHandIK).")]
		[SerializeField]
		private Transform rightHandIkTarget;

		[Tooltip("TPS gösterilirken ağırlığı 1'e çekilecek rigler - en azından RigLayer_HandIK, yoksa eller hedeflere gitmez. Oyunda bunu PlayerAnimator yapıyor; sahnede o yok.")]
		[SerializeField]
		private Rig[] tpsRigs;

		[Tooltip("TPS gösterilirken ağırlığı 0'a çekilecek Animator katmanları. Oyunda PlayerAnimator Fighting katmanını yalnızca yumruk dövüşünde açar; sahnede onu süren yok ve controller'daki varsayılan 1 olduğu için karakter gard duruşunda kalıyor, eller de o duruştan zorlanıyordu.")]
		[SerializeField]
		private string[] tpsZeroedLayers = new string[1] { "Fighting" };

		private GameObject fpsInstance;

		private GameObject tpsInstance;

		private WeaponDefinition instanceWeapon;

		private WeaponSkinAssembler fpsAssembler;

		private WeaponSkinAssembler tpsAssembler;

		private WeaponVisual fpsVisual;

		private WeaponVisual tpsVisual;

		private RenderTexture ownedTexture;

		private int seenBuild = -1;

		private int seenGrid = -1;

		private WeaponSkinAssembler seenSource;

		private Vector3 seenLeft;

		private Vector3 seenRight;

		private Vector3 seenMuzzle;

		private Vector3 seenAds;

		private bool seenPlacedAds;

		private float aimWeight;

		private Vector3 lastBase;

		private Vector3 lastWritten;

		private bool wroteLastFrame;

		private bool restCaptured;

		private Vector3 leftRest;

		private Vector3 rightRest;

		private Quaternion leftRestRotation = Quaternion.identity;

		private Quaternion rightRestRotation = Quaternion.identity;

		private FirstPersonCharacterArms arms;

		private CharacterAssembler stageCharacter;

		private readonly List<Renderer> stageRenderers = new List<Renderer>();

		private bool stageVisibilityKnown;

		private bool lastStageVisible;

		private int lastCharacterBuild = -1;

		private int lastTpsBuild = -1;

		private GameObject lastTpsInstance;

		public PreviewMode Mode { get; private set; }

		private CharacterAssembler StageCharacter
		{
			get
			{
				if (stageCharacter == null && tpsStage != null)
				{
					stageCharacter = tpsStage.GetComponentInChildren<CharacterAssembler>(includeInactive: true);
				}
				return stageCharacter;
			}
		}

		private int PreviewLayerIndex
		{
			get
			{
				if (!string.IsNullOrWhiteSpace(previewLayer))
				{
					return LayerMask.NameToLayer(previewLayer);
				}
				return -1;
			}
		}

		public void SetMode(int mode)
		{
			SetMode((PreviewMode)Mathf.Clamp(mode, 0, 2));
		}

		public void ShowFps()
		{
			SetMode(PreviewMode.Fps);
		}

		public void ShowAds()
		{
			SetMode(PreviewMode.Ads);
		}

		public void ShowTps()
		{
			SetMode(PreviewMode.Tps);
		}

		public void SetMode(PreviewMode mode)
		{
			Mode = mode;
			ApplyMode();
		}

		private void OnEnable()
		{
			EnsureTexture();
			Mode = startMode;
			ApplyMode();
		}

		private void OnDisable()
		{
			Despawn();
			if (fpsCamera != null)
			{
				fpsCamera.enabled = false;
			}
			if (tpsCamera != null)
			{
				tpsCamera.enabled = false;
			}
			if (tpsStage != null)
			{
				tpsStage.SetActive(value: false);
			}
			stageVisibilityKnown = false;
		}

		private void UpdateArms()
		{
			if (arms == null)
			{
				arms = base.gameObject.AddComponent<FirstPersonCharacterArms>();
			}
			arms.SetSources(StageCharacter, fpsInstance);
		}

		private void UpdateStageVisibility(bool visible)
		{
			if (tpsStage == null)
			{
				return;
			}
			CharacterAssembler characterAssembler = StageCharacter;
			int num = ((characterAssembler != null) ? characterAssembler.BuildVersion : (-1));
			int num2 = ((tpsAssembler != null) ? tpsAssembler.BuildVersion : (-1));
			if (stageVisibilityKnown && visible == lastStageVisible && num == lastCharacterBuild && num2 == lastTpsBuild && tpsInstance == lastTpsInstance)
			{
				return;
			}
			stageVisibilityKnown = true;
			lastStageVisible = visible;
			lastCharacterBuild = num;
			lastTpsBuild = num2;
			lastTpsInstance = tpsInstance;
			tpsStage.GetComponentsInChildren(includeInactive: true, stageRenderers);
			foreach (Renderer stageRenderer in stageRenderers)
			{
				if (stageRenderer != null)
				{
					stageRenderer.forceRenderingOff = !visible;
				}
			}
		}

		private void OnDestroy()
		{
			if (!(ownedTexture == null))
			{
				ownedTexture.Release();
				Object.Destroy(ownedTexture);
			}
		}

		private void EnsureTexture()
		{
			if (renderTexture == null)
			{
				if (ownedTexture == null)
				{
					ownedTexture = new RenderTexture(Mathf.Max(16, textureSize.x), Mathf.Max(16, textureSize.y), 24)
					{
						name = "WeaponFpsPreview"
					};
				}
				renderTexture = ownedTexture;
			}
			if (fpsCamera != null && fpsCamera.targetTexture != renderTexture)
			{
				fpsCamera.targetTexture = renderTexture;
			}
			if (tpsCamera != null && tpsCamera.targetTexture != renderTexture)
			{
				tpsCamera.targetTexture = renderTexture;
			}
			if (output != null && output.texture != renderTexture)
			{
				output.texture = renderTexture;
			}
		}

		private void ApplyMode()
		{
			bool flag = Mode == PreviewMode.Tps;
			if (fpsCamera != null)
			{
				fpsCamera.enabled = !flag;
			}
			if (tpsCamera != null)
			{
				tpsCamera.enabled = flag;
			}
			if (tpsStage != null)
			{
				if (!tpsStage.activeSelf)
				{
					tpsStage.SetActive(value: true);
				}
				PrepareStage();
				stageVisibilityKnown = false;
			}
		}

		private void PrepareStage()
		{
			int previewLayerIndex = PreviewLayerIndex;
			if (previewLayerIndex >= 0)
			{
				SetLayerRecursively(tpsStage, previewLayerIndex);
			}
			if (tpsAnimator == null)
			{
				tpsAnimator = tpsStage.GetComponentInChildren<Animator>(includeInactive: true);
			}
			WearSelectedCharacter();
		}

		private void WearSelectedCharacter()
		{
			CharacterAssembler componentInChildren = tpsStage.GetComponentInChildren<CharacterAssembler>(includeInactive: true);
			if (componentInChildren == null)
			{
				return;
			}
			string text = CharacterSelection.Resolve();
			if (string.IsNullOrEmpty(text))
			{
				CharacterData defaultCharacterData = componentInChildren.DefaultCharacterData;
				if (defaultCharacterData != null)
				{
					componentInChildren.Apply(defaultCharacterData);
				}
				else
				{
					componentInChildren.Clear();
				}
			}
			else
			{
				componentInChildren.Apply(CharacterStorage.Load(text, componentInChildren.Rig));
			}
		}

		private void PoseStage()
		{
			if (tpsAnimator != null && tpsAnimator.isActiveAndEnabled && tpsAnimator.isInitialized)
			{
				if (HasBool(tpsAnimator, "Armed"))
				{
					tpsAnimator.SetBool("Armed", value: true);
				}
				if (tpsZeroedLayers != null)
				{
					string[] array = tpsZeroedLayers;
					foreach (string text in array)
					{
						int num = (string.IsNullOrWhiteSpace(text) ? (-1) : tpsAnimator.GetLayerIndex(text));
						if (num >= 0 && tpsAnimator.GetLayerWeight(num) != 0f)
						{
							tpsAnimator.SetLayerWeight(num, 0f);
						}
					}
				}
			}
			if (tpsRigs == null)
			{
				return;
			}
			Rig[] array2 = tpsRigs;
			foreach (Rig rig in array2)
			{
				if (rig != null && rig.weight != 1f)
				{
					rig.weight = 1f;
				}
			}
		}

		private void LateUpdate()
		{
			if (screen == null)
			{
				screen = GetComponentInParent<WeaponCustomizationView>(includeInactive: true);
			}
			WeaponSkinAssembler weaponSkinAssembler = ((screen != null) ? screen.Selected : null);
			WeaponDefinition weaponDefinition = ((weaponSkinAssembler != null) ? weaponSkinAssembler.Weapon : null);
			if (weaponDefinition == null)
			{
				Despawn();
				return;
			}
			if (instanceWeapon != weaponDefinition)
			{
				Despawn();
				instanceWeapon = weaponDefinition;
			}
			bool flag = Mode == PreviewMode.Tps;
			if (!flag && fpsInstance == null && fpsCamera != null && weaponDefinition.HeldPrefab != null)
			{
				fpsInstance = Spawn(weaponDefinition.HeldPrefab, fpsCamera.transform, out fpsAssembler, out fpsVisual);
			}
			if (flag && tpsInstance == null && weaponDefinition.TpsPrefab != null)
			{
				Transform transform = ResolveTpsSocket();
				if (transform != null)
				{
					tpsInstance = Spawn(weaponDefinition.TpsPrefab, transform, out tpsAssembler, out tpsVisual);
				}
			}
			if (fpsInstance != null && fpsInstance.activeSelf == flag)
			{
				fpsInstance.SetActive(!flag);
			}
			if (tpsInstance != null && tpsInstance.activeSelf != flag)
			{
				tpsInstance.SetActive(flag);
			}
			SyncSkin(weaponSkinAssembler);
			UpdateStageVisibility(flag);
			UpdateArms();
			if (flag)
			{
				PoseStage();
				FollowGrips();
			}
			else
			{
				MoveAimPivot();
			}
		}

		private GameObject Spawn(GameObject prefab, Transform parent, out WeaponSkinAssembler assembler, out WeaponVisual visual)
		{
			GameObject gameObject = Object.Instantiate(prefab, parent);
			gameObject.name = "Preview_" + prefab.name;
			gameObject.transform.localPosition = Vector3.zero;
			gameObject.transform.localRotation = Quaternion.identity;
			int previewLayerIndex = PreviewLayerIndex;
			if (previewLayerIndex >= 0)
			{
				SetLayerRecursively(gameObject, previewLayerIndex);
			}
			assembler = gameObject.GetComponentInChildren<WeaponSkinAssembler>(includeInactive: true);
			visual = gameObject.GetComponentInChildren<WeaponVisual>(includeInactive: true);
			seenSource = null;
			return gameObject;
		}

		private void Despawn()
		{
			if (fpsInstance != null)
			{
				Object.Destroy(fpsInstance);
			}
			if (tpsInstance != null)
			{
				Object.Destroy(tpsInstance);
			}
			fpsInstance = null;
			tpsInstance = null;
			instanceWeapon = null;
			fpsAssembler = null;
			tpsAssembler = null;
			fpsVisual = null;
			tpsVisual = null;
			seenSource = null;
			aimWeight = 0f;
			wroteLastFrame = false;
			if (restCaptured)
			{
				if (leftHandIkTarget != null)
				{
					leftHandIkTarget.SetLocalPositionAndRotation(leftRest, leftRestRotation);
				}
				if (rightHandIkTarget != null)
				{
					rightHandIkTarget.SetLocalPositionAndRotation(rightRest, rightRestRotation);
				}
			}
		}

		private Transform ResolveTpsSocket()
		{
			if (tpsSocket != null)
			{
				return tpsSocket;
			}
			if (tpsStage == null)
			{
				return null;
			}
			tpsSocket = FindDeep(tpsStage.transform, "TPSSocket") ?? FindDeep(tpsStage.transform, "WeaponPivot");
			return tpsSocket;
		}

		private void SyncSkin(WeaponSkinAssembler source)
		{
			if (fpsAssembler == null && tpsAssembler == null)
			{
				return;
			}
			int num = ((source.Model != null) ? source.Model.Grid.Version : (-1));
			Vector3 leftGrip = source.LeftGrip;
			Vector3 rightGrip = source.RightGrip;
			Vector3 muzzle = source.Muzzle;
			Vector3 adsPos = source.AdsPos;
			bool hasPlacedAdsPos = source.HasPlacedAdsPos;
			if (!(source == seenSource) || source.BuildVersion != seenBuild || num != seenGrid || !(leftGrip == seenLeft) || !(rightGrip == seenRight) || !(muzzle == seenMuzzle) || !(adsPos == seenAds) || hasPlacedAdsPos != seenPlacedAds)
			{
				seenSource = source;
				seenBuild = source.BuildVersion;
				seenGrid = num;
				seenLeft = leftGrip;
				seenRight = rightGrip;
				seenMuzzle = muzzle;
				seenAds = adsPos;
				seenPlacedAds = hasPlacedAdsPos;
				WeaponSkinData skin = source.Capture();
				if (fpsAssembler != null)
				{
					fpsAssembler.Apply(skin);
				}
				if (tpsAssembler != null)
				{
					tpsAssembler.Apply(skin);
				}
			}
		}

		private void MoveAimPivot()
		{
			Transform transform = ((fpsVisual != null) ? fpsVisual.AimPivot : null);
			if (transform == null)
			{
				return;
			}
			float num = ((Mode == PreviewMode.Ads) ? 1f : 0f);
			aimWeight = Mathf.Lerp(aimWeight, num, 1f - Mathf.Exp((0f - adsMoveSpeed) * Time.deltaTime));
			if (Mathf.Abs(aimWeight - num) < 0.001f)
			{
				aimWeight = num;
			}
			Vector3 localPosition = transform.localPosition;
			Vector3 vector = ((wroteLastFrame && localPosition == lastWritten) ? lastBase : localPosition);
			if (aimWeight <= 0f)
			{
				if (wroteLastFrame)
				{
					transform.localPosition = vector;
					wroteLastFrame = false;
					fpsVisual.SolveArms();
				}
			}
			else
			{
				Vector3 vector2 = (transform.localPosition = vector + fpsVisual.AdsPivotDeltaFrom(vector, transform.localRotation, transform.localScale) * aimWeight);
				lastBase = vector;
				lastWritten = vector2;
				wroteLastFrame = true;
				fpsVisual.SolveArms();
			}
		}

		private void FollowGrips()
		{
			if (!restCaptured)
			{
				restCaptured = true;
				if (leftHandIkTarget != null)
				{
					leftRest = leftHandIkTarget.localPosition;
					leftRestRotation = leftHandIkTarget.localRotation;
				}
				if (rightHandIkTarget != null)
				{
					rightRest = rightHandIkTarget.localPosition;
					rightRestRotation = rightHandIkTarget.localRotation;
				}
			}
			Follow(leftHandIkTarget, (tpsVisual != null) ? tpsVisual.LeftGrip : null, leftRest, leftRestRotation);
			Follow(rightHandIkTarget, (tpsVisual != null) ? tpsVisual.RightGrip : null, rightRest, rightRestRotation);
		}

		private static void Follow(Transform target, Transform grip, Vector3 restPosition, Quaternion restRotation)
		{
			if (!(target == null))
			{
				if (grip != null)
				{
					target.SetPositionAndRotation(grip.position, grip.rotation);
				}
				else
				{
					target.SetLocalPositionAndRotation(restPosition, restRotation);
				}
			}
		}

		private static bool HasBool(Animator animator, string parameter)
		{
			if (!animator.isActiveAndEnabled || animator.runtimeAnimatorController == null)
			{
				return false;
			}
			AnimatorControllerParameter[] parameters = animator.parameters;
			foreach (AnimatorControllerParameter animatorControllerParameter in parameters)
			{
				if (animatorControllerParameter.type == AnimatorControllerParameterType.Bool && animatorControllerParameter.name == parameter)
				{
					return true;
				}
			}
			return false;
		}

		private static Transform FindDeep(Transform root, string name)
		{
			if (root.name == name)
			{
				return root;
			}
			foreach (Transform item in root)
			{
				Transform transform = FindDeep(item, name);
				if (transform != null)
				{
					return transform;
				}
			}
			return null;
		}

		private static void SetLayerRecursively(GameObject root, int layer)
		{
			root.layer = layer;
			foreach (Transform item in root.transform)
			{
				SetLayerRecursively(item.gameObject, layer);
			}
		}
	}
}
