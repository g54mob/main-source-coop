using UnityEngine;

public class DistanceTest : MonoBehaviour
{
	public Transform root;

	private void Update()
	{
		if ((bool)Camera.main)
		{
			GetComponent<Animator>().SetFloat("Distance", Vector3.Distance(Camera.main.transform.position, root.position));
		}
	}
}
