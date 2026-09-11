using UnityEngine;
using UnityEngine.Serialization;

public class LightBobber : MonoBehaviour
{
	[FormerlySerializedAs("animationCurve")]
	public AnimationCurve rotationCurve;

	public float forwardOffset = 1f;

	public AnimationCurve upDownCurve;

	[FormerlySerializedAs("animationSpeed")]
	public float rotationSpeed = 1f;

	public float upDownSpeed = 1f;

	public float upDownMul = 1f;

	public GameObject origPos;

	private float elapsedTimeRotation;

	private float elapsedTimeUpDown;

	private void Start()
	{
		origPos = new GameObject(base.gameObject.name + " origPos");
		origPos.transform.position = base.transform.position;
		origPos.transform.rotation = Quaternion.identity;
	}

	private void Update()
	{
		elapsedTimeRotation += Time.deltaTime * rotationSpeed;
		elapsedTimeRotation %= 1f;
		elapsedTimeUpDown += Time.deltaTime * upDownSpeed;
		elapsedTimeUpDown %= 1f;
		float yAngle = rotationCurve.Evaluate(elapsedTimeRotation);
		float num = upDownCurve.Evaluate(elapsedTimeUpDown);
		origPos.transform.Rotate(0f, yAngle, 0f);
		Vector3 position = origPos.transform.TransformPoint(0f, num * upDownMul, forwardOffset);
		base.transform.position = position;
	}
}
