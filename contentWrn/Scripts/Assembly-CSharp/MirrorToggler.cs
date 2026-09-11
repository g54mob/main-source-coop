using System.Collections.Generic;
using UnityEngine;

public class MirrorToggler : MonoBehaviour
{
	public Camera mirror;

	public List<Transform> m_visionChecks;

	public float fogDensity = 0.005f;

	private void Update()
	{
		bool flag = Vector3.Distance(MainCamera.instance.transform.position, base.transform.position) < 20f;
		if (flag)
		{
			flag = HelperFunctions.CanSee(MainCamera.instance.transform, base.transform.position, 90f);
			if (!flag)
			{
				foreach (Transform visionCheck in m_visionChecks)
				{
					flag = HelperFunctions.CanSee(MainCamera.instance.transform, visionCheck.position, 90f);
					if (flag)
					{
						break;
					}
				}
			}
		}
		mirror.enabled = flag;
		Vector3 forward = Vector3.Reflect(mirror.transform.position - MainCamera.instance.transform.position, -base.transform.forward);
		mirror.transform.rotation = Quaternion.LookRotation(forward);
		float value = Vector3.Distance(MainCamera.instance.transform.position, base.transform.position);
		float f = Mathf.InverseLerp(0f, 15f, value);
		mirror.fieldOfView = Mathf.Lerp(140f, 30f, Mathf.Pow(f, 0.25f));
	}
}
