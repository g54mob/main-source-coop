using System;
using UnityEngine;

[Serializable]
public class SpringShakeInstance
{
	public SpringType springType;

	public float spring;

	public float drag;

	public Vector3 vel;

	public Vector3 currentValue;

	public Vector3 currentValue2;

	public float sleepCounter;

	public SpringShakeInstance(Vector3 startVel, float spring, float drag, SpringType springType)
	{
		switch (springType)
		{
		case SpringType.Position:
			currentValue = Vector3.zero;
			break;
		case SpringType.Rotation:
			currentValue = Vector3.forward;
			currentValue2 = Vector3.up;
			break;
		}
		vel = startVel;
		this.spring = spring;
		this.drag = drag;
		this.springType = springType;
	}

	internal void Init()
	{
		if (springType == SpringType.Rotation)
		{
			currentValue = Vector3.forward;
			currentValue2 = Vector3.up;
		}
	}
}
