using Cysharp.Threading.Tasks;
using Features.MultiplayerSessionServices.Scripts;
using RSG.Muffin.UINavigationSubmodule.UINavigationModule.Scripts;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Features.MainMenuModule.Scripts
{
	public class ReconnectPopupController : MonoBehaviour
	{
		[SerializeField]
		private Button _reconnectButton;

		[SerializeField]
		private Button _dismissButton;

		[SerializeField]
		private GameObject _container;

		[SerializeField]
		private Selectable _exitSelected;

		[SerializeField]
		private Selectable _firstSelected;

		private INavigationService _navigationService;

		private ISessionReconnectService _sessionReconnectService;

		private ISessionRecoverySource _sessionRecoverySource;

		private string _sessionName;

		[Inject]
		public void InjectDependencies(ISessionReconnectService sessionReconnectService, INavigationService navigationService, ISessionRecoverySource sessionRecoverySource)
		{
			_sessionReconnectService = sessionReconnectService;
			_navigationService = navigationService;
			_sessionRecoverySource = sessionRecoverySource;
		}

		private void Start()
		{
			ValidateAndShowAsync().Forget();
		}

		private async UniTaskVoid ValidateAndShowAsync()
		{
			Hide();
			if (_sessionRecoverySource.TryGetReconnectSession(out var sessionName) && _sessionRecoverySource.HasError)
			{
				if (!(await _sessionReconnectService.IsSessionValidAsync(sessionName)))
				{
					Hide();
				}
				else
				{
					Initialize(sessionName);
				}
			}
		}

		private void Initialize(string sessionName)
		{
			_sessionName = sessionName;
			_reconnectButton.onClick.RemoveAllListeners();
			_dismissButton.onClick.RemoveAllListeners();
			_reconnectButton.onClick.AddListener(OnReconnectClicked);
			_dismissButton.onClick.AddListener(OnDismissClicked);
			if (_container != null)
			{
				_navigationService.SetNavigationToObject(_firstSelected);
				_container.SetActive(value: true);
			}
			else
			{
				base.gameObject.SetActive(value: true);
			}
		}

		public void Hide()
		{
			if (!(this == null))
			{
				if (_container != null)
				{
					_container.SetActive(value: false);
				}
				else
				{
					base.gameObject.SetActive(value: false);
				}
			}
		}

		private void OnReconnectClicked()
		{
			_navigationService.SetNavigationToObject(_exitSelected);
			Hide();
			AttemptReconnect().Forget();
		}

		private async UniTaskVoid AttemptReconnect()
		{
			await _sessionReconnectService.TryReconnectAsync(_sessionName);
		}

		private void OnDismissClicked()
		{
			_navigationService.SetNavigationToObject(_exitSelected);
			Hide();
		}
	}
}
