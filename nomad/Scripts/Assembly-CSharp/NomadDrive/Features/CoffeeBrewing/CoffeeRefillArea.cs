using System;
using System.Runtime.InteropServices;
using Ami.BroAudio;
using EvilCore.UI.Scripts;
using Mirror;
using Mirror.RemoteCalls;
using NomadDrive.Features.Interaction;
using NomadDrive.Features.Player;
using PrimeTween;
using UnityEngine;
using VContainer;

namespace NomadDrive.Features.CoffeeBrewing
{
	public class CoffeeRefillArea : Interactable
	{
		[SerializeField]
		private Transform lidTransform;

		[SerializeField]
		private SkinnedMeshRenderer coffeeMeshRenderer;

		[SerializeField]
		private float refillDuration = 1f;

		[SyncVar(hook = "OnCoffeeAmountChanged")]
		private float _coffeeAmount;

		[SerializeField]
		private float _lidRotationDuration = 1f;

		[Header("Audio")]
		[SerializeField]
		private SoundID lidOpenSound;

		[SerializeField]
		private SoundID lidCloseSound;

		private CoffeePackage _hoveredCoffeePackage;

		private Sequence _currentLidSequence;

		private Sequence _currentFillSequence;

		[Inject]
		private IPlayerService _playerReferenceService;

		private InteractionStateMachine<CoffeeRefillState> _stateMachine;

		public Action<float, float> _Mirror_SyncVarHookDelegate__coffeeAmount;

		public float CoffeeAmount
		{
			get
			{
				return _coffeeAmount;
			}
			private set
			{
				if (base.isServer)
				{
					Network_coffeeAmount = Mathf.Clamp(value, 0f, CoffeeCapacity);
				}
			}
		}

		[field: SerializeField]
		public float CoffeeCapacity { get; private set; } = 0.8f;

		protected override bool UseStateMachine => true;

		public float Network_coffeeAmount
		{
			get
			{
				return _coffeeAmount;
			}
			[param: In]
			set
			{
				GeneratedSyncVarSetter(value, ref _coffeeAmount, 64uL, _Mirror_SyncVarHookDelegate__coffeeAmount);
			}
		}

		protected override void InitializeStateMachine()
		{
			_stateMachine = new InteractionStateMachine<CoffeeRefillState>(this);
			base.BaseStateMachine = _stateMachine;
			ConfigureStates();
			_stateMachine.Initialize(DetermineState());
		}

		protected override void ConfigureStates()
		{
			_stateMachine.RegisterState(CoffeeRefillState.Idle, (InteractionStateConfig config) => config.WithCrosshair(CrosshairType.Interact).WithNameLabelVisibility(visible: true).WithInteractionLabelVisibility(visible: false)).RegisterState(CoffeeRefillState.Refillable, (InteractionStateConfig config) => config.WithHoldInteraction(InteractionKey.Primary, "@interaction.refill", RefillCoffee, refillDuration).WithCrosshair(CrosshairType.Interact).WithNameLabelVisibility(visible: false)
				.WithInteractionLabelVisibility(visible: true));
		}

		private CoffeeRefillState DetermineState()
		{
			IEquipmentManager equipmentManager = _playerReferenceService.EquipmentManager;
			if (equipmentManager == null)
			{
				return CoffeeRefillState.Idle;
			}
			if (!(equipmentManager.EquippedEntity != null))
			{
				return CoffeeRefillState.Idle;
			}
			Component component;
			bool num = equipmentManager.EquippedEntity.TryGetComponent(typeof(CoffeePackage), out component);
			bool flag = CoffeeAmount < CoffeeCapacity;
			if (num && flag)
			{
				_hoveredCoffeePackage = equipmentManager.EquippedEntity.GetComponent<CoffeePackage>();
				return CoffeeRefillState.Refillable;
			}
			_hoveredCoffeePackage = null;
			return CoffeeRefillState.Idle;
		}

		public override void UpdateState()
		{
			_stateMachine?.TransitionTo(DetermineState());
		}

		public override void OnStartServer()
		{
			base.OnStartServer();
			InitializeServerState();
		}

		public override void OnStartClient()
		{
			base.OnStartClient();
			InitializeClientState();
		}

		private void OnDestroy()
		{
			CleanupSequences();
		}

		private void InitializeServerState()
		{
			Network_coffeeAmount = 0f;
		}

		private void InitializeClientState()
		{
			UpdateVisuals(_coffeeAmount);
		}

		protected override void OnHovered()
		{
			base.OnHovered();
			UpdateState();
			CmdRotateLid(-100f);
		}

		protected override void OnUnhovered()
		{
			base.OnUnhovered();
			UpdateState();
			CmdRotateLid(0f);
		}

		private void OnCoffeeAmountChanged(float oldAmount, float newAmount)
		{
			if (IsLateJoinCompleted)
			{
				AnimateCoffeeLevel(oldAmount, newAmount);
			}
		}

		private void UpdateVisuals(float amount)
		{
			float value = 100f - amount / CoffeeCapacity * 100f;
			coffeeMeshRenderer.SetBlendShapeWeight(0, value);
		}

		private void AnimateCoffeeLevel(float oldAmount, float newAmount)
		{
			_ = oldAmount / CoffeeCapacity;
			float num = newAmount / CoffeeCapacity;
			CleanupFillSequence();
			DeactivateAllInteractions();
			float endValue = 100f - num * 100f;
			float duration = ((newAmount > oldAmount) ? 1f : 2.5f);
			_currentFillSequence = Sequence.Create().Chain(Tween.Custom(coffeeMeshRenderer, coffeeMeshRenderer.GetBlendShapeWeight(0), endValue, duration, delegate(SkinnedMeshRenderer target, float val)
			{
				target.SetBlendShapeWeight(0, val);
			}, Ease.Linear)).ChainCallback(this, delegate(CoffeeRefillArea target)
			{
				target.ActivateAllInteractions();
			});
		}

		[Command(requiresAuthority = false)]
		private void CmdRotateLid(float angle)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			writer.WriteFloat(angle);
			SendCommandInternal("System.Void NomadDrive.Features.CoffeeBrewing.CoffeeRefillArea::CmdRotateLid(System.Single)", -2127508703, writer, 0, requiresAuthority: false);
			NetworkWriterPool.Return(writer);
		}

		[ClientRpc]
		private void RpcRotateLid(float angle)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			writer.WriteFloat(angle);
			SendRPCInternal("System.Void NomadDrive.Features.CoffeeBrewing.CoffeeRefillArea::RpcRotateLid(System.Single)", 933992466, writer, 0, includeOwner: true);
			NetworkWriterPool.Return(writer);
		}

		[Command(requiresAuthority = false)]
		private void CmdRefillCoffee()
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			SendCommandInternal("System.Void NomadDrive.Features.CoffeeBrewing.CoffeeRefillArea::CmdRefillCoffee()", 886596318, writer, 0, requiresAuthority: false);
			NetworkWriterPool.Return(writer);
		}

		public void EmptyCoffee(float amount)
		{
			CmdEmptyCoffee(amount);
		}

		[Command(requiresAuthority = false)]
		private void CmdEmptyCoffee(float amount)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			writer.WriteFloat(amount);
			SendCommandInternal("System.Void NomadDrive.Features.CoffeeBrewing.CoffeeRefillArea::CmdEmptyCoffee(System.Single)", -1685738318, writer, 0, requiresAuthority: false);
			NetworkWriterPool.Return(writer);
		}

		private void CleanupSequences()
		{
			CleanupLidSequence();
			CleanupFillSequence();
		}

		private void CleanupLidSequence()
		{
			_currentLidSequence.Stop();
		}

		private void CleanupFillSequence()
		{
			_currentFillSequence.Stop();
		}

		private void RefillCoffee()
		{
			CmdRefillCoffee();
		}

		public CoffeeRefillArea()
		{
			_Mirror_SyncVarHookDelegate__coffeeAmount = OnCoffeeAmountChanged;
		}

		public override bool Weaved()
		{
			return true;
		}

		protected void UserCode_CmdRotateLid__Single(float angle)
		{
			RpcRotateLid(angle);
		}

		protected static void InvokeUserCode_CmdRotateLid__Single(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdRotateLid called on client.");
			}
			else
			{
				((CoffeeRefillArea)obj).UserCode_CmdRotateLid__Single(reader.ReadFloat());
			}
		}

		protected void UserCode_RpcRotateLid__Single(float angle)
		{
			if (base.isClient)
			{
				CleanupLidSequence();
				Vector3 euler = new Vector3(0f, 0f, angle);
				Ease ease = ((angle == 0f) ? Ease.Linear : Ease.OutBounce);
				_currentLidSequence = Sequence.Create().Chain(Tween.LocalRotation(lidTransform, Quaternion.Euler(euler), _lidRotationDuration, ease));
				AudioManager?.PlayOneShotAttached((angle == 0f) ? lidCloseSound : lidOpenSound, base.gameObject);
			}
		}

		protected static void InvokeUserCode_RpcRotateLid__Single(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkClient.active)
			{
				Debug.LogError("RPC RpcRotateLid called on server.");
			}
			else
			{
				((CoffeeRefillArea)obj).UserCode_RpcRotateLid__Single(reader.ReadFloat());
			}
		}

		protected void UserCode_CmdRefillCoffee()
		{
			CoffeeAmount = CoffeeCapacity;
			if (_hoveredCoffeePackage != null)
			{
				NetworkServer.Destroy(_hoveredCoffeePackage.gameObject);
			}
		}

		protected static void InvokeUserCode_CmdRefillCoffee(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdRefillCoffee called on client.");
			}
			else
			{
				((CoffeeRefillArea)obj).UserCode_CmdRefillCoffee();
			}
		}

		protected void UserCode_CmdEmptyCoffee__Single(float amount)
		{
			if (amount > _coffeeAmount)
			{
				amount = _coffeeAmount;
			}
			Network_coffeeAmount = _coffeeAmount - amount;
		}

		protected static void InvokeUserCode_CmdEmptyCoffee__Single(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdEmptyCoffee called on client.");
			}
			else
			{
				((CoffeeRefillArea)obj).UserCode_CmdEmptyCoffee__Single(reader.ReadFloat());
			}
		}

		static CoffeeRefillArea()
		{
			RemoteProcedureCalls.RegisterCommand(typeof(CoffeeRefillArea), "System.Void NomadDrive.Features.CoffeeBrewing.CoffeeRefillArea::CmdRotateLid(System.Single)", InvokeUserCode_CmdRotateLid__Single, requiresAuthority: false);
			RemoteProcedureCalls.RegisterCommand(typeof(CoffeeRefillArea), "System.Void NomadDrive.Features.CoffeeBrewing.CoffeeRefillArea::CmdRefillCoffee()", InvokeUserCode_CmdRefillCoffee, requiresAuthority: false);
			RemoteProcedureCalls.RegisterCommand(typeof(CoffeeRefillArea), "System.Void NomadDrive.Features.CoffeeBrewing.CoffeeRefillArea::CmdEmptyCoffee(System.Single)", InvokeUserCode_CmdEmptyCoffee__Single, requiresAuthority: false);
			RemoteProcedureCalls.RegisterRpc(typeof(CoffeeRefillArea), "System.Void NomadDrive.Features.CoffeeBrewing.CoffeeRefillArea::RpcRotateLid(System.Single)", InvokeUserCode_RpcRotateLid__Single);
		}

		public override void SerializeSyncVars(NetworkWriter writer, bool forceAll)
		{
			base.SerializeSyncVars(writer, forceAll);
			if (forceAll)
			{
				writer.WriteFloat(_coffeeAmount);
				return;
			}
			writer.WriteVarULong(syncVarDirtyBits);
			if ((syncVarDirtyBits & 0x40L) != 0L)
			{
				writer.WriteFloat(_coffeeAmount);
			}
		}

		public override void DeserializeSyncVars(NetworkReader reader, bool initialState)
		{
			base.DeserializeSyncVars(reader, initialState);
			if (initialState)
			{
				GeneratedSyncVarDeserialize(ref _coffeeAmount, _Mirror_SyncVarHookDelegate__coffeeAmount, reader.ReadFloat());
				return;
			}
			long num = (long)reader.ReadVarULong();
			if ((num & 0x40L) != 0L)
			{
				GeneratedSyncVarDeserialize(ref _coffeeAmount, _Mirror_SyncVarHookDelegate__coffeeAmount, reader.ReadFloat());
			}
		}
	}
}
