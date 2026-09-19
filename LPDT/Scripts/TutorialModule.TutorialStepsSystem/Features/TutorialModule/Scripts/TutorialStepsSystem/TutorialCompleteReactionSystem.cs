using System;
using Features.DisconnectHandlerModule.Scripts.Data;
using Zenject;

namespace Features.TutorialModule.Scripts.TutorialStepsSystem
{
	public class TutorialCompleteReactionSystem : IInitializable, IDisposable
	{
		private readonly BaseTutorialService _baseTutorialService;

		private readonly DisconnectRequestEventClass _disconnectRequestEvent;

		public TutorialCompleteReactionSystem(BaseTutorialService baseTutorialService, DisconnectRequestEventClass disconnectRequestEvent)
		{
			_baseTutorialService = baseTutorialService;
			_disconnectRequestEvent = disconnectRequestEvent;
		}

		public void Initialize()
		{
			_baseTutorialService.OnTutorialEnded += ReturnPlayerToMenu;
		}

		public void Dispose()
		{
			_baseTutorialService.OnTutorialEnded -= ReturnPlayerToMenu;
		}

		private void ReturnPlayerToMenu()
		{
			_disconnectRequestEvent.Publish(DisconnectRequestReason.TutorialComplete);
		}
	}
}
