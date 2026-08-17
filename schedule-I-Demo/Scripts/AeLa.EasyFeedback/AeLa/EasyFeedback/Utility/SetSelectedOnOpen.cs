using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace AeLa.EasyFeedback.Utility
{
	[RequireComponent(typeof(FormElement))]
	public class SetSelectedOnOpen : MonoBehaviour
	{
		private FeedbackForm form;

		private Coroutine coroutine;

		private void Awake()
		{
			form = GetComponentInParent<FeedbackForm>();
			form.OnFormOpened.AddListener(StartSelectedCoroutine);
			form.OnFormClosed.AddListener(StopCoroutineIfExists);
		}

		private void StartSelectedCoroutine()
		{
			coroutine = StartCoroutine(SetSelfAsSelected());
		}

		private void StopCoroutineIfExists()
		{
			if (coroutine != null)
			{
				StopCoroutine(coroutine);
			}
		}

		private IEnumerator SetSelfAsSelected()
		{
			if (!EventSystem.current)
			{
				Debug.LogError("Scene is missing an EventSystem.");
				yield break;
			}
			EventSystem.current.SetSelectedGameObject(null);
			yield return new WaitForEndOfFrame();
			EventSystem.current.SetSelectedGameObject(base.gameObject, null);
			coroutine = null;
		}
	}
}
