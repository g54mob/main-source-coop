using System;
using UnityEngine;

public class EventManager : MonoBehaviour
{
	public static EventManager instance;

	public event Action<Transform> PlayerHang;

	public event Action<Transform> PlayerStopHangging;

	private void Awake()
	{
		if (instance == null)
		{
			instance = this;
		}
		else
		{
			UnityEngine.Object.Destroy(this);
		}
	}

	public void OnPlayerHang(Transform transform)
	{
		this.PlayerHang?.Invoke(transform);
	}

	public void OnPlayerStopHangging(Transform transform)
	{
		this.PlayerStopHangging?.Invoke(transform);
	}
}
