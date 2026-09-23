using Mimicraft.Cameras;
using Mimicraft.Networking;
using Mimicraft.Settings;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

namespace Mimicraft.Gameplay
{
	public class PlayerCameraRig : MonoBehaviour
	{
		private static readonly Vector3 EyeOffsetLocal = new Vector3(0f, 1.6f, 0f);

		[SerializeField]
		private GameObject fpsRig;

		[SerializeField]
		private GameObject tpsRig;

		[SerializeField]
		private ThirdPersonCamera thirdPersonCamera;

		[Tooltip("Root of this player's character model. Its renderers are switched to shadows-only while the first-person camera is active, so the owner doesn't see their own body from the inside. Auto-resolved from the Animator if left empty.")]
		[SerializeField]
		private GameObject characterModel;

		[Tooltip("Çömelirken kameranın ne kadar hızlı alçaldığı. Yüksek değer ani, düşük değer yumuşak.")]
		[SerializeField]
		[Min(0.1f)]
		private float crouchLerpSpeed = 12f;

		[Tooltip("Üçüncü şahıstayken çömelmenin nişan hedefini ne kadar indireceği - birinci şahıs değerinin çarpanı. Ayrı bir sayı, çünkü ikisi aynı ölçü değil: birinci şahısta bu bir GÖZÜN inişi ve çömelmenin neredeyse tamamı kadar. Omuz kamerasında ise hiçbir görüntüyü oynatmıyor, karakterin nişan aldığı noktayı indiriyor - gözün tam inişini vermek karakteri ikiye katlıyor. 0 = çömelince nişan hiç inmez, 1 = birinci şahısla aynı.")]
		[SerializeField]
		[Range(0f, 1f)]
		private float thirdPersonCrouchScale = 0.5f;

		private float crouchDropTarget;

		private float crouchDropApplied;

		private FirstPersonLook firstPersonLook;

		private GameModeController roundManager;

		private bool isOwner;

		private Renderer[] characterRenderers;

		private Camera fpsCamera;

		private bool warnedStalledCamera;

		private bool hidOwnBodyLastFrame;

		private const float CloseBodyHideDistance = 0.65f;

		private bool cameraInsideCharacter;

		private Camera tpsCamera;

		private PlayerRagdoll ragdoll;

		private SpectatorController spectator;

		private PlayerVoxelBody voxelBody;

		private bool retiredPlaceholders;

		private bool appliedRig;

		private bool fpsActive;

		private PlayerViewMode viewMode;

		private PlayerMovement movement;

		private PlayerFight fight;

		private bool seededZoom;

		private const float DrawPullOutGraceSeconds = 1f;

		private float drawPullOutSuppressedUntil = float.NegativeInfinity;

		private bool wasArmed;

		private bool wasHolstered;

		public Transform TpsCameraTransform => tpsRig.transform;

		public Camera FpsCamera
		{
			get
			{
				if (fpsCamera == null && fpsRig != null)
				{
					fpsCamera = fpsRig.GetComponent<Camera>();
				}
				return fpsCamera;
			}
		}

		public GameObject CharacterModel => characterModel;

		public bool IsFirstPerson => fpsActive;

		public bool IsOverShoulder
		{
			get
			{
				if (thirdPersonCamera != null)
				{
					return thirdPersonCamera.OverShoulder;
				}
				return false;
			}
		}

		private FirstPersonLook Look
		{
			get
			{
				if (!(fpsRig != null))
				{
					return null;
				}
				return fpsRig.GetComponent<FirstPersonLook>();
			}
		}

		public bool ForceFirstPerson { get; set; }

		public Transform AimCameraTransform
		{
			get
			{
				if (fpsActive)
				{
					Camera camera = FpsCamera;
					if (!(camera != null))
					{
						return null;
					}
					return camera.transform;
				}
				if (!(tpsRig != null))
				{
					return null;
				}
				return tpsRig.transform;
			}
		}

		public float AimPitch
		{
			get
			{
				if (fpsActive)
				{
					FirstPersonLook look = Look;
					if (!(look != null))
					{
						return 0f;
					}
					return look.Pitch;
				}
				if (!(thirdPersonCamera != null))
				{
					return 0f;
				}
				return thirdPersonCamera.Pitch;
			}
		}

		public Vector2 AimPunch
		{
			get
			{
				if (fpsActive)
				{
					FirstPersonLook look = Look;
					if (!(look != null))
					{
						return Vector2.zero;
					}
					return look.AimPunch;
				}
				if (!(thirdPersonCamera != null))
				{
					return Vector2.zero;
				}
				return thirdPersonCamera.AimPunch;
			}
		}

		public void SetRigs(GameObject fpsRig, GameObject tpsRig, ThirdPersonCamera thirdPersonCamera)
		{
			this.fpsRig = fpsRig;
			this.tpsRig = tpsRig;
			firstPersonLook = null;
			ResolveFirstPersonLook();
			this.thirdPersonCamera = thirdPersonCamera;
		}

		private void Awake()
		{
			if (characterModel == null)
			{
				Animator componentInChildren = GetComponentInChildren<Animator>(includeInactive: true);
				if (componentInChildren != null)
				{
					characterModel = componentInChildren.gameObject;
				}
			}
			RefreshCharacterRenderers();
			ResolveFirstPersonLook();
		}

		public void RefreshCharacterRenderers()
		{
			if (!(characterModel == null))
			{
				characterRenderers = characterModel.GetComponentsInChildren<Renderer>(includeInactive: true);
				if (isOwner && appliedRig)
				{
					ApplyCharacterModelVisibility(fpsActive || cameraInsideCharacter);
				}
			}
		}

		private void ResolveFirstPersonLook()
		{
			if (firstPersonLook == null && fpsRig != null)
			{
				firstPersonLook = fpsRig.GetComponent<FirstPersonLook>();
			}
		}

		public void SetCrouchDrop(float drop)
		{
			crouchDropTarget = Mathf.Min(0f, drop);
		}

		private void LateUpdate()
		{
			if (!isOwner)
			{
				return;
			}
			ResolveFirstPersonLook();
			crouchDropApplied = Mathf.Lerp(crouchDropApplied, crouchDropTarget, 1f - Mathf.Exp((0f - crouchLerpSpeed) * Time.deltaTime));
			if (firstPersonLook != null)
			{
				firstPersonLook.CrouchOffset = crouchDropApplied;
				if (!fpsActive)
				{
					firstPersonLook.ApplyRestPoseWhileInactive(crouchDropApplied * thirdPersonCrouchScale);
				}
			}
			bool flag = thirdPersonCamera != null && thirdPersonCamera.Target == base.transform;
			if (flag)
			{
				thirdPersonCamera.PivotOffsetLocal = PivotLocal() + Vector3.up * crouchDropApplied;
			}
			ApplyOwnBodyVisibility(flag);
		}

		private void WatchForStalledCamera()
		{
			bool flag = isOwner && appliedRig && !fpsActive && thirdPersonCamera != null && !thirdPersonCamera.enabled && tpsRig != null && tpsRig.activeInHierarchy;
			if (flag != warnedStalledCamera)
			{
				warnedStalledCamera = flag;
				if (flag)
				{
					Debug.LogWarning("[PlayerCameraRig] TPS rig acik ama ThirdPersonCamera KAPALI - kamera oyuncuyu takip etmiyor. Gorunen sey 'karakter kayboldu' oluyor, ama karakter yerinde: ona bakan kimse yok. Kapatabilecek yerler: model duzenleme oturumu (PlayerEditSession), spectator serbest kamera (SpectatorController), tur sonu sinematigi, ve additive yuklenen bir editor sahnesindeki PlayModeManager.", this);
				}
			}
		}

		private void ApplyOwnBodyVisibility(bool watchingSelf)
		{
			WatchForStalledCamera();
			if (voxelBody == null)
			{
				voxelBody = GetComponent<PlayerVoxelBody>();
			}
			if (voxelBody == null)
			{
				return;
			}
			bool flag = watchingSelf && !fpsActive && thirdPersonCamera.enabled && tpsRig != null && tpsRig.activeInHierarchy && thirdPersonCamera.ActualDistance < 0.65f;
			bool flag2 = flag || (watchingSelf && !fpsActive && thirdPersonCamera.enabled && tpsRig != null && tpsRig.activeInHierarchy && voxelBody.IsInsideBody(tpsRig.transform.position, TpsNearClip()));
			if (flag != cameraInsideCharacter)
			{
				cameraInsideCharacter = flag;
				if (appliedRig)
				{
					ApplyCharacterModelVisibility(fpsActive || cameraInsideCharacter);
				}
			}
			if (flag2 != hidOwnBodyLastFrame)
			{
				hidOwnBodyLastFrame = flag2;
				string arg = ((tpsRig != null) ? $"kamera govde uzayinda {base.transform.InverseTransformPoint(tpsRig.transform.position)}" : "TPS rig yok");
				Debug.Log("[PlayerCameraRig] Kendi govden " + (flag2 ? "gizleniyor" : "gosteriliyor") + " - " + $"{arg}, pay {TpsNearClip():0.###}m, kendine mi bakiyor={watchingSelf}, " + $"fps={fpsActive}, tps kamera acik={thirdPersonCamera.enabled}.", this);
			}
			voxelBody.SetBodyHiddenFromOwner(flag2);
		}

		private float TpsNearClip()
		{
			if (tpsCamera == null && tpsRig != null)
			{
				tpsCamera = tpsRig.GetComponent<Camera>();
			}
			if (!(tpsCamera != null))
			{
				return 0.3f;
			}
			return tpsCamera.nearClipPlane;
		}

		private Vector3 PivotLocal()
		{
			if (ragdoll == null)
			{
				ragdoll = GetComponent<PlayerRagdoll>();
			}
			if (ragdoll != null && ragdoll.TryGetLimpPivotLocal(out var pivotLocal) && IsFinite(pivotLocal))
			{
				return pivotLocal;
			}
			if (voxelBody == null)
			{
				voxelBody = GetComponent<PlayerVoxelBody>();
			}
			if (voxelBody != null && voxelBody.TryGetCameraPivotLocal(out var pivotLocal2) && IsFinite(pivotLocal2))
			{
				return pivotLocal2;
			}
			return EyeOffsetLocal;
		}

		private static bool IsFinite(Vector3 v)
		{
			if (!float.IsNaN(v.x) && !float.IsNaN(v.y) && !float.IsNaN(v.z) && !float.IsInfinity(v.x) && !float.IsInfinity(v.y))
			{
				return !float.IsInfinity(v.z);
			}
			return false;
		}

		public void Initialize(bool isOwner)
		{
			this.isOwner = isOwner;
			if (isOwner)
			{
				thirdPersonCamera.Target = base.transform;
				thirdPersonCamera.PivotOffsetLocal = PivotLocal();
			}
			fpsRig.SetActive(value: false);
			tpsRig.SetActive(value: false);
			appliedRig = false;
		}

		public bool TryGetAimRay(float eyeHeight, out Ray ray, out float extraRange)
		{
			Transform aimCameraTransform = AimCameraTransform;
			Vector3 vector = base.transform.position + Vector3.up * eyeHeight;
			if (aimCameraTransform == null)
			{
				ray = new Ray(vector, base.transform.forward);
				extraRange = 0f;
				return false;
			}
			ray = new Ray(aimCameraTransform.position, aimCameraTransform.forward);
			extraRange = Mathf.Max(0f, Vector3.Dot(vector - aimCameraTransform.position, aimCameraTransform.forward));
			return true;
		}

		public void AddAimPunch(Vector2 degrees)
		{
			if (fpsActive)
			{
				Look?.AddAimPunch(degrees);
			}
			else
			{
				thirdPersonCamera?.AddAimPunch(degrees);
			}
		}

		public void AddAimTrauma(float trauma)
		{
			if (fpsActive)
			{
				Look?.AddTrauma(trauma);
			}
			else
			{
				thirdPersonCamera?.AddTrauma(trauma);
			}
		}

		private void Update()
		{
			if (!isOwner)
			{
				return;
			}
			if (roundManager == null)
			{
				roundManager = GameModeController.Current;
			}
			if (roundManager == null)
			{
				return;
			}
			if (ragdoll == null)
			{
				ragdoll = GetComponent<PlayerRagdoll>();
			}
			bool num = ragdoll != null && ragdoll.State != RagdollState.None;
			if (spectator == null)
			{
				spectator = GetComponent<SpectatorController>();
			}
			bool flag = spectator != null && spectator.IsSpectating;
			if (viewMode == null)
			{
				viewMode = GetComponent<PlayerViewMode>();
			}
			bool flag2 = viewMode != null && viewMode.IsActive;
			if (fpsActive && thirdPersonCamera != null && Mouse.current != null && !GameMenuState.LookCaptured)
			{
				thirdPersonCamera.AddZoom(Mouse.current.scroll.ReadValue().y);
			}
			bool localPlayerUsesHunterMovement = roundManager.LocalPlayerUsesHunterMovement;
			bool isLocalPlayerParticipating = roundManager.IsLocalPlayerParticipating;
			bool flag3 = isLocalPlayerParticipating && !roundManager.LocalPlayerShouldHaveModel;
			if (thirdPersonCamera != null)
			{
				thirdPersonCamera.AllowFirstPersonZoom = flag3;
				if (flag3 && !seededZoom)
				{
					seededZoom = true;
					thirdPersonCamera.SetZoom(CameraViewMemory.Recall(thirdPersonCamera.FirstPersonDistance));
					drawPullOutSuppressedUntil = Time.unscaledTime + 1f;
				}
				if (GameInput.ShoulderSwap.WasPressedThisFrame() && !GameMenuState.InputCaptured)
				{
					thirdPersonCamera.SwapShoulder();
				}
				if (movement == null)
				{
					movement = GetComponent<PlayerMovement>();
				}
				if (fight == null)
				{
					fight = GetComponent<PlayerFight>();
				}
				bool flag4 = movement != null && movement.IsArmed;
				thirdPersonCamera.OverShoulder = flag3 && (flag4 || (fight != null && fight.IsSquaredUp));
				bool flag5 = movement != null && movement.IsHolstered;
				if (flag3 && flag4 && !wasArmed && !wasHolstered && Time.unscaledTime >= drawPullOutSuppressedUntil)
				{
					thirdPersonCamera.PullOutToShoulder();
				}
				wasArmed = flag4;
				wasHolstered = flag5;
			}
			bool flag6 = !num && !flag && !flag2 && isLocalPlayerParticipating && thirdPersonCamera != null && (ForceFirstPerson || (flag3 ? thirdPersonCamera.WantsFirstPerson : localPlayerUsesHunterMovement));
			if (!flag3 && seededZoom)
			{
				if (thirdPersonCamera != null)
				{
					CameraViewMemory.Remember(thirdPersonCamera.Distance);
				}
				seededZoom = false;
			}
			if (appliedRig && flag6 == fpsActive)
			{
				return;
			}
			CarryAnglesAcross(flag6);
			if (thirdPersonCamera != null)
			{
				if (!flag6 && fpsActive)
				{
					thirdPersonCamera.BeginPullOut();
				}
				else if (!flag6)
				{
					thirdPersonCamera.SkipPullOut();
				}
			}
			appliedRig = true;
			fpsActive = flag6;
			fpsRig.SetActive(flag6);
			tpsRig.SetActive(!flag6);
			ApplyCharacterModelVisibility(flag6 || cameraInsideCharacter);
			if (isOwner && !retiredPlaceholders)
			{
				retiredPlaceholders = true;
				PlaceholderCameras.Retire();
			}
		}

		private void OnDestroy()
		{
			if (isOwner && seededZoom && thirdPersonCamera != null)
			{
				CameraViewMemory.Remember(thirdPersonCamera.Distance);
			}
			if (retiredPlaceholders)
			{
				PlaceholderCameras.Restore();
			}
		}

		private void CarryAnglesAcross(bool toFirstPerson)
		{
			if (thirdPersonCamera == null || toFirstPerson == fpsActive)
			{
				return;
			}
			FirstPersonLook firstPersonLook = ((fpsRig != null) ? fpsRig.GetComponent<FirstPersonLook>() : null);
			if (toFirstPerson)
			{
				base.transform.rotation = Quaternion.Euler(0f, thirdPersonCamera.Yaw, 0f);
				if (firstPersonLook != null)
				{
					firstPersonLook.SetPitch(thirdPersonCamera.Pitch);
				}
			}
			else
			{
				thirdPersonCamera.SyncAngles(base.transform.eulerAngles.y, (firstPersonLook != null) ? firstPersonLook.Pitch : 0f);
			}
		}

		private void ApplyCharacterModelVisibility(bool firstPerson)
		{
			if (characterRenderers == null)
			{
				return;
			}
			ShadowCastingMode shadowCastingMode = ((!firstPerson) ? ShadowCastingMode.On : ShadowCastingMode.ShadowsOnly);
			Renderer[] array = characterRenderers;
			foreach (Renderer renderer in array)
			{
				if (renderer != null)
				{
					renderer.shadowCastingMode = shadowCastingMode;
				}
			}
		}

		public void Shake(float trauma)
		{
			if (isOwner && appliedRig)
			{
				if (fpsActive)
				{
					fpsRig.GetComponent<FirstPersonLook>().AddTrauma(trauma);
				}
				else
				{
					thirdPersonCamera.AddTrauma(trauma);
				}
			}
		}
	}
}
