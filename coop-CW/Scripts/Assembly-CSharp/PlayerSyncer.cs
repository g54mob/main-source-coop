using Photon.Pun;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;
using Zorro.Core;
using Zorro.Core.Compression;
using Zorro.Core.Serizalization;

public class PlayerSyncer : MonoBehaviour, IPunObservable
{
	private Player player;

	private Vector3 targetPos;

	private Vector3 lastPos;

	private float sinceLastPackage;

	private Vector2 targetLook;

	private float lookDistance;

	private bool first = true;

	private bool hasRecieved;

	private void Start()
	{
		player = GetComponent<Player>();
	}

	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.IsWriting)
		{
			Vector3 relativePosition_Rig = player.GetRelativePosition_Rig(BodypartType.Hip, Vector3.zero);
			PlayerSyncerBoolFlag playerSyncerBoolFlag = PlayerSyncerBoolFlag.NONE;
			if (player.data.isSprinting)
			{
				playerSyncerBoolFlag |= PlayerSyncerBoolFlag.SPRINT;
			}
			if (player.data.isCrouching)
			{
				playerSyncerBoolFlag |= PlayerSyncerBoolFlag.CROUCH;
			}
			if (player.input.movementInput.x > 0.01f)
			{
				playerSyncerBoolFlag |= PlayerSyncerBoolFlag.WALK_RIGHT;
			}
			if (player.input.movementInput.x < -0.01f)
			{
				playerSyncerBoolFlag |= PlayerSyncerBoolFlag.WALK_LEFT;
			}
			if (player.input.movementInput.y > 0.01f)
			{
				playerSyncerBoolFlag |= PlayerSyncerBoolFlag.WALK_FORWARD;
			}
			if (player.input.movementInput.y < -0.01f)
			{
				playerSyncerBoolFlag |= PlayerSyncerBoolFlag.WALK_BACKWARD;
			}
			if (player.input.aimIsPressed)
			{
				playerSyncerBoolFlag |= PlayerSyncerBoolFlag.AIMING;
			}
			BinarySerializer binarySerializer = new BinarySerializer(20, Allocator.Temp);
			Vector2 playerLookValues = player.data.playerLookValues;
			binarySerializer.WriteHalf((half)player.data.sinceGrounded);
			binarySerializer.WriteHalf((half)relativePosition_Rig.x);
			binarySerializer.WriteHalf((half)relativePosition_Rig.y);
			binarySerializer.WriteHalf((half)relativePosition_Rig.z);
			binarySerializer.WriteHalf((half)playerLookValues.x);
			binarySerializer.WriteHalf((half)playerLookValues.y);
			binarySerializer.WriteByte(FloatCompression.CompressZeroOne(player.data.microphoneValue));
			binarySerializer.WriteByte((byte)playerSyncerBoolFlag);
			binarySerializer.WriteByte((player.data.selectedItemSlot == -1) ? byte.MaxValue : ((byte)player.data.selectedItemSlot));
			byte[] obj = binarySerializer.buffer.ToByteArray();
			stream.SendNext(obj);
			binarySerializer.Dispose();
			return;
		}
		hasRecieved = true;
		if (!(player == null) && player.data != null)
		{
			sinceLastPackage = 0f;
			lastPos = targetPos;
			BinaryDeserializer binaryDeserializer = new BinaryDeserializer((byte[])stream.ReceiveNext(), Allocator.Temp);
			player.data.sinceGrounded = binaryDeserializer.ReadHalf();
			half half5 = binaryDeserializer.ReadHalf();
			half half6 = binaryDeserializer.ReadHalf();
			half half7 = binaryDeserializer.ReadHalf();
			targetPos = new Vector3(half5, half6, half7);
			if (first)
			{
				first = false;
				lastPos = targetPos;
			}
			half half8 = binaryDeserializer.ReadHalf();
			half half9 = binaryDeserializer.ReadHalf();
			targetLook = new Vector2(half8, half9);
			lookDistance = Vector2.Distance(player.data.playerLookValues, targetLook);
			player.data.microphoneValue = FloatCompression.DecompressZeroOne(binaryDeserializer.ReadByte());
			PlayerSyncerBoolFlag lhs = (PlayerSyncerBoolFlag)binaryDeserializer.ReadByte();
			byte b = binaryDeserializer.ReadByte();
			if (b == byte.MaxValue)
			{
				player.data.selectedItemSlot = -1;
			}
			else
			{
				player.data.selectedItemSlot = b;
			}
			player.data.isSprinting = lhs.HasFlagUnsafe(PlayerSyncerBoolFlag.SPRINT);
			player.data.isCrouching = lhs.HasFlagUnsafe(PlayerSyncerBoolFlag.CROUCH);
			Vector2 zero = Vector2.zero;
			if (lhs.HasFlagUnsafe(PlayerSyncerBoolFlag.WALK_RIGHT))
			{
				zero.x += 1f;
			}
			if (lhs.HasFlagUnsafe(PlayerSyncerBoolFlag.WALK_LEFT))
			{
				zero.x -= 1f;
			}
			if (lhs.HasFlagUnsafe(PlayerSyncerBoolFlag.WALK_FORWARD))
			{
				zero.y += 1f;
			}
			if (lhs.HasFlagUnsafe(PlayerSyncerBoolFlag.WALK_BACKWARD))
			{
				zero.y -= 1f;
			}
			if (lhs.HasFlagUnsafe(PlayerSyncerBoolFlag.AIMING))
			{
				player.input.aimIsPressed = true;
			}
			else
			{
				player.input.aimIsPressed = false;
			}
			player.input.movementInput = zero;
			binaryDeserializer.Dispose();
		}
	}

	private void Update()
	{
		if (!player.refs.view.IsMine && hasRecieved)
		{
			float num = 1f / (float)PhotonNetwork.SerializationRate;
			float num2 = lookDistance / num;
			player.data.playerLookValues = Vector2.MoveTowards(player.data.playerLookValues, targetLook, num2 * Time.deltaTime);
		}
	}

	private void FixedUpdate()
	{
		if (!player.refs.view.IsMine && hasRecieved)
		{
			float num = 1f / (float)PhotonNetwork.SerializationRate;
			sinceLastPackage += Time.fixedDeltaTime;
			Vector3 vector = Vector3.Lerp(lastPos, targetPos, sinceLastPackage / num);
			Vector3 relativePosition_Rig = player.GetRelativePosition_Rig(BodypartType.Hip, Vector3.zero);
			Vector3 vector2 = vector - relativePosition_Rig;
			vector2.y *= 0f;
			float f = vector.y - relativePosition_Rig.y;
			float value = Mathf.Abs(f);
			float num2 = Mathf.InverseLerp(0.3f, 0.6f, value) * Mathf.Sign(f);
			vector2 += Vector3.up * num2;
			player.MoveAllRigsInDirection(vector2 * 0.5f);
		}
	}
}
