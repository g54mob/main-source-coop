using UnityEngine;

public static class AudioObstructability
{
	public static float GetObstructionValue(Vector3 position, float obstructability, out float rangeFactor)
	{
		RaycastHit raycastHit = HelperFunctions.LineCheck(position, MainCamera.instance.transform.position, HelperFunctions.LayerType.Terrain);
		if ((bool)raycastHit.transform)
		{
			float num = 1f - obstructability;
			float value = Vector3.Distance(raycastHit.point, MainCamera.instance.transform.position);
			float t = Mathf.InverseLerp(12f, 2f, value);
			float num2 = Mathf.Lerp(num, Mathf.Lerp(1f, num, 0.5f), t);
			rangeFactor = Mathf.Lerp(1f, num2, 0.35f);
			return num2;
		}
		rangeFactor = 1f;
		return 1f;
	}
}
