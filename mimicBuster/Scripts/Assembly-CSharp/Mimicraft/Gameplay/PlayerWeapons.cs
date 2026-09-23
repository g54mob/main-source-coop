using System;
using System.Collections.Generic;
using Mimicraft.Cameras;
using Mimicraft.Customization;
using Mimicraft.Localization;
using Mimicraft.Networking;
using Mimicraft.Settings;
using Mimicraft.UI;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Rendering;

namespace Mimicraft.Gameplay
{
	[RequireComponent(typeof(NetworkObject))]
	public class PlayerWeapons : NetworkBehaviour
	{
		private enum SwitchPhase
		{
			None = 0,
			Unequipping = 1,
			Equipping = 2
		}

		[Tooltip("Bir silahı almak için ona ne kadar yakından bakman gerektiği, metre.")]
		[SerializeField]
		[Min(0.5f)]
		private float pickupRange = 3f;

		[Tooltip("Nereden bakıldığı sayılıyor - göz hizası. Kapsülün orijini ayakta olduğu için elle veriliyor.")]
		[SerializeField]
		private float eyeHeight = 1.6f;

		[Tooltip("Bakış ışınının çarpabileceği katmanlar. Silah objeleri ve önlerini kapatabilecek duvarlar dahil olmalı - duvarın ardındaki silahı almamak için ışının duvara çarpabilmesi gerekiyor.")]
		[SerializeField]
		private LayerMask pickupLayers = -1;

		[Tooltip("BİRİNCİ ŞAHIS görünümündeki silahın bağlanacağı nokta - WeaponParent. Sadece sahibi görür. Boş bırakılırsa birinci şahıs modeli hiç oluşturulmaz.")]
		[SerializeField]
		private Transform fpsSocket;

		[Tooltip("Birinci şahıs view model'inin katmanı - sadece WeaponCamera'nın çizdiği katman. Boş bırakılırsa soketin katmanı miras alınır (eski davranış). Ayrı bir alan çünkü bir view model'in katmanı, nereye parent'landığının değil NE olduğunun özelliği: soket Default'ta unutulunca silah dünyada herkesin önünde asılı kalıyordu ve bunu söyleyen hiçbir şey yoktu.")]
		[SerializeField]
		private string fpsLayerName = "FPSVisual";

		[Tooltip("ÜÇÜNCÜ ŞAHIS silahının bağlanacağı nokta - karakter modelinin sağ eli. HERKES bunu görür, sahibi dahil değil. Boş bırakılırsa karakterin eli boş görünür.")]
		[SerializeField]
		private Transform tpsSocket;

		[Tooltip("Silahsızken birinci şahısta gösterilecek view model - yumruk kolları. Üstünde FirstPersonArmSlots olmalı, tıpkı bir silahın HeldPrefab'inde olduğu gibi; oyuncunun kendi modellediği kolları oraya takılıyor. Boş bırakılırsa silahsız oyuncu birinci şahısta hiçbir şey görmez - eski davranış.")]
		[SerializeField]
		private GameObject unarmedFpsPrefab;

		[Header("Kuşanma animasyonları (Equip / Unequip)")]
		[Tooltip("Açıkken silah değiştirmek ve silahı asıp çıkarmak anında olmaz: önce Unequip oynar, model değişir, sonra Equip oynar. Karakterin Animator'ünde ve/veya birinci şahıs silah prefabının Animator'ünde bu isimde Trigger parametreleri YOKSA işlevsizdir - geçiş eskisi gibi anında olur. Geçiş sürerken ateş edilemez ve nişan alınamaz.")]
		[SerializeField]
		private bool playSwitchAnimations = true;

		[Tooltip("Elindeki silahı indirirken tetiklenen Trigger'ın adı.")]
		[SerializeField]
		private string unequipTrigger = "Unequip";

		[Tooltip("Yeni silahı çıkarırken tetiklenen Trigger'ın adı.")]
		[SerializeField]
		private string equipTrigger = "Equip";

		[Tooltip("Unequip'in süresi, saniye. Klip bu süreye sığacak hızda oynar (bkz. Switch Speed Parameter) ve model süre dolunca, yani klip bittiğinde değişir.")]
		[SerializeField]
		[Min(0f)]
		private float unequipSeconds = 0.35f;

		[Tooltip("Equip'in süresi, saniye. Klip bu süreye sığacak hızda oynar; süre dolana kadar ateş edilemez ve nişan alınamaz.")]
		[SerializeField]
		[Min(0f)]
		private float equipSeconds = 0.45f;

		[Tooltip("Equip ve Unequip kliplerinin kendi uzunluğu, saniye. Hız = bu değer / süre - 1 sn'lik bir klip 0.35 sn sürecekse 1 / 0.35 ≈ 2.86 hızında oynar.")]
		[SerializeField]
		[Min(0.01f)]
		private float switchClipSeconds = 1f;

		[Tooltip("Hızın yazılacağı FLOAT parametresinin adı. Animator'de Equip ve Unequip state'lerinde Speed > Multiplier kutusunu işaretleyip bu parametreyi seç. Her tetiklemeden hemen önce ayarlanır. Parametre yoksa klipler kendi hızında oynar - süreler yine beklenir.")]
		[SerializeField]
		private string switchSpeedParameter = "SwitchSpeed";

		[Header("El IK hedefleri")]
		[Tooltip("Karakterin sol eli için TwoBoneIK hedefi. Silah kuşanılınca modelin sol kavrama noktasına taşınır. Boş bırakılırsa sol el animasyonun bıraktığı yerde kalır.")]
		[SerializeField]
		private Transform leftHandIkTarget;

		[Tooltip("Sağ el için aynısı.")]
		[SerializeField]
		private Transform rightHandIkTarget;

		[Tooltip("Oyuncunun doğduğu anda kuşandığı silah. Boş bırakılırsa eli boş başlar ve haritadan bir silah alana kadar öyle kalır.\n\nHaritadan alınan bir silahın üstüne yazmaz - sadece doğuşta, o da elde hiçbir şey yokken uygulanır.")]
		[SerializeField]
		private WeaponDefinition defaultWeapon;

		private readonly NetworkVariable<FixedString32Bytes> equippedId = new NetworkVariable<FixedString32Bytes>();

		private FirstPersonLook look;

		private const float PickupMatchTolerance = 0.5f;

		private PlayerMovement movement;

		private SwitchPhase switchPhase;

		private float switchTimer;

		private bool builtOnce;

		private WeaponDefinition builtShown;

		private bool equipViewModelPending;

		private bool equipOnCharacter;

		private WeaponSway tpsSway;

		private bool warnedAboutFpsLayer;

		private bool restPosesCaptured;

		private Vector3 leftRestPosition;

		private Quaternion leftRestRotation = Quaternion.identity;

		private Vector3 rightRestPosition;

		private Quaternion rightRestRotation = Quaternion.identity;

		private PlayerRagdoll ragdoll;

		private bool armedWhenBuilt;

		private string lastWeaponReport;

		private bool hasWeaponReport;

		private GameObject fpsInstance;

		private GameObject previousFpsInstance;

		private GameObject tpsInstance;

		private PlayerAnimator playerAnimator;

		private PlayerCameraRig cameraRig;

		public WeaponDefinition Equipped { get; private set; }

		public WeaponDefinition Shown => builtShown;

		public WeaponPickup HoveredPickup { get; private set; }

		public DuelDoor HoveredDuelDoor { get; private set; }

		public ShootingRangeButton HoveredRangeButton { get; private set; }

		private bool ShouldShowWeapon
		{
			get
			{
				if (Equipped == null)
				{
					return false;
				}
				if (movement == null)
				{
					movement = GetComponent<PlayerMovement>();
				}
				if (!(movement == null))
				{
					return movement.IsArmed;
				}
				return true;
			}
		}

		public GameObject FirstPersonInstance => fpsInstance;

		public bool IsSwitching => switchPhase != SwitchPhase.None;

		public GameObject ThirdPersonInstance => tpsInstance;

		public WeaponSway TpsSway
		{
			get
			{
				if (tpsSway != null || tpsSocket == null)
				{
					return tpsSway;
				}
				tpsSway = tpsSocket.GetComponent<WeaponSway>();
				if (tpsSway == null)
				{
					tpsSway = tpsSocket.gameObject.AddComponent<WeaponSway>();
					tpsSway.ConfigureRecoilOnly();
				}
				return tpsSway;
			}
		}

		private bool HeldModelWentMissing
		{
			get
			{
				if (armedWhenBuilt && Equipped != null)
				{
					if (!Gone(fpsInstance, Equipped.HeldPrefab, fpsSocket))
					{
						return Gone(tpsInstance, Equipped.TpsPrefab, tpsSocket);
					}
					return true;
				}
				return false;
			}
		}

		public WeaponVisual FpsVisual { get; private set; }

		public WeaponVisual TpsVisual { get; private set; }

		public override void OnNetworkSpawn()
		{
			NetworkVariable<FixedString32Bytes> networkVariable = equippedId;
			networkVariable.OnValueChanged = (NetworkVariable<FixedString32Bytes>.OnValueChangedDelegate)Delegate.Combine(networkVariable.OnValueChanged, new NetworkVariable<FixedString32Bytes>.OnValueChangedDelegate(OnEquippedChanged));
			ApplyEquipped(equippedId.Value.ToString());
			if (base.IsServer && defaultWeapon != null && equippedId.Value.IsEmpty && ModeCarriesWeapons())
			{
				ServerEquip(defaultWeapon);
			}
		}

		public override void OnNetworkDespawn()
		{
			NetworkVariable<FixedString32Bytes> networkVariable = equippedId;
			networkVariable.OnValueChanged = (NetworkVariable<FixedString32Bytes>.OnValueChangedDelegate)Delegate.Remove(networkVariable.OnValueChanged, new NetworkVariable<FixedString32Bytes>.OnValueChangedDelegate(OnEquippedChanged));
		}

		private void OnEquippedChanged(FixedString32Bytes previous, FixedString32Bytes current)
		{
			ApplyEquipped(current.ToString());
		}

		private void ApplyEquipped(string weaponId)
		{
			Equipped = WeaponCatalog.Find(weaponId);
			if (!builtOnce)
			{
				RebuildHeldModel();
			}
		}

		public void ServerEquip(WeaponDefinition weapon)
		{
			if (base.IsServer)
			{
				FixedString32Bytes fixedString32Bytes = ((weapon != null) ? new FixedString32Bytes(weapon.WeaponId) : default(FixedString32Bytes));
				if (!equippedId.Value.Equals(fixedString32Bytes))
				{
					equippedId.Value = fixedString32Bytes;
				}
			}
		}

		public void ServerApplyRoundLoadout(bool shouldCarry)
		{
			if (base.IsServer)
			{
				shouldCarry &= ModeCarriesWeapons();
				if (!shouldCarry)
				{
					ServerEquip(null);
				}
				else if (Equipped == null)
				{
					ServerEquip(defaultWeapon);
				}
			}
		}

		private bool ModeCarriesWeapons()
		{
			GameModeController current = GameModeController.Current;
			if (!(current == null))
			{
				return current.ModeUsesWeapons;
			}
			return true;
		}

		private void Update()
		{
			if (!base.IsOwner)
			{
				return;
			}
			WeaponPickup weaponPickup = FindLookedAtPickup();
			ShootingRangeButton shootingRangeButton = ((weaponPickup == null) ? FindLookedAtButton() : null);
			DuelDoor duelDoor = ((weaponPickup == null && shootingRangeButton == null) ? FindLookedAtDoor() : null);
			if (duelDoor != HoveredDuelDoor)
			{
				if (HoveredDuelDoor != null)
				{
					HoveredDuelDoor.SetHighlighted(highlighted: false);
				}
				HoveredDuelDoor = duelDoor;
				if (HoveredDuelDoor != null)
				{
					HoveredDuelDoor.SetHighlighted(highlighted: true);
				}
			}
			if (shootingRangeButton != HoveredRangeButton)
			{
				if (HoveredRangeButton != null)
				{
					HoveredRangeButton.SetHighlighted(highlighted: false);
				}
				HoveredRangeButton = shootingRangeButton;
				if (HoveredRangeButton != null)
				{
					HoveredRangeButton.SetHighlighted(highlighted: true);
				}
			}
			if (weaponPickup != HoveredPickup)
			{
				if (HoveredPickup != null)
				{
					HoveredPickup.SetHighlighted(highlighted: false);
				}
				HoveredPickup = weaponPickup;
				if (HoveredPickup != null)
				{
					HoveredPickup.SetHighlighted(highlighted: true);
				}
			}
			ShowPrompt();
			if (GameInput.Interact.WasPressedThisFrame() && !GameMenuState.InputCaptured)
			{
				if (HoveredPickup != null)
				{
					RequestPickupServerRpc(HoveredPickup.transform.position);
				}
				else if (HoveredRangeButton != null)
				{
					RequestRangeStopServerRpc(HoveredRangeButton.transform.position);
				}
				else if (HoveredDuelDoor != null)
				{
					HoveredDuelDoor.Interact();
				}
			}
		}

		private ShootingRangeButton FindLookedAtButton()
		{
			if (!LookedAt(out var hit))
			{
				return null;
			}
			return hit.collider.GetComponentInParent<ShootingRangeButton>();
		}

		private DuelDoor FindLookedAtDoor()
		{
			if (!LookedAt(out var hit))
			{
				return null;
			}
			DuelDoor componentInParent = hit.collider.GetComponentInParent<DuelDoor>();
			if (!(componentInParent != null) || !componentInParent.IsUsable)
			{
				return null;
			}
			return componentInParent;
		}

		[Rpc(SendTo.Server, InvokePermission = RpcInvokePermission.Everyone)]
		private void RequestRangeStopServerRpc(Vector3 buttonPosition, RpcParams rpcParams = default(RpcParams))
		{
			NetworkManager networkManager = base.NetworkManager;
			if ((object)networkManager == null || !networkManager.IsListening)
			{
				Debug.LogError("Rpc methods can only be invoked after starting the NetworkManager!");
				return;
			}
			if (__rpc_exec_stage != __RpcExecStage.Execute)
			{
				RpcParams rpcParams2 = rpcParams;
				RpcAttribute.RpcAttributeParams attributeParams = new RpcAttribute.RpcAttributeParams
				{
					InvokePermission = RpcInvokePermission.Everyone
				};
				FastBufferWriter bufferWriter = __beginSendRpc(1929014657u, rpcParams2, attributeParams, SendTo.Server, RpcDelivery.Reliable);
				bufferWriter.WriteValueSafe(in buttonPosition);
				__endSendRpc(ref bufferWriter, 1929014657u, rpcParams, attributeParams, SendTo.Server, RpcDelivery.Reliable);
			}
			if (__rpc_exec_stage != __RpcExecStage.Execute)
			{
				return;
			}
			__rpc_exec_stage = __RpcExecStage.Send;
			if (rpcParams.Receive.SenderClientId != base.OwnerClientId)
			{
				return;
			}
			ShootingRangeButton shootingRangeButton = ShootingRangeButton.Nearest(buttonPosition, 0.5f);
			if (!(shootingRangeButton == null) && !(shootingRangeButton.Station == null))
			{
				float num = pickupRange * 2f;
				if (!((shootingRangeButton.transform.position - base.transform.position).sqrMagnitude > num * num))
				{
					MoveRangeTargetClientRpc(shootingRangeButton.Station.transform.position, shootingRangeButton.Stop);
				}
			}
		}

		[Rpc(SendTo.ClientsAndHost)]
		private void MoveRangeTargetClientRpc(Vector3 stationPosition, int stop)
		{
			NetworkManager networkManager = base.NetworkManager;
			if ((object)networkManager == null || !networkManager.IsListening)
			{
				Debug.LogError("Rpc methods can only be invoked after starting the NetworkManager!");
				return;
			}
			if (__rpc_exec_stage != __RpcExecStage.Execute)
			{
				RpcAttribute.RpcAttributeParams attributeParams = default(RpcAttribute.RpcAttributeParams);
				RpcParams rpcParams = default(RpcParams);
				FastBufferWriter bufferWriter = __beginSendRpc(974522609u, rpcParams, attributeParams, SendTo.ClientsAndHost, RpcDelivery.Reliable);
				bufferWriter.WriteValueSafe(in stationPosition);
				BytePacker.WriteValueBitPacked(bufferWriter, stop);
				__endSendRpc(ref bufferWriter, 974522609u, rpcParams, attributeParams, SendTo.ClientsAndHost, RpcDelivery.Reliable);
			}
			if (__rpc_exec_stage == __RpcExecStage.Execute)
			{
				__rpc_exec_stage = __RpcExecStage.Send;
				ShootingRangeStation shootingRangeStation = ShootingRangeStation.Nearest(stationPosition, 0.5f);
				if (shootingRangeStation != null)
				{
					shootingRangeStation.GoTo(stop);
				}
			}
		}

		[Rpc(SendTo.SpecifiedInParams)]
		public void RangeHitClientRpc(int points, RpcParams rpcParams = default(RpcParams))
		{
			NetworkManager networkManager = base.NetworkManager;
			if ((object)networkManager == null || !networkManager.IsListening)
			{
				Debug.LogError("Rpc methods can only be invoked after starting the NetworkManager!");
				return;
			}
			if (__rpc_exec_stage != __RpcExecStage.Execute)
			{
				RpcAttribute.RpcAttributeParams attributeParams = default(RpcAttribute.RpcAttributeParams);
				FastBufferWriter bufferWriter = __beginSendRpc(1582666596u, rpcParams, attributeParams, SendTo.SpecifiedInParams, RpcDelivery.Reliable);
				BytePacker.WriteValueBitPacked(bufferWriter, points);
				__endSendRpc(ref bufferWriter, 1582666596u, rpcParams, attributeParams, SendTo.SpecifiedInParams, RpcDelivery.Reliable);
			}
			if (__rpc_exec_stage == __RpcExecStage.Execute)
			{
				__rpc_exec_stage = __RpcExecStage.Send;
				if (ScorePopupView.Instance != null)
				{
					ScorePopupView.Instance.Pop(points);
				}
			}
		}

		private void ShowPrompt()
		{
			if (!(InteractPromptView.Instance == null))
			{
				if (HoveredPickup == null && HoveredRangeButton != null && !GameMenuState.InputCaptured)
				{
					InteractPromptView.Instance.Show(Loc.Format("Interact.PickUp", HoveredRangeButton.Prompt));
					return;
				}
				if (HoveredPickup == null && HoveredDuelDoor != null && !GameMenuState.InputCaptured)
				{
					InteractPromptView.Instance.Show(HoveredDuelDoor.Prompt);
					return;
				}
				if (HoveredPickup == null || GameMenuState.InputCaptured)
				{
					InteractPromptView.Instance.Hide();
					return;
				}
				string prompt = HoveredPickup.Prompt;
				bool flag = Equipped != null;
				InteractPromptView.Instance.Show(string.IsNullOrEmpty(prompt) ? Loc.Get(flag ? "Interact.SwapWeapon" : "Interact.PickUpWeapon") : Loc.Format(flag ? "Interact.Swap" : "Interact.PickUp", prompt));
			}
		}

		private void OnDisable()
		{
			EndSwitchNow();
			if (HoveredPickup != null)
			{
				HoveredPickup.SetHighlighted(highlighted: false);
				HoveredPickup = null;
			}
			if (HoveredDuelDoor != null)
			{
				HoveredDuelDoor.SetHighlighted(highlighted: false);
				HoveredDuelDoor = null;
			}
			if (base.IsOwner && InteractPromptView.Instance != null)
			{
				InteractPromptView.Instance.Hide();
			}
		}

		private WeaponPickup FindLookedAtPickup()
		{
			if (!LookedAt(out var hit))
			{
				return null;
			}
			return hit.collider.GetComponentInParent<WeaponPickup>();
		}

		private bool LookedAt(out RaycastHit hit)
		{
			if (cameraRig == null)
			{
				cameraRig = GetComponent<PlayerCameraRig>();
			}
			float extraRange = 0f;
			if (cameraRig == null || !cameraRig.TryGetAimRay(eyeHeight, out var ray, out extraRange))
			{
				if (look == null)
				{
					look = GetComponentInChildren<FirstPersonLook>(includeInactive: true);
				}
				float x = ((look != null) ? Mathf.Clamp(look.Pitch, look.MinPitch, look.MaxPitch) : 0f);
				Quaternion quaternion = base.transform.rotation * Quaternion.Euler(x, 0f, 0f);
				ray = new Ray(base.transform.position + Vector3.up * eyeHeight, quaternion * Vector3.forward);
			}
			return Physics.Raycast(ray, out hit, pickupRange + extraRange, pickupLayers, QueryTriggerInteraction.Collide);
		}

		[Rpc(SendTo.Server, InvokePermission = RpcInvokePermission.Everyone)]
		private void RequestPickupServerRpc(Vector3 pickupPosition, RpcParams rpcParams = default(RpcParams))
		{
			NetworkManager networkManager = base.NetworkManager;
			if ((object)networkManager == null || !networkManager.IsListening)
			{
				Debug.LogError("Rpc methods can only be invoked after starting the NetworkManager!");
				return;
			}
			if (__rpc_exec_stage != __RpcExecStage.Execute)
			{
				RpcParams rpcParams2 = rpcParams;
				RpcAttribute.RpcAttributeParams attributeParams = new RpcAttribute.RpcAttributeParams
				{
					InvokePermission = RpcInvokePermission.Everyone
				};
				FastBufferWriter bufferWriter = __beginSendRpc(2069929923u, rpcParams2, attributeParams, SendTo.Server, RpcDelivery.Reliable);
				bufferWriter.WriteValueSafe(in pickupPosition);
				__endSendRpc(ref bufferWriter, 2069929923u, rpcParams, attributeParams, SendTo.Server, RpcDelivery.Reliable);
			}
			if (__rpc_exec_stage != __RpcExecStage.Execute)
			{
				return;
			}
			__rpc_exec_stage = __RpcExecStage.Send;
			if (rpcParams.Receive.SenderClientId != base.OwnerClientId)
			{
				return;
			}
			float num = pickupRange * 2f;
			if (!((pickupPosition - base.transform.position).sqrMagnitude > num * num))
			{
				WeaponPickup weaponPickup = FindPickupAt(pickupPosition);
				if (weaponPickup != null && weaponPickup.Weapon != null)
				{
					ServerEquip(weaponPickup.Weapon);
				}
			}
		}

		private static WeaponPickup FindPickupAt(Vector3 position)
		{
			WeaponPickup result = null;
			float num = 0.25f;
			WeaponPickup[] array = UnityEngine.Object.FindObjectsByType<WeaponPickup>(FindObjectsSortMode.None);
			foreach (WeaponPickup weaponPickup in array)
			{
				float sqrMagnitude = (weaponPickup.transform.position - position).sqrMagnitude;
				if (!(sqrMagnitude > num))
				{
					num = sqrMagnitude;
					result = weaponPickup;
				}
			}
			return result;
		}

		private void RebuildHeldModel()
		{
			armedWhenBuilt = ShouldShowWeapon;
			bool flag = armedWhenBuilt && Equipped != null;
			builtOnce = true;
			builtShown = (flag ? Equipped : null);
			GameObject prefab = (flag ? Equipped.HeldPrefab : unarmedFpsPrefab);
			fpsInstance = Respawn(fpsInstance, prefab, fpsSocket, ResolveFpsLayer());
			if (fpsInstance != null)
			{
				WeaponVisual[] componentsInChildren = fpsInstance.GetComponentsInChildren<WeaponVisual>(includeInactive: true);
				for (int i = 0; i < componentsInChildren.Length; i++)
				{
					componentsInChildren[i].SimulateFlashWithWeapon();
				}
			}
			tpsInstance = Respawn(tpsInstance, flag ? Equipped.TpsPrefab : null, tpsSocket, null);
			FpsVisual = ((fpsInstance != null) ? fpsInstance.GetComponentInChildren<WeaponVisual>(includeInactive: true) : null);
			TpsVisual = ((tpsInstance != null) ? tpsInstance.GetComponentInChildren<WeaponVisual>(includeInactive: true) : null);
			if (FpsVisual != null)
			{
				FpsVisual.SolveArms();
			}
			if (cameraRig == null)
			{
				cameraRig = GetComponent<PlayerCameraRig>();
			}
			if (cameraRig != null)
			{
				cameraRig.RefreshCharacterRenderers();
			}
			if (playerAnimator == null)
			{
				playerAnimator = GetComponentInChildren<PlayerAnimator>(includeInactive: true);
			}
			if (playerAnimator != null)
			{
				playerAnimator.SetCustomWeaponHeld(flag);
				if (fpsInstance != previousFpsInstance)
				{
					playerAnimator.RebuildRig();
				}
			}
			previousFpsInstance = fpsInstance;
		}

		private PlayerAnimator SwitchAnimator()
		{
			if (playerAnimator == null)
			{
				playerAnimator = GetComponentInChildren<PlayerAnimator>(includeInactive: true);
			}
			return playerAnimator;
		}

		private void UpdateHeldModel()
		{
			if (switchPhase == SwitchPhase.None && HeldModelWentMissing)
			{
				RebuildHeldModel();
				return;
			}
			if ((ShouldShowWeapon ? Equipped : null) != builtShown && switchPhase != SwitchPhase.Unequipping)
			{
				BeginSwitch();
			}
			TickSwitch();
		}

		private void BeginSwitch()
		{
			if (playSwitchAnimations && fpsInstance != null)
			{
				PlayerAnimator playerAnimator = SwitchAnimator();
				bool num = playerAnimator != null && builtShown != null && TriggerSwitch(playerAnimator.Animator, unequipTrigger, unequipSeconds);
				bool flag = TriggerSwitch(ViewModelAnimator(), unequipTrigger, unequipSeconds);
				if (num || flag)
				{
					switchPhase = SwitchPhase.Unequipping;
					switchTimer = unequipSeconds;
					if (playerAnimator != null)
					{
						playerAnimator.HoldArmedPose(hold: true);
					}
					return;
				}
			}
			SwapAndEquip();
		}

		private void SwapAndEquip()
		{
			RebuildHeldModel();
			PlayerAnimator playerAnimator = SwitchAnimator();
			if (playerAnimator != null)
			{
				playerAnimator.HoldArmedPose(hold: false);
			}
			if (!playSwitchAnimations || fpsInstance == null)
			{
				switchPhase = SwitchPhase.None;
				return;
			}
			equipOnCharacter = playerAnimator != null && builtShown != null && TriggerSwitch(playerAnimator.Animator, equipTrigger, equipSeconds);
			Animator animator = ViewModelAnimator();
			bool flag = animator != null && animator.isActiveAndEnabled;
			bool flag2 = false;
			if (flag)
			{
				animator.Update(0f);
				flag2 = TriggerSwitch(animator, equipTrigger, equipSeconds);
				if (flag2)
				{
					animator.Update(0f);
				}
			}
			equipViewModelPending = flag && !flag2 && !animator.isInitialized;
			if (!flag2 && !equipOnCharacter && !equipViewModelPending)
			{
				switchPhase = SwitchPhase.None;
				return;
			}
			switchPhase = SwitchPhase.Equipping;
			switchTimer = equipSeconds;
		}

		private void TickSwitch()
		{
			if (switchPhase == SwitchPhase.None)
			{
				return;
			}
			if (switchPhase == SwitchPhase.Equipping && equipViewModelPending)
			{
				Animator animator = ((fpsInstance != null) ? fpsInstance.GetComponentInChildren<Animator>(includeInactive: true) : null);
				bool flag = animator != null && animator.isActiveAndEnabled;
				if (!flag || animator.isInitialized)
				{
					equipViewModelPending = false;
					if ((!flag || !TriggerSwitch(animator, equipTrigger, equipSeconds)) && !equipOnCharacter)
					{
						switchPhase = SwitchPhase.None;
						return;
					}
				}
			}
			switchTimer -= Time.deltaTime;
			if (!(switchTimer > 0f))
			{
				if (switchPhase == SwitchPhase.Unequipping)
				{
					SwapAndEquip();
				}
				else
				{
					switchPhase = SwitchPhase.None;
				}
			}
		}

		private bool TriggerSwitch(Animator target, string trigger, float seconds)
		{
			float value = ((seconds > 0.0001f) ? Mathf.Min(switchClipSeconds / seconds, 100f) : 100f);
			PlayerAnimator.SetFloatIfPresent(target, switchSpeedParameter, value);
			return PlayerAnimator.TriggerIfPresent(target, trigger);
		}

		private Animator ViewModelAnimator()
		{
			if (!(fpsInstance != null))
			{
				return null;
			}
			return fpsInstance.GetComponentInChildren<Animator>(includeInactive: true);
		}

		private void EndSwitchNow()
		{
			if (switchPhase != SwitchPhase.None)
			{
				switchPhase = SwitchPhase.None;
				equipViewModelPending = false;
				PlayerAnimator playerAnimator = SwitchAnimator();
				if (playerAnimator != null)
				{
					playerAnimator.HoldArmedPose(hold: false);
				}
			}
		}

		public void DevShowWeapon(WeaponDefinition weapon)
		{
			tpsInstance = Respawn(tpsInstance, (weapon != null) ? weapon.TpsPrefab : null, tpsSocket, null);
			TpsVisual = ((tpsInstance != null) ? tpsInstance.GetComponentInChildren<WeaponVisual>(includeInactive: true) : null);
			builtShown = weapon;
		}

		public void DevFollowGrips()
		{
			FollowGrips();
		}

		private static GameObject Respawn(GameObject existing, GameObject prefab, Transform socket, int? layer)
		{
			if (existing != null)
			{
				UnityEngine.Object.Destroy(existing);
			}
			if (socket == null || prefab == null)
			{
				return null;
			}
			GameObject obj = UnityEngine.Object.Instantiate(prefab, socket);
			obj.transform.localPosition = Vector3.zero;
			obj.transform.localRotation = Quaternion.identity;
			SetLayerRecursively(obj, layer ?? socket.gameObject.layer);
			return obj;
		}

		private int? ResolveFpsLayer()
		{
			if (string.IsNullOrWhiteSpace(fpsLayerName))
			{
				return null;
			}
			int num = LayerMask.NameToLayer(fpsLayerName);
			if (num >= 0)
			{
				return num;
			}
			if (!warnedAboutFpsLayer)
			{
				warnedAboutFpsLayer = true;
				Debug.LogWarning("[PlayerWeapons] '" + fpsLayerName + "' diye bir katman yok - view model soketin katmanini alacak. Project Settings > Tags and Layers'a bak.", this);
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

		private void FollowGrips()
		{
			CaptureRestPoses();
			Follow(leftHandIkTarget, (TpsVisual != null) ? TpsVisual.LeftGrip : null, leftRestPosition, leftRestRotation);
			Follow(rightHandIkTarget, (TpsVisual != null) ? TpsVisual.RightGrip : null, rightRestPosition, rightRestRotation);
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

		private void CaptureRestPoses()
		{
			if (!restPosesCaptured)
			{
				restPosesCaptured = true;
				if (leftHandIkTarget != null)
				{
					leftRestPosition = leftHandIkTarget.localPosition;
					leftRestRotation = leftHandIkTarget.localRotation;
				}
				if (rightHandIkTarget != null)
				{
					rightRestPosition = rightHandIkTarget.localPosition;
					rightRestRotation = rightHandIkTarget.localRotation;
				}
			}
		}

		private void LateUpdate()
		{
			UpdateHeldModel();
			HideWhileDown();
			ReportMissingWeapon();
			FollowGrips();
		}

		private void HideWhileDown()
		{
			if (!(tpsInstance == null))
			{
				if (ragdoll == null)
				{
					ragdoll = GetComponent<PlayerRagdoll>();
				}
				bool flag = ragdoll != null && ragdoll.State != RagdollState.None;
				if (tpsInstance.activeSelf == flag)
				{
					tpsInstance.SetActive(!flag);
				}
			}
		}

		private static bool Gone(GameObject instance, GameObject prefab, Transform socket)
		{
			if (prefab != null && socket != null)
			{
				return instance == null;
			}
			return false;
		}

		private void ReportMissingWeapon()
		{
			string text = ((Equipped == null) ? "Equipped yok - equippedId bos (sunucu bu oyuncuya silah vermemis)" : ((!ShouldShowWeapon) ? ("'" + Equipped.WeaponId + "' var ama IsArmed=false - " + ((movement != null) ? movement.ArmedStateReport : "PlayerMovement yok")) : WatchedModelTrouble()));
			if (!(text == lastWeaponReport))
			{
				bool flag = hasWeaponReport && lastWeaponReport == null;
				hasWeaponReport = true;
				lastWeaponReport = text;
				if (text == null)
				{
					Debug.Log($"[Silah] {base.OwnerClientId}: GERI GELDI.", this);
				}
				else
				{
					Debug.LogWarning(string.Format("[Silah] {0}: {1} - {2}.", base.OwnerClientId, flag ? "KAYBOLDU" : "yok", text), this);
				}
			}
		}

		private string WatchedModelTrouble()
		{
			if (cameraRig == null)
			{
				cameraRig = GetComponent<PlayerCameraRig>();
			}
			if (!base.IsOwner || !(cameraRig != null) || !cameraRig.IsFirstPerson)
			{
				return Trouble("tps", tpsInstance, Equipped.TpsPrefab, tpsSocket);
			}
			return Trouble("fps", fpsInstance, Equipped.HeldPrefab, fpsSocket);
		}

		private static string Trouble(string which, GameObject instance, GameObject prefab, Transform socket)
		{
			if (prefab == null)
			{
				return which + ": silahin prefab'i bos";
			}
			if (socket == null)
			{
				return which + "Socket YOK - karakter modeli yeniden kurulmus olabilir";
			}
			if (instance == null)
			{
				return which + "Instance yok - Instantiate edilmemis ya da yok edilmis";
			}
			if (!instance.activeInHierarchy)
			{
				return which + "Instance kapali (ya da ust objesi kapali)";
			}
			Renderer[] componentsInChildren = instance.GetComponentsInChildren<Renderer>(includeInactive: true);
			if (componentsInChildren.Length == 0)
			{
				return which + ": '" + instance.name + "' icinde hic renderer yok - skin insa edilmemis";
			}
			List<string> list = new List<string>();
			int num = 0;
			Renderer[] array = componentsInChildren;
			foreach (Renderer renderer in array)
			{
				if (!(renderer == null))
				{
					if (!renderer.gameObject.activeInHierarchy)
					{
						list.Add(renderer.name + "=obje kapali");
					}
					else if (!renderer.enabled)
					{
						list.Add(renderer.name + "=renderer kapali");
					}
					else if (renderer.shadowCastingMode == ShadowCastingMode.ShadowsOnly)
					{
						list.Add(renderer.name + "=ShadowsOnly");
					}
					else
					{
						num++;
					}
				}
			}
			if (num == 0)
			{
				return which + ": hicbiri cizilmiyor - " + string.Join(", ", list);
			}
			foreach (string item in list)
			{
				if (item.StartsWith("WeaponSkin", StringComparison.Ordinal))
				{
					return $"{which}: SKIN gizli - {item} (cizilen {num} renderer: kollar)";
				}
			}
			if (instance.GetComponentInChildren<WeaponSkinAssembler>(includeInactive: true) == null)
			{
				return null;
			}
			array = componentsInChildren;
			foreach (Renderer renderer2 in array)
			{
				if (renderer2 != null && renderer2.name.StartsWith("WeaponSkin", StringComparison.Ordinal))
				{
					return null;
				}
			}
			return which + ": SKIN YOK - hic kurulmamis ya da yok edilmis " + $"(cizilen {num} renderer, gizli {list.Count})";
		}

		protected override void __initializeVariables()
		{
			if (equippedId == null)
			{
				throw new Exception("PlayerWeapons.equippedId cannot be null. All NetworkVariableBase instances must be initialized.");
			}
			equippedId.Initialize(this);
			__nameNetworkVariable(equippedId, "equippedId");
			NetworkVariableFields.Add(equippedId);
			base.__initializeVariables();
		}

		protected override void __initializeRpcs()
		{
			__registerRpc(1929014657u, __rpc_handler_1929014657, "RequestRangeStopServerRpc", RpcInvokePermission.Everyone);
			__registerRpc(974522609u, __rpc_handler_974522609, "MoveRangeTargetClientRpc", RpcInvokePermission.Everyone);
			__registerRpc(1582666596u, __rpc_handler_1582666596, "RangeHitClientRpc", RpcInvokePermission.Everyone);
			__registerRpc(2069929923u, __rpc_handler_2069929923, "RequestPickupServerRpc", RpcInvokePermission.Everyone);
			base.__initializeRpcs();
		}

		private static void __rpc_handler_1929014657(NetworkBehaviour target, FastBufferReader reader, __RpcParams rpcParams)
		{
			NetworkManager networkManager = target.NetworkManager;
			if ((object)networkManager != null && networkManager.IsListening)
			{
				reader.ReadValueSafe(out Vector3 value);
				RpcParams ext = rpcParams.Ext;
				target.__rpc_exec_stage = __RpcExecStage.Execute;
				((PlayerWeapons)target).RequestRangeStopServerRpc(value, ext);
				target.__rpc_exec_stage = __RpcExecStage.Send;
			}
		}

		private static void __rpc_handler_974522609(NetworkBehaviour target, FastBufferReader reader, __RpcParams rpcParams)
		{
			NetworkManager networkManager = target.NetworkManager;
			if ((object)networkManager != null && networkManager.IsListening)
			{
				reader.ReadValueSafe(out Vector3 value);
				ByteUnpacker.ReadValueBitPacked(reader, out int value2);
				target.__rpc_exec_stage = __RpcExecStage.Execute;
				((PlayerWeapons)target).MoveRangeTargetClientRpc(value, value2);
				target.__rpc_exec_stage = __RpcExecStage.Send;
			}
		}

		private static void __rpc_handler_1582666596(NetworkBehaviour target, FastBufferReader reader, __RpcParams rpcParams)
		{
			NetworkManager networkManager = target.NetworkManager;
			if ((object)networkManager != null && networkManager.IsListening)
			{
				ByteUnpacker.ReadValueBitPacked(reader, out int value);
				RpcParams ext = rpcParams.Ext;
				target.__rpc_exec_stage = __RpcExecStage.Execute;
				((PlayerWeapons)target).RangeHitClientRpc(value, ext);
				target.__rpc_exec_stage = __RpcExecStage.Send;
			}
		}

		private static void __rpc_handler_2069929923(NetworkBehaviour target, FastBufferReader reader, __RpcParams rpcParams)
		{
			NetworkManager networkManager = target.NetworkManager;
			if ((object)networkManager != null && networkManager.IsListening)
			{
				reader.ReadValueSafe(out Vector3 value);
				RpcParams ext = rpcParams.Ext;
				target.__rpc_exec_stage = __RpcExecStage.Execute;
				((PlayerWeapons)target).RequestPickupServerRpc(value, ext);
				target.__rpc_exec_stage = __RpcExecStage.Send;
			}
		}

		protected internal override string __getTypeName()
		{
			return "PlayerWeapons";
		}
	}
}
