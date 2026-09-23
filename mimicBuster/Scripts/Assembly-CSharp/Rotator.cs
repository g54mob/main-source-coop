using UnityEngine;

public class Rotator : MonoBehaviour
{
	public float rotationSpeed = 100f;

	public Vector3 scale = Vector3.zero;

	public Vector3 _r = Vector3.one;

	private void Update()
	{
		_r.y = Mathf.Cos(Time.time * scale.y) * 360f;
		_r.x = Mathf.Sin(Time.time * scale.x) * 360f;
		_r.z = Mathf.Tan(Time.time * scale.z) * 360f;
		base.transform.Rotate(_r * Time.deltaTime * rotationSpeed);
	}
}
