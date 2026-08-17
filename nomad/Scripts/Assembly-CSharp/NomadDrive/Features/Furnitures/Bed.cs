using EvilCore.UI.Scripts;
using NomadDrive.Features.Interaction;
using NomadDrive.Features.Player;
using UnityEngine;
using VContainer;

namespace NomadDrive.Features.Furnitures
{
	public class Bed : Interactable
	{
		public Transform sleepingTransform;

		public Transform exitTransform;

		public Camera sleepingCamera;

		private HoldInteraction _holdingInteraction;

		private IGameUIManager _guiManager;

		[Inject]
		private IPlayerService _playerReferenceService;

		[Inject]
		private void Construct(IGameUIManager guiManager)
		{
			_guiManager = guiManager;
			_holdingInteraction = GetComponent<HoldInteraction>();
			_holdingInteraction.OnInteractionCompleted.AddListener(OnInteract);
		}

		private void OnInteract()
		{
			OpenSleepingHUD();
		}

		private void OpenSleepingHUD()
		{
		}

		public override bool Weaved()
		{
			return true;
		}
	}
}
