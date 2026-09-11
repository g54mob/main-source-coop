using System;
using System.Collections;
using UnityEngine;

namespace Photon.Realtime
{
	internal class MonoBehaviourEmpty : MonoBehaviour
	{
		internal Action<RegionHandler> onCompleteCall;

		private RegionHandler obj;

		public static MonoBehaviourEmpty BuildInstance(string id = null)
		{
			GameObject obj = new GameObject(id ?? "MonoBehaviourEmpty");
			UnityEngine.Object.DontDestroyOnLoad(obj);
			return obj.AddComponent<MonoBehaviourEmpty>();
		}

		public void SelfDestroy()
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}

		private void Update()
		{
			if (obj != null)
			{
				onCompleteCall(obj);
				obj = null;
				onCompleteCall = null;
				SelfDestroy();
			}
		}

		public void CompleteOnMainThread(RegionHandler obj)
		{
			this.obj = obj;
		}

		public void StartCoroutineAndDestroy(IEnumerator coroutine)
		{
			StartCoroutine(Routine());
			IEnumerator Routine()
			{
				yield return coroutine;
				SelfDestroy();
			}
		}
	}
}
