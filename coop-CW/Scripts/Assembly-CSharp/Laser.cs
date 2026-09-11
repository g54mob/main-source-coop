using UnityEngine;

public class Laser : MonoBehaviour
{
	public bool liveLaser;

	private Transform beam;

	private void Start()
	{
		beam = base.transform.GetChild(0);
		if (!liveLaser)
		{
			RaycastHit raycastHit = HelperFunctions.LineCheck(base.transform.position + base.transform.forward * 0.2f, base.transform.position - base.transform.forward, HelperFunctions.LayerType.TerrainProp);
			if ((bool)raycastHit.transform)
			{
				base.transform.position = raycastHit.point;
			}
			RaycastHit raycastHit2 = HelperFunctions.LineCheck(base.transform.position, base.transform.position + base.transform.forward * 1000f, HelperFunctions.LayerType.TerrainProp);
			if ((bool)raycastHit2.transform)
			{
				beam.transform.localScale = new Vector3(1f, 1f, raycastHit2.distance);
			}
		}
	}

	private void LateUpdate()
	{
		if (liveLaser)
		{
			RaycastHit raycastHit = HelperFunctions.LineCheck(base.transform.position, base.transform.position + base.transform.forward * 100f, HelperFunctions.LayerType.TerrainProp);
			if ((bool)raycastHit.transform)
			{
				beam.transform.localScale = new Vector3(1f, 1f, raycastHit.distance);
			}
			else
			{
				beam.transform.localScale = new Vector3(1f, 1f, 1000f);
			}
		}
	}
}
