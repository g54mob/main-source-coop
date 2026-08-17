using System.Collections.Generic;
using UnityEngine;

namespace FMODUnity
{
	[AddComponentMenu("FMOD Studio/FMOD Studio Listener")]
	public class StudioListener : MonoBehaviour
	{
		[SerializeField]
		private GameObject attenuationObject;

		private Rigidbody rigidBody;

		private Rigidbody2D rigidBody2D;

		private static List<StudioListener> listeners = new List<StudioListener>();

		public static int ListenerCount => listeners.Count;

		public int ListenerNumber => listeners.IndexOf(this);

		public static float DistanceToNearestListener(Vector3 position)
		{
			float num = float.MaxValue;
			for (int i = 0; i < listeners.Count; i++)
			{
				num = Mathf.Min(num, Vector3.Distance(position, listeners[i].transform.position));
			}
			return num;
		}

		public static float DistanceSquaredToNearestListener(Vector3 position)
		{
			float num = float.MaxValue;
			for (int i = 0; i < listeners.Count; i++)
			{
				num = Mathf.Min(num, (position - listeners[i].transform.position).sqrMagnitude);
			}
			return num;
		}

		private static void AddListener(StudioListener listener)
		{
			if (listeners.Contains(listener))
			{
				Debug.LogWarning($"[FMOD] Listener has already been added at index {listener.ListenerNumber}.");
				return;
			}
			if (listeners.Count >= 8)
			{
				Debug.LogWarning($"[FMOD] Max number of listeners reached : {8}.");
			}
			listeners.Add(listener);
			RuntimeManager.StudioSystem.setNumListeners(Mathf.Clamp(listeners.Count, 1, 8));
		}

		private static void RemoveListener(StudioListener listener)
		{
			listeners.Remove(listener);
			RuntimeManager.StudioSystem.setNumListeners(Mathf.Clamp(listeners.Count, 1, 8));
		}

		private void OnEnable()
		{
			RuntimeUtils.EnforceLibraryOrder();
			rigidBody = base.gameObject.GetComponent<Rigidbody>();
			rigidBody2D = base.gameObject.GetComponent<Rigidbody2D>();
			AddListener(this);
		}

		private void OnDisable()
		{
			RemoveListener(this);
		}

		private void Update()
		{
			if (ListenerNumber >= 0 && ListenerNumber < 8)
			{
				SetListenerLocation();
			}
		}

		private void SetListenerLocation()
		{
			if ((bool)rigidBody)
			{
				RuntimeManager.SetListenerLocation(ListenerNumber, base.gameObject, rigidBody, attenuationObject);
			}
			else if ((bool)rigidBody2D)
			{
				RuntimeManager.SetListenerLocation(ListenerNumber, base.gameObject, rigidBody2D, attenuationObject);
			}
			else
			{
				RuntimeManager.SetListenerLocation(ListenerNumber, base.gameObject, attenuationObject);
			}
		}
	}
}
