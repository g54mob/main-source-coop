using UnityEngine;

public class LightingGenerator : MonoBehaviour
{
	private Bounds levelBounds;

	public GameObject[] lights;

	public int nrOfLights = 30;

	private void Go()
	{
		Clear();
		CalculateLevelBounds();
		for (int i = 0; i < nrOfLights; i++)
		{
			LightPosition lightPos = GetLightPos();
			if (lightPos != null)
			{
				HelperFunctions.SpawnPrefab(lights[Random.Range(0, lights.Length)], lightPos.position, Quaternion.identity, base.transform);
			}
		}
	}

	private void Clear()
	{
		for (int num = base.transform.childCount - 1; num >= 0; num--)
		{
			Object.DestroyImmediate(base.transform.GetChild(num).gameObject);
		}
	}

	private LightPosition GetLightPos()
	{
		int num = 10;
		while (num > 0)
		{
			Vector3 randomPositionInBounds = HelperFunctions.GetRandomPositionInBounds(levelBounds);
			num--;
			LightPosition lightPosition = LightPosition.Sample(randomPositionInBounds);
			if (lightPosition.stepsTaken > 3 && !lightPosition.neverTouched)
			{
				return lightPosition;
			}
		}
		return null;
	}

	private void CalculateLevelBounds()
	{
		Room[] componentsInChildren = base.transform.root.GetComponentsInChildren<Room>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			if (i == 0)
			{
				levelBounds = new Bounds(componentsInChildren[i].transform.position, Vector3.one * 15f);
			}
			else
			{
				levelBounds.Encapsulate(componentsInChildren[i].transform.position);
			}
		}
	}

	private void OnDrawGizmos()
	{
		Gizmos.DrawWireCube(levelBounds.center, levelBounds.size);
	}
}
