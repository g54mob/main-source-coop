using System.Collections.Generic;
using UnityEngine;

public class SpringShake : MonoBehaviour
{
	public List<SpringShakeInstance> shakes = new List<SpringShakeInstance>();

	public void AddPositionShake(Vector3 force, float spring, float drag)
	{
		SpringShakeInstance item = new SpringShakeInstance(force, spring, drag, SpringType.Position);
		shakes.Add(item);
	}

	public void AddRotationShake(Vector3 torque, float spring, float drag)
	{
		SpringShakeInstance item = new SpringShakeInstance(torque, spring, drag, SpringType.Rotation);
		shakes.Add(item);
	}

	private void Update()
	{
		Vector3 zero = Vector3.zero;
		Vector3 forward = Vector3.forward;
		Vector3 up = Vector3.up;
		for (int num = shakes.Count - 1; num >= 0; num--)
		{
			if (shakes[num].springType == SpringType.Position)
			{
				FRILerp.PositionSpring(ref shakes[num].currentValue, Vector3.zero, shakes[num].spring, shakes[num].drag, ref shakes[num].vel);
				zero += shakes[num].currentValue;
			}
			else
			{
				FRILerp.RotationSpring(ref shakes[num].currentValue, Vector3.forward, ref shakes[num].currentValue2, Vector3.up, shakes[num].spring, shakes[num].drag, ref shakes[num].vel);
				forward += shakes[num].currentValue;
				up += shakes[num].currentValue2;
			}
			if (shakes[num].vel.magnitude < 0.1f)
			{
				shakes[num].sleepCounter += Time.deltaTime;
				if (shakes[num].sleepCounter > 1f)
				{
					shakes.RemoveAt(num);
				}
			}
			else
			{
				shakes[num].sleepCounter = 0f;
			}
		}
		base.transform.SetLocalPositionAndRotation(zero, Quaternion.LookRotation(forward, up));
	}
}
