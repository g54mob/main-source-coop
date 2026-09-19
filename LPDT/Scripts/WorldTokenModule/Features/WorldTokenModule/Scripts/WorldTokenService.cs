using Features.ViewSystemModule.Scripts.Windows;
using Features.WorldTokenModule.Scripts.Views;
using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;
using UnityEngine;
using Zenject;

namespace Features.WorldTokenModule.Scripts
{
	public class WorldTokenService : IWorldTokenService
	{
		private readonly WorldTokenConfiguration _worldTokenConfiguration;

		private readonly DiContainer _container;

		private readonly WorldCanvasWindow _worldCanvasWindow;

		public bool IsReady => _worldCanvasWindow.WindowStatus != WindowStatus.Closed;

		public WorldTokenService(WorldTokenConfiguration worldTokenConfiguration, DiContainer container, WorldCanvasWindow worldCanvasWindow)
		{
			_worldTokenConfiguration = worldTokenConfiguration;
			_container = container;
			_worldCanvasWindow = worldCanvasWindow;
		}

		public WorldTokenPresenter CreateWorldToken(WorldTokenType worldTokenType, Vector3 position)
		{
			GameObject gameObject = _container.InstantiatePrefab(_worldTokenConfiguration.WorldTokenByType[worldTokenType]);
			_worldCanvasWindow.AddView(gameObject.transform, worldPositionStays: false);
			gameObject.transform.position = position;
			return _worldCanvasWindow.GetPresenterForView<WorldTokenPresenter>(gameObject.GetComponent<WorldTokenViewBase>());
		}

		public BigButtWorldTokenPresenter CreateBigButtWWorldToken(WorldTokenType worldTokenType, Vector3 position)
		{
			GameObject gameObject = _container.InstantiatePrefab(_worldTokenConfiguration.BigButtWorldTokenByType[worldTokenType]);
			_worldCanvasWindow.AddView(gameObject.transform, worldPositionStays: false);
			gameObject.transform.position = position;
			return _worldCanvasWindow.GetPresenterForView<BigButtWorldTokenPresenter>(gameObject.GetComponent<BigButtWorldTokenViewBase>());
		}
	}
}
