using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ConnectionStateHandler : RetrievableSingleton<ConnectionStateHandler>
{
	public ConnectionStateMachine StateMachine;

	protected override void OnCreated()
	{
		base.OnCreated();
		Object.DontDestroyOnLoad(base.gameObject);
		StateMachine = new ConnectionStateMachine();
		StateMachine.RegisterState(new NoneState());
		StateMachine.RegisterState(new InRoomState());
		StateMachine.RegisterState(new JoiningSpecificRoomState());
		StateMachine.RegisterState(new JoiningSpecificRegionState());
		StateMachine.RegisterState(new DisconnectingState());
		StateMachine.RegisterState(new JoiningSpecificRoomFromRoomState());
		StateMachine.SwitchState<NoneState>();
	}

	protected override void OnRemoved()
	{
		base.OnRemoved();
	}

	public ConnectionState CheckState()
	{
		return StateMachine.CurrentState;
	}

	public void Disconnect()
	{
		RetrievableSingleton<ConnectionStateHandler>.Instance.StateMachine.SwitchState<DisconnectingState>();
		if (MainMenuHandler.SteamLobbyHandler != null)
		{
			MainMenuHandler.SteamLobbyHandler.HideLobby();
		}
		if (PhotonNetwork.InRoom)
		{
			PhotonNetwork.Disconnect();
		}
		else
		{
			SceneManager.LoadScene("NewMainMenuOptimized");
		}
	}
}
