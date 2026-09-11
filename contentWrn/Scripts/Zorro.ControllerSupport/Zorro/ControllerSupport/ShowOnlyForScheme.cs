using UnityEngine;

namespace Zorro.ControllerSupport
{
	public class ShowOnlyForScheme : MonoBehaviour
	{
		public InputScheme scheme;

		public GameObject target;

		private void Start()
		{
			UpdateVisibility();
		}

		private void UpdateVisibility()
		{
			target.SetActive(InputHandler.GetCurrentUsedInputScheme() == scheme);
		}

		private void Update()
		{
			UpdateVisibility();
		}
	}
}
