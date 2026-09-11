using UnityEngine;

public class SetBounds : MonoBehaviour
{
	private void Start()
	{
		GetComponent<MeshRenderer>().bounds = new Bounds(Vector3.zero, Vector3.one * 10000f);
	}
}
