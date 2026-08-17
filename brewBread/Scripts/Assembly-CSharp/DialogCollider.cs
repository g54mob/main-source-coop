using UnityEngine;
using UnityEngine.Events;

public class DialogCollider : MonoBehaviour
{
	private int _counter;

	public UnityEvent OnTriggerEnter { get; private set; }

	public UnityEvent OnTriggerExit { get; private set; }

	private void Awake()
	{
		OnTriggerEnter = new UnityEvent();
		OnTriggerExit = new UnityEvent();
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.layer == 9)
		{
			_counter++;
			if (_counter == 2)
			{
				OnTriggerEnter?.Invoke();
			}
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.gameObject.layer == 9)
		{
			_counter--;
			if (_counter == 0)
			{
				OnTriggerExit?.Invoke();
			}
		}
	}
}
