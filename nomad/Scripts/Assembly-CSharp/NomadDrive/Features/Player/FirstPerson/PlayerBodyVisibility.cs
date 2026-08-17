using EvilCore.Recording;
using NomadDrive.Features.Player.Core;
using NomadDrive.Features.Player.Cosmetics;
using UnityEngine;
using UnityEngine.Rendering;
using VContainer;

namespace NomadDrive.Features.Player.FirstPerson
{
	public class PlayerBodyVisibility : MonoBehaviour, IPlayerComponent
	{
		[Header("Head Renderers")]
		[Tooltip("Renderers that should be hidden for the local player (head, neck, hair, etc.)")]
		[SerializeField]
		private Renderer[] headRenderers;

		[Inject]
		private DirectorManager _directorManager;

		private PlayerCosmeticManager _cosmeticManager;

		private bool _isSubscribedToDirector;

		public int SetupPriority => 30;

		public void SetupForPlayer(bool isLocalPlayer)
		{
			if (isLocalPlayer)
			{
				_cosmeticManager = GetComponent<PlayerCosmeticManager>();
				SetHeadRenderersMode(ShadowCastingMode.ShadowsOnly);
				if (_directorManager != null)
				{
					_directorManager.OnDirectorModeEntered += HandleDirectorEntered;
					_directorManager.OnDirectorModeExited += HandleDirectorExited;
					_isSubscribedToDirector = true;
				}
			}
			base.enabled = false;
		}

		private void HandleDirectorEntered()
		{
			ShowAll();
		}

		private void HandleDirectorExited()
		{
			HideForFirstPerson();
		}

		private void OnDestroy()
		{
			if (_isSubscribedToDirector && _directorManager != null)
			{
				_directorManager.OnDirectorModeEntered -= HandleDirectorEntered;
				_directorManager.OnDirectorModeExited -= HandleDirectorExited;
				_isSubscribedToDirector = false;
			}
		}

		public void ShowAll()
		{
			SetHeadRenderersMode(ShadowCastingMode.On);
			if (_cosmeticManager != null)
			{
				_cosmeticManager.SetThirdPersonView(isThirdPerson: true);
				_cosmeticManager.RestoreLocalVisibility();
			}
		}

		public void HideForFirstPerson()
		{
			SetHeadRenderersMode(ShadowCastingMode.ShadowsOnly);
			if (_cosmeticManager != null)
			{
				_cosmeticManager.SetThirdPersonView(isThirdPerson: false);
				_cosmeticManager.ApplyLocalVisibilityToAllActive();
			}
		}

		private void SetHeadRenderersMode(ShadowCastingMode mode)
		{
			if (headRenderers == null)
			{
				return;
			}
			Renderer[] array = headRenderers;
			foreach (Renderer renderer in array)
			{
				if (renderer != null)
				{
					renderer.shadowCastingMode = mode;
				}
			}
		}
	}
}
