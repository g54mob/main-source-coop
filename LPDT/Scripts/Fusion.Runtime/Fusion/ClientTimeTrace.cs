using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Fusion
{
	public class ClientTimeTrace
	{
		private readonly long Timestamp;

		private readonly int Player;

		private int Frames;

		private double FrameDeltaTime;

		private bool PacketReceived;

		private int PacketNumber;

		private int Packets;

		private double PacketDeltaTime;

		private double RoundTripTime;

		private Simulation.TimeFeedback PacketFeedback;

		internal string Folder => Application.persistentDataPath + "/Photon/Fusion/Dev/Logs";

		internal string File
		{
			get
			{
				string arg = DateTimeOffset.FromUnixTimeMilliseconds(Timestamp).UtcDateTime.ToString("yyyy-MM-ddTHH-mm-ssZ");
				return $"fusion_player{Player}_timing_{arg}.csv";
			}
		}

		internal ClientTimeTrace(int player, TickRate.Resolved tickRate)
		{
			Timestamp = ((DateTimeOffset)DateTime.Now).ToUnixTimeMilliseconds();
			Player = player;
			WriteHeaders(tickRate);
		}

		internal void OnFeedback(Simulation.TimeFeedback packetFeedback)
		{
			PacketFeedback = packetFeedback;
		}

		internal void OnPacket(int packetNumber, double packetDeltaTime, double roundTripTime)
		{
			RoundTripTime = roundTripTime;
			PacketDeltaTime = packetDeltaTime;
			Packets++;
			PacketNumber = packetNumber;
			PacketReceived = true;
		}

		internal void OnFrame(double frameDeltaTime)
		{
			FrameDeltaTime = frameDeltaTime;
			Frames++;
			WriteLine();
			PacketReceived = false;
		}

		private void WriteHeaders(TickRate.Resolved tickRate)
		{
			Directory.CreateDirectory(Folder);
			System.IO.File.WriteAllText(Folder + "/" + File, string.Empty);
			StreamWriter streamWriter = System.IO.File.AppendText(Folder + "/" + File);
			string text = $"{Application.platform}";
			streamWriter.WriteLine("client_platform, client_tick_hz, client_send_hz, server_tick_hz, server_send_hz");
			streamWriter.WriteLine(FormattableString.Invariant($"{text},{tickRate.Client},{tickRate.ClientSend},{tickRate.Server},{tickRate.ServerSend}"));
			streamWriter.WriteLine("frame, frame_dt, received_packet, packet, packet_sequence, packet_dt, rtt, fb_packet_dt_avg, fb_packet_dt_dev, fb_buffer_avg, fb_buffer_dev");
			streamWriter.Close();
		}

		private void WriteLine()
		{
			Directory.CreateDirectory(Folder);
			StreamWriter streamWriter = System.IO.File.AppendText(Folder + "/" + File);
			if (PacketReceived)
			{
				streamWriter.WriteLine(FormattableString.Invariant($"{Frames - 1},{FrameDeltaTime:f4},{Convert.ToInt32(PacketReceived)},{Packets - 1},{PacketNumber},{PacketDeltaTime:f4},{RoundTripTime:f4},{PacketFeedback.RecvDeltaAvg:f4},{PacketFeedback.RecvDeltaDev:f4},{PacketFeedback.OffsetAvg:f4},{PacketFeedback.OffsetDev:f4}"));
			}
			else
			{
				streamWriter.WriteLine(FormattableString.Invariant($"{Frames - 1},{FrameDeltaTime:f4},{Convert.ToInt32(PacketReceived)}"));
			}
			streamWriter.Close();
		}

		public static void Replay(TimeSyncConfiguration tsc, string inPath, string outPath)
		{
			if (!System.IO.File.Exists(inPath))
			{
				return;
			}
			ITimeProvider timeProvider = new ClientTimeProvider();
			timeProvider.Configure(tsc);
			int i = 0;
			using (IEnumerator<string> enumerator = System.IO.File.ReadLines(inPath).Take(3).GetEnumerator())
			{
				for (; enumerator.MoveNext(); i++)
				{
					string current = enumerator.Current;
					string[] array = current.Split(',');
					switch (i)
					{
					case 0:
						Assert.Always(array.Length == 5, "columns.Length == 5");
						Assert.Always(array[0].Trim() == "client_platform", "columns[0].Trim() == \"client_platform\"");
						Assert.Always(array[1].Trim() == "client_tick_hz", "columns[1].Trim() == \"client_tick_hz\"");
						Assert.Always(array[2].Trim() == "client_send_hz", "columns[2].Trim() == \"client_send_hz\"");
						Assert.Always(array[3].Trim() == "server_tick_hz", "columns[3].Trim() == \"server_tick_hz\"");
						Assert.Always(array[4].Trim() == "server_send_hz", "columns[4].Trim() == \"server_send_hz\"");
						continue;
					case 1:
					{
						Assert.Always(array.Length == 5, "columns.Length == 5");
						int client = int.Parse(array[1], CultureInfo.InvariantCulture);
						int clientSend = int.Parse(array[2], CultureInfo.InvariantCulture);
						int server = int.Parse(array[3], CultureInfo.InvariantCulture);
						int serverSend = int.Parse(array[4], CultureInfo.InvariantCulture);
						timeProvider.Configure(new SimulationRuntimeConfig
						{
							TickRate = new TickRate.Resolved(client, clientSend, server, serverSend),
							Topology = Topologies.ClientServer
						});
						continue;
					}
					case 2:
						Assert.Always(array.Length == 11, "columns.Length == 11");
						Assert.Always(array[0].Trim() == "frame", "columns[0].Trim() == \"frame\"");
						Assert.Always(array[1].Trim() == "frame_dt", "columns[1].Trim() == \"frame_dt\"");
						Assert.Always(array[2].Trim() == "received_packet", "columns[2].Trim() == \"received_packet\"");
						Assert.Always(array[3].Trim() == "packet", "columns[3].Trim() == \"packet\"");
						Assert.Always(array[4].Trim() == "packet_sequence", "columns[4].Trim() == \"packet_sequence\"");
						Assert.Always(array[5].Trim() == "packet_dt", "columns[5].Trim() == \"packet_dt\"");
						Assert.Always(array[6].Trim() == "rtt", "columns[6].Trim() == \"rtt\"");
						Assert.Always(array[7].Trim() == "fb_packet_dt_avg", "columns[7].Trim() == \"fb_packet_dt_avg\"");
						Assert.Always(array[8].Trim() == "fb_packet_dt_dev", "columns[8].Trim() == \"fb_packet_dt_dev\"");
						Assert.Always(array[9].Trim() == "fb_buffer_avg", "columns[9].Trim() == \"fb_buffer_avg\"");
						Assert.Always(array[10].Trim() == "fb_buffer_dev", "columns[10].Trim() == \"fb_buffer_dev\"");
						continue;
					}
					break;
				}
			}
			System.IO.File.WriteAllText(outPath, string.Empty);
			StreamWriter streamWriter = System.IO.File.AppendText(outPath);
			streamWriter.WriteLine("input_time, local_time, remote_time, input_offset, target_input_offset, input_delay, target_input_delay, interp_delay, target_interp_delay");
			int num = 0;
			foreach (string item in System.IO.File.ReadLines(inPath).Skip(3))
			{
				string[] array2 = item.Split(',');
				float num2 = float.Parse(array2[1], CultureInfo.InvariantCulture);
				if (int.Parse(array2[2], CultureInfo.InvariantCulture) != 0)
				{
					int num3 = int.Parse(array2[4], CultureInfo.InvariantCulture);
					float num4 = float.Parse(array2[5], CultureInfo.InvariantCulture);
					float num5 = float.Parse(array2[6], CultureInfo.InvariantCulture);
					Simulation.TimeFeedback feedback = new Simulation.TimeFeedback
					{
						RecvDeltaAvg = float.Parse(array2[7], CultureInfo.InvariantCulture),
						RecvDeltaDev = float.Parse(array2[8], CultureInfo.InvariantCulture),
						OffsetAvg = float.Parse(array2[9], CultureInfo.InvariantCulture),
						OffsetDev = float.Parse(array2[10], CultureInfo.InvariantCulture)
					};
					timeProvider.OnSnapshotReceived(num5, num3, num4);
					timeProvider.OnFeedbackReceived(feedback);
					if (num == 0)
					{
						timeProvider.Reset(num5, num3);
					}
				}
				else
				{
					Assert.Always(num != 0, "the first row of data is expected to show a packet received");
				}
				Assert.Always(timeProvider.IsRunning(), "provider.IsRunning()");
				timeProvider.Update(num2);
				DebugInstant debugInstant = timeProvider.DebugNow();
				streamWriter.WriteLine(FormattableString.Invariant($"{debugInstant.Input:f4}, {debugInstant.Local:f4}, {debugInstant.Remote:f4}, {debugInstant.InputOffset:f4}, {debugInstant.TargetInputOffset:f4}, {debugInstant.InputDelay:f4}, {debugInstant.TargetInputDelay:f4}, {debugInstant.InterpDelay:f4}, {debugInstant.TargetInterpDelay:f4}"));
				num++;
			}
			streamWriter.Close();
		}
	}
}
