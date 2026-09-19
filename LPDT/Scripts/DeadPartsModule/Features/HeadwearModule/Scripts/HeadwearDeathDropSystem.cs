using System;
using GameplayEvents;
using Zenject;

namespace Features.HeadwearModule.Scripts
{
	public class HeadwearDeathDropSystem : IInitializable, IDisposable
	{
		private readonly GameplayEventBus _gameplayEventBus;

		private readonly HeadwearModel _headwearModel;

		public HeadwearDeathDropSystem(GameplayEventBus gameplayEventBus, HeadwearModel headwearModel)
		{
			_gameplayEventBus = gameplayEventBus;
			_headwearModel = headwearModel;
		}

		public void Initialize()
		{
			_gameplayEventBus.Subscribe<OnPlayerDiedGameplayEvent>(HandlePlayerDied);
		}

		public void Dispose()
		{
			_gameplayEventBus.Unsubscribe<OnPlayerDiedGameplayEvent>(HandlePlayerDied);
		}

		private void HandlePlayerDied(OnPlayerDiedGameplayEvent playerDiedEvent)
		{
			if (_headwearModel.TryGetHeadwear(playerDiedEvent.PlayerId, out var headwear) && !(headwear == null) && !(headwear.Object == null) && headwear.Object.IsValid)
			{
				headwear.DropOnWearerLeft();
			}
		}
	}
}
