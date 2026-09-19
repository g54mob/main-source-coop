using Dissonance;
using Mirror;
using Mirror.RemoteCalls;
using UnityEngine;

public class RaceGameParticipant : NetworkBehaviour
{
	[Tooltip("Server'a ne sıklıkla amplitude raporlanır (sn)")]
	public float reportInterval = 0.1f;

	private RaceTeam? _localTeam;

	private float _reportTimer;

	private DissonanceComms _comms;

	private VoicePlayerState _localState;

	private VoiceBroadcastTrigger _broadcast;

	public RaceTeam? ServerTeam { get; private set; }

	public float ServerAmplitude { get; private set; }

	public void EnterZone(RaceTeam team, RaceGameController controller)
	{
		if (base.isLocalPlayer)
		{
			_localTeam = team;
			CmdSetZone(inZone: true, team);
		}
	}

	public void ExitZone(RaceTeam team)
	{
		if (base.isLocalPlayer && _localTeam == team)
		{
			_localTeam = null;
			CmdSetZone(inZone: false, team);
		}
	}

	private void Update()
	{
		if (base.isLocalPlayer && _localTeam.HasValue)
		{
			_reportTimer -= Time.deltaTime;
			if (!(_reportTimer > 0f))
			{
				_reportTimer = reportInterval;
				CmdReportAmplitude(ReadLocalAmplitude());
			}
		}
	}

	private float ReadLocalAmplitude()
	{
		if (_comms == null)
		{
			_comms = Object.FindObjectOfType<DissonanceComms>();
		}
		if (_comms == null)
		{
			return 0f;
		}
		if (_broadcast == null)
		{
			_broadcast = Object.FindObjectOfType<VoiceBroadcastTrigger>();
		}
		if (_broadcast == null || !_broadcast.IsTransmitting)
		{
			return 0f;
		}
		if (_localState == null && !string.IsNullOrEmpty(_comms.LocalPlayerName))
		{
			_localState = _comms.FindPlayer(_comms.LocalPlayerName);
		}
		if (_localState == null)
		{
			return 0f;
		}
		return _localState.Amplitude;
	}

	[Command]
	private void CmdSetZone(bool inZone, RaceTeam team)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		writer.WriteBool(inZone);
		GeneratedNetworkCode._Write_RaceTeam(writer, team);
		SendCommandInternal("System.Void RaceGameParticipant::CmdSetZone(System.Boolean,RaceTeam)", 1056688566, writer, 0);
		NetworkWriterPool.Return(writer);
	}

	[Command]
	private void CmdReportAmplitude(float amplitude)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		writer.WriteFloat(amplitude);
		SendCommandInternal("System.Void RaceGameParticipant::CmdReportAmplitude(System.Single)", -1152518827, writer, 0);
		NetworkWriterPool.Return(writer);
	}

	public override bool Weaved()
	{
		return true;
	}

	protected void UserCode_CmdSetZone__Boolean__RaceTeam(bool inZone, RaceTeam team)
	{
		ServerTeam = (inZone ? new RaceTeam?(team) : ((RaceTeam?)null));
		if (!inZone)
		{
			ServerAmplitude = 0f;
		}
	}

	protected static void InvokeUserCode_CmdSetZone__Boolean__RaceTeam(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdSetZone called on client.");
		}
		else
		{
			((RaceGameParticipant)obj).UserCode_CmdSetZone__Boolean__RaceTeam(reader.ReadBool(), GeneratedNetworkCode._Read_RaceTeam(reader));
		}
	}

	protected void UserCode_CmdReportAmplitude__Single(float amplitude)
	{
		ServerAmplitude = (ServerTeam.HasValue ? amplitude : 0f);
	}

	protected static void InvokeUserCode_CmdReportAmplitude__Single(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdReportAmplitude called on client.");
		}
		else
		{
			((RaceGameParticipant)obj).UserCode_CmdReportAmplitude__Single(reader.ReadFloat());
		}
	}

	static RaceGameParticipant()
	{
		RemoteProcedureCalls.RegisterCommand(typeof(RaceGameParticipant), "System.Void RaceGameParticipant::CmdSetZone(System.Boolean,RaceTeam)", InvokeUserCode_CmdSetZone__Boolean__RaceTeam, requiresAuthority: true);
		RemoteProcedureCalls.RegisterCommand(typeof(RaceGameParticipant), "System.Void RaceGameParticipant::CmdReportAmplitude(System.Single)", InvokeUserCode_CmdReportAmplitude__Single, requiresAuthority: true);
	}
}
