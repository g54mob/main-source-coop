using UnityEngine;

namespace EvilCore.Networking
{
	public abstract class OnlineServiceBridge : MonoBehaviour, IOnlineService
	{
		public void Activate()
		{
			base.gameObject.SetActive(value: true);
		}

		public void Deactivate()
		{
			base.gameObject.SetActive(value: false);
		}
	}
}
