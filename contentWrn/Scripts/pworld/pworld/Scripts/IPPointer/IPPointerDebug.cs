using UnityEngine;

namespace pworld.Scripts.IPPointer
{
	public class IPPointerDebug : MonoBehaviour, IPPointerHandler
	{
		public void OnPPointerEnter()
		{
			Debug.Log("IPPointerDebug OnPPointerEnter");
		}

		public void OnPPointerExit()
		{
			Debug.Log("IPPointerDebug OnPPointerExit");
		}

		public void OnPPointerClick()
		{
			Debug.Log("IPPointerDebug OnPPointerClick");
		}

		public void OnPPointerUp()
		{
			Debug.Log("IPPointerDebug OnPPointerUp");
		}

		public void OnPPointerDown()
		{
			Debug.Log("IPPointerDebug OnPPointerDown");
		}
	}
}
