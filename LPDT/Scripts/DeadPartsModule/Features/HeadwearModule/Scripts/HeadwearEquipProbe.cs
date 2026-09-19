using UnityEngine;

namespace Features.HeadwearModule.Scripts
{
	[RequireComponent(typeof(Collider))]
	public class HeadwearEquipProbe : MonoBehaviour
	{
		[SerializeField]
		private Headwear _headwear;

		private void OnTriggerStay(Collider other)
		{
			if (other.TryGetComponent<HeadwearSlot>(out var component))
			{
				_headwear.TryEquip(component);
			}
		}
	}
}
