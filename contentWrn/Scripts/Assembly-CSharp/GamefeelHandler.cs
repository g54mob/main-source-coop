using UnityEngine;

public class GamefeelHandler : MonoBehaviour
{
	public static GamefeelHandler instance;

	internal PerlinShake perlin;

	internal SpringShake spring;

	internal Vector3 GetEulerOffsets()
	{
		Vector3 zero = Vector3.zero;
		for (int i = 0; i < base.transform.childCount; i++)
		{
			zero += base.transform.GetChild(i).localEulerAngles;
		}
		return zero;
	}

	internal Vector3 GetPositionOffsets()
	{
		Vector3 zero = Vector3.zero;
		for (int i = 0; i < base.transform.childCount; i++)
		{
			zero += base.transform.GetChild(i).localPosition;
		}
		return zero;
	}

	private void Awake()
	{
		instance = this;
		perlin = GetComponentInChildren<PerlinShake>();
		spring = GetComponentInChildren<SpringShake>();
	}
}
