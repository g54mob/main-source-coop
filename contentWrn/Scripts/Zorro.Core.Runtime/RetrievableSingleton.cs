using UnityEngine;

public class RetrievableSingleton<T> : MonoBehaviour where T : RetrievableSingleton<T>
{
	private static bool m_shuttingDown;

	private static T _instance;

	public static T Instance
	{
		get
		{
			if (m_shuttingDown)
			{
				return _instance;
			}
			if (_instance == null)
			{
				_instance = CreateInstance();
			}
			return _instance;
		}
	}

	protected virtual void OnCreated()
	{
	}

	private static T CreateInstance()
	{
		_instance = new GameObject(typeof(T).Name).AddComponent<T>();
		_instance.OnCreated();
		return _instance;
	}

	private void OnApplicationQuit()
	{
		m_shuttingDown = true;
	}

	protected virtual void OnRemoved()
	{
	}

	public static void RemoveInstance()
	{
		if (_instance != null)
		{
			GameObject obj = _instance.gameObject;
			_instance.OnRemoved();
			_instance = null;
			Object.Destroy(obj);
		}
	}
}
