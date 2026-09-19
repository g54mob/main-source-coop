using UnityEngine;

public class Generic_WaterBob : MonoBehaviour
{
	public float bobbingHeight = 0.08f;

	public float bobbingSpeed = 1.5f;

	public float rotationAmount = 0.8f;

	public bool randomOffset = true;

	public Vector2 randomRange = new Vector2(0.1f, 1f);

	private Vector3 startPos;

	private Quaternion startRotation;

	private void Start()
	{
		startPos = base.transform.position;
		startRotation = base.transform.rotation;
		if (randomOffset)
		{
			bobbingSpeed += Random.Range(randomRange.x, randomRange.y);
			rotationAmount += Random.Range(randomRange.x, randomRange.y);
		}
	}

	private void Update()
	{
		float y = startPos.y + Mathf.Sin(Time.time * bobbingSpeed) * bobbingHeight;
		Vector3 position = new Vector3(base.transform.position.x, y, base.transform.position.z);
		base.transform.position = position;
		float x = Mathf.Sin(Time.time * bobbingSpeed * 0.5f) * rotationAmount;
		float y2 = Mathf.Sin(Time.time * bobbingSpeed * 0.7f) * rotationAmount;
		float z = Mathf.Sin(Time.time * bobbingSpeed * 0.9f) * rotationAmount;
		Quaternion quaternion = Quaternion.Euler(x, y2, z);
		base.transform.rotation = startRotation * quaternion;
	}
}
