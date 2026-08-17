using UnityEngine;

public abstract class Singleton<T> : StaticInstance<T> where T : MonoBehaviour
{
	protected override void Awake()
	{
		if (StaticInstance<T>.Instance != null)
		{
			Object.Destroy(this);
			Object.Destroy(base.gameObject);
		}
		else
		{
			base.Awake();
		}
	}
}
