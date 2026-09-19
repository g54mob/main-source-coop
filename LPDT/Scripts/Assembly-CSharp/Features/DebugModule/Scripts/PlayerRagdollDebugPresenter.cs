using Features.MultiplayerSessionServices.Scripts;
using Features.RagdollModule.Scripts;
using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;

namespace Features.DebugModule.Scripts
{
	public class PlayerRagdollDebugPresenter : PresenterBehaviour<PlayerRagdollDebugViewBase>
	{
		private readonly MultiplayerModel _multiplayerModel;

		private readonly PlayersRagdollModel _playersRagdollModel;

		public PlayerRagdollDebugPresenter(MultiplayerModel multiplayerModel, PlayersRagdollModel playersRagdollModel)
		{
			_multiplayerModel = multiplayerModel;
			_playersRagdollModel = playersRagdollModel;
		}

		protected override void OnViewEnabled()
		{
			base.OnViewEnabled();
			base.View.OnRagdollSimulationChanged += SwitchRagdollSimulation;
			base.View.RefreshToggle(_playersRagdollModel.PlayersRagdoll[_multiplayerModel.NetworkRunner.LocalPlayer.PlayerId].IsSimulated);
		}

		protected override void OnViewDisabled()
		{
			base.OnViewDisabled();
			base.View.OnRagdollSimulationChanged -= SwitchRagdollSimulation;
		}

		private void SwitchRagdollSimulation(bool isSimulated)
		{
			PlayerRagdollEntity playerRagdollEntity = _playersRagdollModel.PlayersRagdoll[_multiplayerModel.NetworkRunner.LocalPlayer.PlayerId];
			if (isSimulated != playerRagdollEntity.IsSimulated)
			{
				if (isSimulated)
				{
					playerRagdollEntity.AddSimulationReason(RagdollSimulationReasonEnum.Debug);
				}
				else
				{
					playerRagdollEntity.RemoveSimulationReason(RagdollSimulationReasonEnum.Debug);
				}
				base.View.RefreshToggle(isSimulated);
			}
		}
	}
}
