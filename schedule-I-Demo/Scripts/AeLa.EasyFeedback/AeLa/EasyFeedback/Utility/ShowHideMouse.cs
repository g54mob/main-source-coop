using UnityEngine;

namespace AeLa.EasyFeedback.Utility
{
	[RequireComponent(typeof(FeedbackForm))]
	public class ShowHideMouse : MonoBehaviour
	{
		private FeedbackForm form;

		private CursorLockMode previousLockState;

		private bool previousVisibility;

		private void Awake()
		{
			form = GetComponent<FeedbackForm>();
		}

		private void OnEnable()
		{
			form.OnFormOpened.AddListener(FormOpened);
			form.OnFormClosed.AddListener(FormClosed);
			if (form.IsOpen)
			{
				FormOpened();
			}
		}

		private void OnDisable()
		{
			if ((bool)form)
			{
				form.OnFormOpened.RemoveListener(FormOpened);
				form.OnFormClosed.RemoveListener(FormClosed);
			}
		}

		private void FormOpened()
		{
			previousVisibility = Cursor.visible;
			previousLockState = Cursor.lockState;
			Cursor.visible = true;
			Cursor.lockState = CursorLockMode.None;
		}

		private void FormClosed()
		{
			Cursor.visible = previousVisibility;
			Cursor.lockState = previousLockState;
		}
	}
}
