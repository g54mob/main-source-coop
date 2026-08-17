using System;
using EvilCore;
using EvilCore.DI.Core;
using NomadDrive.Features.Player.Downed;
using NomadDrive.Features.Vehicle;
using VContainer;

namespace NomadDrive.Features.Player.DI
{
	[Serializable]
	public class PlayerBootstrapInstaller : MonoInstaller
	{
		public override void Install(IContainerBuilder builder)
		{
			builder.Register<PlayerService>(Lifetime.Singleton).As<IPlayerService>();
			builder.Register<PlayerRespawnService>(Lifetime.Singleton).As<IPlayerRespawnService>();
			builder.Register<VehicleRescueService>(Lifetime.Singleton).As<IVehicleRescueService>();
			builder.Register<GameOverService>(Lifetime.Singleton).As<IGameOverService>();
		}
	}
}
