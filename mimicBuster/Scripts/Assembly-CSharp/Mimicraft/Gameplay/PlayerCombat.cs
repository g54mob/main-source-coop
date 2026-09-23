using System;
using System.Collections.Generic;
using System.Text;
using Mimicraft.Cameras;
using Mimicraft.Customization;
using Mimicraft.Dev;
using Mimicraft.Networking;
using Mimicraft.Settings;
using Mimicraft.UI;
using Unity.Netcode;
using UnityEngine;

namespace Mimicraft.Gameplay
{
	public class PlayerCombat : NetworkBehaviour
	{
		private enum PelletResult
		{
			Miss = 0,
			StruckNonTarget = 1,
			StruckHider = 2
		}

		private const float Range = 100f;

		private const float EyeHeight = 1.6f;

		private const int MissSelfDamage = 10;

		private const float MinEyeHeight = 0.4f;

		[Header("Silah")]
		[Tooltip("Tek atışta çıkan saçma sayısı. 1 = klasik tek çizgi hitscan. Arttırırken Hasar (Saçma Başına) değerini düşür, yoksa toplam hasar aynı oranda katlanır.")]
		[SerializeField]
		[Min(1f)]
		private int projectilesPerShot = 1;

		[Tooltip("Saçmaların nişan çizgisinden sapabileceği en büyük açı (derece). 0 = sapma yok, her saçma tam nişan aldığın yere gider. Küçük hareketli hedefleri vurulabilir yapan asıl ayar bu - saçma sayısını tek başına arttırmak isabeti kolaylaştırmaz.")]
		[SerializeField]
		[Min(0f)]
		private float spreadDegrees;

		[Tooltip("Bir saçmanın Modelci'ye verdiği hasar. Modelci canı 100. Saçma sayısıyla birlikte düşün: 8 saçma x 34 hasar anında öldürür.")]
		[SerializeField]
		[Min(0f)]
		private int damagePerProjectile = 34;

		[Tooltip("Merminin çarpabileceği katmanlar. Buradan çıkardığın bir katmanın içinden mermi geçip gider - durdurmaz, hasar vermez, duvar efekti de çıkarmaz.\n\nOyuncuların ve modellerin katmanlarını (PlayerController, VoxelBody) SAKIN çıkarma: vurulacak şey onlar, çıkarınca hiçbir atış isabet etmez.\n\nÇıkarmaya aday olanlar: FPSVisual (kendi elindeki silah modeli - ışın gözünün hizasından çıktığı için tam onun içinden başlıyor), MapBlocker ve tetikleyici hacimler.")]
		[SerializeField]
		private LayerMask hitLayers = -1;

		private const double FireCooldownSeconds = 0.5;

		private const float ShootTrauma = 0.15f;

		private const float RecoilCooldownSeconds = 0.55f;

		private const int RecoilSuppressionShots = 4;

		private const float RecoilSuppressionFactor = 0.35f;

		private const float MaxYawPunch = 20f;

		private int recoilShot;

		private float lastShotTime;

		private PlayerScope scope;

		private PlayerWeaponSkins weaponSkins;

		private const float SpeedSmoothing = 10f;

		private float trackedSpeed;

		private Vector3 lastSpeedPosition;

		private bool hasSpeedSample;

		private PlayerWeapons weapons;

		private bool warnedAboutDroppedShot;

		[SerializeField]
		private PlayerHealth health;

		[SerializeField]
		private FirstPersonLook look;

		private PlayerMovement movement;

		[SerializeField]
		private WeaponSway weaponSway;

		private bool triggerLocked;

		private RoundPhase lastSeenPhase;

		private bool sawPhase;

		private RoundManager roundManager;

		private double nextAllowedFireServerTime;

		private double nextAllowedFireLocalTime;

		private const double FireCooldownTolerance = 0.15;

		private const int MaxImpactsPerShot = 6;

		private readonly List<Vector3> impactPoints = new List<Vector3>();

		private readonly List<Vector3> impactNormals = new List<Vector3>();

		private readonly List<Vector3> wallPoints = new List<Vector3>();

		private readonly List<Vector3> voidPoints = new List<Vector3>();

		private readonly List<Vector3> wallNormals = new List<Vector3>();

		private PlayerCameraRig aimRig;

		private GameModeController cachedMode;

		private readonly RaycastHit[] aimHits = new RaycastHit[16];

		private const float MaxAimCorrectionDegrees = 50f;

		public static bool DebugShots;

		private WeaponDefinition ActiveWeapon
		{
			get
			{
				if (weapons == null)
				{
					weapons = GetComponent<PlayerWeapons>();
				}
				if (!(weapons != null))
				{
					return null;
				}
				return weapons.Equipped;
			}
		}

		private WeaponSoundKind ActiveWeaponSound
		{
			get
			{
				WeaponDefinition activeWeapon = ActiveWeapon;
				if (activeWeapon == null)
				{
					return WeaponSoundKind.Normal;
				}
				if (weaponSkins == null)
				{
					weaponSkins = GetComponentInChildren<PlayerWeaponSkins>(includeInactive: true);
				}
				if (!(weaponSkins != null))
				{
					return WeaponSoundKind.Normal;
				}
				return weaponSkins.SoundFor(activeWeapon.WeaponId);
			}
		}

		private double CurrentFireCooldown
		{
			get
			{
				if (!(ActiveWeapon != null))
				{
					return 0.5;
				}
				return ActiveWeapon.FireCooldownSeconds;
			}
		}

		private int CurrentProjectiles
		{
			get
			{
				if (!(ActiveWeapon != null))
				{
					return projectilesPerShot;
				}
				return ActiveWeapon.ProjectilesPerShot;
			}
		}

		private int CurrentDamage
		{
			get
			{
				if (!(ActiveWeapon != null))
				{
					return damagePerProjectile;
				}
				return ActiveWeapon.DamagePerProjectile;
			}
		}

		private float CurrentSpread
		{
			get
			{
				WeaponDefinition activeWeapon = ActiveWeapon;
				if (activeWeapon == null)
				{
					return spreadDegrees;
				}
				return activeWeapon.SpreadDegrees * activeWeapon.MovementSpreadMultiplier(Speed01);
			}
		}

		private float Speed01 => Mathf.Clamp01(trackedSpeed / 6.4f);

		private int CurrentMissSelfDamage
		{
			get
			{
				if (!(ActiveWeapon != null))
				{
					return 10;
				}
				return ActiveWeapon.MissSelfDamage;
			}
		}

		private bool CurrentAutomatic
		{
			get
			{
				if (ActiveWeapon != null)
				{
					return ActiveWeapon.Automatic;
				}
				return false;
			}
		}

		private float CurrentTrauma
		{
			get
			{
				if (!(ActiveWeapon != null))
				{
					return 0.15f;
				}
				return ActiveWeapon.ShootTrauma;
			}
		}

		private float LocalEyeHeight
		{
			get
			{
				if (!(look != null))
				{
					return 1.6f;
				}
				return look.transform.position.y - base.transform.position.y;
			}
		}

		private void ApplyCameraRecoil()
		{
			WeaponDefinition activeWeapon = ActiveWeapon;
			RecoilPattern recoilPattern = ((activeWeapon != null) ? activeWeapon.RecoilPattern : null);
			if (recoilPattern != null && !(look == null))
			{
				if (Time.time - lastShotTime > 0.55f)
				{
					recoilShot = 0;
				}
				lastShotTime = Time.time;
				bool automatic = activeWeapon.Automatic;
				int shot = (automatic ? recoilShot : UnityEngine.Random.Range(0, 64));
				Vector2 vector = recoilPattern[shot];
				if (automatic && recoilShot < 4)
				{
					vector *= Mathf.Lerp(0.35f, 1f, (float)recoilShot / 4f);
				}
				recoilShot++;
				AimRig()?.AddAimPunch(vector * RecoilMultiplier());
			}
		}

		private float RecoilMultiplier()
		{
			if (scope == null)
			{
				scope = GetComponent<PlayerScope>();
			}
			if (!(scope != null))
			{
				return 1f;
			}
			return scope.RecoilMultiplier;
		}

		private void TrackSpeed()
		{
			Vector3 position = base.transform.position;
			if (!hasSpeedSample)
			{
				hasSpeedSample = true;
				lastSpeedPosition = position;
				return;
			}
			if (Time.deltaTime > 0f)
			{
				Vector3 vector = position - lastSpeedPosition;
				vector.y = 0f;
				float b = vector.magnitude / Time.deltaTime;
				trackedSpeed = Mathf.Lerp(trackedSpeed, b, 1f - Mathf.Exp(-10f * Time.deltaTime));
			}
			lastSpeedPosition = position;
		}

		private void ReportDroppedShot()
		{
			if (!warnedAboutDroppedShot)
			{
				warnedAboutDroppedShot = true;
				Debug.LogWarning($"[PlayerCombat] Client {base.OwnerClientId} atis hizini asti - atis dusuruldu. " + "Oyuncu animasyonu gordu ama atis gerceklesmedi.");
			}
		}

		private void FlashMuzzle()
		{
			if (weapons == null)
			{
				weapons = GetComponent<PlayerWeapons>();
			}
			bool flag = false;
			if (weapons != null)
			{
				if (weapons.FpsVisual != null && weapons.FpsVisual.HasMuzzleFlash)
				{
					weapons.FpsVisual.PlayMuzzleFlash();
					flag = true;
				}
				if (weapons.TpsVisual != null && weapons.TpsVisual.HasMuzzleFlash)
				{
					weapons.TpsVisual.PlayMuzzleFlash();
					flag = true;
				}
			}
			if (!flag && weaponSway != null)
			{
				weaponSway.TriggerMuzzleFlash(MuzzleForFallback());
			}
		}

		private Transform MuzzleForFallback()
		{
			if (weapons == null)
			{
				return null;
			}
			if (weapons.FpsVisual != null)
			{
				return weapons.FpsVisual.Muzzle;
			}
			if (!(weapons.TpsVisual != null))
			{
				return null;
			}
			return weapons.TpsVisual.Muzzle;
		}

		private static bool CanFireInPhase(RoundPhase phase)
		{
			if (phase != RoundPhase.Hunt)
			{
				return phase == RoundPhase.Prep;
			}
			return true;
		}

		private void WatchPhaseForTriggerRelease()
		{
			GameModeController gameModeController = ResolveMode();
			if (gameModeController == null)
			{
				return;
			}
			RoundPhase value = gameModeController.CurrentPhase.Value;
			if (!sawPhase)
			{
				sawPhase = true;
				lastSeenPhase = value;
			}
			else if (value != lastSeenPhase)
			{
				lastSeenPhase = value;
				if (GameInput.Fire.IsPressed())
				{
					triggerLocked = true;
				}
			}
		}

		public void SetReferences(PlayerHealth health, FirstPersonLook look, WeaponSway weaponSway)
		{
			this.health = health;
			this.look = look;
			this.weaponSway = weaponSway;
		}

		private PlayerCameraRig AimRig()
		{
			if (!(aimRig != null))
			{
				return aimRig = GetComponent<PlayerCameraRig>();
			}
			return aimRig;
		}

		private void KickWeaponModel()
		{
			WeaponSway weaponSway = ActiveSway();
			if (!(weaponSway == null))
			{
				WeaponDefinition activeWeapon = ActiveWeapon;
				if (activeWeapon != null)
				{
					weaponSway.SetRecoilRecoverySpeed(activeWeapon.RecoilRecoverySpeed);
					float num = RecoilMultiplier();
					weaponSway.TriggerRecoil(activeWeapon.RecoilKickBack * num, activeWeapon.RecoilKickUpDegrees * num);
				}
				else
				{
					weaponSway.TriggerRecoil();
				}
			}
		}

		private WeaponSway ActiveSway()
		{
			PlayerCameraRig playerCameraRig = AimRig();
			if (!base.IsOwner || (playerCameraRig != null && !playerCameraRig.IsFirstPerson))
			{
				if (weapons == null)
				{
					weapons = GetComponent<PlayerWeapons>();
				}
				if (weapons != null && weapons.TpsSway != null)
				{
					return weapons.TpsSway;
				}
			}
			return weaponSway;
		}

		private RoundManager ResolveRoundManager()
		{
			if (!(roundManager != null))
			{
				return roundManager = GameModeController.Current as RoundManager;
			}
			return roundManager;
		}

		private GameModeController ResolveMode()
		{
			if (!(cachedMode != null))
			{
				GameModeController obj = GameModeController.Current ?? UnityEngine.Object.FindFirstObjectByType<GameModeController>();
				GameModeController result = obj;
				cachedMode = obj;
				return result;
			}
			return cachedMode;
		}

		private void Update()
		{
			TrackSpeed();
			if (!base.IsOwner)
			{
				return;
			}
			WatchPhaseForTriggerRelease();
			if (GameMenuState.LookCaptured)
			{
				return;
			}
			GameModeController gameModeController = ResolveMode();
			if (gameModeController == null || !gameModeController.LocalPlayerMayShoot || !CanFireInPhase(gameModeController.CurrentPhase.Value))
			{
				return;
			}
			if (movement == null)
			{
				movement = GetComponent<PlayerMovement>();
			}
			if ((movement != null && movement.Frozen) || (movement != null && movement.IsHolstered))
			{
				return;
			}
			if (weapons == null)
			{
				weapons = GetComponent<PlayerWeapons>();
			}
			if (weapons != null && weapons.IsSwitching)
			{
				return;
			}
			bool flag = (CurrentAutomatic ? GameInput.Fire.IsPressed() : GameInput.Fire.WasPressedThisFrame());
			if (triggerLocked)
			{
				if (!GameInput.Fire.IsPressed())
				{
					triggerLocked = false;
				}
			}
			else if (flag && !(Time.timeAsDouble < nextAllowedFireLocalTime))
			{
				nextAllowedFireLocalTime = Time.timeAsDouble + CurrentFireCooldown;
				FlashMuzzle();
				ImpactEffects.SpawnShotSound(base.transform.position, ActiveWeapon, ActiveWeaponSound);
				ImpactEffects.ReportShot(base.OwnerClientId, base.transform.position, ActiveWeapon);
				ImpactEffects.ReportShot(base.OwnerClientId, base.transform.position, ActiveWeapon);
				KickWeaponModel();
				AimRig()?.AddAimTrauma(CurrentTrauma);
				PlayerCameraRig playerCameraRig = AimRig();
				Vector2 vector = ((playerCameraRig != null) ? playerCameraRig.AimPunch : Vector2.zero);
				FireServerRpc(((playerCameraRig != null) ? playerCameraRig.AimPitch : 0f) + vector.x, vector.y, LocalEyeHeight, ResolveAimPoint(playerCameraRig));
				ApplyCameraRecoil();
			}
		}

		private Vector3 ResolveAimPoint(PlayerCameraRig rig)
		{
			Transform transform = ((rig != null) ? rig.AimCameraTransform : null);
			if (transform == null)
			{
				return base.transform.position + base.transform.forward * 100f;
			}
			Vector3 position = transform.position;
			Vector3 forward = transform.forward;
			int num = Physics.RaycastNonAlloc(position, forward, aimHits, 100f, hitLayers, QueryTriggerInteraction.Ignore);
			float num2 = float.MaxValue;
			Vector3 result = position + forward * 100f;
			for (int i = 0; i < num; i++)
			{
				RaycastHit raycastHit = aimHits[i];
				if (!(raycastHit.collider == null) && !raycastHit.collider.transform.IsChildOf(base.transform) && !(raycastHit.distance >= num2))
				{
					num2 = raycastHit.distance;
					result = raycastHit.point;
				}
			}
			return result;
		}

		[ServerRpc]
		private void FireServerRpc(float pitch, float yawPunch, float eyeHeight, Vector3 aimPoint)
		{
			NetworkManager networkManager = base.NetworkManager;
			if ((object)networkManager == null || !networkManager.IsListening)
			{
				Debug.LogError("Rpc methods can only be invoked after starting the NetworkManager!");
				return;
			}
			if (__rpc_exec_stage != __RpcExecStage.Execute && (networkManager.IsClient || networkManager.IsHost))
			{
				if (base.OwnerClientId != networkManager.LocalClientId)
				{
					if (networkManager.LogLevel <= LogLevel.Normal)
					{
						Debug.LogError("Only the owner can invoke a ServerRpc that requires ownership!");
					}
					return;
				}
				ServerRpcParams serverRpcParams = default(ServerRpcParams);
				FastBufferWriter bufferWriter = __beginSendServerRpc(571127316u, serverRpcParams, RpcDelivery.Reliable);
				bufferWriter.WriteValueSafe(in pitch, default(FastBufferWriter.ForPrimitives));
				bufferWriter.WriteValueSafe(in yawPunch, default(FastBufferWriter.ForPrimitives));
				bufferWriter.WriteValueSafe(in eyeHeight, default(FastBufferWriter.ForPrimitives));
				bufferWriter.WriteValueSafe(in aimPoint);
				__endSendServerRpc(ref bufferWriter, 571127316u, serverRpcParams, RpcDelivery.Reliable);
			}
			if (__rpc_exec_stage != __RpcExecStage.Execute || (!networkManager.IsServer && !networkManager.IsHost))
			{
				return;
			}
			__rpc_exec_stage = __RpcExecStage.Send;
			GameModeController gameModeController = ResolveMode();
			if (gameModeController == null || !CanFireInPhase(gameModeController.CurrentPhase.Value) || !gameModeController.ServerAllowsShooting(base.OwnerClientId))
			{
				return;
			}
			if (movement == null)
			{
				movement = GetComponent<PlayerMovement>();
			}
			if ((movement != null && movement.IsHolstered) || (health != null && health.Health.Value <= 0))
			{
				return;
			}
			double num = CurrentFireCooldown * 0.15;
			if (base.NetworkManager.ServerTime.Time < nextAllowedFireServerTime - num)
			{
				ReportDroppedShot();
				return;
			}
			double time = base.NetworkManager.ServerTime.Time;
			double num2 = ((time < nextAllowedFireServerTime) ? nextAllowedFireServerTime : time);
			nextAllowedFireServerTime = num2 + CurrentFireCooldown;
			ShotFiredClientRpc();
			float x = ((look != null) ? Mathf.Clamp(pitch, look.MinPitch, look.MaxPitch) : 0f);
			Vector3 vector = base.transform.position + Vector3.up * Mathf.Clamp(eyeHeight, 0.4f, 1.6f);
			Quaternion quaternion = base.transform.rotation * Quaternion.Euler(x, Mathf.Clamp(yawPunch, -20f, 20f), 0f);
			Vector3 vector2 = aimPoint - vector;
			bool flag = false;
			float num3 = 0f;
			if (vector2.sqrMagnitude > 0.01f)
			{
				Vector3 normalized = vector2.normalized;
				num3 = Vector3.Angle(quaternion * Vector3.forward, normalized);
				flag = num3 <= 50f;
				if (flag)
				{
					quaternion = Quaternion.LookRotation(normalized, Vector3.up);
				}
			}
			if (DebugShots)
			{
				DescribeShot(vector, eyeHeight, quaternion, aimPoint, num3, flag);
			}
			bool flag2 = false;
			bool flag3 = false;
			ulong num4 = 0uL;
			impactPoints.Clear();
			impactNormals.Clear();
			wallPoints.Clear();
			wallNormals.Clear();
			voidPoints.Clear();
			for (int i = 0; i < CurrentProjectiles; i++)
			{
				Vector3 vector3 = Scatter(quaternion);
				Vector3 hitPoint;
				Vector3 hitNormal;
				ulong victimId;
				bool eliminated;
				PelletResult pelletResult = FirePellet(gameModeController, vector, vector3, out hitPoint, out hitNormal, out victimId, out eliminated);
				if (pelletResult == PelletResult.Miss)
				{
					if (hitNormal.sqrMagnitude > 0.001f)
					{
						if (wallPoints.Count < 6)
						{
							wallPoints.Add(hitPoint);
							wallNormals.Add(hitNormal);
						}
					}
					else if (voidPoints.Count < 6)
					{
						voidPoints.Add(vector + vector3 * 100f);
					}
					continue;
				}
				flag2 = true;
				if (pelletResult == PelletResult.StruckHider)
				{
					flag3 = true;
					if (impactPoints.Count < 6)
					{
						impactPoints.Add(hitPoint);
						impactNormals.Add(hitNormal);
					}
					if (num4 == 0L)
					{
						num4 = victimId;
					}
					if (eliminated)
					{
						gameModeController.ServerReportKill(base.OwnerClientId, victimId);
					}
				}
			}
			if (impactPoints.Count > 0)
			{
				HitFeedbackClientRpc(impactPoints.ToArray(), impactNormals.ToArray(), num4);
			}
			if (wallPoints.Count > 0)
			{
				WallFeedbackClientRpc(wallPoints.ToArray(), wallNormals.ToArray());
			}
			if (voidPoints.Count > 0)
			{
				TracerOnlyClientRpc(voidPoints.ToArray());
			}
			if (!flag2 && gameModeController.PenalisesMissedShots && gameModeController.CurrentPhase.Value == RoundPhase.Hunt)
			{
				health.ServerTakeDamage(gameModeController.ScaleSelfDamage(CurrentMissSelfDamage));
			}
			else if (flag3 && gameModeController.CurrentPhase.Value == RoundPhase.Hunt)
			{
				health.ServerHeal(gameModeController.HitHealthReward);
			}
		}

		private void DescribeShot(Vector3 origin, float eyeHeight, Quaternion aim, Vector3 aimPoint, float correction, bool corrected)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(string.Format("[shot] shooter={0}{1} origin={2} ", base.OwnerClientId, base.IsOwner ? " (host)" : "", F(origin)) + $"eye={eyeHeight:F2} aim={F(aim * Vector3.forward)} crosshair={F(aimPoint)} " + string.Format("correction={0:F1}deg {1}", correction, corrected ? "applied" : "REFUSED"));
			PlayerHealth[] array = UnityEngine.Object.FindObjectsByType<PlayerHealth>(FindObjectsSortMode.None);
			foreach (PlayerHealth playerHealth in array)
			{
				if (!(playerHealth == null) && !(playerHealth == health))
				{
					Transform transform = playerHealth.transform;
					CharacterController component = playerHealth.GetComponent<CharacterController>();
					string arg = "no capsule";
					if (component != null)
					{
						float num = Mathf.Max(0.01f, transform.lossyScale.y);
						float y = transform.TransformPoint(component.center).y;
						float num2 = component.height * 0.5f * num;
						arg = $"capsule y {y - num2:F2}..{y + num2:F2}" + string.Format("{0} r={1:F2}", component.enabled ? "" : " DISABLED", component.radius * num);
					}
					string arg2 = "no head bone";
					PlayerAnimator componentInChildren = playerHealth.GetComponentInChildren<PlayerAnimator>(includeInactive: true);
					Animator animator = ((componentInChildren != null) ? componentInChildren.Animator : null);
					if (animator != null && animator.isHuman)
					{
						Transform boneTransform = animator.GetBoneTransform(HumanBodyBones.Head);
						arg2 = ((boneTransform != null) ? string.Format("head y={0:F2} animator={1}", boneTransform.position.y, animator.enabled ? "on" : "OFF") : "no head bone");
					}
					float num3 = Vector3.Distance(origin, transform.position);
					stringBuilder.Append($"\n  target={playerHealth.OwnerClientId} root={F(transform.position)} dist={num3:F1} " + $"{arg} {arg2} hp={playerHealth.Health.Value}");
				}
			}
			Say(stringBuilder.ToString());
		}

		private static void DescribePelletPath(Vector3 origin, Vector3 direction, RaycastHit[] hits)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append($"[pellet] dir={F(direction)} hits={hits.Length}");
			int num = 0;
			for (int i = 0; i < hits.Length; i++)
			{
				RaycastHit raycastHit = hits[i];
				if (num++ >= 3)
				{
					break;
				}
				PlayerHealth componentInParent = raycastHit.collider.GetComponentInParent<PlayerHealth>();
				stringBuilder.Append($"\n  {raycastHit.distance:F2}m {Path(raycastHit.collider.transform)} " + "layer=" + LayerMask.LayerToName(raycastHit.collider.gameObject.layer) + " " + string.Format("{0} at y={1:F2}", (componentInParent != null) ? ("player " + componentInParent.OwnerClientId) : "no player", raycastHit.point.y));
			}
			if (hits.Length == 0)
			{
				stringBuilder.Append("\n  nothing within range");
			}
			Say(stringBuilder.ToString());
		}

		private static string Path(Transform t)
		{
			string text = t.name;
			int num = 0;
			while (t.parent != null && num++ < 3)
			{
				t = t.parent;
				text = t.name + "/" + text;
			}
			return text;
		}

		private static string F(Vector3 v)
		{
			return $"({v.x:F2}, {v.y:F2}, {v.z:F2})";
		}

		private static void Say(string text)
		{
			Debug.Log(text);
			DevConsole.Log(text);
		}

		private PelletResult FirePellet(GameModeController mode, Vector3 origin, Vector3 direction, out Vector3 hitPoint, out Vector3 hitNormal, out ulong victimId, out bool eliminated)
		{
			hitPoint = default(Vector3);
			hitNormal = default(Vector3);
			victimId = 0uL;
			eliminated = false;
			RaycastHit[] array = Physics.RaycastAll(origin, direction, 100f, hitLayers, QueryTriggerInteraction.Ignore);
			Array.Sort(array, (RaycastHit a, RaycastHit b) => a.distance.CompareTo(b.distance));
			if (DebugShots)
			{
				DescribePelletPath(origin, direction, array);
			}
			float num = ((ActiveWeapon != null) ? ActiveWeapon.Penetration : 0f);
			float num2 = 1f;
			RaycastHit[] array2 = array;
			for (int num3 = 0; num3 < array2.Length; num3++)
			{
				RaycastHit raycastHit = array2[num3];
				PlayerHealth componentInParent = raycastHit.collider.GetComponentInParent<PlayerHealth>();
				if (componentInParent == null)
				{
					PenetrableSurface componentInParent2 = raycastHit.collider.GetComponentInParent<PenetrableSurface>();
					if (!(componentInParent2 != null) || !(num > 0f) || !(num >= componentInParent2.Resistance))
					{
						hitPoint = raycastHit.point;
						hitNormal = raycastHit.normal;
						ShootingRangeZone componentInParent3 = raycastHit.collider.GetComponentInParent<ShootingRangeZone>();
						if (componentInParent3 != null)
						{
							ReportRangeHit(componentInParent3.Points);
						}
						return PelletResult.Miss;
					}
					num -= componentInParent2.Resistance;
					num2 *= componentInParent2.DamageRetained;
					if (wallPoints.Count < 6)
					{
						wallPoints.Add(raycastHit.point);
						wallNormals.Add(raycastHit.normal);
					}
				}
				else
				{
					if (componentInParent == health)
					{
						continue;
					}
					ulong ownerClientId = componentInParent.OwnerClientId;
					if (mode.ServerIsShotTarget(ownerClientId))
					{
						if (!mode.ServerMayDamage(base.OwnerClientId, ownerClientId))
						{
							return PelletResult.StruckNonTarget;
						}
						HitZone zone = PlayerHitZones.Classify(componentInParent.gameObject, raycastHit.point);
						float num4 = ((ActiveWeapon != null) ? ActiveWeapon.DamageMultiplier(zone) : 1f);
						int amount = Mathf.Max(1, Mathf.RoundToInt((float)CurrentDamage * num4 * num2));
						eliminated = componentInParent.ServerTakeDamage(amount);
						if (mode.ShowsDamageDirection)
						{
							componentInParent.ServerReportDamageFrom(base.transform.position);
						}
						float shotImpulseOnDowned = mode.ShotImpulseOnDowned;
						if (shotImpulseOnDowned > 0f)
						{
							PlayerRagdoll component = componentInParent.GetComponent<PlayerRagdoll>();
							if (component != null && component.IsLimp)
							{
								if (raycastHit.rigidbody != null)
								{
									component.ServerNudge(raycastHit.rigidbody, raycastHit.point, direction * shotImpulseOnDowned);
								}
								else
								{
									component.ServerNudgeNearest(raycastHit.point, direction * shotImpulseOnDowned);
								}
							}
						}
						hitPoint = raycastHit.point;
						hitNormal = raycastHit.normal;
						victimId = ownerClientId;
						mode.ServerRecordDamage(base.OwnerClientId, ownerClientId, amount);
						return PelletResult.StruckHider;
					}
					float shotImpulseOnDowned2 = mode.ShotImpulseOnDowned;
					if (shotImpulseOnDowned2 > 0f && raycastHit.rigidbody != null)
					{
						PlayerRagdoll component2 = componentInParent.GetComponent<PlayerRagdoll>();
						if (component2 != null)
						{
							component2.ServerNudge(raycastHit.rigidbody, raycastHit.point, direction * shotImpulseOnDowned2);
						}
					}
				}
			}
			return PelletResult.Miss;
		}

		private void ReportRangeHit(int points)
		{
			if (points > 0)
			{
				if (weapons == null)
				{
					weapons = GetComponent<PlayerWeapons>();
				}
				weapons?.RangeHitClientRpc(points, base.RpcTarget.Single(base.OwnerClientId, RpcTargetUse.Temp));
			}
		}

		private Vector3 Scatter(Quaternion aim)
		{
			float currentSpread = CurrentSpread;
			if (currentSpread <= 0f)
			{
				return aim * Vector3.forward;
			}
			Vector2 vector = UnityEngine.Random.insideUnitCircle * currentSpread;
			return aim * Quaternion.Euler(vector.y, vector.x, 0f) * Vector3.forward;
		}

		[ClientRpc]
		private void ShotFiredClientRpc()
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
				FastBufferWriter bufferWriter = __beginSendClientRpc(1373763417u, clientRpcParams, RpcDelivery.Reliable);
				__endSendClientRpc(ref bufferWriter, 1373763417u, clientRpcParams, RpcDelivery.Reliable);
			}
			if (__rpc_exec_stage == __RpcExecStage.Execute && (networkManager.IsClient || networkManager.IsHost))
			{
				__rpc_exec_stage = __RpcExecStage.Send;
				if (!base.IsOwner)
				{
					FlashMuzzle();
					KickWeaponModel();
					ImpactEffects.SpawnShotSound(base.transform.position, ActiveWeapon, ActiveWeaponSound);
				}
			}
		}

		[ClientRpc]
		private void HitFeedbackClientRpc(Vector3[] hitPoints, Vector3[] hitNormals, ulong victimId)
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
				FastBufferWriter bufferWriter = __beginSendClientRpc(608926990u, clientRpcParams, RpcDelivery.Reliable);
				bool value = hitPoints != null;
				bufferWriter.WriteValueSafe(in value, default(FastBufferWriter.ForPrimitives));
				if (value)
				{
					bufferWriter.WriteValueSafe(hitPoints);
				}
				bool value2 = hitNormals != null;
				bufferWriter.WriteValueSafe(in value2, default(FastBufferWriter.ForPrimitives));
				if (value2)
				{
					bufferWriter.WriteValueSafe(hitNormals);
				}
				BytePacker.WriteValueBitPacked(bufferWriter, victimId);
				__endSendClientRpc(ref bufferWriter, 608926990u, clientRpcParams, RpcDelivery.Reliable);
			}
			if (__rpc_exec_stage == __RpcExecStage.Execute && (networkManager.IsClient || networkManager.IsHost))
			{
				__rpc_exec_stage = __RpcExecStage.Send;
				GameModeController gameModeController = ResolveMode();
				bool withSound = gameModeController == null || gameModeController.PlaysHitConfirm;
				Vector3 vector = TracerOrigin();
				for (int i = 0; i < hitPoints.Length; i++)
				{
					ImpactEffects.SpawnHitEffect(hitPoints[i], (i < hitNormals.Length) ? hitNormals[i] : default(Vector3), withSound);
					ImpactEffects.SpawnTracer(vector, hitPoints[i]);
				}
				FlashVictim(victimId);
				if (base.IsOwner)
				{
					CrosshairView.Instance?.ShowHitMarker();
				}
			}
		}

		[ClientRpc]
		private void WallFeedbackClientRpc(Vector3[] hitPoints, Vector3[] hitNormals)
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
				FastBufferWriter bufferWriter = __beginSendClientRpc(186913772u, clientRpcParams, RpcDelivery.Reliable);
				bool value = hitPoints != null;
				bufferWriter.WriteValueSafe(in value, default(FastBufferWriter.ForPrimitives));
				if (value)
				{
					bufferWriter.WriteValueSafe(hitPoints);
				}
				bool value2 = hitNormals != null;
				bufferWriter.WriteValueSafe(in value2, default(FastBufferWriter.ForPrimitives));
				if (value2)
				{
					bufferWriter.WriteValueSafe(hitNormals);
				}
				__endSendClientRpc(ref bufferWriter, 186913772u, clientRpcParams, RpcDelivery.Reliable);
			}
			if (__rpc_exec_stage == __RpcExecStage.Execute && (networkManager.IsClient || networkManager.IsHost))
			{
				__rpc_exec_stage = __RpcExecStage.Send;
				GameModeController gameModeController = ResolveMode();
				bool withSound = gameModeController == null || (gameModeController.PenalisesMissedShots && gameModeController.CurrentPhase.Value == RoundPhase.Hunt);
				Vector3 vector = TracerOrigin();
				for (int i = 0; i < hitPoints.Length; i++)
				{
					ImpactEffects.SpawnWallEffect(hitPoints[i], (i < hitNormals.Length) ? hitNormals[i] : default(Vector3), withSound);
					ImpactEffects.SpawnTracer(vector, hitPoints[i]);
				}
			}
		}

		[ClientRpc]
		private void TracerOnlyClientRpc(Vector3[] ends)
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
				FastBufferWriter bufferWriter = __beginSendClientRpc(3256463353u, clientRpcParams, RpcDelivery.Reliable);
				bool value = ends != null;
				bufferWriter.WriteValueSafe(in value, default(FastBufferWriter.ForPrimitives));
				if (value)
				{
					bufferWriter.WriteValueSafe(ends);
				}
				__endSendClientRpc(ref bufferWriter, 3256463353u, clientRpcParams, RpcDelivery.Reliable);
			}
			if (__rpc_exec_stage == __RpcExecStage.Execute && (networkManager.IsClient || networkManager.IsHost))
			{
				__rpc_exec_stage = __RpcExecStage.Send;
				Vector3 vector = TracerOrigin();
				for (int i = 0; i < ends.Length; i++)
				{
					ImpactEffects.SpawnTracer(vector, ends[i]);
				}
			}
		}

		private Vector3 TracerOrigin()
		{
			if (weapons == null)
			{
				weapons = GetComponent<PlayerWeapons>();
			}
			if (weapons != null)
			{
				if (base.IsOwner && weapons.FpsVisual != null && weapons.FpsVisual.gameObject.activeInHierarchy)
				{
					return weapons.FpsVisual.Muzzle.position;
				}
				if (weapons.TpsVisual != null && weapons.TpsVisual.gameObject.activeInHierarchy)
				{
					return weapons.TpsVisual.Muzzle.position;
				}
			}
			return base.transform.position + Vector3.up * 1.6f;
		}

		private static void FlashVictim(ulong victimId)
		{
			NetworkManager singleton = NetworkManager.Singleton;
			if (!(singleton == null) && singleton.ConnectedClients.TryGetValue(victimId, out var value) && !(value.PlayerObject == null))
			{
				value.PlayerObject.GetComponent<VoxelHitFlash>()?.Flash();
			}
		}

		protected override void __initializeVariables()
		{
			base.__initializeVariables();
		}

		protected override void __initializeRpcs()
		{
			__registerRpc(571127316u, __rpc_handler_571127316, "FireServerRpc", RpcInvokePermission.Owner);
			__registerRpc(1373763417u, __rpc_handler_1373763417, "ShotFiredClientRpc", RpcInvokePermission.Server);
			__registerRpc(608926990u, __rpc_handler_608926990, "HitFeedbackClientRpc", RpcInvokePermission.Server);
			__registerRpc(186913772u, __rpc_handler_186913772, "WallFeedbackClientRpc", RpcInvokePermission.Server);
			__registerRpc(3256463353u, __rpc_handler_3256463353, "TracerOnlyClientRpc", RpcInvokePermission.Server);
			base.__initializeRpcs();
		}

		private static void __rpc_handler_571127316(NetworkBehaviour target, FastBufferReader reader, __RpcParams rpcParams)
		{
			NetworkManager networkManager = target.NetworkManager;
			if ((object)networkManager == null || !networkManager.IsListening)
			{
				return;
			}
			if (rpcParams.Server.Receive.SenderClientId != target.OwnerClientId)
			{
				if (networkManager.LogLevel <= LogLevel.Normal)
				{
					Debug.LogError("Only the owner can invoke a ServerRpc that requires ownership!");
				}
				return;
			}
			reader.ReadValueSafe(out float value, default(FastBufferWriter.ForPrimitives));
			reader.ReadValueSafe(out float value2, default(FastBufferWriter.ForPrimitives));
			reader.ReadValueSafe(out float value3, default(FastBufferWriter.ForPrimitives));
			reader.ReadValueSafe(out Vector3 value4);
			target.__rpc_exec_stage = __RpcExecStage.Execute;
			((PlayerCombat)target).FireServerRpc(value, value2, value3, value4);
			target.__rpc_exec_stage = __RpcExecStage.Send;
		}

		private static void __rpc_handler_1373763417(NetworkBehaviour target, FastBufferReader reader, __RpcParams rpcParams)
		{
			NetworkManager networkManager = target.NetworkManager;
			if ((object)networkManager != null && networkManager.IsListening)
			{
				target.__rpc_exec_stage = __RpcExecStage.Execute;
				((PlayerCombat)target).ShotFiredClientRpc();
				target.__rpc_exec_stage = __RpcExecStage.Send;
			}
		}

		private static void __rpc_handler_608926990(NetworkBehaviour target, FastBufferReader reader, __RpcParams rpcParams)
		{
			NetworkManager networkManager = target.NetworkManager;
			if ((object)networkManager != null && networkManager.IsListening)
			{
				reader.ReadValueSafe(out bool value, default(FastBufferWriter.ForPrimitives));
				Vector3[] value2 = null;
				if (value)
				{
					reader.ReadValueSafe(out value2);
				}
				reader.ReadValueSafe(out bool value3, default(FastBufferWriter.ForPrimitives));
				Vector3[] value4 = null;
				if (value3)
				{
					reader.ReadValueSafe(out value4);
				}
				ByteUnpacker.ReadValueBitPacked(reader, out ulong value5);
				target.__rpc_exec_stage = __RpcExecStage.Execute;
				((PlayerCombat)target).HitFeedbackClientRpc(value2, value4, value5);
				target.__rpc_exec_stage = __RpcExecStage.Send;
			}
		}

		private static void __rpc_handler_186913772(NetworkBehaviour target, FastBufferReader reader, __RpcParams rpcParams)
		{
			NetworkManager networkManager = target.NetworkManager;
			if ((object)networkManager != null && networkManager.IsListening)
			{
				reader.ReadValueSafe(out bool value, default(FastBufferWriter.ForPrimitives));
				Vector3[] value2 = null;
				if (value)
				{
					reader.ReadValueSafe(out value2);
				}
				reader.ReadValueSafe(out bool value3, default(FastBufferWriter.ForPrimitives));
				Vector3[] value4 = null;
				if (value3)
				{
					reader.ReadValueSafe(out value4);
				}
				target.__rpc_exec_stage = __RpcExecStage.Execute;
				((PlayerCombat)target).WallFeedbackClientRpc(value2, value4);
				target.__rpc_exec_stage = __RpcExecStage.Send;
			}
		}

		private static void __rpc_handler_3256463353(NetworkBehaviour target, FastBufferReader reader, __RpcParams rpcParams)
		{
			NetworkManager networkManager = target.NetworkManager;
			if ((object)networkManager != null && networkManager.IsListening)
			{
				reader.ReadValueSafe(out bool value, default(FastBufferWriter.ForPrimitives));
				Vector3[] value2 = null;
				if (value)
				{
					reader.ReadValueSafe(out value2);
				}
				target.__rpc_exec_stage = __RpcExecStage.Execute;
				((PlayerCombat)target).TracerOnlyClientRpc(value2);
				target.__rpc_exec_stage = __RpcExecStage.Send;
			}
		}

		protected internal override string __getTypeName()
		{
			return "PlayerCombat";
		}
	}
}
