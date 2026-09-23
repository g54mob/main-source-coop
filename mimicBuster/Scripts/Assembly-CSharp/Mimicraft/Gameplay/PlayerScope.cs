using System;
using Mimicraft.Cameras;
using Mimicraft.Networking;
using Mimicraft.Settings;
using Mimicraft.UI;
using Unity.Netcode;
using UnityEngine;

namespace Mimicraft.Gameplay
{
	[RequireComponent(typeof(NetworkObject))]
	public class PlayerScope : NetworkBehaviour
	{
		[Tooltip("Yakınlaşacak kamera - dünyayı çizen FPS kamerası. Boş bırakılırsa PlayerCameraRig'in FPS rig'inden bulunur.")]
		[SerializeField]
		private Camera worldCamera;

		[Tooltip("Silah modelini çizen ayrı kamera - DÜRBÜN için. Boş bırakmak normalde DOĞRU olan: dürbüne girerken dünya yakınlaşır, elindeki silah olduğu gibi kalır. Buraya bir kamera koyarsan silah da dürbünle birlikte yakınlaşır.\n\nNişan alma (ADS) için atamana gerek yok: FpsCamera'nın altındaki kamera kendiliğinden bulunur ve nişan alırken dünyayla aynı çarpanla daralır.")]
		[SerializeField]
		private Camera weaponCamera;

		private Camera viewModelCamera;

		private float viewModelRestFov;

		private WeaponSway viewModelSway;

		[Tooltip("Görüş açısının hedefe ulaşma hızı. Yüksek = daha ani. Anında değil, çünkü tek karede değişen bir görüş açısı yakınlaşma değil, kesme gibi okunuyor.")]
		[SerializeField]
		[Min(1f)]
		private float zoomSpeed = 12f;

		[Header("Nişan alma (ADS) - Features.AimDownSights kapalıysa hiçbiri okunmaz")]
		[Tooltip("Nişan alırken dünya kamerasının görüş açısı bununla çarpılır. 0.5 = yarıya iner.")]
		[SerializeField]
		[Range(0.2f, 1f)]
		private float adsFieldOfViewMultiplier = 0.5f;

		[Tooltip("Nişan alırken geri tepme bununla çarpılır - hem kamera vuruşu hem silah modelinin tepmesi. 0.5 = yarıya iner.")]
		[SerializeField]
		[Range(0f, 1f)]
		private float adsRecoilMultiplier = 0.5f;

		[Tooltip("Nişan alırken fare hassasiyeti bununla çarpılır. Görüş açısı yarıya inince aynı fare hareketi ekranda iki kat yol alır; 0.5 bunu dengeler.")]
		[SerializeField]
		[Range(0.1f, 1f)]
		private float adsSensitivityMultiplier = 0.5f;

		[Tooltip("Nişan alırken silah modelinin fare sallantısı (WeaponSway) bununla çarpılır. 0 = nişangâh fareye hiç takılmaz.")]
		[SerializeField]
		[Range(0f, 1f)]
		private float adsSwayMultiplier = 0.5f;

		[Tooltip("Nişan alırken silah modelinin yürüme sallantısı (WeaponSway'in bob'u) bununla çarpılır.")]
		[SerializeField]
		[Range(0f, 1f)]
		private float adsBobMultiplier = 0.5f;

		[Tooltip("WeaponPivot'un ADS pozisyonuna gidiş ve dönüş hızı. Yüksek = daha ani.")]
		[SerializeField]
		[Min(1f)]
		private float adsMoveSpeed = 14f;

		private PlayerWeapons weapons;

		private PlayerMovement movement;

		private FirstPersonLook look;

		private float worldRestFov;

		private float weaponRestFov;

		private bool restFovCaptured;

		private bool scopeLatched;

		private bool scopeButtonWasDown;

		private readonly NetworkVariable<bool> aimingDown = new NetworkVariable<bool>(value: false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

		private PlayerAnimator playerAnimator;

		private Transform aimPivot;

		private float aimWeight;

		private Vector3 aimLastBase;

		private Vector3 aimLastWritten;

		private bool aimWroteLastFrame;

		private PlayerCameraRig cameraRig;

		private PlayerVoxelBody voxelBody;

		private float WorldRestFov
		{
			get
			{
				if (!(GameSettings.FieldOfView > 0f))
				{
					return worldRestFov;
				}
				return GameSettings.FieldOfView;
			}
		}

		public bool IsScoped { get; private set; }

		public bool IsAiming { get; private set; }

		public float RecoilMultiplier
		{
			get
			{
				if (!IsAiming)
				{
					return 1f;
				}
				return adsRecoilMultiplier;
			}
		}

		private WeaponDefinition ScopedWeapon
		{
			get
			{
				if (weapons == null || weapons.Equipped == null || !weapons.Equipped.Scope || WearingModel)
				{
					return null;
				}
				if ((!(movement == null) && !movement.IsArmed) || weapons.IsSwitching)
				{
					return null;
				}
				return weapons.Equipped;
			}
		}

		private bool WearingModel
		{
			get
			{
				if (voxelBody == null)
				{
					voxelBody = GetComponent<PlayerVoxelBody>();
				}
				if (voxelBody != null)
				{
					return voxelBody.HasVoxelBody;
				}
				return false;
			}
		}

		private WeaponDefinition AimedWeapon
		{
			get
			{
				if (!Features.AimDownSights || weapons == null || weapons.Equipped == null || weapons.Equipped.Scope || WearingModel)
				{
					return null;
				}
				if ((!(movement == null) && !movement.IsArmed) || weapons.IsSwitching)
				{
					return null;
				}
				return weapons.Equipped;
			}
		}

		public override void OnNetworkSpawn()
		{
			weapons = GetComponent<PlayerWeapons>();
			movement = GetComponent<PlayerMovement>();
			look = GetComponentInChildren<FirstPersonLook>(includeInactive: true);
		}

		private void Update()
		{
			if (playerAnimator == null)
			{
				playerAnimator = GetComponentInChildren<PlayerAnimator>(includeInactive: true);
			}
			if (playerAnimator != null)
			{
				playerAnimator.SetAiming(aimingDown.Value);
			}
			if (base.IsOwner)
			{
				ResolveCameras();
				WeaponDefinition scopedWeapon = ScopedWeapon;
				WeaponDefinition weaponDefinition = ((scopedWeapon == null) ? AimedWeapon : null);
				bool num = WantsScope(scopedWeapon ?? weaponDefinition, (scopedWeapon != null) ? GameSettings.ToggleScope : GameSettings.ToggleAds);
				bool flag = num && scopedWeapon != null;
				bool flag2 = num && weaponDefinition != null;
				SetScoped(flag);
				IsAiming = flag2;
				HoldFirstPerson();
				bool flag3 = IsScoped || IsAiming;
				if (base.IsSpawned && aimingDown.Value != flag3)
				{
					aimingDown.Value = flag3;
				}
				if (CrosshairView.Instance != null)
				{
					CrosshairView.Instance.SetSuppressed(scopedWeapon != null || flag2);
				}
				ApplySensitivity(flag ? (scopedWeapon.ScopeSensitivityMultiplier * GameSettings.ScopeSensitivity) : (flag2 ? adsSensitivityMultiplier : 1f));
				if (viewModelSway != null)
				{
					viewModelSway.SetAimScale(flag2 ? adsSwayMultiplier : 1f, flag2 ? adsBobMultiplier : 1f);
				}
				float target = (flag ? scopedWeapon.ScopeFieldOfView : (flag2 ? (WorldRestFov * adsFieldOfViewMultiplier) : WorldRestFov));
				ApproachFov(worldCamera, target);
				if (viewModelCamera != null)
				{
					float target2 = ((!flag) ? (flag2 ? (viewModelRestFov * adsFieldOfViewMultiplier) : viewModelRestFov) : ((weaponCamera != null) ? scopedWeapon.ScopeFieldOfView : viewModelRestFov));
					ApproachFov(viewModelCamera, target2);
				}
			}
		}

		private void LateUpdate()
		{
			if (base.IsOwner)
			{
				MoveAimPivot();
			}
		}

		private void MoveAimPivot()
		{
			WeaponVisual weaponVisual = ((weapons != null) ? weapons.FpsVisual : null);
			Transform transform = ((weaponVisual != null) ? weaponVisual.AimPivot : null);
			if (transform != aimPivot)
			{
				aimPivot = transform;
				aimWeight = 0f;
				aimWroteLastFrame = false;
			}
			if (aimPivot == null)
			{
				return;
			}
			float num = (IsAiming ? 1f : 0f);
			aimWeight = Mathf.Lerp(aimWeight, num, 1f - Mathf.Exp((0f - adsMoveSpeed) * Time.deltaTime));
			if (Mathf.Abs(aimWeight - num) < 0.001f)
			{
				aimWeight = num;
			}
			Vector3 localPosition = aimPivot.localPosition;
			Vector3 vector = ((aimWroteLastFrame && localPosition == aimLastWritten) ? aimLastBase : localPosition);
			if (aimWeight <= 0f)
			{
				if (aimWroteLastFrame)
				{
					aimPivot.localPosition = vector;
					aimWroteLastFrame = false;
					weaponVisual.SolveArms();
				}
			}
			else
			{
				Vector3 vector2 = weaponVisual.AdsPivotDeltaFrom(vector, aimPivot.localRotation, aimPivot.localScale) * aimWeight;
				Vector3 localPosition2 = vector + vector2;
				aimPivot.localPosition = localPosition2;
				aimLastBase = vector;
				aimLastWritten = localPosition2;
				aimWroteLastFrame = true;
				weaponVisual.SolveArms();
			}
		}

		private void ReleaseAimPivot()
		{
			if (aimWroteLastFrame && aimPivot != null && aimPivot.localPosition == aimLastWritten)
			{
				aimPivot.localPosition = aimLastBase;
			}
			aimWroteLastFrame = false;
			aimWeight = 0f;
		}

		private void HoldFirstPerson()
		{
			if (cameraRig == null)
			{
				cameraRig = GetComponent<PlayerCameraRig>();
			}
			if (cameraRig != null)
			{
				cameraRig.ForceFirstPerson = IsScoped || IsAiming;
			}
		}

		private bool WantsScope(WeaponDefinition weapon, bool toggle)
		{
			bool flag = GameInput.Scope.IsPressed();
			if (!(weapon != null) || GameMenuState.LookCaptured)
			{
				scopeLatched = false;
				scopeButtonWasDown = flag;
				return false;
			}
			if (!toggle)
			{
				scopeLatched = false;
				scopeButtonWasDown = flag;
				return flag;
			}
			if (flag && !scopeButtonWasDown)
			{
				scopeLatched = !scopeLatched;
			}
			scopeButtonWasDown = flag;
			return scopeLatched;
		}

		private void SetScoped(bool scoped)
		{
			if (IsScoped == scoped)
			{
				return;
			}
			IsScoped = scoped;
			if (!(ScopeView.Instance == null))
			{
				if (scoped)
				{
					ScopeView.Instance.Show();
				}
				else
				{
					ScopeView.Instance.Hide();
				}
			}
		}

		private void ApplySensitivity(float multiplier)
		{
			if (look == null)
			{
				look = GetComponentInChildren<FirstPersonLook>(includeInactive: true);
			}
			if (look != null)
			{
				look.SensitivityMultiplier = multiplier;
			}
		}

		private void ApproachFov(Camera camera, float target)
		{
			if (!(camera == null) && !Mathf.Approximately(camera.fieldOfView, target))
			{
				float num = Mathf.Lerp(camera.fieldOfView, target, 1f - Mathf.Exp((0f - zoomSpeed) * Time.deltaTime));
				camera.fieldOfView = ((Mathf.Abs(num - target) < 0.05f) ? target : num);
			}
		}

		private void ResolveCameras()
		{
			if (worldCamera != null)
			{
				CaptureRestFov();
				return;
			}
			PlayerCameraRig component = GetComponent<PlayerCameraRig>();
			worldCamera = ((component != null) ? component.FpsCamera : null);
			CaptureRestFov();
		}

		private void CaptureRestFov()
		{
			if (!restFovCaptured && !(worldCamera == null))
			{
				restFovCaptured = true;
				worldRestFov = worldCamera.fieldOfView;
				viewModelCamera = ((weaponCamera != null) ? weaponCamera : FindViewModelCamera(worldCamera));
				viewModelRestFov = ((viewModelCamera != null) ? viewModelCamera.fieldOfView : 0f);
				viewModelSway = worldCamera.GetComponentInChildren<WeaponSway>(includeInactive: true);
			}
		}

		private static Camera FindViewModelCamera(Camera world)
		{
			Camera[] componentsInChildren = world.GetComponentsInChildren<Camera>(includeInactive: true);
			foreach (Camera camera in componentsInChildren)
			{
				if (camera != world)
				{
					return camera;
				}
			}
			return null;
		}

		private void OnDisable()
		{
			SetScoped(scoped: false);
			IsAiming = false;
			HoldFirstPerson();
			ReleaseAimPivot();
			if (base.IsSpawned && base.IsOwner && aimingDown.Value)
			{
				aimingDown.Value = false;
			}
			ApplySensitivity(1f);
			scopeLatched = false;
			scopeButtonWasDown = false;
			if (restFovCaptured && worldCamera != null)
			{
				worldCamera.fieldOfView = WorldRestFov;
			}
			if (restFovCaptured && viewModelCamera != null)
			{
				viewModelCamera.fieldOfView = viewModelRestFov;
			}
			if (viewModelSway != null)
			{
				viewModelSway.SetAimScale(1f, 1f);
			}
		}

		protected override void __initializeVariables()
		{
			if (aimingDown == null)
			{
				throw new Exception("PlayerScope.aimingDown cannot be null. All NetworkVariableBase instances must be initialized.");
			}
			aimingDown.Initialize(this);
			__nameNetworkVariable(aimingDown, "aimingDown");
			NetworkVariableFields.Add(aimingDown);
			base.__initializeVariables();
		}

		protected override void __initializeRpcs()
		{
			base.__initializeRpcs();
		}

		protected internal override string __getTypeName()
		{
			return "PlayerScope";
		}
	}
}
