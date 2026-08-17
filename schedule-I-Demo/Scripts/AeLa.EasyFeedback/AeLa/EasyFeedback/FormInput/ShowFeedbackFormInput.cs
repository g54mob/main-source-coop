using UnityEngine;

namespace AeLa.EasyFeedback.FormInput
{
	[RequireComponent(typeof(FeedbackForm))]
	public class ShowFeedbackFormInput : MonoBehaviour, IToggleFormInput
	{
		[Tooltip("Key used to toggle the feedback form")]
		public KeyCode ToggleKey = KeyCode.F12;

		[Tooltip("Key used to hide the feedback form")]
		public KeyCode ShowKey;

		[Tooltip("Key used to hide the feedback form")]
		public KeyCode HideKey = KeyCode.Escape;

		private FeedbackForm form;

		public string Descriptor
		{
			get
			{
				if (ToggleKey == KeyCode.None)
				{
					return ShowKey.ToString();
				}
				return ToggleKey.ToString();
			}
		}

		private void Start()
		{
			form = GetComponent<FeedbackForm>();
		}

		private void Update()
		{
			if (Input.GetKeyDown(ToggleKey))
			{
				form.Toggle();
			}
			else if (Input.GetKeyDown(ShowKey))
			{
				form.Show();
			}
			else if (Input.GetKeyDown(HideKey))
			{
				form.Hide();
			}
		}
	}
}
