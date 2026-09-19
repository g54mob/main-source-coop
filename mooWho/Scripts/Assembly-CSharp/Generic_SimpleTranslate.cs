using UnityEngine;

public class Generic_SimpleTranslate : MonoBehaviour
{
	public bool moveX;

	public float moveXSpeed = 2f;

	public bool moveY;

	public float moveYSpeed = 2f;

	public bool moveZ;

	public float moveZSpeed = 2f;

	private void Update()
	{
		if (moveX)
		{
			base.transform.Translate(Vector3.left * Time.deltaTime * moveXSpeed);
		}
		if (moveY)
		{
			base.transform.Translate(Vector3.up * Time.deltaTime * moveYSpeed);
		}
		if (moveZ)
		{
			base.transform.Translate(Vector3.back * Time.deltaTime * moveZSpeed);
		}
	}
}
