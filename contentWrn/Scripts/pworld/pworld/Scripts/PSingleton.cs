using UnityEngine;

namespace pworld.Scripts
{
	public class PSingleton<T> : MonoBehaviour where T : Component
	{
		private static T me;

		public static T Me
		{
			get
			{
				if (me == null)
				{
					me = Object.FindObjectOfType<T>();
					if (me == null)
					{
						me = new GameObject
						{
							name = typeof(T).Name
						}.AddComponent<T>();
					}
				}
				return me;
			}
		}

		public virtual void Awake()
		{
			if (me == null)
			{
				me = this as T;
				Object.DontDestroyOnLoad(base.gameObject);
			}
			else
			{
				Debug.LogWarning("Duplicate PSingleton, Deleting " + base.gameObject, base.gameObject);
				Debug.LogWarning("Leaving " + me.gameObject?.ToString() + " alive", me.gameObject);
				Object.Destroy(this);
			}
		}
	}
}
