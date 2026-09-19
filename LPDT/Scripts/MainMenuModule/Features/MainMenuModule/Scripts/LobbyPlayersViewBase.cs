using Features.PlayerItemViewModule.Scripts;
using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;
using UnityEngine;

namespace Features.MainMenuModule.Scripts
{
	public abstract class LobbyPlayersViewBase : ViewBehaviour
	{
		[SerializeField]
		private Transform _container;

		[field: SerializeField]
		public PlayerItemViewBase PlayerItemViewBase { get; private set; }

		public Transform GetPlayerItemsContainer()
		{
			return _container;
		}
	}
}
