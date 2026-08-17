using System;
using System.Collections;
using System.IO;
using AeLa.EasyFeedback.APIs;
using AeLa.EasyFeedback.Utility;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace AeLa.EasyFeedback
{
	public class FeedbackForm : MonoBehaviour
	{
		[Serializable]
		public class SubmissionMessageEvent : UnityEvent<string>
		{
		}

		public const int MAX_SUBMISSIONS_PER_SESSION = 5;

		[Tooltip("Easy Feedback configuration file")]
		public EFConfig Config;

		[Tooltip("Include screenshot with reports?")]
		public bool IncludeScreenshot = true;

		[Tooltip("Method used to capture the screenshot.")]
		public ScreenshotMode ScreenshotCaptureMode;

		[Tooltip("Resizes screenshots larger than 1080p to help with Trello's filesize limit.\nNOTE: Not supported in Legacy screenshot mode.")]
		public bool ResizeLargeScreenshots = true;

		public Transform Form;

		[Tooltip("Functions to be called when the form is first opened")]
		public UnityEvent OnFormOpened;

		[Tooltip("Functions to be called when the form is submitted")]
		public UnityEvent OnFormSubmitted;

		[Tooltip("Functions to be called when the form is closed")]
		public UnityEvent OnFormClosed;

		[Tooltip("Called to notify of any errors during submission")]
		public SubmissionMessageEvent OnSubmissionError;

		[Tooltip("Called when the submission has successfully completed")]
		public UnityEvent OnSubmissionSucceeded;

		[Tooltip("Called if the submission fails")]
		public UnityEvent OnSubmissionFailed;

		public Report CurrentReport;

		private string screenshotPath;

		private Coroutine ssCoroutine;

		private Trello trello;

		private int submissionCount;

		public bool IsOpen => Form.gameObject.activeSelf;

		public virtual void Awake()
		{
			if (!Config.StoreLocal)
			{
				InitTrelloAPI();
			}
			InitCurrentReport();
			OnSubmissionError.AddListener(ReportError);
		}

		private void ReportError(string message)
		{
			Debug.LogError(message);
		}

		public void InitTrelloAPI()
		{
			trello = new Trello(Config.Token);
		}

		private void InitCurrentReport()
		{
			CurrentReport = new Report();
		}

		public void Toggle()
		{
			if (IsOpen)
			{
				Hide();
			}
			else
			{
				Show();
			}
		}

		public void Show()
		{
			if (!IsOpen && ssCoroutine == null)
			{
				InitCurrentReport();
				ssCoroutine = StartCoroutine(ScreenshotAndOpenForm());
			}
		}

		public virtual void Submit()
		{
		}

		protected IEnumerator SubmitAsync()
		{
			yield return new WaitForEndOfFrame();
			DisableForm();
			OnFormSubmitted.Invoke();
			Hide();
			if (!Config.StoreLocal)
			{
				yield return trello.AddCard(CurrentReport.Title ?? "[no summary]", CurrentReport.ToString() ?? "[no detail]", CurrentReport.Labels, CurrentReport.List.id ?? Config.Board.ListIds[0]);
				if (trello.LastAddCardResponse != null && !trello.UploadError)
				{
					yield return AttachFilesAsync(trello.LastAddCardResponse.id);
				}
			}
			else
			{
				Debug.Log(WriteLocal(CurrentReport));
			}
			if (!Config.StoreLocal && (trello.UploadError || submissionCount >= 5))
			{
				Debug.Log(WriteLocal(CurrentReport));
				if (submissionCount >= 5)
				{
					trello.ErrorMessage = "Too many submissions";
				}
				OnSubmissionError.Invoke("Trello upload failed.\nReason: " + trello.ErrorMessage);
				if (trello.UploadException != null)
				{
					Debug.LogException(trello.UploadException);
				}
				else
				{
					Debug.LogError(trello.ErrorMessage);
				}
				OnSubmissionFailed.Invoke();
				EnableForm();
				CurrentReport = new Report();
			}
			else
			{
				OnSubmissionSucceeded.Invoke();
				submissionCount++;
				EnableForm();
				CurrentReport = new Report();
			}
		}

		private IEnumerator AttachFilesAsync(string cardID)
		{
			foreach (FileAttachment attachment in CurrentReport.Attachments)
			{
				yield return trello.AddAttachmentAsync(cardID, attachment.Data, null, attachment.Name);
				if (trello.UploadError)
				{
					OnSubmissionError.Invoke("Failed to attach file to report.\nReason: " + trello.ErrorMessage);
				}
			}
		}

		private string WriteLocal(Report report)
		{
			string text = Application.persistentDataPath + "/feedback-" + DateTime.Now.ToString("MMddyyyy-HHmmss");
			Directory.CreateDirectory(text);
			File.WriteAllText(text + "/report.txt", report.GetLocalFileText());
			foreach (FileAttachment attachment in CurrentReport.Attachments)
			{
				File.WriteAllBytes(text + "/" + attachment.Name, attachment.Data);
			}
			return text;
		}

		public void DisableForm()
		{
			foreach (Transform item in Form)
			{
				Selectable component = item.GetComponent<Selectable>();
				if (component != null)
				{
					component.interactable = false;
				}
			}
		}

		public void EnableForm()
		{
			foreach (Transform item in Form)
			{
				Selectable component = item.GetComponent<Selectable>();
				if (component != null)
				{
					component.interactable = true;
				}
			}
		}

		public void Hide()
		{
			if (!IsOpen)
			{
				return;
			}
			if (!Config.StoreLocal && IncludeScreenshot && File.Exists(screenshotPath))
			{
				if (ssCoroutine != null)
				{
					StopCoroutine(ssCoroutine);
				}
				File.Delete(screenshotPath);
			}
			screenshotPath = string.Empty;
			ssCoroutine = null;
			OnFormClosed.Invoke();
		}

		private IEnumerator ScreenshotAndOpenForm()
		{
			if (IncludeScreenshot)
			{
				yield return ScreenshotUtil.CaptureScreenshot(ScreenshotCaptureMode, ResizeLargeScreenshots, delegate(byte[] ss)
				{
					CurrentReport.AttachFile("screenshot.png", ss);
				}, delegate(string err)
				{
					OnSubmissionError.Invoke(err);
				});
			}
			EnableForm();
			Form.gameObject.SetActive(value: true);
			OnFormOpened.Invoke();
			ssCoroutine = null;
		}
	}
}
