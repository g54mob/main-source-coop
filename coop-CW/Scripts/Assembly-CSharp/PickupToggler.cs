using UnityEngine;

public class PickupToggler : MonoBehaviour
{
	private void Start()
	{
		if (!GetComponentInParent<Pickup>())
		{
			base.gameObject.SetActive(value: false);
		}
	}
}
