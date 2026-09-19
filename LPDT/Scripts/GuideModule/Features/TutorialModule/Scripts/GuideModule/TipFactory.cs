using Zenject;

namespace Features.TutorialModule.Scripts.GuideModule
{
	public class TipFactory : ITipFactory
	{
		private readonly DiContainer _diContainer;

		private readonly TipConfiguration _configuration;

		public TipFactory(DiContainer diContainer, TipConfiguration configuration)
		{
			_diContainer = diContainer;
			_configuration = configuration;
		}

		public ITipEntity Create(TipType tipType)
		{
			_configuration.Prefabs.TryGetValue(tipType, out var value);
			return _diContainer.InstantiatePrefabForComponent<TipEntity>(value);
		}
	}
}
