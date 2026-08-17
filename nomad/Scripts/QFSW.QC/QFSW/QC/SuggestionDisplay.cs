using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace QFSW.QC
{
	public class SuggestionDisplay : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
	{
		[SerializeField]
		private QuantumConsole _quantumConsole;

		[SerializeField]
		private TextMeshProUGUI _textArea;

		public void OnPointerClick(PointerEventData eventData)
		{
			int num = TMP_TextUtilities.FindIntersectingLink(_textArea, eventData.position, null);
			if (num >= 0)
			{
				TMP_LinkInfo tMP_LinkInfo = _textArea.textInfo.linkInfo[num];
				if (int.TryParse(tMP_LinkInfo.GetLinkID(), out var result))
				{
					_quantumConsole.SetSuggestion(result);
				}
			}
		}
	}
}
