using UnityEngine;

namespace Coffee.UIEffectInternal
{
	public abstract class PreloadedProjectSettings : ScriptableObject
	{
	}
	public abstract class PreloadedProjectSettings<T> : PreloadedProjectSettings where T : PreloadedProjectSettings<T>
	{
		private static T s_Instance;

		public static T instance
		{
			get
			{
				if (!s_Instance)
				{
					return s_Instance = ScriptableObject.CreateInstance<T>();
				}
				return s_Instance;
			}
		}

		protected virtual void OnEnable()
		{
			if (!s_Instance)
			{
				s_Instance = this as T;
			}
		}

		protected virtual void OnDisable()
		{
			if (!(s_Instance != this))
			{
				s_Instance = null;
			}
		}
	}
}
