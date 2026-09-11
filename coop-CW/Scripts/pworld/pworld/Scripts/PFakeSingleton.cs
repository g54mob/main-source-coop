using UnityEngine;

namespace pworld.Scripts
{
	public class PFakeSingleton<T> : MonoBehaviour where T : Component
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
						Debug.LogWarning("Singelton not in scene");
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
			}
			else if (me != this)
			{
				Debug.LogWarning("Duplicate PSingleton" + base.gameObject, base.gameObject);
				Object.Destroy(this);
			}
		}
	}
}
