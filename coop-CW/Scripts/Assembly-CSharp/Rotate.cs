using UnityEngine;

public class Rotate : MonoBehaviour
{
	public Vector3 rotation;

	[Range(0f, 1f)]
	public float random;

	public Space space = Space.Self;

	public bool randomDirection;

	private void Start()
	{
		rotation += new Vector3(Random.Range(0f - rotation.x, rotation.x), Random.Range(0f - rotation.y, rotation.y), Random.Range(0f - rotation.z, rotation.z)) * random;
		if (randomDirection && Random.value < 0.5f)
		{
			rotation *= -1f;
		}
	}

	private void Update()
	{
		base.transform.Rotate(rotation * Time.deltaTime, space);
	}
}
