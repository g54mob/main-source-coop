using UnityEngine;

public class ItemKillbox : MonoBehaviour
{
	[SerializeField]
	private Collider m_collider;

	private Vector3 m_spawnPosition = new Vector3(0f, 2f, 0f);

	private void Start()
	{
		if (!m_collider)
		{
			m_collider = GetComponent<Collider>();
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if ((bool)other.GetComponentInParent<Pickup>())
		{
			if (other.TryGetComponent<Rigidbody>(out var component))
			{
				component.linearVelocity = Vector3.zero;
				component.angularVelocity = Vector3.zero;
			}
			other.transform.parent.position = m_spawnPosition;
		}
	}
}
