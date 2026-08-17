using AeLa.EasyFeedback.UI.Interfaces;
using UnityEngine;
using UnityEngine.Serialization;

namespace AeLa.EasyFeedback.UI.Toaster
{
	[RequireComponent(typeof(RectTransform))]
	public class Toast : MonoBehaviour
	{
		[FormerlySerializedAs("text")]
		[SerializeField]
		protected GameObject Text;

		private IText textComponent;

		public string Message
		{
			get
			{
				return textComponent.Text;
			}
			set
			{
				textComponent.Text = value;
			}
		}

		public RectTransform RectTransform => (RectTransform)base.transform;

		private void Awake()
		{
			textComponent = UIInterop.GetText(Text);
		}
	}
}
