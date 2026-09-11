using UnityEngine;

public class BodyPartCollision
{
	public Bodypart bodyPart;

	public Vector3 point;

	public Vector3 normal;

	public Rigidbody rigidbody;

	public Collider collider;

	public GameObject gameObject;

	public void ConfigFromCollision(Collision collision)
	{
		point = collision.contacts[0].point;
		normal = collision.contacts[0].normal;
		rigidbody = collision.rigidbody;
		collider = collision.collider;
		gameObject = collision.gameObject;
	}

	internal void ConfigFromRaycastHit(RaycastHit hit)
	{
		point = hit.point;
		normal = hit.normal;
		rigidbody = hit.rigidbody;
		collider = hit.collider;
		gameObject = hit.transform.gameObject;
	}
}
