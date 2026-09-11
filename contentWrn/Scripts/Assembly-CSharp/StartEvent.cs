using UnityEngine;
using UnityEngine.Events;

public class StartEvent : MonoBehaviour
{
	public UnityEvent awakeEvent;

	public UnityEvent startEvent;

	private void Awake()
	{
		awakeEvent.Invoke();
	}

	private void Start()
	{
		startEvent.Invoke();
	}
}
