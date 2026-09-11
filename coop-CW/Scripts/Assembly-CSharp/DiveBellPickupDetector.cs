using System.Collections.Generic;
using UnityEngine;

public class DiveBellPickupDetector : MonoBehaviour
{
	public Transform[] m_detectors;

	private Collider[] results = new Collider[200];

	private HashSet<Pickup> m_pickups = new HashSet<Pickup>();

	public ICollection<Pickup> CheckForPickups()
	{
		m_pickups.Clear();
		Transform[] detectors = m_detectors;
		foreach (Transform transform in detectors)
		{
			int num = Physics.OverlapSphereNonAlloc(transform.position, transform.lossyScale.x * 0.5f, results);
			for (int j = 0; j < num; j++)
			{
				Collider collider = results[j];
				Pickup componentInParent = collider.GetComponentInParent<Pickup>();
				if ((bool)componentInParent)
				{
					m_pickups.Add(componentInParent);
					Debug.DrawLine(collider.ClosestPoint(transform.position), transform.position, Color.red);
				}
			}
		}
		return m_pickups;
	}
}
