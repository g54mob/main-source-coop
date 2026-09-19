using Zenject;

namespace Features.TutorialModule.Scripts.GuideModule
{
	public class GuideLineFactory : IGuideLineFactory
	{
		private readonly DiContainer _diContainer;

		private readonly GuideLineConfiguration _configuration;

		public GuideLineFactory(DiContainer diContainer, GuideLineConfiguration configuration)
		{
			_diContainer = diContainer;
			_configuration = configuration;
		}

		public IGuideLineEntity Create(GuideLineBuildType buildType)
		{
			_configuration.Prefabs.TryGetValue(buildType, out var value);
			return _diContainer.InstantiatePrefabForComponent<PlaneGuideLineEntity>(value);
		}
	}
}
