using System.Threading;
using UnityEngine;

namespace NomadDrive
{
	public class ThreadChangingExample : MonoBehaviour
	{
		private int mainThreadId = Thread.CurrentThread.ManagedThreadId;

		private async void Start()
		{
			Vector3[] array = await DoHeavyWorkAsync();
			for (int i = 0; i < 100; i++)
			{
				GameObject.CreatePrimitive(PrimitiveType.Sphere).transform.position = array[i];
			}
		}

		private async Awaitable<Vector3[]> DoHeavyWorkAsync()
		{
			LogThread("Before BackgroundThreadAsync");
			await Awaitable.BackgroundThreadAsync();
			LogThread("After BackgroundThreadAsync");
			Vector3[] results = new Vector3[5000000];
			for (int i = 0; i < results.Length; i++)
			{
				results[i] = new Vector3(Mathf.Sin((float)i * 0.001f), 0f, Mathf.Cos((float)i * 0.001f));
			}
			await Awaitable.MainThreadAsync();
			LogThread("Final Switch Back");
			new GameObject("Result").transform.position = Vector3.zero;
			return results;
		}

		private void LogThread(string label)
		{
			int managedThreadId = Thread.CurrentThread.ManagedThreadId;
			bool flag = managedThreadId == mainThreadId;
			Debug.Log($"{label} | Thread ID: {managedThreadId} | Main Thread: {flag}");
		}
	}
}
