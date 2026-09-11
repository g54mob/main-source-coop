using UnityEngine;
using UnityEngine.InputSystem;

public class Spectate : MonoBehaviour
{
	private Vector2 look;

	private Vector3 lookDirection = Vector3.forward;

	private MainCameraMovement move;

	public static bool spectating;

	private float playerDeadTime;

	private void OnDestroy()
	{
		spectating = false;
	}

	private void Start()
	{
		move = GetComponent<MainCameraMovement>();
	}

	private void Update()
	{
		if ((bool)Player.localPlayer)
		{
			if (spectating)
			{
				DoSpectate();
			}
			else if (playerDeadTime > 3f)
			{
				StartSpectate();
			}
			else
			{
				Player.observedPlayer = Player.localPlayer;
			}
			if (Player.localPlayer.data.dead)
			{
				playerDeadTime += Time.deltaTime;
			}
			else
			{
				playerDeadTime = 0f;
			}
		}
	}

	private void DoSpectate()
	{
		if ((bool)Player.localPlayer && !Player.localPlayer.data.dead)
		{
			StopSpectate();
		}
		else if (!(Player.observedPlayer == null))
		{
			base.transform.SetPositionAndRotation(Player.observedPlayer.TransformCenter() + Vector3.up * 0.75f, Quaternion.LookRotation(lookDirection));
			Vector3 vector = base.transform.position + base.transform.forward * -3f;
			RaycastHit raycastHit = HelperFunctions.LineCheck(base.transform.position, vector, HelperFunctions.LayerType.TerrainProp);
			if ((bool)raycastHit.transform)
			{
				base.transform.position += -base.transform.forward * (raycastHit.distance - 0.2f);
			}
			else
			{
				base.transform.position = vector + base.transform.forward * 0.2f;
			}
			Look();
			Switching();
		}
	}

	private void StopSpectate()
	{
		Player.observedPlayer = null;
		spectating = false;
		GetComponent<MainCameraMovement>().enabled = true;
	}

	private void Switching()
	{
		bool flag = false;
		flag = Keyboard.current.aKey.wasPressedThisFrame;
		if (!flag && Gamepad.current != null)
		{
			flag = Gamepad.current.leftStick.left.wasPressedThisFrame || Gamepad.current.dpad.left.wasPressedThisFrame || Gamepad.current.leftShoulder.wasPressedThisFrame;
		}
		if (flag)
		{
			Debug.Log("Before switch: " + Player.observedPlayer.refs.view.Owner.NickName);
			Player.observedPlayer = PlayerHandler.instance.GetNextPlayerAlive(Player.observedPlayer, -1);
			Debug.Log("After switch: " + Player.observedPlayer.refs.view.Owner.NickName);
			Debug.Log("All players alive");
			for (int i = 0; i < PlayerHandler.instance.playersAlive.Count; i++)
			{
				Debug.Log("After switch: " + PlayerHandler.instance.playersAlive[i].refs.view.Owner.NickName);
			}
		}
		bool flag2 = false;
		flag2 = Keyboard.current.dKey.wasPressedThisFrame;
		if (!flag2 && Gamepad.current != null)
		{
			flag2 = Gamepad.current.leftStick.right.wasPressedThisFrame || Gamepad.current.dpad.right.wasPressedThisFrame || Gamepad.current.rightShoulder.wasPressedThisFrame;
		}
		if (flag2)
		{
			Debug.Log("Before switch: " + Player.observedPlayer.refs.view.Owner.NickName);
			Player.observedPlayer = PlayerHandler.instance.GetNextPlayerAlive(Player.observedPlayer);
			Debug.Log("After switch: " + Player.observedPlayer.refs.view.Owner.NickName);
			Debug.Log("All players alive");
			for (int j = 0; j < PlayerHandler.instance.playersAlive.Count; j++)
			{
				Debug.Log("After switch: " + PlayerHandler.instance.playersAlive[j].refs.view.Owner.NickName);
			}
		}
	}

	private void Look()
	{
		Player localPlayer = Player.localPlayer;
		Vector2 vector = Vector2.zero;
		if (localPlayer != null)
		{
			vector = localPlayer.input.lookInput;
		}
		look.x += vector.x;
		look.y += vector.y;
		look.y = Mathf.Clamp(look.y, -80f, 80f);
		lookDirection = HelperFunctions.LookToDirection(look, Vector3.forward);
	}

	private void StartSpectate()
	{
		Player.observedPlayer = PlayerHandler.instance.GetNextPlayerAlive(Player.observedPlayer);
		GetComponent<MainCameraMovement>().enabled = false;
		spectating = true;
	}
}
