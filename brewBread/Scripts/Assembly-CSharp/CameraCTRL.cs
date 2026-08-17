using UnityEngine;

public class CameraCTRL : MonoBehaviour
{
	public Transform BreadPos;

	public Transform FredPos;

	public GameObject followPoint;

	private void Update()
	{
		Vector3 zero = Vector3.zero;
		zero = new Vector3((BreadPos.position.x + FredPos.position.x) / 2f, (BreadPos.position.y + FredPos.position.y) / 2f, -10f);
		zero.z = 0f;
		followPoint.transform.position = zero;
	}
}
