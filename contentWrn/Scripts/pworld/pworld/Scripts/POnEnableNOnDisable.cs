using System;
using System.Linq;
using UnityEngine;

namespace pworld.Scripts
{
	public class POnEnableNOnDisable : MonoBehaviour
	{
		public MonoBehaviour creator;

		public Action OnDisabled;

		public Action OnEnabled;

		private void OnEnable()
		{
			OnEnabled?.Invoke();
		}

		private void OnDisable()
		{
			OnDisabled?.Invoke();
		}

		public static bool RemoveByCreator(GameObject host, MonoBehaviour creator)
		{
			bool removed = false;
			host.gameObject.GetComponents<POnEnableNOnDisable>().ToList().ForEach(delegate(POnEnableNOnDisable disable)
			{
				if (disable.creator == creator)
				{
					UnityEngine.Object.Destroy(disable);
					removed = true;
				}
			});
			return removed;
		}
	}
}
