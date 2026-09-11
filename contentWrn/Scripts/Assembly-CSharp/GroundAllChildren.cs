using UnityEngine;

public class GroundAllChildren : MonoBehaviour
{
	public float checkHeight;

	public HelperFunctions.LayerType layerType;

	private void Go()
	{
		for (int i = 0; i < base.transform.childCount; i++)
		{
			Transform child = base.transform.GetChild(i);
			RaycastHit groundPosRaycast = HelperFunctions.GetGroundPosRaycast(child.position + Vector3.up * checkHeight, layerType);
			if ((bool)groundPosRaycast.transform)
			{
				child.transform.position = groundPosRaycast.point;
			}
		}
	}
}
