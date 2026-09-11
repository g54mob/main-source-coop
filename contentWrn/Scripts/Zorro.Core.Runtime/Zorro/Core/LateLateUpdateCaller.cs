using UnityEngine;

namespace Zorro.Core
{
	[DefaultExecutionOrder(100)]
	public class LateLateUpdateCaller : MonoBehaviour
	{
		private ILateLateUpdateReceiver Receiver;

		private void Start()
		{
			Receiver = GetComponent<ILateLateUpdateReceiver>();
		}

		private void LateUpdate()
		{
			Receiver?.LateLateUpdate();
		}
	}
}
