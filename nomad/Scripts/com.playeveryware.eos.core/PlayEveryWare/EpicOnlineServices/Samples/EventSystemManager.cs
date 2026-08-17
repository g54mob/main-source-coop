using UnityEngine;

namespace PlayEveryWare.EpicOnlineServices.Samples
{
	public class EventSystemManager : MonoBehaviour
	{
		public GameObject inputSystemPrefab;

		public GameObject inputManagerPrefab;

		private void Awake()
		{
			Object.Instantiate(inputSystemPrefab, base.transform);
		}
	}
}
