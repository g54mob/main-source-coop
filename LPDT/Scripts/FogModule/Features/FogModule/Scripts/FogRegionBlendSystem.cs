using System;
using Features.GameUpdaterModule;
using Features.Movement.Scripts;
using Zenject;

namespace Features.FogModule.Scripts
{
	public class FogRegionBlendSystem : IInitializable, IDisposable
	{
		private readonly IGameUpdater _gameUpdater;

		private readonly PlayerMovableModel _playerMovableModel;

		private readonly FogRegionsModel _fogRegionsModel;

		private readonly FogRegionResolverService _fogRegionResolverService;

		public FogRegionBlendSystem(IGameUpdater gameUpdater, PlayerMovableModel playerMovableModel, FogRegionsModel fogRegionsModel, FogRegionResolverService fogRegionResolverService)
		{
			_gameUpdater = gameUpdater;
			_playerMovableModel = playerMovableModel;
			_fogRegionsModel = fogRegionsModel;
			_fogRegionResolverService = fogRegionResolverService;
		}

		public void Initialize()
		{
			_gameUpdater.OnUpdate += OnUpdate;
		}

		public void Dispose()
		{
			_gameUpdater.OnUpdate -= OnUpdate;
		}

		private void OnUpdate()
		{
			if (!(_playerMovableModel.LocalMovable == null) && _fogRegionResolverService.TryResolve(_fogRegionsModel.FogRegions, _playerMovableModel.LocalMovable.GetPosition(), out var fogBlend))
			{
				_fogRegionsModel.SetCurrentBlend(fogBlend);
			}
		}
	}
}
