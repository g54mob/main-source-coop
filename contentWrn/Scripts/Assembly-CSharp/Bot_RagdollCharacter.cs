using System;
using UnityEngine;

public class Bot_RagdollCharacter : MonoBehaviour
{
	private Player player;

	private Bot bot;

	private static Vector3 upDelta = Vector3.up * 1.5f;

	private int id;

	private Rigidbody hip;

	private void Start()
	{
		player = GetComponentInParent<Player>();
		bot = GetComponent<Bot>();
		id = UnityEngine.Random.Range(0, 10);
		Bot obj = bot;
		obj.teleportAction = (Action<Vector3>)Delegate.Combine(obj.teleportAction, new Action<Vector3>(Teleport));
		hip = player.refs.ragdoll.GetBodypart(BodypartType.Hip).rig;
	}

	private void Teleport(Vector3 position)
	{
		player.data.framesSinceBotTeleport = 0;
		Vector3 delta = position + upDelta - player.TransformCenter();
		player.data.lastSimplifiedPosition = position + Vector3.up;
		player.MoveAllRigsInDirection(delta);
		GetComponent<Bot_RagdollCharacter>().DontCullForABit();
	}

	private void Update()
	{
		ApplyState();
		CullPhysics();
	}

	public void DontCullForABit()
	{
		id -= 30;
	}

	private void CullPhysics()
	{
		id++;
		if (id <= 10)
		{
			return;
		}
		id = 0;
		float num = float.MaxValue;
		for (int i = 0; i < PlayerHandler.instance.playersAlive.Count; i++)
		{
			float num2 = Vector3.Distance(bot.Center(), PlayerHandler.instance.playersAlive[i].Center());
			if (num2 < num)
			{
				num = num2;
			}
		}
		player.refs.ragdoll.ToggleSimplifiedRagdoll(num > 30f);
	}

	private void ApplyState()
	{
		player.SetLookDirection(bot.syncData.lookDireciton);
		player.input.movementInput = bot.syncData.movementInput;
		player.input.sprintIsPressed = bot.syncData.sprint;
		bot.turnVel = hip.angularVelocity.y;
		if (player.refs.view.IsMine)
		{
			player.input.movementInput *= bot.SpeedFactor();
			player.data.isSprinting = bot.syncData.sprint && !player.data.staminaDepleated;
		}
		else
		{
			player.data.isSprinting = bot.syncData.sprint;
		}
	}
}
