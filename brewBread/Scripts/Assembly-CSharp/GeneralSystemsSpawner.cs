using UnityEngine;

public class GeneralSystemsSpawner : MonoBehaviour
{
	[SerializeField]
	private GameObject _generalSystemsPrefab;

	private void Awake()
	{
		if (StaticInstance<PersistentSingleton>.Instance == null)
		{
			Object.Instantiate(_generalSystemsPrefab);
		}
	}
}
