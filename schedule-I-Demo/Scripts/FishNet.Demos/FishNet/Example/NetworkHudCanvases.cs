using FishNet.Managing;
using FishNet.Transporting;
using UnityEngine;
using UnityEngine.UI;

namespace FishNet.Example
{
	public class NetworkHudCanvases : MonoBehaviour
	{
		private enum AutoStartType
		{
			Disabled = 0,
			Host = 1,
			Server = 2,
			Client = 3
		}

		[Tooltip("What connections to automatically start on play.")]
		[SerializeField]
		private AutoStartType _autoStartType;

		[Tooltip("Color when socket is stopped.")]
		[SerializeField]
		private Color _stoppedColor;

		[Tooltip("Color when socket is changing.")]
		[SerializeField]
		private Color _changingColor;

		[Tooltip("Color when socket is started.")]
		[SerializeField]
		private Color _startedColor;

		[Header("Indicators")]
		[Tooltip("Indicator for server state.")]
		[SerializeField]
		private Image _serverIndicator;

		[Tooltip("Indicator for client state.")]
		[SerializeField]
		private Image _clientIndicator;

		[SerializeField]
		private bool DrawGUI = true;

		private NetworkManager _networkManager;

		private LocalConnectionState _clientState;

		private LocalConnectionState _serverState;

		private void OnGUI()
		{
			GUILayout.BeginArea(new Rect(16f, 16f, 256f, 9000f));
			Vector2 vector = new Vector2(1920f, 1080f);
			GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3((float)Screen.width / vector.x, (float)Screen.height / vector.y, 1f));
			GUIStyle style = GUI.skin.GetStyle("button");
			int fontSize = style.fontSize;
			if ((GetNextStateText(_serverState) == "Start" || GetNextStateText(_clientState) == "Start") && DrawGUI)
			{
				Vector2 vector2 = new Vector2(256f, 64f);
				style.fontSize = 28;
				if (Application.platform != RuntimePlatform.WebGLPlayer)
				{
					if (GUILayout.Button(GetNextStateText(_serverState) + " Server", GUILayout.Width(vector2.x), GUILayout.Height(vector2.y)))
					{
						OnClick_Server();
					}
					GUILayout.Space(10f);
				}
				if (GUILayout.Button(GetNextStateText(_clientState) + " Client", GUILayout.Width(vector2.x), GUILayout.Height(vector2.y)))
				{
					OnClick_Client();
				}
			}
			style.fontSize = fontSize;
			GUILayout.EndArea();
			static string GetNextStateText(LocalConnectionState state)
			{
				return state switch
				{
					LocalConnectionState.Stopped => "Start", 
					LocalConnectionState.Starting => "Starting", 
					LocalConnectionState.Stopping => "Stopping", 
					LocalConnectionState.Started => "Stop", 
					_ => "Invalid", 
				};
			}
		}

		private void Start()
		{
			_serverIndicator.transform.parent.gameObject.SetActive(value: false);
			_clientIndicator.transform.parent.gameObject.SetActive(value: false);
			_networkManager = UnityEngine.Object.FindObjectOfType<NetworkManager>();
			if (_networkManager == null)
			{
				Debug.LogError("NetworkManager not found, HUD will not function.");
				return;
			}
			UpdateColor(LocalConnectionState.Stopped, ref _serverIndicator);
			UpdateColor(LocalConnectionState.Stopped, ref _clientIndicator);
			_networkManager.ServerManager.OnServerConnectionState += ServerManager_OnServerConnectionState;
			_networkManager.ClientManager.OnClientConnectionState += ClientManager_OnClientConnectionState;
			if (_autoStartType == AutoStartType.Host || _autoStartType == AutoStartType.Server)
			{
				OnClick_Server();
			}
			if (!Application.isBatchMode && (_autoStartType == AutoStartType.Host || _autoStartType == AutoStartType.Client))
			{
				OnClick_Client();
			}
		}

		private void OnDestroy()
		{
			if (!(_networkManager == null))
			{
				_networkManager.ServerManager.OnServerConnectionState -= ServerManager_OnServerConnectionState;
				_networkManager.ClientManager.OnClientConnectionState -= ClientManager_OnClientConnectionState;
			}
		}

		private void UpdateColor(LocalConnectionState state, ref Image img)
		{
			Color color = state switch
			{
				LocalConnectionState.Started => _startedColor, 
				LocalConnectionState.Stopped => _stoppedColor, 
				_ => _changingColor, 
			};
			img.color = color;
		}

		private void ClientManager_OnClientConnectionState(ClientConnectionStateArgs obj)
		{
			_clientState = obj.ConnectionState;
			UpdateColor(obj.ConnectionState, ref _clientIndicator);
		}

		private void ServerManager_OnServerConnectionState(ServerConnectionStateArgs obj)
		{
			_serverState = obj.ConnectionState;
			UpdateColor(obj.ConnectionState, ref _serverIndicator);
		}

		public void OnClick_Server()
		{
			if (!(_networkManager == null))
			{
				if (_serverState != LocalConnectionState.Stopped)
				{
					_networkManager.ServerManager.StopConnection(sendDisconnectMessage: true);
				}
				else
				{
					_networkManager.ServerManager.StartConnection();
				}
				DeselectButtons();
			}
		}

		public void OnClick_Client()
		{
			if (!(_networkManager == null))
			{
				if (_clientState != LocalConnectionState.Stopped)
				{
					_networkManager.ClientManager.StopConnection();
				}
				else
				{
					_networkManager.ClientManager.StartConnection();
				}
				DeselectButtons();
			}
		}

		private void SetEventSystem()
		{
		}

		private void DeselectButtons()
		{
		}
	}
}
