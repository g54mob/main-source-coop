using UnityEngine;

public class Hospital : MonoBehaviour
{
	public static Hospital instance;

	private void Awake()
	{
		instance = this;
	}
}
