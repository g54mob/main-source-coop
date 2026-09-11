using UnityEngine;

public class Drone : MonoBehaviour
{
	public GameObject dropSound;

	public Item[] items;

	public AnimationCurve forceCurve;

	public float spring = 15f;

	public float grav = 15f;

	public float drag = 0.9f;

	public float angularDrag = 0.95f;

	public float ropeLength;

	public Vector3 localPos;

	private Rigidbody rig;

	private Transform dron;

	private float counter;

	private bool done;

	private Vector3 lastForce;

	private Vector3 lastForceTarget;

	private void Start()
	{
		dron = base.transform.GetChild(0);
		rig = GetComponentInChildren<Rigidbody>();
	}

	private void LateUpdate()
	{
		if (counter > 12f)
		{
			dron.gameObject.SetActive(value: false);
		}
		if (counter > 30f)
		{
			Object.Destroy(base.gameObject);
		}
		if (counter > 7.5f && !done)
		{
			done = true;
			lastForceTarget = Vector3.zero;
			GetComponentInChildren<DroneBox>().ready = true;
			dropSound.SetActive(value: true);
		}
		counter += Time.deltaTime;
		if (done)
		{
			lastForce = Vector3.Lerp(lastForce, lastForceTarget, Time.deltaTime);
		}
		else
		{
			lastForce = Vector3.Lerp(lastForce, lastForceTarget, Time.deltaTime * 10f);
		}
		dron.position -= lastForce * 0.1f;
	}

	private void FixedUpdate()
	{
		if (!done)
		{
			float time = Vector3.Distance(rig.position, dron.position);
			float num = forceCurve.Evaluate(time);
			Vector3 vector = dron.position + (rig.position - dron.position).normalized * ropeLength;
			rig.linearVelocity *= Mathf.Lerp(1f, drag, num);
			rig.angularVelocity *= Mathf.Lerp(1f, angularDrag, num);
			Vector3 force = (vector - rig.position) * num * spring;
			rig.AddForceAtPosition(force, rig.transform.TransformPoint(localPos), ForceMode.Acceleration);
			rig.AddForce(Vector3.down * num * grav, ForceMode.Acceleration);
			lastForceTarget = force;
		}
	}
}
