using Features.GrabModule.Scripts;
using Features.MultiplayerSessionServices.Scripts;
using Features.RagdollModule.Scripts;
using Fusion;
using UnityEngine;
using Zenject;

namespace Features.SwimmingModule.Scripts
{
	[NetworkBehaviourWeaved(0)]
	public class SwimmingPlayerController : NetworkBehaviour
	{
		[SerializeField]
		private SwimmingComponent _swimmingComponent;

		[SerializeField]
		private SimplePointGrabable _simplePointGrabable;

		[SerializeField]
		private float _timerBeforeGrabDisable = 1f;

		[SerializeField]
		private float _swimmingTimerValue = 2f;

		private bool _isSwimming;

		private float _timer;

		private float _swimmingTimer;

		private MultiplayerModel _multiplayerModel;

		private PlayersRagdollModel _playersRagdollModel;

		[Inject]
		private void InjectDependencies(MultiplayerModel multiplayerModel, PlayersRagdollModel playersRagdollModel)
		{
			_multiplayerModel = multiplayerModel;
			_playersRagdollModel = playersRagdollModel;
		}

		public override void Despawned(NetworkRunner runner, bool hasState)
		{
			base.Despawned(runner, hasState);
			RemoveLocalRagdollReason(RagdollSimulationReasonEnum.Swim);
		}

		private void Update()
		{
			if (!base.HasStateAuthority)
			{
				return;
			}
			if (_swimmingComponent.ShouldReset)
			{
				if (!_isSwimming)
				{
					return;
				}
				_timer += Time.deltaTime;
				if (!(_timer < _timerBeforeGrabDisable))
				{
					_timer = 0f;
					_isSwimming = false;
					if (base.Object.InputAuthority == base.Runner.LocalPlayer)
					{
						RemoveLocalRagdollReason(RagdollSimulationReasonEnum.Swim);
					}
				}
			}
			else if (_isSwimming)
			{
				_swimmingTimer += Time.deltaTime;
				if (_swimmingTimer >= _swimmingTimerValue)
				{
					if (base.Object.InputAuthority == base.Runner.LocalPlayer && !HasLocalRagdollReason(RagdollSimulationReasonEnum.Swim))
					{
						AddLocalRagdollReason(RagdollSimulationReasonEnum.Swim);
					}
					_swimmingTimer = 0f;
				}
			}
			else
			{
				_isSwimming = true;
				if (base.Object.InputAuthority == base.Runner.LocalPlayer)
				{
					AddLocalRagdollReason(RagdollSimulationReasonEnum.Swim);
				}
			}
		}

		private void AddLocalRagdollReason(RagdollSimulationReasonEnum reason)
		{
			if (_playersRagdollModel.TryGetPlayerRagdoll(_multiplayerModel.NetworkRunner.LocalPlayer.PlayerId, out var ragdoll))
			{
				ragdoll.AddSimulationReason(reason);
			}
		}

		private void RemoveLocalRagdollReason(RagdollSimulationReasonEnum reason)
		{
			if (_playersRagdollModel.TryGetPlayerRagdoll(_multiplayerModel.NetworkRunner.LocalPlayer.PlayerId, out var ragdoll))
			{
				ragdoll.RemoveSimulationReason(reason);
			}
		}

		private bool HasLocalRagdollReason(RagdollSimulationReasonEnum reason)
		{
			if (_playersRagdollModel.TryGetPlayerRagdoll(_multiplayerModel.NetworkRunner.LocalPlayer.PlayerId, out var ragdoll))
			{
				return ragdoll.HasSimulationReason(reason);
			}
			return false;
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
		}
	}
}
