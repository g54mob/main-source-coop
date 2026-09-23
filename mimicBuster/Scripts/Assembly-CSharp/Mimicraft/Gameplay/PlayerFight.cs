using System;
using System.Collections.Generic;
using Mimicraft.Customization;
using Mimicraft.Networking;
using Mimicraft.Settings;
using Mimicraft.UI;
using Unity.Netcode;
using UnityEngine;

namespace Mimicraft.Gameplay
{
	[RequireComponent(typeof(NetworkObject))]
	public class PlayerFight : NetworkBehaviour
	{
		private struct PendingPunch
		{
			public double ContactAt;

			public Vector3 Origin;

			public Vector3 Forward;
		}

		[Header("Duruş")]
		[Tooltip("Son yumruktan sonra bu kadar saniye vurmazsa normal duruşa döner.")]
		[SerializeField]
		private float stanceTimeoutSeconds = 3f;

		[Tooltip("İki yumruk arasındaki en kısa süre. Animasyonun uzunluğuna yakın tutulmalı, yoksa yumruklar birbirinin üstüne biner.")]
		[SerializeField]
		private float punchCooldownSeconds = 0.55f;

		[Tooltip("Kaç farklı yumruk animasyonu var. Her yumrukta rastgele biri seçilir ve numarası Animator'e PunchIndex olarak yazılır.")]
		[SerializeField]
		[Min(1f)]
		private int punchVariants = 4;

		[Header("Vuruş")]
		[Tooltip("Yumruğun temas ettiği an, tuşa basıldıktan ne kadar sonra. Animasyondaki temas karesine denk getirilmeli - sunucu isabet testini o anda yapar.")]
		[SerializeField]
		private float contactDelaySeconds = 0.25f;

		[Tooltip("Yumruğun erişimi, metre.")]
		[SerializeField]
		private float reach = 1.8f;

		[Tooltip("Hedefin ne kadar önde olması gerektiği. 1 = tam karşıda, 0 = yan taraf da sayılır.")]
		[SerializeField]
		[Range(0f, 1f)]
		private float facingTolerance = 0.35f;

		[Tooltip("Blok yaparken korunan ön yayın genişliği. Yumruğu atan bu koninin içindeyse blok sayılır; arkadan ve yandan gelen yumruk bloklanmaz. 0 = tam yarım daire, 1 = sadece tam karşıdan.")]
		[SerializeField]
		[Range(0f, 1f)]
		private float blockArc = 0.35f;

		[Tooltip("Bloklanan bir yumruğun kaça bölüneceği. 2 = bloklarken devrilmen için iki katı yumruk gerekir (varsayılan üç yerine altı).")]
		[SerializeField]
		[Min(1f)]
		private float blockDivisor = 2f;

		[Tooltip("Toz efektinin ayak hizasından ne kadar yukarıda çıkacağı. Kapsüllerin orijini ayakta olduğu için göğüs hizası elle veriliyor.")]
		[SerializeField]
		private float contactHeight = 1.2f;

		[Tooltip("Yumruğun ÇARPABİLECEĞİ katmanlar - kimseye isabet etmeyen bir yumruk bunlarda duvar arar. Hangi yüzeyin nasıl ses çıkaracağı buradan değil, sahnedeki EffectLibrary'nin Yumruk Yüzeyleri listesinden geliyor (bkz. PunchSurface); burası sadece 'neye vurulabilir' sorusunu yanıtlıyor.")]
		[SerializeField]
		private LayerMask punchableLayers = -1;

		[Tooltip("Düşüren yumruğun bedeni ittiği hız, metre/saniye. Yükseldikçe daha çok savrulur; PlayerRagdoll bunu ayaklarda zayıflatıp göğüste tam uygulayarak devrilmeye çevirir.")]
		[SerializeField]
		[Min(0f)]
		private float knockdownImpulse = 3.5f;

		[Tooltip("İtmenin ne kadarının yukarı doğru olduğu. Biraz yukarı bileşen olmadan beden yerde sürünüyor gibi görünüyor; çok fazlası havaya fırlatıyor.")]
		[SerializeField]
		[Range(0f, 1f)]
		private float knockdownLift = 0.25f;

		[Tooltip("Yerde yatan bir bedene (devrilmiş ya da ölü) atılan yumruğun ittiği hız (m/sn). Beden yeniden devrilmez - zaten yerde - ama yumruk yönünde savrulur. 0 = yerdekiler yumruktan etkilenmez.")]
		[SerializeField]
		[Min(0f)]
		private float downedPunchImpulse = 2.5f;

		[Header("Ses")]
		[Tooltip("Bu oyuncunun gövdesine bağlı 3D kaynak - efor ve acı sesleri buradan çıkar, çünkü ikisi de bir BEDENE ait ve onunla birlikte hareket eder. Darbe sesi buradan çıkmaz; o iki kişinin arasındaki noktaya aittir ve ImpactEffects oraya koyar.\n\nPlayerMovement'ın ayak sesi kaynağı kullanılabilir - aynı gövde, aynı mesafe eğrisi.")]
		[SerializeField]
		private AudioSource bodySource;

		[Header("Dayanıklılık")]
		[Tooltip("Kaç yumruk yiyince düşülür. Her yumruk 1 götürür.")]
		[SerializeField]
		[Min(1f)]
		private float staminaMax = 3f;

		[Tooltip("Yumruk yemediğinde saniyede ne kadar dolar.")]
		[SerializeField]
		[Min(0f)]
		private float staminaRegenPerSecond = 0.5f;

		[Tooltip("Son yumruktan sonra dolmaya başlaması için geçmesi gereken süre.")]
		[SerializeField]
		[Min(0f)]
		private float staminaRegenDelaySeconds = 2f;

		private readonly NetworkVariable<bool> isFighting = new NetworkVariable<bool>(value: false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

		private readonly NetworkVariable<float> stamina = new NetworkVariable<float>(0f);

		private readonly NetworkVariable<bool> isBlocking = new NetworkVariable<bool>(value: false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

		private PlayerMovement movement;

		private PlayerRagdoll ragdoll;

		private PlayerAnimator playerAnimator;

		private PlayerCameraRig cameraRig;

		private float nextPunchAllowedAt;

		private float lastFightActionAt = float.NegativeInfinity;

		private float guardingSince = float.NegativeInfinity;

		private bool warnedAboutStuckGuard;

		private double serverNextPunchAllowedAt;

		private double staminaRegenResumesAt;

		private readonly List<PendingPunch> pendingPunches = new List<PendingPunch>();

		private readonly List<PendingPunch> stillPending = new List<PendingPunch>();

		private bool warnedAboutRefusal;

		private const float StuckGuardSeconds = 15f;

		private const float FistRadius = 0.12f;

		private static readonly RaycastHit[] surfaceHits = new RaycastHit[8];

		private PlayerCharacterAppearance appearance;

		public bool IsFighting => isFighting.Value;

		public bool IsBlocking => isBlocking.Value;

		public bool IsSquaredUp
		{
			get
			{
				if (!isFighting.Value)
				{
					return isBlocking.Value;
				}
				return true;
			}
		}

		public float Stamina => stamina.Value;

		public float StaminaMax => staminaMax;

		private bool CanFight => WhyCannotFight() == null;

		public string FightRefusalReason => WhyCannotFight();

		private CharacterVoice VoiceType
		{
			get
			{
				if (appearance == null)
				{
					appearance = GetComponentInParent<PlayerCharacterAppearance>();
				}
				if (!(appearance != null))
				{
					return CharacterVoice.Male;
				}
				return appearance.VoiceType;
			}
		}

		public override void OnNetworkSpawn()
		{
			movement = GetComponent<PlayerMovement>();
			ragdoll = GetComponent<PlayerRagdoll>();
			playerAnimator = GetComponentInChildren<PlayerAnimator>(includeInactive: true);
			cameraRig = GetComponent<PlayerCameraRig>();
			if (base.IsServer)
			{
				stamina.Value = staminaMax;
			}
			if (bodySource == null)
			{
				bodySource = base.gameObject.AddComponent<AudioSource>();
				bodySource.playOnAwake = false;
			}
			SpatialAudio.Apply(bodySource);
			NetworkVariable<bool> networkVariable = isFighting;
			networkVariable.OnValueChanged = (NetworkVariable<bool>.OnValueChangedDelegate)Delegate.Combine(networkVariable.OnValueChanged, new NetworkVariable<bool>.OnValueChangedDelegate(OnFightingChanged));
			NetworkVariable<bool> networkVariable2 = isBlocking;
			networkVariable2.OnValueChanged = (NetworkVariable<bool>.OnValueChangedDelegate)Delegate.Combine(networkVariable2.OnValueChanged, new NetworkVariable<bool>.OnValueChangedDelegate(OnFightingChanged));
			ApplyStance();
		}

		public override void OnNetworkDespawn()
		{
			NetworkVariable<bool> networkVariable = isFighting;
			networkVariable.OnValueChanged = (NetworkVariable<bool>.OnValueChangedDelegate)Delegate.Remove(networkVariable.OnValueChanged, new NetworkVariable<bool>.OnValueChangedDelegate(OnFightingChanged));
			NetworkVariable<bool> networkVariable2 = isBlocking;
			networkVariable2.OnValueChanged = (NetworkVariable<bool>.OnValueChangedDelegate)Delegate.Remove(networkVariable2.OnValueChanged, new NetworkVariable<bool>.OnValueChangedDelegate(OnFightingChanged));
		}

		private void OnFightingChanged(bool previous, bool current)
		{
			ApplyStance();
		}

		private void ApplyStance()
		{
			if (!(playerAnimator == null))
			{
				playerAnimator.SetFighting(isFighting.Value || isBlocking.Value);
				playerAnimator.SetBlocking(isBlocking.Value);
			}
		}

		private Vector3 PunchAim()
		{
			Transform transform = ((cameraRig != null) ? cameraRig.AimCameraTransform : null);
			if (!(transform != null))
			{
				return base.transform.forward;
			}
			return transform.forward;
		}

		private string WhyCannotFight()
		{
			if (movement == null)
			{
				return "PlayerMovement yok";
			}
			if (movement.IsArmed)
			{
				return "silahli - dovus sadece silahsizken";
			}
			if (!movement.InputEnabled)
			{
				return "girdi kapali (duzenleme modunda olabilir)";
			}
			if (movement.SpectatorFrozen)
			{
				return "izleyici modunda donmus";
			}
			if (movement.RagdollFrozen)
			{
				return "zaten yerde";
			}
			if (ragdoll != null && !ragdoll.HasHumanoid)
			{
				return "gorunur bir insan modeli yok (voxel govde giyiliyor olabilir)";
			}
			return null;
		}

		private void Update()
		{
			if (base.IsServer)
			{
				TickServer();
			}
			if (base.IsOwner)
			{
				TickOwner();
			}
		}

		private void TickOwner()
		{
			if (!CanFight)
			{
				SetFighting(fighting: false);
				SetBlocking(blocking: false);
				ReportRefusedPunch();
				return;
			}
			bool flag = GameInput.Block.IsPressed() && !GameMenuState.LookCaptured;
			SetBlocking(flag);
			if (flag)
			{
				lastFightActionAt = Time.time;
			}
			SetFighting(Time.time < lastFightActionAt + stanceTimeoutSeconds);
			WarnAboutStuckGuard(flag);
			if (!flag && GameInput.Fire.WasPressedThisFrame() && !GameMenuState.LookCaptured && !(Time.time < nextPunchAllowedAt))
			{
				Punch();
			}
		}

		private void ReportRefusedPunch()
		{
			if (!warnedAboutRefusal && GameInput.Fire.WasPressedThisFrame())
			{
				warnedAboutRefusal = true;
				Debug.LogWarning("[PlayerFight] Yumruk atilamadi: " + WhyCannotFight() + ".", this);
			}
		}

		private void Punch()
		{
			nextPunchAllowedAt = Time.time + punchCooldownSeconds;
			lastFightActionAt = Time.time;
			SetFighting(fighting: true);
			int variantIndex = UnityEngine.Random.Range(0, Mathf.Max(1, punchVariants));
			if (playerAnimator != null)
			{
				playerAnimator.TriggerPunch(variantIndex);
			}
			PlayerEffortVoice.Play(this, AudioLibrary.NextPunchEffortClip(VoiceType));
			PunchServerRpc(base.transform.position, PunchAim());
		}

		private void SetFighting(bool fighting)
		{
			if (base.IsOwner && base.IsSpawned && isFighting.Value != fighting)
			{
				isFighting.Value = fighting;
			}
		}

		private void WarnAboutStuckGuard(bool guarding)
		{
			if (!guarding)
			{
				guardingSince = float.NegativeInfinity;
				warnedAboutStuckGuard = false;
				return;
			}
			if (float.IsNegativeInfinity(guardingSince))
			{
				guardingSince = Time.time;
			}
			if (!warnedAboutStuckGuard && !(Time.time - guardingSince < 15f))
			{
				warnedAboutStuckGuard = true;
				Debug.LogWarning($"[PlayerFight] Blok {15f:0} saniyedir basili gorunuyor " + "(" + GameInput.DisplayFor("Human/Block") + "). Durus bu yuzden inmiyor - tusa basili kalan bir sey var demektir.", this);
			}
		}

		private void SetBlocking(bool blocking)
		{
			if (base.IsOwner && base.IsSpawned && isBlocking.Value != blocking)
			{
				isBlocking.Value = blocking;
			}
		}

		[Rpc(SendTo.Server, InvokePermission = RpcInvokePermission.Everyone)]
		private void PunchServerRpc(Vector3 origin, Vector3 forward, RpcParams rpcParams = default(RpcParams))
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
				FastBufferWriter bufferWriter = __beginSendRpc(957879652u, rpcParams2, attributeParams, SendTo.Server, RpcDelivery.Reliable);
				bufferWriter.WriteValueSafe(in origin);
				bufferWriter.WriteValueSafe(in forward);
				__endSendRpc(ref bufferWriter, 957879652u, rpcParams, attributeParams, SendTo.Server, RpcDelivery.Reliable);
			}
			if (__rpc_exec_stage == __RpcExecStage.Execute)
			{
				__rpc_exec_stage = __RpcExecStage.Send;
				if (rpcParams.Receive.SenderClientId == base.OwnerClientId && CanFight && !isBlocking.Value && !(Time.timeAsDouble < serverNextPunchAllowedAt))
				{
					serverNextPunchAllowedAt = Time.timeAsDouble + (double)punchCooldownSeconds;
					PunchEffortClientRpc();
					Vector3 origin2 = ((Vector3.Distance(origin, base.transform.position) > reach) ? base.transform.position : origin);
					pendingPunches.Add(new PendingPunch
					{
						ContactAt = Time.timeAsDouble + (double)contactDelaySeconds,
						Origin = origin2,
						Forward = ((forward.sqrMagnitude > 0.001f) ? forward.normalized : base.transform.forward)
					});
				}
			}
		}

		private void TickServer()
		{
			RegenerateStamina();
			ResolvePendingPunches();
		}

		private void RegenerateStamina()
		{
			if (!(stamina.Value >= staminaMax) && !(Time.timeAsDouble < staminaRegenResumesAt) && (!(ragdoll != null) || ragdoll.State == RagdollState.None))
			{
				stamina.Value = Mathf.Min(staminaMax, stamina.Value + staminaRegenPerSecond * Time.deltaTime);
			}
		}

		private void ResolvePendingPunches()
		{
			if (pendingPunches.Count == 0)
			{
				return;
			}
			stillPending.Clear();
			foreach (PendingPunch pendingPunch in pendingPunches)
			{
				if (Time.timeAsDouble < pendingPunch.ContactAt)
				{
					stillPending.Add(pendingPunch);
				}
				else
				{
					LandPunch(pendingPunch);
				}
			}
			pendingPunches.Clear();
			pendingPunches.AddRange(stillPending);
		}

		private Vector3 FlatAim(Vector3 aim)
		{
			Vector3 vector = new Vector3(aim.x, 0f, aim.z);
			if (!(vector.sqrMagnitude > 0.0001f))
			{
				return base.transform.forward;
			}
			return vector.normalized;
		}

		private void LandPunch(PendingPunch punch)
		{
			PlayerFight playerFight = FindVictim(punch);
			if (playerFight == null)
			{
				LandPunchOnSurface(punch);
				return;
			}
			int num;
			Vector3 vector;
			if (playerFight.ragdoll != null)
			{
				num = (playerFight.ragdoll.IsLimp ? 1 : 0);
				if (num != 0)
				{
					vector = playerFight.ragdoll.BodyPosition;
					goto IL_0050;
				}
			}
			else
			{
				num = 0;
			}
			vector = playerFight.transform.position;
			goto IL_0050;
			IL_0050:
			Vector3 vector2 = vector;
			Vector3 contact = ((num != 0) ? Vector3.Lerp(punch.Origin + Vector3.up * contactHeight, vector2, 0.6f) : (Vector3.Lerp(punch.Origin, vector2, 0.6f) + Vector3.up * contactHeight));
			if (num != 0)
			{
				if (downedPunchImpulse > 0f)
				{
					Vector3 velocityChange = FlatAim(punch.Forward) * downedPunchImpulse + Vector3.up * (downedPunchImpulse * knockdownLift);
					playerFight.ragdoll.ServerNudgeNearest(vector2, velocityChange);
				}
			}
			else
			{
				playerFight.ServerTakePunch(FlatAim(punch.Forward), punch.Origin);
			}
			GameModeController current = GameModeController.Current;
			int num2 = ((current != null) ? current.PunchDamage : 0);
			if (num2 > 0 && current.ServerMayDamage(base.OwnerClientId, playerFight.OwnerClientId))
			{
				PlayerHealth component = playerFight.GetComponent<PlayerHealth>();
				if (component != null)
				{
					bool num3 = component.ServerTakeDamage(num2);
					if (current.ShowsDamageDirection)
					{
						component.ServerReportDamageFrom(base.transform.position);
					}
					if (num3)
					{
						current.ServerReportKill(base.OwnerClientId, playerFight.OwnerClientId);
					}
				}
			}
			PunchLandedClientRpc(contact, -punch.Forward);
		}

		[ClientRpc]
		private void PunchLandedClientRpc(Vector3 contact, Vector3 normal)
		{
			NetworkManager networkManager = base.NetworkManager;
			if ((object)networkManager == null || !networkManager.IsListening)
			{
				Debug.LogError("Rpc methods can only be invoked after starting the NetworkManager!");
				return;
			}
			if (__rpc_exec_stage != __RpcExecStage.Execute && (networkManager.IsServer || networkManager.IsHost))
			{
				ClientRpcParams clientRpcParams = default(ClientRpcParams);
				FastBufferWriter bufferWriter = __beginSendClientRpc(3888965670u, clientRpcParams, RpcDelivery.Reliable);
				bufferWriter.WriteValueSafe(in contact);
				bufferWriter.WriteValueSafe(in normal);
				__endSendClientRpc(ref bufferWriter, 3888965670u, clientRpcParams, RpcDelivery.Reliable);
			}
			if (__rpc_exec_stage == __RpcExecStage.Execute && (networkManager.IsClient || networkManager.IsHost))
			{
				__rpc_exec_stage = __RpcExecStage.Send;
				ImpactEffects.SpawnPunchEffect(contact, normal);
			}
		}

		private void LandPunchOnSurface(PendingPunch punch)
		{
			int num = Physics.SphereCastNonAlloc(punch.Origin + Vector3.up * contactHeight, 0.12f, punch.Forward, surfaceHits, reach, punchableLayers, QueryTriggerInteraction.Ignore);
			float num2 = float.MaxValue;
			RaycastHit raycastHit = default(RaycastHit);
			bool flag = false;
			for (int i = 0; i < num; i++)
			{
				Transform transform = surfaceHits[i].transform;
				if (!(transform == null) && !(transform == base.transform) && !transform.IsChildOf(base.transform) && !(surfaceHits[i].distance <= 0f) && !(surfaceHits[i].normal.sqrMagnitude < 0.5f) && !(surfaceHits[i].distance >= num2))
				{
					num2 = surfaceHits[i].distance;
					raycastHit = surfaceHits[i];
					flag = true;
				}
			}
			if (flag)
			{
				PunchHitSurfaceClientRpc(raycastHit.point, raycastHit.normal, (byte)raycastHit.collider.gameObject.layer);
			}
		}

		[ClientRpc]
		private void PunchHitSurfaceClientRpc(Vector3 contact, Vector3 normal, byte layer)
		{
			NetworkManager networkManager = base.NetworkManager;
			if ((object)networkManager == null || !networkManager.IsListening)
			{
				Debug.LogError("Rpc methods can only be invoked after starting the NetworkManager!");
				return;
			}
			if (__rpc_exec_stage != __RpcExecStage.Execute && (networkManager.IsServer || networkManager.IsHost))
			{
				ClientRpcParams clientRpcParams = default(ClientRpcParams);
				FastBufferWriter bufferWriter = __beginSendClientRpc(4016100613u, clientRpcParams, RpcDelivery.Reliable);
				bufferWriter.WriteValueSafe(in contact);
				bufferWriter.WriteValueSafe(in normal);
				bufferWriter.WriteValueSafe(in layer, default(FastBufferWriter.ForPrimitives));
				__endSendClientRpc(ref bufferWriter, 4016100613u, clientRpcParams, RpcDelivery.Reliable);
			}
			if (__rpc_exec_stage == __RpcExecStage.Execute && (networkManager.IsClient || networkManager.IsHost))
			{
				__rpc_exec_stage = __RpcExecStage.Send;
				ImpactEffects.SpawnPunchWallEffect(contact, normal, layer);
			}
		}

		[ClientRpc]
		private void PunchEffortClientRpc()
		{
			NetworkManager networkManager = base.NetworkManager;
			if ((object)networkManager == null || !networkManager.IsListening)
			{
				Debug.LogError("Rpc methods can only be invoked after starting the NetworkManager!");
				return;
			}
			if (__rpc_exec_stage != __RpcExecStage.Execute && (networkManager.IsServer || networkManager.IsHost))
			{
				ClientRpcParams clientRpcParams = default(ClientRpcParams);
				FastBufferWriter bufferWriter = __beginSendClientRpc(4000096561u, clientRpcParams, RpcDelivery.Reliable);
				__endSendClientRpc(ref bufferWriter, 4000096561u, clientRpcParams, RpcDelivery.Reliable);
			}
			if (__rpc_exec_stage == __RpcExecStage.Execute && (networkManager.IsClient || networkManager.IsHost))
			{
				__rpc_exec_stage = __RpcExecStage.Send;
				if (!base.IsOwner)
				{
					PlayerEffortVoice.Play(this, AudioLibrary.NextPunchEffortClip(VoiceType));
				}
			}
		}

		private void PlayFromBody(AudioClip clip)
		{
			if (bodySource != null && clip != null)
			{
				bodySource.PlayOneShot(clip, AudioLibrary.VolumeOf(clip));
			}
		}

		private PlayerFight FindVictim(PendingPunch punch)
		{
			PlayerFight result = null;
			float num = float.MaxValue;
			foreach (NetworkClient connectedClients in base.NetworkManager.ConnectedClientsList)
			{
				if (connectedClients.PlayerObject == null || connectedClients.ClientId == base.OwnerClientId)
				{
					continue;
				}
				PlayerFight component = connectedClients.PlayerObject.GetComponent<PlayerFight>();
				if (!(component == null) && !(component == this) && (!(component.ragdoll != null) || component.ragdoll.State == RagdollState.None || component.ragdoll.IsLimp) && (!(component.ragdoll != null) || component.ragdoll.HasHumanoid))
				{
					Vector3 vector = ((component.ragdoll != null) ? component.ragdoll.BodyPosition : component.transform.position) - punch.Origin;
					Vector3 vector2 = new Vector3(vector.x, 0f, vector.z);
					float magnitude = vector.magnitude;
					if (!(magnitude > reach) && !(vector2.sqrMagnitude < 0.0001f) && !(Vector3.Dot(vector2.normalized, FlatAim(punch.Forward)) < facingTolerance) && !(magnitude >= num))
					{
						num = magnitude;
						result = component;
					}
				}
			}
			return result;
		}

		private void ServerTakePunch(Vector3 fromDirection, Vector3 fromPosition)
		{
			if (!base.IsServer)
			{
				return;
			}
			staminaRegenResumesAt = Time.timeAsDouble + (double)staminaRegenDelaySeconds;
			stamina.Value = Mathf.Max(0f, stamina.Value - PunchCost(fromPosition));
			if (stamina.Value > 0f)
			{
				HitClientRpc();
				return;
			}
			stamina.Value = staminaMax;
			if (!(ragdoll == null))
			{
				Vector3 impulse = (fromDirection + Vector3.up * knockdownLift).normalized * knockdownImpulse;
				ragdoll.ServerBeginKnockdown(impulse);
			}
		}

		private float PunchCost(Vector3 fromPosition)
		{
			if (!isBlocking.Value)
			{
				return 1f;
			}
			Vector3 vector = fromPosition - base.transform.position;
			vector.y = 0f;
			if (vector.sqrMagnitude > 0.0001f && Vector3.Dot(base.transform.forward, vector.normalized) < blockArc)
			{
				return 1f;
			}
			return 1f / Mathf.Max(1f, blockDivisor);
		}

		[ClientRpc]
		private void HitClientRpc()
		{
			NetworkManager networkManager = base.NetworkManager;
			if ((object)networkManager == null || !networkManager.IsListening)
			{
				Debug.LogError("Rpc methods can only be invoked after starting the NetworkManager!");
				return;
			}
			if (__rpc_exec_stage != __RpcExecStage.Execute && (networkManager.IsServer || networkManager.IsHost))
			{
				ClientRpcParams clientRpcParams = default(ClientRpcParams);
				FastBufferWriter bufferWriter = __beginSendClientRpc(3022422523u, clientRpcParams, RpcDelivery.Reliable);
				__endSendClientRpc(ref bufferWriter, 3022422523u, clientRpcParams, RpcDelivery.Reliable);
			}
			if (__rpc_exec_stage == __RpcExecStage.Execute && (networkManager.IsClient || networkManager.IsHost))
			{
				__rpc_exec_stage = __RpcExecStage.Send;
				if (playerAnimator != null)
				{
					playerAnimator.TriggerHit();
				}
				PlayerEffortVoice.Play(this, AudioLibrary.NextPunchHurtClip(VoiceType));
			}
		}

		protected override void __initializeVariables()
		{
			if (isFighting == null)
			{
				throw new Exception("PlayerFight.isFighting cannot be null. All NetworkVariableBase instances must be initialized.");
			}
			isFighting.Initialize(this);
			__nameNetworkVariable(isFighting, "isFighting");
			NetworkVariableFields.Add(isFighting);
			if (stamina == null)
			{
				throw new Exception("PlayerFight.stamina cannot be null. All NetworkVariableBase instances must be initialized.");
			}
			stamina.Initialize(this);
			__nameNetworkVariable(stamina, "stamina");
			NetworkVariableFields.Add(stamina);
			if (isBlocking == null)
			{
				throw new Exception("PlayerFight.isBlocking cannot be null. All NetworkVariableBase instances must be initialized.");
			}
			isBlocking.Initialize(this);
			__nameNetworkVariable(isBlocking, "isBlocking");
			NetworkVariableFields.Add(isBlocking);
			base.__initializeVariables();
		}

		protected override void __initializeRpcs()
		{
			__registerRpc(957879652u, __rpc_handler_957879652, "PunchServerRpc", RpcInvokePermission.Everyone);
			__registerRpc(3888965670u, __rpc_handler_3888965670, "PunchLandedClientRpc", RpcInvokePermission.Server);
			__registerRpc(4016100613u, __rpc_handler_4016100613, "PunchHitSurfaceClientRpc", RpcInvokePermission.Server);
			__registerRpc(4000096561u, __rpc_handler_4000096561, "PunchEffortClientRpc", RpcInvokePermission.Server);
			__registerRpc(3022422523u, __rpc_handler_3022422523, "HitClientRpc", RpcInvokePermission.Server);
			base.__initializeRpcs();
		}

		private static void __rpc_handler_957879652(NetworkBehaviour target, FastBufferReader reader, __RpcParams rpcParams)
		{
			NetworkManager networkManager = target.NetworkManager;
			if ((object)networkManager != null && networkManager.IsListening)
			{
				reader.ReadValueSafe(out Vector3 value);
				reader.ReadValueSafe(out Vector3 value2);
				RpcParams ext = rpcParams.Ext;
				target.__rpc_exec_stage = __RpcExecStage.Execute;
				((PlayerFight)target).PunchServerRpc(value, value2, ext);
				target.__rpc_exec_stage = __RpcExecStage.Send;
			}
		}

		private static void __rpc_handler_3888965670(NetworkBehaviour target, FastBufferReader reader, __RpcParams rpcParams)
		{
			NetworkManager networkManager = target.NetworkManager;
			if ((object)networkManager != null && networkManager.IsListening)
			{
				reader.ReadValueSafe(out Vector3 value);
				reader.ReadValueSafe(out Vector3 value2);
				target.__rpc_exec_stage = __RpcExecStage.Execute;
				((PlayerFight)target).PunchLandedClientRpc(value, value2);
				target.__rpc_exec_stage = __RpcExecStage.Send;
			}
		}

		private static void __rpc_handler_4016100613(NetworkBehaviour target, FastBufferReader reader, __RpcParams rpcParams)
		{
			NetworkManager networkManager = target.NetworkManager;
			if ((object)networkManager != null && networkManager.IsListening)
			{
				reader.ReadValueSafe(out Vector3 value);
				reader.ReadValueSafe(out Vector3 value2);
				reader.ReadValueSafe(out byte value3, default(FastBufferWriter.ForPrimitives));
				target.__rpc_exec_stage = __RpcExecStage.Execute;
				((PlayerFight)target).PunchHitSurfaceClientRpc(value, value2, value3);
				target.__rpc_exec_stage = __RpcExecStage.Send;
			}
		}

		private static void __rpc_handler_4000096561(NetworkBehaviour target, FastBufferReader reader, __RpcParams rpcParams)
		{
			NetworkManager networkManager = target.NetworkManager;
			if ((object)networkManager != null && networkManager.IsListening)
			{
				target.__rpc_exec_stage = __RpcExecStage.Execute;
				((PlayerFight)target).PunchEffortClientRpc();
				target.__rpc_exec_stage = __RpcExecStage.Send;
			}
		}

		private static void __rpc_handler_3022422523(NetworkBehaviour target, FastBufferReader reader, __RpcParams rpcParams)
		{
			NetworkManager networkManager = target.NetworkManager;
			if ((object)networkManager != null && networkManager.IsListening)
			{
				target.__rpc_exec_stage = __RpcExecStage.Execute;
				((PlayerFight)target).HitClientRpc();
				target.__rpc_exec_stage = __RpcExecStage.Send;
			}
		}

		protected internal override string __getTypeName()
		{
			return "PlayerFight";
		}
	}
}
