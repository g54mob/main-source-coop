using UnityEngine;

public class CameraLean : MonoBehaviour
{
	public float sideTilt = 50f;

	public float forwardTilt = 50f;

	public SpringShakeInstance headTiltSpring;

	public float wobbleMultiplier = 1f;

	public float sprintWobbleMultiplier = 2f;

	public float stepSpeed = 2f;

	public float sprintSpeedMultiplier = 2f;

	public float upDownMultiplier = 1f;

	public AnimationCurve upDownCurve;

	public float sideMultiplier = 1f;

	public AnimationCurve sideCurve;

	public float tiltMultiplier = 1f;

	public AnimationCurve tiltCurve;

	public SpringShakeInstance wobbleSpring;

	private float counter;

	private void Start()
	{
		headTiltSpring.Init();
		wobbleSpring.Init();
	}

	private void Update()
	{
		if ((bool)SimplePlayer.localPlayer)
		{
			HeadTilt();
			Wobble();
			base.transform.rotation = Quaternion.LookRotation(headTiltSpring.currentValue + wobbleSpring.currentValue, headTiltSpring.currentValue2 + wobbleSpring.currentValue2);
		}
	}

	private void Wobble()
	{
		if (SimplePlayer.localPlayer.input.sprintIsPressed)
		{
			counter += Time.deltaTime * stepSpeed * sprintSpeedMultiplier;
		}
		else
		{
			counter += Time.deltaTime * stepSpeed;
		}
		if (counter > 1f)
		{
			counter = 0f;
		}
		float num = 1f;
		if (SimplePlayer.localPlayer.input.sprintIsPressed)
		{
			num *= sprintWobbleMultiplier;
		}
		num *= wobbleMultiplier;
		num *= Mathf.Clamp01(SimplePlayer.localPlayer.refs.rig.linearVelocity.magnitude);
		wobbleSpring.vel += Vector3.forward * (tiltCurve.Evaluate(counter) * num * tiltMultiplier * Time.deltaTime);
		wobbleSpring.vel += Vector3.right * (upDownCurve.Evaluate(counter) * num * upDownMultiplier * Time.deltaTime);
		wobbleSpring.vel += Vector3.up * (sideCurve.Evaluate(counter) * num * sideMultiplier * Time.deltaTime);
		FRILerp.RotationSpring(ref wobbleSpring.currentValue, Vector3.forward, ref wobbleSpring.currentValue2, Vector3.up, wobbleSpring.spring, wobbleSpring.drag, ref wobbleSpring.vel);
	}

	private void HeadTilt()
	{
		headTiltSpring.vel += Vector3.forward * ((0f - SimplePlayer.localPlayer.data.relativeVel.x) * sideTilt * Time.deltaTime);
		headTiltSpring.vel += Vector3.right * (SimplePlayer.localPlayer.data.relativeVel.z * forwardTilt * Time.deltaTime);
		FRILerp.RotationSpring(ref headTiltSpring.currentValue, Vector3.forward, ref headTiltSpring.currentValue2, Vector3.up, headTiltSpring.spring, headTiltSpring.drag, ref headTiltSpring.vel);
	}
}
