using UnityEngine;

namespace NomadDrive.Features.Interaction
{
	public class InteractableRouter : MonoBehaviour
	{
		[field: SerializeField]
		public Interactable RouteInteractable { get; private set; }

		private void OnValidate()
		{
			if (RouteInteractable == null)
			{
				RouteInteractable = GetComponentInParent<Interactable>(includeInactive: true);
			}
		}

		public void SetRouteTarget(Interactable target)
		{
			RouteInteractable = target;
		}
	}
}
