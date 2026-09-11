using UnityEngine;

namespace pworld.Scripts
{
	public class PEnableHandler : MonoBehaviour
	{
		private void Awake()
		{
		}

		private void Start()
		{
			foreach (Transform item in base.transform)
			{
				item.gameObject.SetActive(value: true);
			}
		}

		private void Update()
		{
		}

		private void OnDestroy()
		{
		}
	}
}
