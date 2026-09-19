using UnityEngine;

namespace Features.JacuzziBeachInteractableModule.Scripts
{
	public class JacuzziWaterTriggerZone : MonoBehaviour
	{
		[SerializeField]
		private JacuzziWaterController _jacuzziWaterController;

		[Tooltip("Every trigger collider of this zone. The entry point is resolved against whichever one is nearest the item.")]
		[SerializeField]
		private Collider[] _triggerColliders;

		private void OnTriggerEnter(Collider other)
		{
			_jacuzziWaterController.NotifyItemEntered(other, GetHitPosition(other));
		}

		private void OnTriggerExit(Collider other)
		{
			_jacuzziWaterController.NotifyItemExited(other);
		}

		private Vector3 GetHitPosition(Collider other)
		{
			Vector3 center = other.bounds.center;
			Vector3 result = center;
			float num = float.MaxValue;
			for (int i = 0; i < _triggerColliders.Length; i++)
			{
				Vector3 vector = _triggerColliders[i].ClosestPoint(center);
				float sqrMagnitude = (vector - center).sqrMagnitude;
				if (!(sqrMagnitude >= num))
				{
					num = sqrMagnitude;
					result = vector;
				}
			}
			return result;
		}
	}
}
