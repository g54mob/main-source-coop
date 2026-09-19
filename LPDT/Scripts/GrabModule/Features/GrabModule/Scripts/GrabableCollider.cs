using UnityEngine;

namespace Features.GrabModule.Scripts
{
	public class GrabableCollider : MonoBehaviour
	{
		[SerializeField]
		private GameObject _grabableObject;

		private IGrabableBase _grabable;

		public IGrabableBase Grabable => _grabable;

		private void Awake()
		{
			_grabable = _grabableObject.GetComponent<IGrabableBase>();
		}
	}
}
