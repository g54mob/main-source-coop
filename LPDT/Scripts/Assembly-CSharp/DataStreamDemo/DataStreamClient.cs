using System;
using System.Threading;
using Photon.Client;
using Photon.Realtime;
using Photon.Voice;
using Photon.Voice.Unity;
using UnityEngine;

namespace DataStreamDemo
{
	public class DataStreamClient : MonoBehaviour
	{
		private enum Channel
		{
			DataStream = 1
		}

		public string AppId;

		public string AppVersion = "1";

		public string Region = "EU";

		public const string RoomName = "PhotonDataStream";

		public Photon.Voice.LogLevel LogLevel = Photon.Voice.LogLevel.Info;

		[Space(10f)]
		public int FPS = 30;

		public int FrameSize = 2000;

		public int DecodeDelayFrames = 3;

		public bool Echo;

		private Codec DataStreamCodec = Codec.Custom1;

		private bool started;

		private ByteStreamEncoder encoder;

		private LocalVoice localDataStream;

		private Realtime5Transport2 lbt;

		protected Photon.Voice.Unity.Logger logger = new Photon.Voice.Unity.Logger();

		private bool streaming;

		private int cnt;

		private int nextReport = Environment.TickCount + 1000;

		private int sent;

		public VoiceClient VoiceClient => lbt.VoiceClient;

		protected virtual void Start()
		{
			logger.Level = LogLevel;
			lbt = new Realtime5Transport2(logger);
			lbt.RealtimePeer.LogLevel = Photon.Client.LogLevel.Info;
			lbt.StateChanged += delegate(ClientState stateOld, ClientState s)
			{
				logger.Log(Photon.Voice.LogLevel.Info, $"LBC: state: {s}");
				switch (s)
				{
				case ClientState.ConnectedToMasterServer:
					lbt.OpJoinRandomOrCreateRoom(null, new EnterRoomArgs
					{
						RoomName = "PhotonDataStream",
						RoomOptions = new RoomOptions
						{
							MaxPlayers = 5
						}
					});
					break;
				case ClientState.Joined:
					CreateDataStream();
					break;
				case ClientState.Disconnected:
					RemoveDataStream();
					break;
				}
			};
			VoiceClient.SetRemoteVoiceDelayFrames(DataStreamCodec, DecodeDelayFrames);
			VoiceClient voiceClient = VoiceClient;
			voiceClient.OnRemoteVoiceInfoAction = (VoiceClient.RemoteVoiceInfoDelegate)Delegate.Combine(voiceClient.OnRemoteVoiceInfoAction, new VoiceClient.RemoteVoiceInfoDelegate(OnRemoteVoiceAdd));
			Connect();
			Debug.LogFormat("LBC: init");
			started = true;
		}

		protected virtual void Update()
		{
			if (started)
			{
				VoiceClient.SetRemoteVoiceDelayFrames(DataStreamCodec, DecodeDelayFrames);
				if (localDataStream != null)
				{
					localDataStream.DebugEchoMode = Echo;
				}
				lbt.Service();
			}
		}

		protected void OnApplicationQuit()
		{
			Disconnect();
		}

		public void Connect()
		{
			lbt.ConnectUsingSettings(new AppSettings
			{
				AppIdVoice = AppId,
				FixedRegion = Region
			});
		}

		public void Disconnect()
		{
			if (lbt != null)
			{
				lbt.Disconnect();
			}
		}

		private void OnRemoteVoiceAdd(int channelId, int playerId, byte voiceId, VoiceInfo i, ref RemoteVoiceOptions options)
		{
			if (i.Codec == DataStreamCodec)
			{
				options.Decoder = new ByteStreamDecoder(consumeDecoderOutput, delegate
				{
					Debug.LogWarning("Decoder missing frame");
				});
			}
			else
			{
				Debug.LogErrorFormat("LBC: unsupported codec " + i.Codec);
			}
		}

		protected void CreateDataStream()
		{
			VoiceInfo voiceInfo = new VoiceInfo
			{
				Codec = DataStreamCodec
			};
			encoder = new ByteStreamEncoder();
			VoiceCreateOptions options = new VoiceCreateOptions
			{
				DebugEchoMode = Echo,
				Encoder = encoder,
				EventBufSize = 1024,
				Fragment = true,
				Reliable = true
			};
			localDataStream = VoiceClient.CreateLocalVoice(voiceInfo, 1, options);
			new Thread(produceEncoderInput).Start();
		}

		protected void RemoveDataStream()
		{
			streaming = false;
			if (localDataStream != null)
			{
				localDataStream?.RemoveSelf();
			}
		}

		private void produceEncoderInput()
		{
			Debug.LogFormat("Streaming start");
			streaming = true;
			byte[] array = null;
			System.Random random = new System.Random();
			while (streaming)
			{
				if (lbt.State == ClientState.Joined)
				{
					if (array == null || array.Length != FrameSize + 4)
					{
						array = new byte[FrameSize + 4];
					}
					random.NextBytes(array);
					Array.Copy(BitConverter.GetBytes(Util.CalculateCrc(array, 0, array.Length - 4)), 0, array, array.Length - 4, 4);
					encoder.Input(array);
					sent++;
				}
				Thread.Sleep(1000 / FPS);
			}
			Debug.LogFormat("Streaming stop");
		}

		private void consumeDecoderOutput(ref FrameBuffer buf)
		{
			if (Util.CalculateCrc(buf.Array, buf.Offset, buf.Length - 4) != BitConverter.ToUInt32(buf.Array, buf.Offset + buf.Length - 4))
			{
				Debug.LogErrorFormat("Decoder corrupted frame, FrameSize: {0}, buf len: {1}", FrameSize, buf.Length);
			}
			cnt += buf.Length;
			int tickCount = Environment.TickCount;
			if (tickCount - nextReport > 0)
			{
				Debug.LogFormat("Decoder received {0} bytes/sec, FrameSize: {1}, buf len: {2}", cnt, FrameSize, buf.Length);
				cnt = 0;
				nextReport = tickCount + 1000;
			}
		}

		private void OnGUI()
		{
			GUILayout.Label("Sent: " + sent);
		}
	}
}
