using UnityEngine;

public class Hazard : MonoBehaviour
{
	private void Start()
	{
		HazardHandler.instance.hazards.Add(this);
	}

	private void OnDestroy()
	{
		HazardHandler.instance.hazards.Remove(this);
	}
}
