using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WindController : MonoBehaviour
{
	[SerializeField]
	private float _windForce;

	private float _windForceAux;

	[Tooltip("Time the wind is active")]
	[SerializeField]
	private float _windActiveTime;

	[Tooltip("Time the wind is off before returning to blow")]
	[SerializeField]
	private float _windWaitTime;

	[SerializeField]
	private List<GameObject> _objs;

	[SerializeField]
	private GameObject _particleSystem;

	[HideInInspector]
	public static UnityEvent<GameObject, WindController> OnPenguinEnter;

	[HideInInspector]
	public static UnityEvent<GameObject, WindController> OnPenguinExit;

	[HideInInspector]
	public UnityEvent<bool> WindBlow;

	private void Awake()
	{
		OnPenguinEnter = new UnityEvent<GameObject, WindController>();
		OnPenguinExit = new UnityEvent<GameObject, WindController>();
		WindBlow = new UnityEvent<bool>();
	}

	private void Start()
	{
		_windForceAux = _windForce;
		StartCoroutine(WindCicle());
	}

	private void OnDestroy()
	{
		OnPenguinEnter.RemoveAllListeners();
		OnPenguinExit.RemoveAllListeners();
		WindBlow.RemoveAllListeners();
	}

	private IEnumerator WindCicle()
	{
		while (true)
		{
			float t = _windActiveTime;
			_windForceAux = _windForce;
			_particleSystem.SetActive(value: true);
			WindBlow?.Invoke(arg0: true);
			while (true)
			{
				t -= Time.deltaTime;
				yield return new WaitForEndOfFrame();
				if (t <= 0f)
				{
					break;
				}
				foreach (GameObject obj in _objs)
				{
					obj.transform.position += Vector3.left * _windForce * Time.deltaTime;
				}
			}
			t = _windWaitTime;
			_windForceAux = 0f;
			_particleSystem.SetActive(value: false);
			WindBlow?.Invoke(arg0: false);
			do
			{
				t -= Time.deltaTime;
				yield return new WaitForEndOfFrame();
			}
			while (!(t <= 0f));
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		_objs.Add(collision.gameObject);
		OnPenguinEnter?.Invoke(collision.gameObject, this);
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		_objs.Remove(collision.gameObject);
		OnPenguinExit?.Invoke(collision.gameObject, this);
	}
}
