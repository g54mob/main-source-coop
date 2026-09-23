using Mimicraft.Networking;
using UnityEngine;
using UnityEngine.UI;

namespace Mimicraft.UI
{
	[RequireComponent(typeof(Button))]
	public class ResetBodyButton : MonoBehaviour
	{
		private void Awake()
		{
			Button component = GetComponent<Button>();
			component.onClick.RemoveListener(ResetBody);
			component.onClick.AddListener(ResetBody);
		}

		private static void ResetBody()
		{
			PlayerVoxelBody.RequestLocalReset();
		}
	}
}
