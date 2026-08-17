using UnityEngine;

namespace AeLa.EasyFeedback
{
	public abstract class FormElement : MonoBehaviour
	{
		protected FeedbackForm Form;

		protected abstract void FormOpened();

		protected abstract void FormSubmitted();

		protected abstract void FormClosed();

		public virtual void Awake()
		{
			Form = GetComponentInParent<FeedbackForm>();
			if (!Form)
			{
				Debug.LogError("This field is not part of a Feedback Form!");
			}
			Form.OnFormOpened.AddListener(FormOpened);
			Form.OnFormSubmitted.AddListener(FormSubmitted);
			Form.OnFormClosed.AddListener(FormClosed);
		}
	}
}
