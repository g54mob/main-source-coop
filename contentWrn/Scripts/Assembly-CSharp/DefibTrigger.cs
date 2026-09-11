using UnityEngine;
using UnityEngine.Serialization;

public class DefibTrigger : MonoBehaviour
{
	[FormerlySerializedAs("defib_g")]
	public Defib defib_gp;

	private void Awake()
	{
		defib_gp = GetComponentInParent<Defib>();
	}

	private void OnTriggerStay(Collider other)
	{
		defib_gp.OnDefibTrigger(other);
	}
}
