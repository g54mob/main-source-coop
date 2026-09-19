using QuickOutline.Scripts;
using UnityEngine;

namespace Features.PlayerGrabModule.Scripts
{
	public class PlayerOutlineRegistrar : MonoBehaviour
	{
		[SerializeField]
		private Outline _outline;

		public Outline Outline => _outline;

		private void OnEnable()
		{
			if (_outline != null)
			{
				_outline.enabled = false;
			}
		}
	}
}
