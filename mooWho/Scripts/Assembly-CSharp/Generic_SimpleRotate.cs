using UnityEngine;

public class Generic_SimpleRotate : MonoBehaviour
{
	public bool rotX;

	public float rotXSpeed = 50f;

	public bool rotY;

	public float rotYSpeed = 50f;

	public bool rotZ;

	public float rotZSpeed = 50f;

	private void Update()
	{
		if (rotX)
		{
			base.transform.Rotate(Vector3.left * Time.deltaTime * rotXSpeed);
		}
		if (rotY)
		{
			base.transform.Rotate(Vector3.up * Time.deltaTime * rotYSpeed);
		}
		if (rotZ)
		{
			base.transform.Rotate(Vector3.back * Time.deltaTime * rotZSpeed);
		}
	}
}
