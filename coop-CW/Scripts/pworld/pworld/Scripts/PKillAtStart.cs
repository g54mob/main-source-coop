using UnityEngine;

namespace pworld.Scripts
{
	public class PKillAtStart : MonoBehaviour
	{
		private void Awake()
		{
			Object.DestroyImmediate(base.gameObject);
		}
	}
}
