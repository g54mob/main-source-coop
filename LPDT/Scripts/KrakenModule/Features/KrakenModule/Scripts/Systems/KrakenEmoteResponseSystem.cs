using System;
using Features.EmotesModule.Scripts;
using Features.KrakenModule.Scripts.Data;
using Zenject;

namespace Features.KrakenModule.Scripts.Systems
{
	public class KrakenEmoteResponseSystem : IInitializable, IDisposable
	{
		private readonly PlayerBodyEmoteNetworkEvent _playerBodyEmoteNetworkEvent;

		private readonly KrakenRuntimeModel _runtimeModel;

		public KrakenEmoteResponseSystem(PlayerBodyEmoteNetworkEvent playerBodyEmoteNetworkEvent, KrakenRuntimeModel runtimeModel)
		{
			_playerBodyEmoteNetworkEvent = playerBodyEmoteNetworkEvent;
			_runtimeModel = runtimeModel;
		}

		public void Initialize()
		{
			_playerBodyEmoteNetworkEvent.OnNetworkEventSend += OnPlayerBodyEmote;
		}

		public void Dispose()
		{
			_playerBodyEmoteNetworkEvent.OnNetworkEventSend -= OnPlayerBodyEmote;
		}

		private void OnPlayerBodyEmote(PlayerBodyEmoteNetworkEvent playerBodyEmoteNetworkEvent)
		{
			if (_runtimeModel.TryGetController(out var controller))
			{
				controller.ProcessPlayerBodyEmote(playerBodyEmoteNetworkEvent);
			}
		}
	}
}
