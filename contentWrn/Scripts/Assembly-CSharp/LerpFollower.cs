using UnityEngine;

public class LerpFollower : MonoBehaviour
{
	public Transform target;

	public float rotationSpeed = 5f;

	public float positionSpeed = 5f;

	private void Start()
	{
		base.transform.position = target.position;
		base.transform.rotation = target.rotation;
	}

	private void Update()
	{
		base.transform.SetPositionAndRotation(Vector3.Lerp(base.transform.position, target.position, Time.deltaTime * positionSpeed), Quaternion.Lerp(base.transform.rotation, target.rotation, Time.deltaTime * rotationSpeed));
	}
}
