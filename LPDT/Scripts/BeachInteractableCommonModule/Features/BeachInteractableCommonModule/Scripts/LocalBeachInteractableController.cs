using UnityEngine;

namespace Features.BeachInteractableCommonModule.Scripts
{
	public class LocalBeachInteractableController : IBeachInteractableController
	{
		private GameObject _beachInteractable;

		public BeachInteractableIdentifier BeachInteractableIdentifier { get; private set; }

		public void Initialize(GameObject beachInteractable, BeachInteractableIdentifier beachInteractableIdentifier)
		{
			_beachInteractable = beachInteractable;
			BeachInteractableIdentifier = beachInteractableIdentifier;
		}

		public void DespawnInteractable()
		{
			Object.Destroy(_beachInteractable);
		}
	}
}
