using TMPro;
using UnityEngine;

namespace GameKit.Utilities.Types.CanvasContainers
{
	public class OptionMenuButton : MonoBehaviour
	{
		[Tooltip("Text component to show button text.")]
		[SerializeField]
		private TextMeshProUGUI _text;

		public ButtonData ButtonData { get; protected set; }

		public virtual void Initialize(ButtonData buttonData)
		{
			ButtonData = buttonData;
			_text.text = buttonData.Text;
		}
	}
}
