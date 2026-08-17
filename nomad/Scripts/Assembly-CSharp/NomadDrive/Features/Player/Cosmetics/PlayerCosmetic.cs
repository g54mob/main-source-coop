using UnityEngine;

namespace NomadDrive.Features.Player.Cosmetics
{
	public class PlayerCosmetic : MonoBehaviour
	{
		[SerializeField]
		private CosmeticCategory category;

		[SerializeField]
		private string cosmeticId;

		[SerializeField]
		private bool hideForLocalPlayer;

		public CosmeticCategory Category => category;

		public string CosmeticId => cosmeticId;

		public bool HideForLocalPlayer => hideForLocalPlayer;

		public void SetActive(bool active)
		{
			base.gameObject.SetActive(active);
		}

		private void OnValidate()
		{
			if (string.IsNullOrEmpty(cosmeticId))
			{
				cosmeticId = base.gameObject.name;
			}
		}
	}
}
