using UnityEngine;

namespace pworld.Scripts
{
	public class PKillChildrenAtStart : MonoBehaviour
	{
		private void Awake()
		{
			foreach (Transform item in base.transform)
			{
				Object.DestroyImmediate(item.gameObject);
			}
		}

		private void Start()
		{
		}

		private void Update()
		{
		}

		private void OnDestroy()
		{
		}
	}
}
