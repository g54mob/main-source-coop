using UnityEngine;

public class SuckTrigger : MonoBehaviour
{
	public Transform suckTowards;

	public float suckForce;

	public bool requireLineOfSight;

	public float maxSuckTimeScale = 1f;

	internal float suckTime;

	public float extraDrag = 1f;

	private void OnEnable()
	{
		suckTime = 0f;
	}

	private void Update()
	{
		suckTime += Time.deltaTime;
	}

	private void OnTriggerStay(Collider other)
	{
		if (!base.enabled)
		{
			return;
		}
		Player componentInParent = other.GetComponentInParent<Player>();
		if (!componentInParent || componentInParent.ai)
		{
			return;
		}
		Rigidbody componentInParent2 = other.GetComponentInParent<Rigidbody>();
		if ((bool)componentInParent2 && (!requireLineOfSight || !HelperFunctions.LineCheck(suckTowards.position, componentInParent2.position, HelperFunctions.LayerType.TerrainProp).transform))
		{
			Vector3 vector = suckTowards.position - componentInParent2.position;
			vector.Normalize();
			componentInParent2.AddForce(vector * suckForce * Mathf.Clamp(suckTime, 0f, maxSuckTimeScale) / maxSuckTimeScale, ForceMode.Acceleration);
			if (extraDrag < 0.99f)
			{
				componentInParent2.linearVelocity *= extraDrag;
				componentInParent2.angularVelocity *= extraDrag;
			}
		}
	}
}
