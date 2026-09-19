namespace Features.PlayerStatesModule.Scripts.Systems.StateMachine.States
{
	public class LocalPlayerDisconnectedState : LocalPlayerStateBase
	{
		public override PlayerState StateEnum => PlayerState.Disconnected;

		protected override void OnEnter()
		{
			SetCommonHudVisible(isVisible: false);
			PlayerMovableModel.LocalMovable?.DisableMovement();
			RequestMouseHidden();
		}

		protected override void OnExit()
		{
			PlayerMovableModel.LocalMovable?.EnableMovement();
		}
	}
}
