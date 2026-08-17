using UnityEngine;

namespace NomadDrive.Features.Interaction
{
	public class StaticInteractableRouter : MonoBehaviour
	{
		[SerializeField]
		private StaticInteractable _routeInteractable;

		public StaticInteractable RouteInteractable => _routeInteractable;

		private void OnValidate()
		{
			if (_routeInteractable == null)
			{
				_routeInteractable = GetComponentInParent<StaticInteractable>();
			}
		}

		public void SetRouteTarget(StaticInteractable target)
		{
			_routeInteractable = target;
		}
	}
}
