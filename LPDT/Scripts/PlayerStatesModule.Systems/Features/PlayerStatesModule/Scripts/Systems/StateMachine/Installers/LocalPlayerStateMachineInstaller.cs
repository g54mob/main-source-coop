using Features.PlayerStatesModule.Scripts.Systems.StateMachine.States;
using Zenject;

namespace Features.PlayerStatesModule.Scripts.Systems.StateMachine.Installers
{
	public class LocalPlayerStateMachineInstaller : Installer<LocalPlayerStateMachineInstaller>
	{
		public override void InstallBindings()
		{
			base.Container.Bind<LocalPlayerStateMachine>().AsSingle();
			BindLocalPlayerState<LocalPlayerAliveState>();
			BindLocalPlayerState<LocalPlayerDeadState>();
			BindLocalPlayerState<LocalPlayerPreDeadCrouchState>();
			BindLocalPlayerState<LocalPlayerFreeFlyState>();
			BindLocalPlayerState<LocalPlayerStoreState>();
			BindLocalPlayerState<LocalPlayerDisconnectedState>();
			base.Container.BindInterfacesTo<LocalPlayerStateMachineDispatcherSystem>().AsSingle();
		}

		private void BindLocalPlayerState<TState>() where TState : LocalPlayerStateBase
		{
			base.Container.Bind<TState>().AsSingle();
			base.Container.Bind<LocalPlayerStateBase>().To<TState>().FromResolve();
		}
	}
}
