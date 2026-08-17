using EvilCore;
using EvilCore.UI.Scripts;
using NomadDrive.Features.Player.Core;
using UnityEngine;
using VContainer;

namespace NomadDrive.Features.Inputs
{
	public class RespawnInputHandler : MonoBehaviour, IPlayerComponent
	{
		[Inject]
		private IPlayerRespawnService _respawnService;

		[Inject]
		private IGameUIManager _gameUIManager;

		public void SetupForPlayer(bool isLocalPlayer)
		{
			base.enabled = isLocalPlayer;
		}

		private void Update()
		{
		}
	}
}
