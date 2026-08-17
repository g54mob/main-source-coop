using UnityEngine;
using UnityEngine.InputSystem;

public class Spawner : StaticInstance<Spawner>
{
	public GameObject Bread;

	public GameObject Fred;

	[HideInInspector]
	private InputDevice _player1Input;

	[HideInInspector]
	private InputDevice _player2Input;

	public void SpawnPlayers()
	{
		_player1Input = StaticInstance<DataBetweenScenes>.Instance.player1Input;
		_player2Input = StaticInstance<DataBetweenScenes>.Instance.player2Input;
		if (_player1Input == null)
		{
			_player1Input = Keyboard.current;
		}
		if (_player2Input == null)
		{
			_player2Input = Keyboard.current;
		}
		string controlScheme = ((_player1Input != Keyboard.current) ? "Keyboard" : "Controller");
		string controlScheme2 = ((_player2Input != Keyboard.current) ? "Keyboard" : "Controller");
		PlayerInput playerInput = PlayerInput.Instantiate(Bread, -1, controlScheme, -1, _player1Input);
		PlayerInput playerInput2 = PlayerInput.Instantiate(Fred, -1, controlScheme2, -1, _player2Input);
		SetComponents(playerInput.gameObject, playerInput2.gameObject, 0, _player1Input);
		SetComponents(playerInput2.gameObject, playerInput.gameObject, 1, _player2Input);
	}

	private void SetComponents(GameObject ower, GameObject companion, int index, InputDevice device)
	{
		CharacterStateController component = ower.GetComponent<CharacterStateController>();
		ower.transform.position = base.transform.GetChild(index).position;
		ower.GetComponent<DistanceJoint2D>().connectedBody = companion.GetComponent<Rigidbody2D>();
		component.companion = companion;
		component.inputDevice = device;
	}
}
