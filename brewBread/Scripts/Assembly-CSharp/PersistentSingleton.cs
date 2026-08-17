using UnityEngine;

public abstract class PersistentSingleton<T> : Singleton<T> where T : MonoBehaviour
{
	protected override void Awake()
	{
		Object.DontDestroyOnLoad(base.gameObject);
		base.Awake();
	}
}
public class PersistentSingleton : PersistentSingleton<PersistentSingleton>
{
}
