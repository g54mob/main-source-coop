using System;
using Photon.Pun;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;
using Zorro.Core;
using Zorro.Core.Serizalization;

public class MonsterSyncer : MonoBehaviour, IPunObservable
{
	public bool applyData = true;

	private Player player;

	private Bot bot;

	private Vector3 targetPos;

	private Vector3 lastPos;

	private float sinceLastPackage;

	private Vector2 targetLook;

	private float lookDistance;

	private bool hasNotRecievedFirstPackage = true;

	public Action<int> AttackAction;

	private PhotonView view;

	private bool m_Inited;

	private void Start()
	{
		player = GetComponent<Player>();
		view = GetComponent<PhotonView>();
		bot = GetComponentInChildren<Bot>();
		if (player == null)
		{
			Debug.LogError("Monster PLAYER is NULL at " + base.gameObject.name);
		}
		if (view == null)
		{
			Debug.LogError("Monster VIEW is NULL at " + base.gameObject.name);
		}
		if (bot == null)
		{
			Debug.LogError("Monster BOT is NULL at " + base.gameObject.name);
		}
		m_Inited = true;
		_ = PhotonNetwork.IsMasterClient;
	}

	[PunRPC]
	public void RPCA_SyncMe(Vector3 position, Vector3 lookDir)
	{
		lastPos = position;
		targetPos = position;
		hasNotRecievedFirstPackage = false;
		if ((bool)bot)
		{
			bot.syncData.lookDireciton = lookDir;
		}
		sinceLastPackage = 0f;
		base.transform.position = position;
	}

	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (!m_Inited)
		{
			Debug.Log("Not Syncing monster since not inited yet!");
		}
		else if (stream.IsWriting)
		{
			Vector3 vector = base.transform.position;
			if (player != null)
			{
				vector = player.GetRelativePosition_Rig(BodypartType.Hip, Vector3.zero);
			}
			BinarySerializer binarySerializer = new BinarySerializer(20, Allocator.Temp);
			binarySerializer.WriteHalf((half)vector.x);
			binarySerializer.WriteHalf((half)vector.y);
			binarySerializer.WriteHalf((half)vector.z);
			Vector3 lookDireciton = bot.syncData.lookDireciton;
			binarySerializer.WriteHalf((half)lookDireciton.x);
			binarySerializer.WriteHalf((half)lookDireciton.y);
			binarySerializer.WriteHalf((half)lookDireciton.z);
			Vector2 vector2 = bot.syncData.movementInput * bot.SpeedFactor();
			binarySerializer.WriteHalf((half)vector2.x);
			binarySerializer.WriteHalf((half)vector2.y);
			byte[] obj = binarySerializer.buffer.ToByteArray();
			stream.SendNext(obj);
			binarySerializer.Dispose();
			bool flag = false;
			if ((bool)player)
			{
				flag = bot.syncData.sprint && !player.data.staminaDepleated;
			}
			stream.SendNext(flag);
			stream.SendNext(bot.syncData.targetPlayerId);
		}
		else
		{
			sinceLastPackage = 0f;
			lastPos = targetPos;
			BinaryDeserializer binaryDeserializer = new BinaryDeserializer((byte[])stream.ReceiveNext(), Allocator.Temp);
			half half5 = binaryDeserializer.ReadHalf();
			half half6 = binaryDeserializer.ReadHalf();
			targetPos = new Vector3(z: binaryDeserializer.ReadHalf(), x: half5, y: half6);
			if (hasNotRecievedFirstPackage)
			{
				hasNotRecievedFirstPackage = false;
				lastPos = targetPos;
			}
			half half7 = binaryDeserializer.ReadHalf();
			half half8 = binaryDeserializer.ReadHalf();
			half half9 = binaryDeserializer.ReadHalf();
			bot.syncData.lookDireciton = new Vector3(half7, half8, half9);
			half half10 = binaryDeserializer.ReadHalf();
			half half11 = binaryDeserializer.ReadHalf();
			bot.syncData.movementInput = new Vector2(half10, half11);
			bot.syncData.sprint = (bool)stream.ReceiveNext();
			bot.syncData.targetPlayerId = (int)stream.ReceiveNext();
			binaryDeserializer.Dispose();
		}
	}

	private void FixedUpdate()
	{
		if (view.IsMine || hasNotRecievedFirstPackage)
		{
			return;
		}
		float timeBetweenPackages = 1f / (float)PhotonNetwork.SerializationRate;
		sinceLastPackage += Time.fixedDeltaTime;
		if (applyData)
		{
			if ((bool)player)
			{
				PhysicsSync(timeBetweenPackages);
			}
			else
			{
				SimpleSync(timeBetweenPackages);
			}
		}
	}

	private void SimpleSync(float timeBetweenPackages)
	{
		Vector3 position = Vector3.Lerp(lastPos, targetPos, sinceLastPackage / timeBetweenPackages);
		base.transform.position = position;
	}

	private void PhysicsSync(float timeBetweenPackages)
	{
		Vector3 vector = Vector3.Lerp(lastPos, targetPos, sinceLastPackage / timeBetweenPackages);
		Vector3 relativePosition_Rig = player.GetRelativePosition_Rig(BodypartType.Hip, Vector3.zero);
		Vector3 vector2 = vector - relativePosition_Rig;
		vector2.y *= 0f;
		float f = vector.y - relativePosition_Rig.y;
		float value = Mathf.Abs(f);
		float num = Mathf.InverseLerp(0.3f, 0.6f, value) * Mathf.Sign(f);
		vector2 += Vector3.up * num;
		player.MoveAllRigsInDirection(vector2 * 0.5f);
	}

	internal void CallAttack(int viewID)
	{
		view.RPC("RPCA_Attack", RpcTarget.All, viewID);
	}

	[PunRPC]
	public void RPCA_Attack(int viewID)
	{
		AttackAction?.Invoke(viewID);
	}
}
