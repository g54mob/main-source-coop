using TMPro;
using UnityEngine;

namespace NomadDrive.Features.Interaction.UI
{
	public class InfoUIObject : MonoBehaviour
	{
		[field: SerializeField]
		private TextMeshProUGUI InfoText { get; set; }

		private void Awake()
		{
			Disable();
		}

		public void Activate()
		{
			base.transform.localScale = Vector3.one;
		}

		public void Disable()
		{
			base.transform.localScale = Vector3.zero;
		}

		public void UpdateText(string infoString)
		{
			InfoText.text = infoString;
		}
	}
}
