using UnityEngine;
using UnityEngine.UI;

namespace AeLa.EasyFeedback.Demo
{
	[RequireComponent(typeof(Text))]
	public class CheckFormOpen : MonoBehaviour
	{
		private Text text;

		private FeedbackForm feedbackForm;

		private void Start()
		{
			text = GetComponent<Text>();
			feedbackForm = Object.FindObjectOfType<FeedbackForm>();
		}

		private void Update()
		{
			text.text = feedbackForm.IsOpen.ToString();
		}
	}
}
