using System.Collections.Generic;
using UnityEngine;

public class LightPosition
{
	public Vector3 position;

	public float radius;

	public int stepsTaken;

	public bool neverTouched = true;

	internal static LightPosition Sample(Vector3 pos)
	{
		int num = 5;
		LightPosition lightPosition = new LightPosition();
		lightPosition.position = pos;
		lightPosition.radius = 5f;
		for (int i = 0; i < num; i++)
		{
			lightPosition.stepsTaken++;
			Collider[] array = Physics.OverlapSphere(lightPosition.position, lightPosition.radius);
			List<Vector3> list = new List<Vector3>();
			Vector3 zero = Vector3.zero;
			for (int j = 0; j < array.Length; j++)
			{
				lightPosition.neverTouched = false;
				_ = array[j];
				Vector3 vector = array[j].ClosestPoint(lightPosition.position);
				Vector3 normalized = (lightPosition.position - vector).normalized;
				list.Add(normalized);
				float num2 = Vector3.Distance(lightPosition.position, vector);
				float num3 = lightPosition.radius - num2;
				zero += normalized * num3;
			}
			if (list.Count > 0)
			{
				lightPosition.position += zero / list.Count;
			}
			for (int k = 0; k < list.Count; k++)
			{
				for (int l = 0; l < list.Count; l++)
				{
					if (Vector3.Angle(list[k], list[l]) > 170f)
					{
						return lightPosition;
					}
				}
			}
			lightPosition.radius += 5f;
		}
		return lightPosition;
	}
}
