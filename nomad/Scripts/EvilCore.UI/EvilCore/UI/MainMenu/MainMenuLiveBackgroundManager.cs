using EvilCore.Networking;
using UnityEngine;
using VContainer;

namespace EvilCore.UI.MainMenu
{
	public class MainMenuLiveBackgroundManager : MonoBehaviour
	{
		[SerializeField]
		private GameObject liveBackgroundCamera;

		[Inject]
		private IEOSLobbyManager _lobbyManager;

		private void OnEnable()
		{
			_lobbyManager.OnLobbyCreated += DisableBackground;
			_lobbyManager.OnLobbyJoined += DisableBackground;
			_lobbyManager.OnLobbyLeft += EnableBackground;
		}

		private void OnDisable()
		{
			if (_lobbyManager != null)
			{
				_lobbyManager.OnLobbyCreated -= DisableBackground;
				_lobbyManager.OnLobbyJoined -= DisableBackground;
				_lobbyManager.OnLobbyLeft -= EnableBackground;
			}
		}

		private void EnableBackground()
		{
			if (liveBackgroundCamera != null)
			{
				liveBackgroundCamera.SetActive(value: true);
			}
		}

		private void DisableBackground()
		{
			if (liveBackgroundCamera != null)
			{
				liveBackgroundCamera.SetActive(value: false);
			}
		}
	}
}
