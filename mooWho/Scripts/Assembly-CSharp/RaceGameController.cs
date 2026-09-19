using System;
using System.Runtime.InteropServices;
using Mirror;
using Mirror.RemoteCalls;
using UnityEngine;

public class RaceGameController : NetworkBehaviour
{
	[Header("Turtle")]
	public Transform turtle;

	[Header("Alan (turtle'ın local X ekseninde, bu objeye göre)")]
	[Tooltip("Turtle bu X'in altına inerse mavi alana girmiş olur — KIRMIZI kazanır")]
	public float blueZoneThreshold = -0.4f;

	[Tooltip("Turtle bu X'in üstüne çıkarsa kırmızı alana girmiş olur — MAVİ kazanır")]
	public float redZoneThreshold = 0.4f;

	[Tooltip("Turtle'ın round başı/reset konumu (local pozisyon)")]
	public Vector3 centerLocalPosition = new Vector3(-0.0054f, -0.33440846f, -0.009f);

	[Header("Hareket")]
	[Tooltip("Toplam amplitude başına saniyede kaç local-X birimi ittirilir — turtle YAVAŞ gitsin diye düşük tut")]
	public float pushSpeedPerAmplitude = 0.6f;

	[Tooltip("Yön değişince rotasyonun dönme hızı (derece/sn) — smooth ama hızlı")]
	public float rotationDegreesPerSecond = 720f;

	[Header("Kazanma Efekti")]
	[Tooltip("Biri kazanınca (round biterken) TÜM client'larda oynatılır. Üzerindeki AudioSource'a tek bir klip atanmış olmalı.")]
	public ParticleSystem raceEndParticleFX;

	private AudioSource _raceEndAudioSource;

	private static readonly Quaternion BlueFacingRotation;

	private static readonly Quaternion RedFacingRotation;

	[SyncVar(hook = "OnTurtleLocalXChanged")]
	private float _turtleLocalX;

	private Quaternion _targetRotation = BlueFacingRotation;

	public Action<float, float> _Mirror_SyncVarHookDelegate__turtleLocalX;

	public float Network_turtleLocalX
	{
		get
		{
			return _turtleLocalX;
		}
		[param: In]
		set
		{
			GeneratedSyncVarSetter(value, ref _turtleLocalX, 1uL, _Mirror_SyncVarHookDelegate__turtleLocalX);
		}
	}

	private void Awake()
	{
		Network_turtleLocalX = centerLocalPosition.x;
		if (raceEndParticleFX != null)
		{
			_raceEndAudioSource = raceEndParticleFX.GetComponent<AudioSource>();
		}
	}

	private void Update()
	{
		if (base.isServer)
		{
			ServerTick();
		}
		if (turtle != null)
		{
			turtle.localRotation = Quaternion.RotateTowards(turtle.localRotation, _targetRotation, rotationDegreesPerSecond * Time.deltaTime);
		}
	}

	[Server]
	private void ServerTick()
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void RaceGameController::ServerTick()' called when server was not active");
			return;
		}
		float num = 0f;
		float num2 = 0f;
		RaceGameParticipant[] array = UnityEngine.Object.FindObjectsOfType<RaceGameParticipant>();
		foreach (RaceGameParticipant raceGameParticipant in array)
		{
			if (raceGameParticipant.ServerTeam == RaceTeam.Blue)
			{
				num += raceGameParticipant.ServerAmplitude;
			}
			else if (raceGameParticipant.ServerTeam == RaceTeam.Red)
			{
				num2 += raceGameParticipant.ServerAmplitude;
			}
		}
		float num3 = (num - num2) * pushSpeedPerAmplitude;
		if (Mathf.Abs(num3) > 0.0001f)
		{
			Network_turtleLocalX = _turtleLocalX + num3 * Time.deltaTime;
		}
		CheckWinCondition();
	}

	[Server]
	private void CheckWinCondition()
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void RaceGameController::CheckWinCondition()' called when server was not active");
		}
		else if (_turtleLocalX <= blueZoneThreshold)
		{
			Debug.Log("[RaceGame] Red Win!");
			RpcPlayRaceEndFX();
			ServerResetTurtle();
		}
		else if (_turtleLocalX >= redZoneThreshold)
		{
			Debug.Log("[RaceGame] Blue Win!");
			RpcPlayRaceEndFX();
			ServerResetTurtle();
		}
	}

	[ClientRpc]
	private void RpcPlayRaceEndFX()
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		SendRPCInternal("System.Void RaceGameController::RpcPlayRaceEndFX()", -69066697, writer, 0, includeOwner: true);
		NetworkWriterPool.Return(writer);
	}

	[Server]
	private void ServerResetTurtle()
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void RaceGameController::ServerResetTurtle()' called when server was not active");
		}
		else
		{
			Network_turtleLocalX = centerLocalPosition.x;
		}
	}

	private void OnTurtleLocalXChanged(float oldX, float newX)
	{
		if (turtle != null)
		{
			Vector3 localPosition = turtle.localPosition;
			localPosition.x = newX;
			turtle.localPosition = localPosition;
		}
		if (Mathf.Approximately(newX, centerLocalPosition.x) && Mathf.Abs(oldX - newX) > 0.05f)
		{
			_targetRotation = BlueFacingRotation;
			if (turtle != null)
			{
				turtle.localRotation = _targetRotation;
			}
		}
		else if (newX > oldX + 0.0001f)
		{
			_targetRotation = RedFacingRotation;
		}
		else if (newX < oldX - 0.0001f)
		{
			_targetRotation = BlueFacingRotation;
		}
	}

	public RaceGameController()
	{
		_Mirror_SyncVarHookDelegate__turtleLocalX = OnTurtleLocalXChanged;
	}

	static RaceGameController()
	{
		BlueFacingRotation = Quaternion.Euler(0f, -90f, 0f);
		RedFacingRotation = Quaternion.Euler(0f, 90f, 0f);
		RemoteProcedureCalls.RegisterRpc(typeof(RaceGameController), "System.Void RaceGameController::RpcPlayRaceEndFX()", InvokeUserCode_RpcPlayRaceEndFX);
	}

	public override bool Weaved()
	{
		return true;
	}

	protected void UserCode_RpcPlayRaceEndFX()
	{
		if (raceEndParticleFX != null)
		{
			raceEndParticleFX.Play();
		}
		if (_raceEndAudioSource != null)
		{
			_raceEndAudioSource.Play();
		}
	}

	protected static void InvokeUserCode_RpcPlayRaceEndFX(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("RPC RpcPlayRaceEndFX called on server.");
		}
		else
		{
			((RaceGameController)obj).UserCode_RpcPlayRaceEndFX();
		}
	}

	public override void SerializeSyncVars(NetworkWriter writer, bool forceAll)
	{
		base.SerializeSyncVars(writer, forceAll);
		if (forceAll)
		{
			writer.WriteFloat(_turtleLocalX);
			return;
		}
		writer.WriteVarULong(syncVarDirtyBits);
		if ((syncVarDirtyBits & 1L) != 0L)
		{
			writer.WriteFloat(_turtleLocalX);
		}
	}

	public override void DeserializeSyncVars(NetworkReader reader, bool initialState)
	{
		base.DeserializeSyncVars(reader, initialState);
		if (initialState)
		{
			GeneratedSyncVarDeserialize(ref _turtleLocalX, _Mirror_SyncVarHookDelegate__turtleLocalX, reader.ReadFloat());
			return;
		}
		long num = (long)reader.ReadVarULong();
		if ((num & 1L) != 0L)
		{
			GeneratedSyncVarDeserialize(ref _turtleLocalX, _Mirror_SyncVarHookDelegate__turtleLocalX, reader.ReadFloat());
		}
	}
}
