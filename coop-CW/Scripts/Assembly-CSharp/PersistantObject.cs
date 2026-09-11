using JetBrains.Annotations;
using UnityEngine;

public class PersistantObject : MonoBehaviour
{
	[SerializeField]
	private Item m_ItemToSwitch;

	[CanBeNull]
	public Item ItemSwitch => m_ItemToSwitch;

	private void Start()
	{
		if (GetComponentInParent<Pickup>() == null)
		{
			Object.Destroy(this);
		}
	}
}
