using System;
using Mimicraft.Customization;
using Mimicraft.Networking;
using Mimicraft.UI;
using Unity.Netcode;
using UnityEngine;

namespace Mimicraft.Gameplay
{
	[RequireComponent(typeof(NetworkObject))]
	public class PlayerHealth : NetworkBehaviour
	{
		public const int MaxHealth = 100;

		private const float HurtTrauma = 0.4f;

		public readonly NetworkVariable<int> Health = new NetworkVariable<int>(100);

		[SerializeField]
		private PlayerCameraRig cameraRig;

		[SerializeField]
		private AudioSource hurtSource;

		private double lastDamageServerTime = double.NegativeInfinity;

		private float regenCarry;

		private bool damageSourcePending;

		private Vector3 damageSource;

		private PlayerCharacterAppearance appearance;

		private PlayerWeapons weapons;

		public bool DevInvulnerable { get; set; }

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

		public static event Action<Vector3> LocalDamageFrom;

		public void SetFeedbackReferences(PlayerCameraRig cameraRig, AudioSource hurtSource)
		{
			this.cameraRig = cameraRig;
			this.hurtSource = hurtSource;
		}

		public override void OnNetworkSpawn()
		{
			NetworkVariable<int> health = Health;
			health.OnValueChanged = (NetworkVariable<int>.OnValueChangedDelegate)Delegate.Combine(health.OnValueChanged, new NetworkVariable<int>.OnValueChangedDelegate(OnHealthChanged));
		}

		public override void OnNetworkDespawn()
		{
			NetworkVariable<int> health = Health;
			health.OnValueChanged = (NetworkVariable<int>.OnValueChangedDelegate)Delegate.Remove(health.OnValueChanged, new NetworkVariable<int>.OnValueChangedDelegate(OnHealthChanged));
		}

		private void Update()
		{
			if (!base.IsServer || Health.Value <= 0 || Health.Value >= 100)
			{
				return;
			}
			GameModeController current = GameModeController.Current;
			if (!(current == null) && current.TryGetHealthRegen(out var delaySeconds, out var perSecond) && !(base.NetworkManager.ServerTime.Time - lastDamageServerTime < (double)delaySeconds))
			{
				regenCarry += perSecond * Time.deltaTime;
				int num = Mathf.FloorToInt(regenCarry);
				if (num > 0)
				{
					regenCarry -= num;
					Health.Value = Mathf.Min(100, Health.Value + num);
				}
			}
		}

		public void ServerReportDamageFrom(Vector3 sourcePosition)
		{
			if (base.IsServer)
			{
				damageSourcePending = true;
				damageSource = sourcePosition;
			}
		}

		private void LateUpdate()
		{
			if (base.IsServer && damageSourcePending)
			{
				damageSourcePending = false;
				DamageFromClientRpc(damageSource, new ClientRpcParams
				{
					Send = new ClientRpcSendParams
					{
						TargetClientIds = new ulong[1] { base.OwnerClientId }
					}
				});
			}
		}

		[ClientRpc]
		private void DamageFromClientRpc(Vector3 sourcePosition, ClientRpcParams clientRpcParams = default(ClientRpcParams))
		{
			NetworkManager networkManager = base.NetworkManager;
			if ((object)networkManager == null || !networkManager.IsListening)
			{
				Debug.LogError("Rpc methods can only be invoked after starting the NetworkManager!");
				return;
			}
			if (__rpc_exec_stage != __RpcExecStage.Execute && (networkManager.IsServer || networkManager.IsHost))
			{
				FastBufferWriter bufferWriter = __beginSendClientRpc(3656838154u, clientRpcParams, RpcDelivery.Reliable);
				bufferWriter.WriteValueSafe(in sourcePosition);
				__endSendClientRpc(ref bufferWriter, 3656838154u, clientRpcParams, RpcDelivery.Reliable);
			}
			if (__rpc_exec_stage == __RpcExecStage.Execute && (networkManager.IsClient || networkManager.IsHost))
			{
				__rpc_exec_stage = __RpcExecStage.Send;
				if (base.IsOwner)
				{
					PlayerHealth.LocalDamageFrom?.Invoke(sourcePosition);
				}
			}
		}

		public void ServerResetHealth()
		{
			if (base.IsServer)
			{
				Health.Value = 100;
			}
		}

		public void ServerHeal(int amount)
		{
			if (base.IsServer && amount > 0 && Health.Value > 0 && Health.Value < 100)
			{
				Health.Value = Mathf.Min(100, Health.Value + amount);
			}
		}

		public bool ServerTakeDamage(int amount)
		{
			if (!base.IsServer || Health.Value <= 0)
			{
				return false;
			}
			if (DevInvulnerable)
			{
				return false;
			}
			Health.Value = Mathf.Max(0, Health.Value - amount);
			lastDamageServerTime = base.NetworkManager.ServerTime.Time;
			regenCarry = 0f;
			if (Health.Value != 0)
			{
				return false;
			}
			GameModeController current = GameModeController.Current;
			if (current != null)
			{
				return current.ServerOnPlayerDied(base.OwnerClientId);
			}
			return false;
		}

		private void OnHealthChanged(int previous, int current)
		{
			if (current >= previous)
			{
				return;
			}
			if (!base.IsOwner)
			{
				PlayerEffortVoice.Play(this, AudioLibrary.NextShotHurtClip(VoiceType));
				return;
			}
			GameModeController current2 = GameModeController.Current;
			bool flag = current2 != null && current2.PenalisesMissedShots && current2.LocalPlayerMayShoot;
			WeaponDefinition weaponDefinition = (flag ? ResolveWeapon() : null);
			float num = ((weaponDefinition != null) ? weaponDefinition.MissFeedbackVolume : 1f);
			float num2 = ((weaponDefinition != null) ? weaponDefinition.MissFeedbackFlash : 1f);
			cameraRig?.Shake(0.4f);
			if (hurtSource != null && num > 0f)
			{
				SpatialAudio.Route(hurtSource);
				AudioClip clip = (flag ? ImpactEffects.GetHurtClip() : AudioLibrary.Or(AudioLibrary.NextShotHurtClip(VoiceType), ImpactEffects.GetHurtClip()));
				hurtSource.PlayOneShot(clip, num * AudioLibrary.VolumeOf(clip));
			}
			if (num2 > 0f)
			{
				DamageFlashView.Instance?.Flash(num2);
			}
		}

		private WeaponDefinition ResolveWeapon()
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

		protected override void __initializeVariables()
		{
			if (Health == null)
			{
				throw new Exception("PlayerHealth.Health cannot be null. All NetworkVariableBase instances must be initialized.");
			}
			Health.Initialize(this);
			__nameNetworkVariable(Health, "Health");
			NetworkVariableFields.Add(Health);
			base.__initializeVariables();
		}

		protected override void __initializeRpcs()
		{
			__registerRpc(3656838154u, __rpc_handler_3656838154, "DamageFromClientRpc", RpcInvokePermission.Server);
			base.__initializeRpcs();
		}

		private static void __rpc_handler_3656838154(NetworkBehaviour target, FastBufferReader reader, __RpcParams rpcParams)
		{
			NetworkManager networkManager = target.NetworkManager;
			if ((object)networkManager != null && networkManager.IsListening)
			{
				reader.ReadValueSafe(out Vector3 value);
				ClientRpcParams client = rpcParams.Client;
				target.__rpc_exec_stage = __RpcExecStage.Execute;
				((PlayerHealth)target).DamageFromClientRpc(value, client);
				target.__rpc_exec_stage = __RpcExecStage.Send;
			}
		}

		protected internal override string __getTypeName()
		{
			return "PlayerHealth";
		}
	}
}
