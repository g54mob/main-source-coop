using UnityEngine;

namespace FishNet.Example.ColliderRollbacks
{
	public class DestroyAfterDelay : MonoBehaviour
	{
		[SerializeField]
		private float _delay = 1f;

		private void Awake()
		{
			UnityEngine.Object.Destroy(base.gameObject, _delay);
		}
	}
}
