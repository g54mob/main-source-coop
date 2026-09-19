using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Reflection;
using System.Text;
using Features.UserReport.CustomUserReporting.Scripts.Client;
using Features.UserReport.CustomUserReporting.Scripts.Plugin;
using TMPro;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Features.UserReport.CustomUserReporting.Scripts
{
	public class UserReportingScript : MonoBehaviour
	{
		private const int MAXIMUM_SCREEN_SHOT_SIZE_CLIENT = 2048;

		private const int MAXIMUM_SIZE_SCREEN_SHOT = 512;

		private const float CLICK_TIME_EPSILON = 0.3f;

		private const string PLATFORM = "Platform";

		private const string VERSION = "Version";

		private const string PLATFORM_VERSION = "Platform.Version";

		private const string CONTENT_TYPE = "text/plain";

		private const string USER_ID_TXT = "UserID.txt";

		private const string USER_ID = "User ID";

		private const string UNKNOWN = "Unknown";

		private const string VERSION_0_0 = "0.0";

		private const string EVENT_SYSTEM = "EventSystem";

		private const string UNITY_CLOUD_USER_REPORTING = "Unity.Cloud.UserReporting.Plugin.Version2018_3.AsyncUnityUserReportingPlatform";

		private const string APPLICATION_JSON = "application/json";

		private const string PING = "\"Ping\"";

		private const string DESCRIPTION = "Description";

		private const string GAME_DATA = "GameData";

		private const string CATEGORY = "Category";

		private const string SAVE_ARCHIVE = "SaveArchive";

		private const string LOGS_FILE_NAME = "Player.log";

		private const string EDITOR_LOGS_FILE_NAME = "Editor.log";

		private const string LOGS_NAME = "PlayerLogs";

		private int _frameCount;

		private bool _isShowingError;

		private bool _isSubmitting;

		private Features.UserReport.CustomUserReporting.Scripts.Client.UserReport _currentUserReport;

		private UserReportingState _currentState;

		private Coroutine _updateRoutine;

		private Coroutine _createReportRoutine;

		[Tooltip("The category dropdown.")]
		public Dropdown CategoryDropdown;

		[Tooltip("The description input on the user report form.")]
		public TMP_InputField DescriptionInput;

		[Tooltip("The UI shown when there's an error.")]
		public Canvas ErrorPopup;

		private bool _isCreatingUserReport;

		[Tooltip("A value indicating whether the hotkey is enabled (Left Alt + Left Shift + B).")]
		public bool IsHotkeyEnabled;

		[Tooltip("A value indicating whether the prefab is in silent mode. Silent mode does not show the user report form.")]
		public bool IsInSilentMode;

		[Tooltip("A value indicating whether the user report client reports metrics about itself.")]
		public bool IsSelfReporting;

		[Tooltip("The display text for the progress text.")]
		public Text ProgressText;

		[Tooltip("A value indicating whether the user report client send events to analytics.")]
		public bool SendEventsToAnalytics;

		[Tooltip("The UI shown while submitting.")]
		public Canvas SubmittingPopup;

		[Tooltip("The UI shown while start submitting.")]
		public Canvas SubmittingStartPopup;

		[Tooltip("The summary input on the user report form.")]
		public TMP_InputField SummaryInput;

		[Tooltip("The thumbnail viewer on the user report form.")]
		public Image ThumbnailViewer;

		private readonly UnityUserReportingUpdater unityUserReportingUpdater;

		[Tooltip("The user report button used to create a user report.")]
		public Button UserReportButton;

		[Tooltip("The UI for the user report form. Shown after a user report is created.")]
		public Canvas UserReportForm;

		public GameObject DescriptionWarning;

		[Tooltip("The User Reporting platform. Different platforms have different features but may require certain Unity versions or target platforms. The Async platform adds async screenshotting and report creation, but requires Unity 2018.3 and above, the package manager version of Unity User Reporting, and a target platform that supports asynchronous GPU readback such as DirectX.")]
		public UserReportingPlatformType UserReportingPlatform;

		[Tooltip("The event raised when a user report is submitting.")]
		public UnityEvent UserReportSubmitting;

		[Header("Elements to localize")]
		[SerializeField]
		private TMP_Text titleText;

		[SerializeField]
		private TMP_Text summaryText;

		[SerializeField]
		private TMP_Text descriptionText;

		[SerializeField]
		private Text submitText;

		[SerializeField]
		private Text cancelText;

		[SerializeField]
		private TMP_Text warningText;

		[SerializeField]
		private bool _useInjectionSystem;

		[SerializeField]
		private Image _backGroundImage;

		[Header("'Secret' options")]
		public int clicksToOpenSecret = 10;

		public int clickCountSecret;

		public float summaryMinSigns = 5f;

		public float lastClickTime;

		private readonly Vector2 _pivot = new Vector2(0.5f, 0.5f);

		private ICollection<UserReportNamedValue> _inGameData = new Collection<UserReportNamedValue>();

		private bool _wasPausedBeforeReport;

		private Features.UserReport.CustomUserReporting.Scripts.Client.UserReport CurrentUserReport
		{
			get
			{
				return _currentUserReport;
			}
			set
			{
				_currentUserReport = value;
				UpdateView();
			}
		}

		private UserReportingState State
		{
			get
			{
				if (CurrentUserReport == null)
				{
					if (!IsCreatingUserReport)
					{
						return UserReportingState.Idle;
					}
					return UserReportingState.CreatingUserReport;
				}
				if (IsInSilentMode)
				{
					return UserReportingState.Idle;
				}
				if (!IsSubmittingProperty)
				{
					return UserReportingState.ShowingForm;
				}
				return UserReportingState.SubmittingForm;
			}
		}

		private bool IsSubmittingProperty
		{
			get
			{
				return _isSubmitting;
			}
			set
			{
				_isSubmitting = value;
				UpdateView();
			}
		}

		private bool IsCreatingUserReport
		{
			get
			{
				return _isCreatingUserReport;
			}
			set
			{
				_isCreatingUserReport = value;
				UpdateView();
			}
		}

		public event Action OnReportClosed;

		public UserReportingScript()
		{
			UserReportSubmitting = new UnityEvent();
			unityUserReportingUpdater = new UnityUserReportingUpdater();
		}

		protected virtual void RaiseUserReportSubmitting()
		{
			UserReportSubmitting?.Invoke();
		}

		public void CancelUserReport()
		{
			_backGroundImage.enabled = false;
			CurrentUserReport = null;
			ClearForm();
			UpdateView();
			StopRoutines();
			UnpauseGame();
			this.OnReportClosed?.Invoke();
		}

		private void StopRoutines()
		{
			StopUpdateRoutine();
			StopCreatRoutine();
		}

		private void StopCreatRoutine()
		{
			if (_createReportRoutine != null)
			{
				StopCoroutine(_createReportRoutine);
			}
		}

		private IEnumerator ClearError()
		{
			yield return new WaitForSeconds(10f);
			_isShowingError = false;
		}

		private void ClearForm()
		{
			SummaryInput.text = null;
			DescriptionInput.text = null;
		}

		public void OnSecretButton()
		{
			if (Time.time - lastClickTime < 0.3f)
			{
				clickCountSecret++;
				if (clickCountSecret >= clicksToOpenSecret)
				{
					CreateUserReport();
				}
			}
			else
			{
				clickCountSecret = 0;
			}
			lastClickTime = Time.time;
			UpdateView();
		}

		public void CreateUserReportWithAdditionalText(ICollection<UserReportNamedValue> inGameData)
		{
			_inGameData = inGameData;
			CreateUserReport();
		}

		public void CreateUserReport()
		{
			PauseGame();
			_backGroundImage.enabled = true;
			if (!IsCreatingUserReport)
			{
				Debug.Log("CreateUserReport");
				_createReportRoutine = StartCoroutine(UpdateCreateReportRoutine());
				DescriptionWarning.SetActive(value: false);
				IsCreatingUserReport = true;
				UnityUserReporting.CurrentClient.ClearScreenshots();
				UnityUserReporting.CurrentClient.TakeScreenshot(2048, 2048, delegate
				{
				});
				UnityUserReporting.CurrentClient.TakeScreenshot(512, 512, delegate
				{
				});
				UnityUserReporting.CurrentClient.CreateUserReport(CreateUserReportCallback);
				UpdateView();
			}
		}

		private void CreateUserReportCallback(Features.UserReport.CustomUserReporting.Scripts.Client.UserReport userReport)
		{
			ProjectIdentifier(userReport);
			Attachments(userReport);
			Dimensions(userReport);
			CurrentUserReport = userReport;
			IsCreatingUserReport = false;
			SetThumbnail(userReport);
			if (IsInSilentMode)
			{
				SubmitUserReport();
			}
			StopCreatRoutine();
		}

		private static void ProjectIdentifier(Features.UserReport.CustomUserReporting.Scripts.Client.UserReport userReport)
		{
			if (string.IsNullOrEmpty(userReport.ProjectIdentifier))
			{
				Debug.LogWarning("The user report's project identifier is not set. Please setup cloud services using the Services tab or manually specify a project identifier when calling UnityUserReporting.Configure().");
			}
		}

		private void Attachments(Features.UserReport.CustomUserReporting.Scripts.Client.UserReport userReport)
		{
			userReport.Attachments.Add(new UserReportAttachment("User ID", "UserID.txt", "text/plain", Encoding.UTF8.GetBytes(AnalyticsSessionInfo.userId)));
			userReport.Attachments.Add(new UserReportAttachment("PlayerLogs", GetLogsFileName(), "text/plain", GetFileData(userReport)));
		}

		private string GetLogsFileName()
		{
			if (!Application.isEditor)
			{
				return "Player.log";
			}
			return "Editor.log";
		}

		private byte[] GetFileData(Features.UserReport.CustomUserReporting.Scripts.Client.UserReport context)
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (UserReportEvent @event in context.Events)
			{
				stringBuilder.Append(@event.FullMessage);
			}
			return Encoding.UTF8.GetBytes(stringBuilder.ToString());
		}

		private static void Dimensions(Features.UserReport.CustomUserReporting.Scripts.Client.UserReport userReport)
		{
			string text = "Unknown";
			string text2 = "0.0";
			foreach (UserReportNamedValue deviceMetadatum in userReport.DeviceMetadata)
			{
				if (deviceMetadatum.Name == "Platform")
				{
					text = deviceMetadatum.Value;
				}
				if (deviceMetadatum.Name == "Version")
				{
					text2 = deviceMetadatum.Value;
				}
			}
			userReport.Dimensions.Add(new UserReportNamedValue("Platform.Version", text + "." + text2));
		}

		private static void CopyFilesFromSaveFolderTo(string savesPath, DirectoryInfo saveCopiesDirectoryInfo)
		{
			Array.ForEach(new DirectoryInfo(savesPath).GetFiles(), delegate(FileInfo file)
			{
				file.CopyTo(Path.Combine(saveCopiesDirectoryInfo.FullName, file.Name));
			});
		}

		private static DirectoryInfo GetClearSaveCopyDirectory()
		{
			string path = Path.Combine(Application.persistentDataPath, "SaveArchive");
			DirectoryInfo directoryInfo;
			if (!Directory.Exists(path))
			{
				directoryInfo = new DirectoryInfo(Application.persistentDataPath).CreateSubdirectory("SaveArchive");
			}
			else
			{
				directoryInfo = new DirectoryInfo(path);
				directoryInfo.Delete(recursive: true);
			}
			return directoryInfo;
		}

		private UserReportingClientConfiguration GetConfiguration()
		{
			return new UserReportingClientConfiguration();
		}

		private void SetThumbnail(Features.UserReport.CustomUserReporting.Scripts.Client.UserReport userReport)
		{
			if (userReport != null && !(ThumbnailViewer == null))
			{
				byte[] data = Convert.FromBase64String(userReport.Thumbnail.DataBase64);
				Texture2D texture2D = new Texture2D(1, 1);
				texture2D.LoadImage(data);
				Rect rect = new Rect(0f, 0f, texture2D.width, texture2D.height);
				ThumbnailViewer.sprite = Sprite.Create(texture2D, rect, _pivot);
				ThumbnailViewer.preserveAspect = true;
			}
		}

		private static void SetupEventSystem()
		{
			if (Application.isPlaying && !(UnityEngine.Object.FindObjectOfType<EventSystem>() != null))
			{
				GameObject obj = new GameObject("EventSystem");
				obj.AddComponent<EventSystem>();
				obj.AddComponent<StandaloneInputModule>();
			}
		}

		private static void Ping()
		{
			string endpoint = "https://userreporting.cloud.unity3d.com/api/userreporting/projects/" + UnityUserReporting.CurrentClient.ProjectIdentifier + "/ping";
			UnityUserReporting.CurrentClient.Platform.Post(endpoint, "application/json", Encoding.UTF8.GetBytes("\"Ping\""), delegate
			{
			}, delegate
			{
			});
		}

		private void ConfigureClient()
		{
			bool flag = false;
			if (UserReportingPlatform == UserReportingPlatformType.Async)
			{
				Type type = Assembly.GetExecutingAssembly().GetType("Unity.Cloud.UserReporting.Plugin.Version2018_3.AsyncUnityUserReportingPlatform");
				if (type != null && Activator.CreateInstance(type) is IUserReportingPlatform platform)
				{
					UnityUserReporting.Configure(platform, GetConfiguration());
					flag = true;
				}
			}
			if (!flag)
			{
				UnityUserReporting.Configure(GetConfiguration());
			}
		}

		private void OnSummaryChanged(string summary)
		{
			if (!string.IsNullOrWhiteSpace(SummaryInput.text) && (float)SummaryInput.text.Length >= summaryMinSigns)
			{
				DescriptionWarning.SetActive(value: false);
			}
		}

		public void SubmitUserReport()
		{
			Debug.Log("SubmitUserReport");
			_backGroundImage.enabled = false;
			_frameCount = Time.frameCount;
			if (!PreconditionsIsNotMet())
			{
				IsSubmittingProperty = true;
				SetSummary();
				SetCategory();
				SetDescription();
				SetInGameData();
				ClearForm();
				RaiseUserReportSubmitting();
				_updateRoutine = StartCoroutine(UpdateSubmitRoutine());
				UnityUserReporting.CurrentClient.SendUserReport(CurrentUserReport, ProgressCallback, SendReportCallback);
				UnpauseGame();
				this.OnReportClosed?.Invoke();
			}
		}

		private void SendReportCallback(bool success, Features.UserReport.CustomUserReporting.Scripts.Client.UserReport br2)
		{
			if (!success)
			{
				_isShowingError = true;
				StartCoroutine(ClearError());
			}
			CurrentUserReport = null;
			IsSubmittingProperty = false;
			StopUpdateRoutine();
		}

		private void ProgressCallback(float uploadProgress, float downloadProgress)
		{
			if (!(ProgressText == null))
			{
				ProgressText.text = $"{uploadProgress:P}";
			}
		}

		private void SetDescription()
		{
			if (!(DescriptionInput == null))
			{
				UserReportNamedValue item = new UserReportNamedValue
				{
					Name = "Description",
					Value = DescriptionInput.text
				};
				CurrentUserReport.Fields.Add(item);
			}
		}

		private void SetSummary()
		{
			if (SummaryInput != null)
			{
				CurrentUserReport.Summary = SummaryInput.text;
			}
		}

		private void SetCategory()
		{
			if (!(CategoryDropdown == null))
			{
				string text = CategoryDropdown.options[CategoryDropdown.value].text;
				CurrentUserReport.Dimensions.Add(new UserReportNamedValue("Category", text));
				CurrentUserReport.Fields.Add(new UserReportNamedValue("Category", text));
			}
		}

		private void SetInGameData()
		{
			foreach (UserReportNamedValue inGameDatum in _inGameData)
			{
				CurrentUserReport.Fields.Add(inGameDatum);
			}
		}

		private bool PreconditionsIsNotMet()
		{
			if (IsSubmittingProperty || CurrentUserReport == null)
			{
				return true;
			}
			if (IsInSilentMode || (!string.IsNullOrWhiteSpace(SummaryInput.text) && !((float)SummaryInput.text.Length < summaryMinSigns)))
			{
				return false;
			}
			DescriptionWarning.SetActive(value: true);
			return true;
		}

		private void StopUpdateRoutine()
		{
			if (_updateRoutine != null)
			{
				StopCoroutine(_updateRoutine);
			}
		}

		private IEnumerator UpdateSubmitRoutine()
		{
			while (_currentUserReport != null)
			{
				UpdateView();
				unityUserReportingUpdater.Reset();
				yield return StartCoroutine(UnityUserReportingUpdater());
			}
		}

		private IEnumerator UpdateCreateReportRoutine()
		{
			while (true)
			{
				UpdateView();
				unityUserReportingUpdater.Reset();
				yield return StartCoroutine(UnityUserReportingUpdater());
			}
		}

		private void UpdateView()
		{
			if (_frameCount != Time.frameCount)
			{
				UnityUserReporting.CurrentClient.IsSelfReporting = IsSelfReporting;
				UnityUserReporting.CurrentClient.SendEventsToAnalytics = SendEventsToAnalytics;
				_currentState = State;
				UserReportButton.interactable = _currentState == UserReportingState.Idle;
				SubmittingStartPopup.enabled = _currentState == UserReportingState.CreatingUserReport;
				SubmittingStartPopup.gameObject.SetActive(SubmittingStartPopup.enabled);
				UserReportForm.enabled = _currentState == UserReportingState.ShowingForm;
				UserReportForm.gameObject.SetActive(UserReportForm.enabled);
				SubmittingPopup.enabled = _currentState == UserReportingState.SubmittingForm;
				SubmittingPopup.gameObject.SetActive(SubmittingPopup.enabled);
				ErrorPopup.enabled = _isShowingError;
			}
		}

		private IEnumerator UnityUserReportingUpdater()
		{
			yield return unityUserReportingUpdater;
		}

		private void Start()
		{
			SetupEventSystem();
			ConfigureClient();
			Ping();
			UpdateView();
		}

		private void OnEnable()
		{
			_backGroundImage.enabled = false;
			SummaryInput.onValueChanged.AddListener(OnSummaryChanged);
		}

		private void OnDisable()
		{
			SummaryInput.onValueChanged.RemoveListener(OnSummaryChanged);
		}

		private void PauseGame()
		{
		}

		private void UnpauseGame()
		{
			if (_wasPausedBeforeReport)
			{
				_wasPausedBeforeReport = false;
			}
		}
	}
}
