using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using Features.Movement.Scripts;
using Features.PlayerStatesModule.Scripts;
using Features.RagdollModule.Scripts;
using Features.StatsUsageModule.Scripts.Entities.EntityStatTypeEntities;
using Features.SynchronizedModelsModule.Scripts.JsonModelSynchronizer;
using Fusion;
using RSG.Muffin.StatsSubmodule.EntityStatsModule.Scripts.StatsEntity;
using UnityEngine;
using UnityEngine.Scripting;
using Zenject;

namespace Features.DamageableTrackModule.Scripts
{
	[NetworkBehaviourWeaved(0)]
	public class PlayerDamageable : NetworkBehaviour, IDamageable
	{
		[SerializeField]
		private EntityStatEntityNetworkedBase _entityStatEntityNetworkedBase;

		[SerializeField]
		private Rigidbody _rigidbody;

		private IStat _healthStat;

		private IPlayerStateService _playerStateService;

		private ILocalPlayerThrowService _localPlayerThrowService;

		private PlayerMovableModel _playerMovableModel;

		private PlayerDamageAbsorbersModel _playerDamageAbsorbersModel;

		private DamageSource? _lastDamageSource;

		public NetworkObject NetworkObject => base.Object;

		public Transform Transform => base.transform;

		public bool IsActive { get; private set; }

		public float Health => _healthStat.FullValue;

		public bool IsFullHealth => _healthStat.FullValue >= _healthStat.MaxValue;

		public List<DamageableTag> DamageableTags { get; set; } = new List<DamageableTag> { DamageableTag.Player };

		public event Action<DamageData> OnDamaged;

		public event Action<float> OnHealed;

		public event Action<bool> OnIsActiveChanged;

		[Inject]
		public void InjectDependencies(IPlayerStateService playerStateService, ILocalPlayerThrowService localPlayerThrowService, PlayerMovableModel playerMovableModel, PlayerDamageAbsorbersModel playerDamageAbsorbersModel)
		{
			_playerStateService = playerStateService;
			_localPlayerThrowService = localPlayerThrowService;
			_playerMovableModel = playerMovableModel;
			_playerDamageAbsorbersModel = playerDamageAbsorbersModel;
		}

		private void Awake()
		{
			_healthStat = _entityStatEntityNetworkedBase.GetStat(EntityStatType.Health);
			_healthStat.MinValue = 0f;
		}

		private void OnEnable()
		{
			NetworkBehaviourUtils.InternalOnEnable(this);
			IsActive = true;
			this.OnIsActiveChanged?.Invoke(IsActive);
		}

		private void OnDisable()
		{
			NetworkBehaviourUtils.InternalOnDisable(this);
			IsActive = false;
			this.OnIsActiveChanged?.Invoke(IsActive);
		}

		private void OnDestroy()
		{
			NetworkBehaviourUtils.InternalOnDestroy(this);
			IsActive = false;
			this.OnIsActiveChanged?.Invoke(IsActive);
		}

		[Rpc(RpcSources.All, RpcTargets.All, Key = 2950992230u)]
		public void DamageRPC([RpcPayload(4)] float damage)
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				int bytePayloadSize = Fusion.RpcDataWriter.GetBytePayloadSize(4);
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(2950992230u, bytePayloadSize, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var writer);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.DamageableTrackModule.Scripts.PlayerDamageable::DamageRPC(System.Single)", invokeInfo, PlayerRef.None);
					}
					if (invokeInfo.SendMessageResult == RpcSendMessageResult.Sent)
					{
						writer.Write(damage, 4);
						rpcBuilder.Send();
					}
				}
				NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromLocalCall((object)this, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), invokeInfo, RpcHostMode.SourceIsServer, PlayerRef.Invalid));
				if (invokeInfo.LocalInvokeResult != RpcLocalInvokeResult.Invoked)
				{
					return;
				}
			}
			Damage(new DamageData
			{
				Damage = damage,
				Source = DamageDataSourceExtensions.ForPlayerAttack(DamageType.Unknown)
			});
		}

		[Rpc(RpcSources.All, RpcTargets.All, Key = 3275043975u)]
		public void DamageRPC([RpcPayload(4)] float damage, [RpcPayload(4)] int dealerPlayerID)
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				int bytePayloadSize = Fusion.RpcDataWriter.GetBytePayloadSize(4);
				bytePayloadSize += Fusion.RpcDataWriter.GetBytePayloadSize(4);
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(3275043975u, bytePayloadSize, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var writer);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.DamageableTrackModule.Scripts.PlayerDamageable::DamageRPC(System.Single,System.Int32)", invokeInfo, PlayerRef.None);
					}
					if (invokeInfo.SendMessageResult == RpcSendMessageResult.Sent)
					{
						writer.Write(damage, 4);
						writer.Write(dealerPlayerID, 4);
						rpcBuilder.Send();
					}
				}
				NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromLocalCall((object)this, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), invokeInfo, RpcHostMode.SourceIsServer, PlayerRef.Invalid));
				if (invokeInfo.LocalInvokeResult != RpcLocalInvokeResult.Invoked)
				{
					return;
				}
			}
			DamageRPC(damage, dealerPlayerID, default(DamageRpcSource));
		}

		[Rpc(RpcSources.All, RpcTargets.All, Key = 2255433589u)]
		public void DamageRPC([RpcPayload(4)] float damage, [RpcPayload(4)] int dealerPlayerID, [RpcPayload(208)] DamageRpcSource rpcSource)
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				int bytePayloadSize = Fusion.RpcDataWriter.GetBytePayloadSize(4);
				bytePayloadSize += Fusion.RpcDataWriter.GetBytePayloadSize(4);
				bytePayloadSize += Fusion.RpcDataWriter.GetBytePayloadSize(208);
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(2255433589u, bytePayloadSize, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var writer);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.DamageableTrackModule.Scripts.PlayerDamageable::DamageRPC(System.Single,System.Int32,Features.DamageableTrackModule.Scripts.DamageRpcSource)", invokeInfo, PlayerRef.None);
					}
					if (invokeInfo.SendMessageResult == RpcSendMessageResult.Sent)
					{
						writer.Write(damage, 4);
						writer.Write(dealerPlayerID, 4);
						writer.Write(rpcSource, 208);
						rpcBuilder.Send();
					}
				}
				NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromLocalCall((object)this, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), invokeInfo, RpcHostMode.SourceIsServer, PlayerRef.Invalid));
				if (invokeInfo.LocalInvokeResult != RpcLocalInvokeResult.Invoked)
				{
					return;
				}
			}
			Damage(new DamageData
			{
				Damage = damage,
				DamageDealerPlayerID = dealerPlayerID,
				Source = (rpcSource.IsDefault ? DamageDataSourceExtensions.ForUnknown() : DamageDataSourceExtensions.FromRpc(rpcSource))
			});
		}

		public void AddRPCForce(float force, Vector3 direction, ForceMode forceMode)
		{
			if (base.Object != null && base.Object.IsValid && force != 0f)
			{
				AddForceRPC(force, direction, forceMode);
			}
		}

		[Rpc(RpcSources.All, RpcTargets.All, Key = 91470905u)]
		private void AddForceRPC([RpcPayload(4)] float force, [RpcPayload(12)] Vector3 direction, [RpcPayload(4)] ForceMode forceMode)
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				int bytePayloadSize = Fusion.RpcDataWriter.GetBytePayloadSize(4);
				bytePayloadSize += Fusion.RpcDataWriter.GetBytePayloadSize(12);
				bytePayloadSize += Fusion.RpcDataWriter.GetBytePayloadSize(4);
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(91470905u, bytePayloadSize, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var writer);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.DamageableTrackModule.Scripts.PlayerDamageable::AddForceRPC(System.Single,UnityEngine.Vector3,UnityEngine.ForceMode)", invokeInfo, PlayerRef.None);
					}
					if (invokeInfo.SendMessageResult == RpcSendMessageResult.Sent)
					{
						writer.Write(force, 4);
						writer.Write(direction, 12);
						writer.Write(forceMode, 4);
						rpcBuilder.Send();
					}
				}
				NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromLocalCall((object)this, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), invokeInfo, RpcHostMode.SourceIsServer, PlayerRef.Invalid));
				if (invokeInfo.LocalInvokeResult != RpcLocalInvokeResult.Invoked)
				{
					return;
				}
			}
			if (!_playerMovableModel.AllMovablePhysics.TryGetValue(base.Object.InputAuthority, out var value))
			{
				Debug.LogWarning($"[ThrowSADebug] AddForceRPC NO physics map p{base.Object.InputAuthority.PlayerId} " + $"local={base.Runner.LocalPlayer.PlayerId} hasSA={base.Object.HasStateAuthority}");
				return;
			}
			Rigidbody rootRb = value.RootRb;
			string arg = ((rootRb != null) ? rootRb.name : "null");
			Vector3 vector = direction * force;
			Vector3 vector2 = ((rootRb != null) ? rootRb.linearVelocity : Vector3.zero);
			rootRb?.AddForce(vector, forceMode);
			Vector3 vector3 = ((rootRb != null) ? rootRb.linearVelocity : Vector3.zero);
			Debug.Log($"[ThrowSADebug] AddForceRPC local={base.Runner.LocalPlayer.PlayerId} " + $"input={base.Object.InputAuthority.PlayerId} hasSA={base.Object.HasStateAuthority} " + $"stateAuth={base.Object.StateAuthority.PlayerId} rb={arg} " + $"kin={rootRb != null && rootRb.isKinematic} force={force:0.#} mode={forceMode} " + $"impulse={vector} speedSameFrame {vector2.magnitude:0.##}->{vector3.magnitude:0.##}");
			if (rootRb != null && base.Object.HasStateAuthority)
			{
				StartCoroutine(LogRootRbAfterPhysics(rootRb, base.Object.InputAuthority.PlayerId, vector));
			}
		}

		private IEnumerator LogRootRbAfterPhysics(Rigidbody rootRb, int inputPlayerId, Vector3 impulse)
		{
			yield return new WaitForFixedUpdate();
			if (!(rootRb == null))
			{
				Debug.Log($"[ThrowSADebug] AddForceAfterFixedUpdate p{inputPlayerId} local={base.Runner.LocalPlayer.PlayerId} " + $"hasSA={base.Object.HasStateAuthority} rb={rootRb.name} kin={rootRb.isKinematic} " + $"speed={rootRb.linearVelocity.magnitude:0.##} vel={rootRb.linearVelocity} impulseWas={impulse}");
				yield return new WaitForFixedUpdate();
				if (!(rootRb == null))
				{
					Debug.Log($"[ThrowSADebug] AddForceAfterFixedUpdate2 p{inputPlayerId} local={base.Runner.LocalPlayer.PlayerId} " + $"hasSA={base.Object.HasStateAuthority} rb={rootRb.name} kin={rootRb.isKinematic} " + $"speed={rootRb.linearVelocity.magnitude:0.##} vel={rootRb.linearVelocity}");
				}
			}
		}

		[Rpc(RpcSources.All, RpcTargets.All, InvokeLocal = false, Key = 4109982958u)]
		private void NotifyDamagedFeedbackRpc([RpcPayload(4)] float damage, [RpcPayload(12)] Vector3 position, [RpcPayload(12)] Vector3 direction, [RpcPayload(4)] int dealerPlayerId, [RpcPayload(4)] float force, [RpcPayload(4)] ForceMode forceMode, [RpcPayload(4)] bool isStunning)
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				int bytePayloadSize = Fusion.RpcDataWriter.GetBytePayloadSize(4);
				bytePayloadSize += Fusion.RpcDataWriter.GetBytePayloadSize(12);
				bytePayloadSize += Fusion.RpcDataWriter.GetBytePayloadSize(12);
				bytePayloadSize += Fusion.RpcDataWriter.GetBytePayloadSize(4);
				bytePayloadSize += Fusion.RpcDataWriter.GetBytePayloadSize(4);
				bytePayloadSize += Fusion.RpcDataWriter.GetBytePayloadSize(4);
				bytePayloadSize += Fusion.RpcDataWriter.GetPayloadSize(isStunning);
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(4109982958u, bytePayloadSize, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var writer);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.DamageableTrackModule.Scripts.PlayerDamageable::NotifyDamagedFeedbackRpc(System.Single,UnityEngine.Vector3,UnityEngine.Vector3,System.Int32,System.Single,UnityEngine.ForceMode,System.Boolean)", invokeInfo, PlayerRef.None);
					}
					if (invokeInfo.SendMessageResult == RpcSendMessageResult.Sent)
					{
						writer.Write(damage, 4);
						writer.Write(position, 12);
						writer.Write(direction, 12);
						writer.Write(dealerPlayerId, 4);
						writer.Write(force, 4);
						writer.Write(forceMode, 4);
						writer.Write(isStunning);
						rpcBuilder.Send();
					}
				}
				NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromLocalCall((object)this, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), invokeInfo, RpcHostMode.SourceIsServer, PlayerRef.Invalid));
				if (invokeInfo.LocalInvokeResult != RpcLocalInvokeResult.Invoked)
				{
					return;
				}
			}
			this.OnDamaged?.Invoke(new DamageData
			{
				Damage = damage,
				Position = position,
				Direction = direction,
				DamageDealerPlayerID = dealerPlayerId,
				Force = force,
				ForceMode = forceMode,
				IsStunning = isStunning
			});
		}

		public void SetDamageableTags(List<DamageableTag> entityDamageableTags)
		{
			DamageableTags = entityDamageableTags;
		}

		public void HealToFullValue()
		{
			ClearLastDamageSource();
			float fullValue = _healthStat.FullValue;
			_healthStat.OverrideValue(_healthStat.MaxValue);
			Debug.Log($"[HealthTrace] HealToFull p{((base.Object != null) ? base.Object.InputAuthority.PlayerId : (-1))} health {fullValue:0.#}->{_healthStat.FullValue:0.#}");
			_entityStatEntityNetworkedBase.SynchronizeStatValues(EntityStatType.Health, RPCType.InAllWays);
		}

		public void Heal(float value, bool isSynchronize = false)
		{
			ClearLastDamageSource();
			float fullValue = _healthStat.FullValue;
			_healthStat.AddValue(value);
			Debug.Log($"[HealthTrace] Heal p{((base.Object != null) ? base.Object.InputAuthority.PlayerId : (-1))} +{value:0.#} health {fullValue:0.#}->{_healthStat.FullValue:0.#} sync={isSynchronize}");
			this.OnHealed?.Invoke(value);
			if (isSynchronize)
			{
				_entityStatEntityNetworkedBase.SynchronizeStatValues(EntityStatType.Health, RPCType.InAllWays);
			}
		}

		public void Damage(DamageData damageData)
		{
			if (base.Object == null || !base.Object.IsValid || _entityStatEntityNetworkedBase.GetStat(EntityStatType.Health).FullValue <= 0f || _entityStatEntityNetworkedBase.GetStat(EntityStatType.Invisibility).FullValue > 0f || _playerDamageAbsorbersModel.TryAbsorb(base.Object.InputAuthority.PlayerId, damageData))
			{
				return;
			}
			NotifyDamagedFeedbackRpc(damageData.Damage, damageData.Position, damageData.Direction, damageData.DamageDealerPlayerID, damageData.Force, damageData.ForceMode, damageData.IsStunning);
			if (damageData.IsStunning)
			{
				PlayerRef inputAuthority = base.Object.InputAuthority;
				bool flag = inputAuthority == base.Runner.LocalPlayer;
				Debug.Log($"[ThrowSADebug] PlayerDamageable.Damage IsStunning p{inputAuthority.PlayerId} " + $"callerLocal={base.Runner.LocalPlayer.PlayerId} hasSA={base.Object.HasStateAuthority} " + $"stateAuth={base.Object.StateAuthority.PlayerId} enterThrowLocal={flag} " + $"force={damageData.Force:0.#} dir={damageData.Direction}");
				if (inputAuthority != PlayerRef.None)
				{
					if (flag)
					{
						ThrowTargetPlayer(damageData.StunDurationPreset);
					}
					else
					{
						ThrowTargetPlayerRPC(inputAuthority, (int)damageData.StunDurationPreset);
					}
				}
			}
			_lastDamageSource = damageData.Source ?? LegacyFallback(damageData);
			this.OnDamaged?.Invoke(damageData);
			float fullValue = _healthStat.FullValue;
			_healthStat.Subtract(damageData.Damage);
			DamageSource value = _lastDamageSource.Value;
			Debug.Log($"[HealthTrace] Damage p{base.Object.InputAuthority.PlayerId} amount={damageData.Damage:0.#} src={value.Category}/{value.Type} dealer={damageData.DamageDealerPlayerID} health {fullValue:0.#}->{_healthStat.FullValue:0.#}");
			_entityStatEntityNetworkedBase.SynchronizeStatValues(EntityStatType.Health, RPCType.InAllWays);
			AddRPCForce(damageData.Force, damageData.Direction, damageData.ForceMode);
		}

		[Rpc(RpcSources.All, RpcTargets.All, InvokeLocal = false, Key = 1965170029u)]
		private void ThrowTargetPlayerRPC([RpcTarget] PlayerRef player, [RpcPayload(4)] int stunDurationPreset)
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				int bytePayloadSize = Fusion.RpcDataWriter.GetBytePayloadSize(4);
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(1965170029u, bytePayloadSize, (NetworkBehaviour)this, player))
				{
					invokeInfo = rpcBuilder.Prepare(out var writer);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.DamageableTrackModule.Scripts.PlayerDamageable::ThrowTargetPlayerRPC(Fusion.PlayerRef,System.Int32)", invokeInfo, player);
					}
					if (invokeInfo.SendMessageResult == RpcSendMessageResult.Sent)
					{
						writer.Write(stunDurationPreset, 4);
						rpcBuilder.Send();
					}
				}
				NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromLocalCall((object)this, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), invokeInfo, RpcHostMode.SourceIsServer, player));
				if (invokeInfo.LocalInvokeResult != RpcLocalInvokeResult.Invoked)
				{
					return;
				}
			}
			ThrowTargetPlayer((StunDurationPreset)stunDurationPreset);
		}

		private void ThrowTargetPlayer(StunDurationPreset stunDurationPreset)
		{
			Debug.Log($"[ThrowSADebug] EnterThrow local={base.Runner.LocalPlayer.PlayerId} " + $"input={base.Object.InputAuthority.PlayerId} hasSA={base.Object.HasStateAuthority} " + $"stateAuth={base.Object.StateAuthority.PlayerId} preset={stunDurationPreset}");
			_localPlayerThrowService.EnterThrow(stunDurationPreset);
		}

		public bool TryGetLastDamageSource(out DamageSource source)
		{
			if (!_lastDamageSource.HasValue)
			{
				source = default(DamageSource);
				return false;
			}
			source = _lastDamageSource.Value;
			return true;
		}

		public void ClearLastDamageSource()
		{
			_lastDamageSource = null;
		}

		private static DamageSource LegacyFallback(DamageData damageData)
		{
			if (damageData.DamageDealerPlayerID != 0)
			{
				return DamageDataSourceExtensions.ForPlayerAttack(DamageType.Unknown);
			}
			return DamageDataSourceExtensions.ForUnknown();
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
		}

		[NetworkRpcWeavedInvoker(2950992230u)]
		[Preserve]
		[WeaverGenerated]
		protected static void DamageRPC_0040Invoker2950992230([In] ref RpcInvokeContext context)
		{
			context.PayloadReader.Read(out float value, 4);
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((PlayerDamageable)context.TargetBehaviour).DamageRPC(value);
		}

		[NetworkRpcWeavedInvoker(3275043975u)]
		[Preserve]
		[WeaverGenerated]
		protected static void DamageRPC_0040Invoker3275043975([In] ref RpcInvokeContext context)
		{
			RpcDataReader payloadReader = context.PayloadReader;
			payloadReader.Read(out float value, 4);
			payloadReader.Read(out int value2, 4);
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((PlayerDamageable)context.TargetBehaviour).DamageRPC(value, value2);
		}

		[NetworkRpcWeavedInvoker(2255433589u)]
		[Preserve]
		[WeaverGenerated]
		protected static void DamageRPC_0040Invoker2255433589([In] ref RpcInvokeContext context)
		{
			RpcDataReader payloadReader = context.PayloadReader;
			payloadReader.Read(out float value, 4);
			payloadReader.Read(out int value2, 4);
			payloadReader.Read(out DamageRpcSource value3, 208);
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((PlayerDamageable)context.TargetBehaviour).DamageRPC(value, value2, value3);
		}

		[NetworkRpcWeavedInvoker(91470905u)]
		[Preserve]
		[WeaverGenerated]
		protected static void AddForceRPC_0040Invoker91470905([In] ref RpcInvokeContext context)
		{
			RpcDataReader payloadReader = context.PayloadReader;
			payloadReader.Read(out float value, 4);
			payloadReader.Read(out Vector3 value2, 12);
			payloadReader.Read(out ForceMode value3, 4);
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((PlayerDamageable)context.TargetBehaviour).AddForceRPC(value, value2, value3);
		}

		[NetworkRpcWeavedInvoker(4109982958u)]
		[Preserve]
		[WeaverGenerated]
		protected static void NotifyDamagedFeedbackRpc_0040Invoker4109982958([In] ref RpcInvokeContext context)
		{
			RpcDataReader payloadReader = context.PayloadReader;
			payloadReader.Read(out float value, 4);
			payloadReader.Read(out Vector3 value2, 12);
			payloadReader.Read(out Vector3 value3, 12);
			payloadReader.Read(out int value4, 4);
			payloadReader.Read(out float value5, 4);
			payloadReader.Read(out ForceMode value6, 4);
			payloadReader.Read(out bool value7);
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((PlayerDamageable)context.TargetBehaviour).NotifyDamagedFeedbackRpc(value, value2, value3, value4, value5, value6, value7);
		}

		[NetworkRpcWeavedInvoker(1965170029u)]
		[Preserve]
		[WeaverGenerated]
		protected static void ThrowTargetPlayerRPC_0040Invoker1965170029([In] ref RpcInvokeContext context)
		{
			PlayerRef targetPlayer = context.TargetPlayer;
			context.PayloadReader.Read(out int value, 4);
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((PlayerDamageable)context.TargetBehaviour).ThrowTargetPlayerRPC(targetPlayer, value);
		}
	}
}
