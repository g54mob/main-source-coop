using UnityEngine;
using UnityEngine.InputSystem;

public class DataBetweenScenes : Singleton<DataBetweenScenes>
{
	public bool loadGame;

	[HideInInspector]
	public InputDevice player1Input;

	[HideInInspector]
	public InputDevice player2Input;

	[HideInInspector]
	public float rumbleFrequency = 1f;

	public bool SinglePlayer;

	public OptionData data;

	protected override void Awake()
	{
		base.Awake();
		player2Input = null;
		player1Input = null;
		data = StaticInstance<PlayerPreferenceLoader>.Instance.LoadPlayerPrefs();
		StaticInstance<PlayerPreferenceLoader>.Instance.LoadInputs();
	}

	public bool SetInputDevice(InputDevice newInput, out int playerJoined)
	{
		playerJoined = -1;
		if (player1Input == null)
		{
			player1Input = newInput;
			playerJoined = 1;
			return true;
		}
		if (player2Input == null)
		{
			if (player1Input == newInput && newInput != Keyboard.current)
			{
				return false;
			}
			player2Input = newInput;
			playerJoined = 2;
			return true;
		}
		return false;
	}

	public Scenes GetGameMode()
	{
		if (!SinglePlayer)
		{
			return Scenes.VictorGame;
		}
		return Scenes.SinglePlayer;
	}

	public bool UnSetInputDevice(InputDevice newInput, out int playerLeft)
	{
		playerLeft = -1;
		if (newInput == player2Input)
		{
			playerLeft = 2;
			player2Input = null;
			return true;
		}
		if (newInput == player1Input)
		{
			playerLeft = 1;
			player1Input = null;
			return true;
		}
		return false;
	}

	internal void RestartPlayersInputs()
	{
		player1Input = null;
		player2Input = null;
	}
}
